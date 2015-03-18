define(function(require, exports, module) {
    main.consumes = ["core", "Plugin"];
    main.provides = ["panels"];
    return main;

    function main(options, imports, register) {
        var Plugin = imports.Plugin;

        var plugin = new Plugin("Ajax.org", main.consumes);
        var emit = plugin.getEmitter();
        
        var loaded = false;
        function load() {
            if (loaded) return false;
            loaded = true;
            //plugin.on("draw", draw);
        }

        var drawn = false;
        function draw() {
            if (drawn) return;
            drawn = true;
        }

        function registerPanel(panel) {
        }

        function unregisterPanel(panel) {
        }

        function enablePanel(name) {
        }

        function disablePanel(name, noAnim, keep) {
        }

        /***** Lifecycle *****/

        plugin.on("load", function() {
            load();
        });

        plugin.on("enable", function() {

        });

        plugin.on("disable", function() {
        });

        plugin.on("unload", function() {
            loaded = false;
            drawn = false;
        });

        /***** Register and define API *****/
        plugin.freezePublicAPI({
        });

        register(null, {
            panels: plugin
        });
    }
})
