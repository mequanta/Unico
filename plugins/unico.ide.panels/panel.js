define(['/signalr/hubs'], function() {
    main.consumes = ["Plugin"];
    main.provides = ["Panel"];
    return main;
    
    function Panel(developer, deps, options) {}
    
    function main(options, imports, register) {
        var plugin = {};
        register(null, {
            Panel: Panel
        });
    }
})
