define([
    "dojo/dom",
    "dijit/MenuBar",
    "dijit/PopupMenuBarItem",
    "dijit/Menu",
    "dijit/MenuItem",
    "dijit/DropDownMenu",
    "i18n!nls/resource",
    "dojo/domReady!"
], function(dom, MenuBar, PopupMenuBarItem, Menu, MenuItem, DropDownMenu, resource) {
    main.consumes = ["Plugin", "layout"];
    main.provides = ["menus"];
    return main;

    function main(options, imports, register) {
        var Plugin = imports.Plugin;
        var plugin = new Plugin("Ajax.org", main.consumes);
        var emit = plugin.getEmitter();

        var menuBar;
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
			
            var topPanel = dom.byId("topPanel");
            menuBar = new MenuBar({});
    
    var pSubMenu = new DropDownMenu({});
    pSubMenu.addChild(new MenuItem({
        label: "File item #1"
    }));
    pSubMenu.addChild(new MenuItem({
        label: "File item #2"
    }));
    menuBar.addChild(new PopupMenuBarItem({
        label: resource.file,
        popup: pSubMenu
    }));

    var pSubMenu2 = new DropDownMenu({});
    pSubMenu2.addChild(new MenuItem({
        label: "Cut",
        iconClass: "dijitEditorIcon dijitEditorIconCut"
    }));
    pSubMenu2.addChild(new MenuItem({
        label: "Copy",
        iconClass: "dijitEditorIcon dijitEditorIconCopy"
    }));
    pSubMenu2.addChild(new MenuItem({
        label: "Paste",
        iconClass: "dijitEditorIcon dijitEditorIconPaste"
    }));

    menuBar.addChild(new PopupMenuBarItem({
        label: "Edit",
        popup: pSubMenu2
    }));

            menuBar.placeAt(topPanel);
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
            get menuBar() { return menuBar; }
        });

        register(null, {
            menus: plugin
        });
    }
})
