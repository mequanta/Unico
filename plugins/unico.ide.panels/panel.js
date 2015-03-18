define(function(require, exports, module) {
    main.consumes = ["Plugin"];
    main.provides = ["Panel"];
    return main;
    
    function Panel(developer, deps, options) {}
    
    function main(options, imports, register) {
        var Plugin = imports.Plugin;

        function Panel(developer, deps, options) {
            var plugin = new Plugin(developer, deps);
            var emit = plugin.getEmitter();
            

            plugin.on("load", function() {
            }

            plugin.on("unload", function() {
            }

            function enable() {}
            function disable() {}

            plugin.on("enable", function() {
                enable();
            });

            plugin.on("disable", function() {
                disable();
            });
            plugin.freezePublicAPI.baseclass();

            plugin.freezePublicAPI({
            }
            return plugin;
        }

        register(null, {
            Panel: Panel
        });
    }
})
