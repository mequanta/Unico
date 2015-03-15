define([
    "dojo/parser",
    "dojo/ready",
], function(parser, ready) {
    main.consumes = ["core", "Plugin"];
    main.provides = ["layout"];
    return main;

    function main(options, imports, register) {
        var Plugin = imports.Plugin;
        var plugin = new Plugin("Ajax.org", main.consumes);
        var emit = plugin.getEmitter();

        var loaded = false;
        function load() {
            if (loaded) return false;
            loaded = true;
            draw();
        }

        var drawn = false;
        function draw() {
            if (drawn) return;
            drawn = true;
            ready(function(){
                parser.parse();
                emit("draw");
            });
        }
       
        plugin.on("load", function(){
            load();
        });

        plugin.on("enable", function(){
            
        });

        plugin.on("disable", function(){
            
        });

        plugin.on("unload", function(){
            loaded = false;
        });

        plugin.freezePublicAPI({
        });

        register(null, {
            layout: plugin
        });
    }
})
