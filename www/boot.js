require(["unico.core/architect"], function (architect) {
    var plugins = window.plugins;
    var start = Date.now();
    plugins.push({
        consumes: [],
        provides: ["auth.bootstrap"],
        setup: function (options, imports, register) {
            register(null, {
                "auth.bootstrap": {
                    login: function (callback) {
                        callback();
                    }
                }
            });
        }
    });
    architect.resolveConfig(plugins, function(err, config) {
        if (err) throw err;

        var errored;
        var app = architect.createApp(config, function (err, app) {
            if (err) {
                errored = true;
                console.error(err.stack);
                alert(err);
            }
        });

        app.lut = {};

        app.on("error", function (err) {
            console.error(err.stack);
            if (!errored)
                alert(err);
        });

        app.on("service", function(name, plugin, options) {
            if (name == "plugin.loader" || name == "plugin.installer" || name == "plugin.debug" || name == "plugin.manager")
                plugin.architect = app;
            if (!plugin.name)
                plugin.name = name;
            if (options)
                app.lut[(options.packagePath || "").replace(/^.*\/home\/.c9\//, "")] = options;
        });

        app.on("ready", function () {
            if (app.services.configure)
                app.services.configure.services = app.services;

            window.app = app.services;
            Object.keys(window.app).forEach(function (n) {
                if (/[^\w]/.test(n))
                    window.app[n.replace(/[^\w]/, "_") + "_"] = window.app[n];
            });

            done();
        });

        // For Development only
        function done() {
            //app.services.core.ready();
            //app.services.core.totalLoadTime = Date.now() - start;
            var end = Date.now();
            console.warn("Total Load Time: ", end - start);
        }
    });
});


