define(function(require, exports, module) {
    main.consumes = ["Plugin", "layout"];
    main.provides = ["menus"];
    return main;

    function main(options, imports, register) {
        var Plugin = imports.Plugin;
        var plugin = new Plugin("Ajax.org", main.consumes);
        var emit = plugin.getEmitter();

		var items = {};
        var menus = {};
		var inited = false;
		
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
			
			emit("draw");
        }
       
	           
        function init() {
            inited = true;
			
			emit.sticky("ready");
		}
		
	    function setRootMenu(name, index, item, menu, plugin) {
		}

		function setSubMenu(parent, name, index, item, menu, plugin, force) {
		}
		
		function setMenuItem(parent, name, menuItem, index, item, plugin) {
		}
		
		function getMenuId(path) {
            var menu = menus[path];
            return menu && menu.id;
        }
		
		function enableItem(path) {
			
		}
		
		function disableItem(path) {
		}
		
		function remove(path) {
		}
		
		function get(path) {
            return {
                item: items[path],
                menu: menus[path]
            };
        }
		
		function addItemByPath(path, menuItem, index, menu, plugin) {
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
            menus: plugin
        });
    }
})
