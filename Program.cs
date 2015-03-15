using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.StaticFiles;
using Mono.Options;
using Owin;

namespace Unico
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            string host = "localhost";
            int port = 9000;
            string wwwroot = "../../www";
            var p = new OptionSet()
            {
                { "h|host",  v => host = v  },
                { "p|port=",  (int v) => port = v },
                { "w|wwwroot", v => wwwroot = v }
            };
            p.Parse(args);
            string url = string.Format("http://{0}:{1}", host, port);
            var options = new StartOptions(url);
            options.Settings["wwwroot"] = wwwroot;

            using (WebApp.Start(url, app =>
            {
                app.Properties["host.AppName"] = "Mso";
                app.MapSignalR(new HubConfiguration()
                {
                    EnableJavaScriptProxies = true,
                    EnableDetailedErrors = true
                });
                app.UseFileServer(new FileServerOptions()
                {
                    RequestPath = PathString.Empty,
                    FileSystem = new PhysicalFileSystem(wwwroot)
                });

                app.UseFileServer(new FileServerOptions()
                {
                    RequestPath = new PathString("/static/lib"),
                    FileSystem = new PhysicalFileSystem("../../www/lib")
                });
                string PLUGINDIR = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "../../plugins"));
                foreach (var dir in Directory.GetDirectories(PLUGINDIR))
                {
                    var plugin = Path.GetFileName(dir);
                    app.UseFileServer(new FileServerOptions()
                    {
                        RequestPath = new PathString(Path.Combine("/static/plugins", plugin)),
                        FileSystem = new PhysicalFileSystem(Path.Combine("../../plugins", plugin))
                    });
                }
                app.UseErrorPage();
            }))
            {
                Console.WriteLine("Server running at {0}", url);
                Console.ReadLine();
            }
        }
    }
}
