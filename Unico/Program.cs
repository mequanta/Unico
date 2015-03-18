﻿using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.StaticFiles;
using Mono.Options;
using Owin;
using Newtonsoft.Json.Linq;
using Owin.Routing;
using RazorEngine;
using System.Text;
using System.Collections.Generic;
using System.Web;
using System.Linq;

namespace Unico
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            string homeDir = (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX)
                ? Environment.GetEnvironmentVariable("HOME")
                : Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
            string binDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string svrPluginsDir = Path.Combine(binDir, "Plugins");
            string baseDir = Path.GetFullPath(Path.Combine(binDir, "..", ".."));
            string sysClientPluginsDir = Path.Combine(baseDir, "plugins");
            string userClientPluginsDir = Path.Combine(homeDir, ".unico", "plugins");

            string host = "localhost";
            int port = 9000;
            string wwwroot = Path.GetFullPath(Path.Combine(baseDir, "www"));
            var p = new OptionSet()
            {
                { "h|host",  v => host = v  },
                { "p|port=",  (int v) => port = v },
                { "w|wwwroot", v => wwwroot = v }
            };
            p.Parse(args);
            string url = string.Format("http://{0}:{1}", host, port);

            string configFile = Path.Combine(baseDir, "configs", "default.json");
            var config = JObject.Parse(File.ReadAllText(configFile));
            var pkgs = new List<string>();
            var packagesForLoader = config["plugins"].ToString();
            string workspaceDir = config.Value<string>("workspace");
            workspaceDir = string.IsNullOrEmpty(workspaceDir) ? baseDir : workspaceDir;

            foreach (var plugin in config["plugins"])
            {
                string str = plugin.Type == JTokenType.String ? 
                    string.Format("{{'packagePath':'{0}'}}", plugin) :
                    string.Format("{{'packagePath':'{0}/{1}'}}", plugin["name"], plugin["main"]);
                pkgs.Add(str);
            }
            var packages = string.Join(",", pkgs.ToArray());
            RegisterServerPluginDlls(svrPluginsDir);
            using (WebApp.Start(url, app =>
            {
                app.Properties["host.AppName"] = "Mso";
                app.MapSignalR(new HubConfiguration()
                {
                    EnableJavaScriptProxies = true,
                    EnableDetailedErrors = true
                });

                app.Route("ide.html").Get(async ctx =>
                {
                    var template = File.ReadAllText(Path.Combine(baseDir, "www", "ide.html"));
                    var model = new { Name = "Alex", Packages = packages, PackagesForLoader = packagesForLoader };
                    var result = Razor.Parse(template, model);
                    result = HttpUtility.HtmlDecode(result);
                    await ctx.Response.WriteAsync(result);
                });

                app.UseFileServer(new FileServerOptions()
                {
                    RequestPath = PathString.Empty,
                    FileSystem = new PhysicalFileSystem(wwwroot)
                });

                var wwwLibDir = Path.Combine(wwwroot, "lib");
                // make sure it exists
                Directory.CreateDirectory(wwwLibDir);
                app.UseFileServer(new FileServerOptions()
                {
                    RequestPath = new PathString("/static/lib"),
                    FileSystem = new PhysicalFileSystem(wwwLibDir)
                });

                foreach (var dir in new string[] { sysClientPluginsDir, userClientPluginsDir})
                {
                    if (Directory.Exists(dir))
                    {
                        foreach (var subdir in Directory.GetDirectories(sysClientPluginsDir))
                        {
                    
                            var plugin = Path.GetFileName(subdir);
                            app.UseFileServer(new FileServerOptions()
                            {
                                RequestPath = new PathString("/static/plugins/" + plugin),
                                FileSystem = new PhysicalFileSystem(subdir)
                            });
                        }
                    }
                }
                app.UseErrorPage();
            }))
            {
                Console.WriteLine("Server running at {0}", url);
                Console.ReadLine();
            }
        }

        private static void RegisterServerPluginDlls(string pluginsDir)
        {
            // Find Assemblies ends with "Plugin.Dll"
            if (!Directory.Exists(pluginsDir))
                return;
            foreach (var path in Directory.GetFiles(pluginsDir, "*Plugin.dll", SearchOption.AllDirectories))
            {
                var assembly = Assembly.LoadFrom(path);
                AppDomain.CurrentDomain.Load(assembly.FullName);
            }
        }
    }
}
