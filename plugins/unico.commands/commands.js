define([
    "ace/commands/command_manager",
    "ace/keyboard/keybinding",
    "ace/lib/keys",
], function(cm, kb, KeyUtil) {
    main.consumes = ["core", "Plugin"];
    main.provides = ["commands"];
    return main;

    function main(options, imports, register) {
        var Plugin = imports.Plugin;
        var CommandManager = cm.CommandManager;
        var KeyBinding = kb.KeyBinding;
        var nav = navigator.platform.toLowerCase();
        var platform = nav.indexOf("mac") > -1 ? "mac" : "win";
        var commandManager = new CommandManager(platform);
        var commands = commandManager.commands;

        var plugin = new Plugin("Ajax.org", main.consumes);
        var emit = plugin.getEmitter();

        var loaded = false;
        function load() {
            if (loaded) return false;
            loaded = true;
            draw();
        }

        function addCommand(command, hostPlugin, asDefault) {
        }
        function addCommands(list, hostPlugin, asDefault) {
        }
        function removeCommands(commands) {
        }
        function removeCommand(command, context, clean) {
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
            commands: plugin
        });
    }
})
