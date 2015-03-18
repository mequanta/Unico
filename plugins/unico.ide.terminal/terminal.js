define(function() {
    main.consumes = ["core", "Plugin", "signalr"];
    main.provides = ["terminal"];
    return main;

    function main(options, imports, register) {
        var Plugin = imports.Plugin;
        var connection = imports.signalr.connection;
        
        var plugin = new Plugin("Ajax.org", main.consumes);
        var emit = plugin.getEmitter();

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
            terminal: plugin
        });
    }
})
