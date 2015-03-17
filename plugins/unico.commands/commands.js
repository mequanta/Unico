define([
    "ace/commands/command_manager",
    "ace/lib/keys",
    "ace/keyboard/keybinding",
    "ace/lib/lang",
    "ace/lib/event"
], function(cm, keyUtil, kb, lang, event) {
    main.consumes = ["core", "Plugin"];
    main.provides = ["commands"];
    return main;

    function main(options, imports, register) {
        var Plugin = imports.Plugin;
		var CommandManager = cm.CommandManager
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
			
			var kb = new KeyBinding({ commands: commandManager, fake: true });
		    event.addCommandKeyListener(document.documentElement, kb.onCommandKey.bind(kb));
            event.addListener(document.documentElement, "keyup", function(e) {
                if (e.keyCode === 18) // do not trigger browser menu on windows
                    e.preventDefault();
            });
        }

        function addCommand(command, hostPlugin, asDefault) {
		    if (!command.name) return console.error("trying to add a command without name", command);	
        }
        function addCommands(list, hostPlugin, asDefault) {
        }
        function removeCommands(commands) {
            Object.keys(commands).forEach(function(name) {
                removeCommand(commands[name]);
            });
        }
        function removeCommand(command, context, clean) {
			if (!command) return;
		    var name = typeof command === 'string' ? command : command.name;

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
			 /**
             * A hash table of all the commands. The index is the name of the
             * command.
             * 
             * See {@link #addCommand} for a description of the command object.
             * 
             * @property {Object[]} commands
             * @readonly
             */
            get commands() { return commands; },
            
            /**
             * The operating system that is being run.
             * @property {String} platform  Possible values are "mac", "win".
             * @readonly
             */
            get platform(){ return platform; },
			
		    /**
             * Adds a command to the list of available commands.
             * 
             * @param {Object}   command                The command definition 
             *   to add.
             * @param {String}   command.name           The name of this command
             * @param {Object}   [command.bindKey]      Object containing an entry 
             *   for each platform. The (modifier) keys are space or dash (-) 
             *   separated. Special named keys are: 
             * 
             * Ctrl, Command, Alt, Option, Shift, Meta, Tab, Esc, Enter, F1-F12, 
             * Up, Down, Left, Right, PgUp, PgDown, Home, End
             * 
             * Example:
             * 
             *     bindKey : { mac: "Command-Option-Z", win: "Ctrl-Alt-Z" }
             * 
             * @param {String}   [command.bindKey.win]  The bind key for windows 
             *   and unix.
             * @param {String}   [command.bindKey.mac]  The bind key for mac. 
             * @param {String}   [command.hint]         A description of this 
             *   command. This is displayed in the key bindings editor.
             * @param {String}   [command.group]        The group to which this 
             *   command belongs. This is used by the key bindings editor to 
             *   group the commands.This function should return true when the 
             *   command is available and otherwise return false. Make sure that 
             *   you implement this to be as exact as possible.
             * @param {Function} [command.isAvailable]  This function should 
             *   return true when the command is available and otherwise return 
             *   false. Make sure that you implement this to be as exact as 
             *   possible.
             * @param {Function} command.exec           This function is called 
             *   when the command is triggered for execution.
             * @param {Plugin}   plugin   The plugin responsible for adding the 
             *   command.
             * @return {Object}                         The  command definition
             */
            addCommand: addCommand,
            
            /**
             * Adds multiple commands to the list of available commands.
             * @param {Object[]} list    The list of commands to add. 
             *   See {@link #addCommand} for a description of the object 
             *   definition.
             * @param {Plugin}   plugin  The plugin responsible for adding the 
             *   commands.
             */
            addCommands: addCommands,
            
            /**
             * Remove multiple commands from the list of available commans.
             * @param {String[]} list    The list of names of commands to remove. 
             */
            removeCommands: removeCommands,
            
            /**
             * Remove a command from the list of available commands
             * @param {String} name  the name of the command to remove.
             */
            removeCommand: removeCommand,
        });

        register(null, {
            commands: plugin
        });
    }
})
