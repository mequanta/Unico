define(['/signalr/hubs'], function() {
    main.consumes = [];
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
