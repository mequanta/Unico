define([
    "dojo/ready",
    "dojo/dom",
    "dojo/dom-construct",
    "text!./layout.html",
    "dojo/parser",
    "dojo/_base/declare",
    "dijit/_WidgetBase",
    "dijit/_TemplatedMixin",
    "dijit/_WidgetsInTemplateMixin",
    "dijit/layout/ContentPane",
    "dijit/layout/BorderContainer",
    "dijit/layout/TabContainer",
    "dijit/layout/AccordionContainer",
    "dijit/layout/AccordionPane"
], function(ready, dom, domConstruct, template, parser, declare, _WidgetBase, _TemplatedMixin, _WidgetsInTemplateMixin) {
    main.consumes = ["core", "Plugin"];
    main.provides = ["layout"];
    return main;

    function main(options, imports, register) {
        var Plugin = imports.Plugin;
        var plugin = new Plugin("Ajax.org", main.consumes);
        var emit = plugin.getEmitter();
   
        var loaded = false;
        function load() {
            if (loaded) return false;
            loaded = true;
            declare("MainWidget",  [_WidgetBase, _TemplatedMixin, _WidgetsInTemplateMixin], {
                templateString: template
            });
            parser.parse();
            draw();
        }

        var drawn = false;
        function draw() {
            if (drawn) return;
            drawn = true;
            ready(function() {
                var widget = new MainWidget({}, domConstruct.create('div'));
                widget.placeAt(dom.byId('layoutContainer'));
                widget.startup();
                emit("draw");
            });
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
            layout: plugin
        });
    }
})
