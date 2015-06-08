define(function(require, exports, module) {
    main.consumes = ["Plugin", "core"];
    main.provides = ["settings"];
    return main;

    function main(options, imports, register) {
        var Plugin = imports.Plugin;
        var core = imports.ext;

        var plugin = new Plugin("Ajax.org", main.consumes);
        var emit = plugin.getEmitter();
         
        // Give the core plugin a reference to settings      
        core.settings = plugin;

        register(null, {
            settings: plugin
        });
    }
});

