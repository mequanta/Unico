
require.config({
    i18n: {
        locale: 'zh-cn'
    },
	shim: {
	   'jquery.signalR': ['jquery'],
		'/signalr/hubs': ['jquery.signalR']
	}, 
    paths: {
	    "dojo": "static/lib/dojo",
	    "dijit": "static/lib/dijit",
	 	"dojox": "static/lib/dojox",
		"unico.core/architect": "static/plugins/unico.core/architect",
        "jquery": "static/lib/jquery/jquery.min",
		"jquery.signalR": "static/lib/signalr/jquery.signalR.min",
		"EventEmitter2": "static/lib/eventemitter2/eventemitter2",
        "events": "/static/plugins/unico.core/events",
        "i18n": "static/lib/requirejs-i18n/i18n",
		"ace": "/static/lib/ace/lib/ace",
		"text": "/static/lib/text/text",

		"unico.core": "/static/plugins/unico.core",
        "unico.commands": "/static/plugins/unico.commands",
        "unico.ide.ui": "/static/plugins/unico.ide.ui",
        "unico.ide.layout": "/static/plugins/unico.ide.layout",
        "unico.signalr": "/static/plugins/unico.signalr"
    },
    map: {
        '*': {
            'css': 'require-css/css',
            'less': 'require-less/less'
        }
    },
	waitSeconds: 0
})

require(["unico.core/architect"], function (architect) {
    var plugins = window.plugins;
    var start = Date.now();
    plugins.push({
        consumes: [],
        provides: ["auth.bootstrap"],
        setup: function (options, imports, register) {
            register(null, {
                "auth.bootstrap": {
                    login: function(callback) {
                        callback();
                    }
                }
            });
        }
    });
    architect.resolveConfig(plugins, function(err, config) {
        if (err) throw err;

        var errored;
        var app = architect.createApp(config, function(err, app) {
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
            if (!plugin.name)
                plugin.name = name;
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


