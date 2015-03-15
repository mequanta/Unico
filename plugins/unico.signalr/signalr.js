define(function() {
    main.consumes = ["core"];
    main.provides = ["signalr"];
    return main;

    function main(options, imports, register) {
        var plugin = {};        
        register(null, {
            signalr: plugin
        });
    }
})
