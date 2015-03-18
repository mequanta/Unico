define([
    "dojo/dom",
    "dojo/domReady!"
], function(require, exports, module) {
    main.consumes = ["Plugin", "panels"];
    main.provides = ["panel.project"];
    return main;
    
    function main(options, imports, register) {
        var Plugin = imports.Plugin;

        var plugin = new Plugin("unico.org", main.consumes);
        
        var loaded = false;
        function load() {
            if (loaded) return false;
            loaded = true;
        }

        var drawn = false;
        function draw() {
            if (drawn) return;
            drawn = true;
            
        }

        register(null, {
            "panel.project": plugin
        });
    }
})
