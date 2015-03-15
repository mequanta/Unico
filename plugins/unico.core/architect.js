// extracted and adapted from http://github.com/c9/architect.git
define(["EventEmitter2"], function(EventEmitter) {
    var DEBUG = typeof location != "undefined" && location.href.match(/debug=[12]/) ? true : false;

    function loadConfig(path, callback) {
        require([path], function (config) {
            resolveConfig(config, callback);
        });
    }

    function resolveConfig(config, base, callback) {
        if (typeof base == "function") {
            callback = base;
            base = "";
        }
        var paths = [], pluginIndexes = {};
        config.forEach(function(plugin, index) {
            // Shortcut where string is used for plugin without any options.
            if (typeof plugin === "string") {
                plugin = config[index] = { packagePath: plugin };
            }
            // The plugin is a package over the network.  We need to load it.
            if (plugin.hasOwnProperty("packagePath") && !plugin.hasOwnProperty("setup")) {
                paths.push((base || "") + plugin.packagePath);
                pluginIndexes[plugin.packagePath] = index;
            }
        });
        // Mass-Load path-based plugins using amd's require
        require(paths, function () {
            var args = arguments;
            paths.forEach(function (name, i) {
                var module = args[i];
                var plugin = config[pluginIndexes[name]];
                plugin.setup = module;
                plugin.provides = module.provides || [];
                plugin.consumes = module.consumes || [];
            });
            callback(null, config);
        });
    }

    // Check a plugin config list for bad dependencies and throw on error
    function checkConfig(config, lookup) {

        // Check for the required fields in each plugin.
        config.forEach(function (plugin) {
            if (plugin.checked) {
                return;
            }
            if (!plugin.hasOwnProperty("setup")) {
                throw new Error("Plugin is missing the setup function " + JSON.stringify(plugin));
            }
            if (!plugin.hasOwnProperty("provides")) {
                throw new Error("Plugin is missing the provides array " + JSON.stringify(plugin));
            }
            if (!plugin.hasOwnProperty("consumes")) {
                throw new Error("Plugin is missing the consumes array " + JSON.stringify(plugin));
            }
        });

        return checkCycles(config, lookup);
    }

    function checkCycles(config, lookup) {
        var plugins = [];
        config.forEach(function(pluginConfig, index) {
            plugins.push({
                packagePath: pluginConfig.packagePath,
                provides: pluginConfig.provides.concat(),
                consumes: pluginConfig.consumes.concat(),
                i: index
            });
        });

        var resolved = {
            hub: true
        };
        var changed = true;
        var sorted = [];

        while (plugins.length && changed) {
            changed = false;

            plugins.concat().forEach(function(plugin) {
                var consumes = plugin.consumes.concat();

                var resolvedAll = true;
                for (var i = 0; i < consumes.length; i++) {
                    var service = consumes[i];
                    if (!resolved[service] && (!lookup || !lookup(service))) {
                        resolvedAll = false;
                    } else {
                        plugin.consumes.splice(plugin.consumes.indexOf(service), 1);
                    }
                }

                if (!resolvedAll)
                    return;

                plugins.splice(plugins.indexOf(plugin), 1);
                plugin.provides.forEach(function(service) {
                    resolved[service] = true;
                });
                sorted.push(config[plugin.i]);
                changed = true;
            });
        }

        if (plugins.length) {
            var unresolved = {};
            plugins.forEach(function(plugin) {
                delete plugin.config;
                plugin.consumes.forEach(function (name) {
                    if (unresolved[name] === false)
                        return;
                    if (!unresolved[name])
                        unresolved[name] = [];
                    unresolved[name].push(plugin.packagePath);
                });
                plugin.provides.forEach(function (name) {
                    unresolved[name] = false;
                });
            });

            Object.keys(unresolved).forEach(function (name) {
                if (unresolved[name] === false)
                    delete unresolved[name];
            });

            var pluginsList = plugins.map(function (p) {
                return p.packagePath;
            }).join("\n");
            var unresolvedList = Object.keys(unresolved);
            console.warn("Could not resolve dependencies of these plugins:\n"
                + pluginsList + "\n", plugins,
                "\nMissing services:\n" + unresolvedList.join("\n") + "\n", unresolved,
                "\nResolved services:", Object.keys(resolved));
            var err = new Error("Could not resolve dependencies\n" + "Missing services: " + unresolvedList);
            err.unresolved = unresolvedList;
            throw err;
        }

        return sorted;
    }

    function Architect(config) {
        var app = this;
        app.config = config;
        app.packages = {};
        app.pluginToPackage = {};

        var isAdditionalMode;
        var services = app.services = {
            hub: {
                on: function(name, callback) {
                    app.on(name, callback);
                }
            }
        };

        // Check the config
        var sortedPlugins = checkConfig(config);

        var destructors = [];
        var recur = 0, callnext, ready;

        function startPlugins(additional) {
            var plugin = sortedPlugins.shift();
            if (!plugin) {
                ready = true;
                return app.emit(additional ? "ready-additional" : "ready", app);
            }

            var imports = {};
            if (plugin.consumes) {
                plugin.consumes.forEach(function(name) {
                    imports[name] = services[name];
                });
            }

            var packageName = (plugin.packagePath || "").replace(/.*plugins\/([^\/]*)\/.*$/, "$1");

            if (!app.packages[packageName]) app.packages[packageName] = [];

            if (DEBUG) {
                recur++;
                plugin.setup(plugin, imports, register);

                while (callnext && recur <= 1) {
                    callnext = false;
                    startPlugins(additional);
                }
                recur--;
            }
            else {
                try {
                    recur++;
                    plugin.setup(plugin, imports, register);
                } catch (e) {
                    e.plugin = plugin;
                    app.emit("error", e);
                    throw e;
                } finally {
                    while (callnext && recur <= 1) {
                        callnext = false;
                        startPlugins(additional);
                    }
                    recur--;
                }
            }

            function register(err, provided) {
                if (err) {
                    return app.emit("error", err);
                }
                plugin.provides.forEach(function(name) {
                    if (!provided.hasOwnProperty(name)) {
                        var err = new Error("Plugin failed to provide " + name + " service. " + JSON.stringify(plugin));
                        err.plugin = plugin;
                        return app.emit("error", err);
                    }

                    services[name] = provided[name];
                    app.pluginToPackage[name] = {
                        path: plugin.packagePath,
                        package: packageName,
                        version: plugin.version,
                        isAdditionalMode: isAdditionalMode
                    };
                    app.packages[packageName].push(name);
                    app.emit("service", name, services[name], plugin);
                });
                if (provided && provided.hasOwnProperty("onDestroy"))
                    destructors.push(provided.onDestroy);

                app.emit("plugin", plugin);

                if (recur) return (callnext = true);
                startPlugins(additional);
            }
        }

        // Give createApp some time to subscribe to our "ready" event
        (typeof process === "object" ? process.nextTick : setTimeout)(startPlugins);

        this.loadAdditionalPlugins = function(additionalConfig, callback) {
            isAdditionalMode = true;

            exports.resolveConfig(additionalConfig, function (err, additionalConfig) {
                if (err) return callback(err);

                app.once(ready ? "ready-additional" : "ready", function (app) {
                    callback(null, app);
                }); // What about error state?

                // Check the config - hopefully this works
                var _sortedPlugins = checkConfig(additionalConfig, function (name) {
                    return services[name];
                });

                if (ready) {
                    sortedPlugins = _sortedPlugins;
                    // Start Loading additional plugins
                    startPlugins(true);
                }
                else {
                    _sortedPlugins.forEach(function (item) {
                        sortedPlugins.push(item);
                    });
                }
            });
        };

        this.destroy = function () {
            destructors.forEach(function (destroy) {
                destroy();
            });

            destructors = [];
        };
    }

    Architect.prototype = Object.create(EventEmitter.prototype, {constructor: {value: Architect}});
    Architect.prototype.getService = function (name) {
        if (!this.services[name]) {
            throw new Error("Service '" + name + "' not found in architect app!");
        }
        return this.services[name];
    };

    // Returns an event emitter that represents the app.  It can emit events.
    // event: ("service" name, service) emitted when a service is ready to be consumed.
    // event: ("plugin", plugin) emitted when a plugin registers.
    // event: ("ready", app) emitted when all plugins are ready.
    // event: ("error", err) emitted when something goes wrong.
    // app.services - a hash of all the services in this app
    // app.config - the plugin config that was passed in.
    function createApp(config, callback) {
        var app;
        try {
            app = new Architect(config);
        } catch (err) {
            if (!callback) throw err;
            return callback(err, app);
        }
        if (callback) {
            app.on("error", done);
            app.on("ready", onReady);
        }
        return app;

        function onReady(app) {
            done();
        }

        function done(err) {
            if (err) {
                app.destroy();
            }
            app.removeListener("error", done);
            app.removeListener("ready", onReady);
            callback(err, app);
        }

        return app;
    }

    var exports = {};
    exports.loadConfig = loadConfig;
    exports.resolveConfig = resolveConfig;
    exports.createApp = createApp;
    exports.Architect = Architect;
    return exports;
})