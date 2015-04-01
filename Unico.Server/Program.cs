using System;
using System.IO;
using Microsoft.Owin.Hosting;
using Mono.Options;

namespace Unico.Server
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            string host = "localhost";
            int port = 9000;
            var p = new OptionSet()
            {
                { "h|host",  v => host = v  },
                { "p|port=",  (int v) => port = v },
            };
            p.Parse(args);
            string url = string.Format("http://{0}:{1}", host, port);
            using (WebApp.Start<Startup>(url)) 
            {
                Console.WriteLine("Server running at {0}", url);
                Console.ReadLine();
            }
        }
    }
}
