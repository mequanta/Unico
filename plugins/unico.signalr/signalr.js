define(['/signalr/hubs'], function() {
    main.consumes = ["core"];
    main.provides = ["signalr"];
    return main;

    function main(options, imports, register) {
        var plugin = {};
        
        plugin.connection = {}
        register(null, {
            signalr: plugin
        });
    }
})
