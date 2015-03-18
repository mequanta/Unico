define(['/signalr/hubs'], function() {
    main.consumes = ["core"];
    main.provides = ["panels"];
    return main;

    function main(options, imports, register) {
        var plugin = {};
        register(null, {
            panels: plugin
        });
    }
})
