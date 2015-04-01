using Microsoft.AspNet.SignalR;
using System.IO;

namespace Unico.Server.SignalR.Hubs
{
    public class FileSystemHub : Hub
    {
        public void GetFileTree(string dir)
        {
            var entires = Directory.GetFileSystemEntries(dir, "", SearchOption.AllDirectories);

        }
    }
}
