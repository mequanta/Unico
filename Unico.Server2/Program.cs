using System;

namespace Unico.Server
{
    public class Program
    {
        public void Main(string[] args)
        {
            new Microsoft.AspNet.Hosting.Program(null).Main(new[] { "--server", "kestrel" });
        }
    }
}