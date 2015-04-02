using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net.Http;
using Microsoft.Owin.Hosting.Loader;
using Microsoft.Owin.Hosting.Services;
using Microsoft.Owin.Testing;
using Xilium.CefGlue;
using System.Reflection;
using Unico.Server;

namespace Unico.Desktop
{
    public sealed class SimpleSchemeHandlerFactory : CefSchemeHandlerFactory
    {
        protected override CefResourceHandler Create(CefBrowser browser, CefFrame frame, string schemeName, CefRequest request)
        {
            return new SimpleResourceHandler();
        }
    }

    internal sealed class SimpleResourceHandler : CefResourceHandler
    {
        private byte[] responseData;
        private TestServer server;
        private HttpResponseMessage responseMessage;
        private int pos;

        public SimpleResourceHandler()
        {
            var loader = ServicesFactory.Create ().GetService<IAppLoader> ();
            var startup = loader.Load ("Unico.Server.Startup", new List<string>());
            this.server = TestServer.Create (startup);
        }

        protected override bool ProcessRequest(CefRequest request, CefCallback callback)
        {
            var uri = new Uri (request.Url);
            var rb = this.server.CreateRequest (uri.AbsolutePath);
            var headers = request.GetHeaderMap();
            foreach (string key in headers.Keys)
                rb.AddHeader(key, headers[key]);
            this.pos = 0;
            this.responseMessage = rb.SendAsync (request.Method).Result;
            this.responseData = this.responseMessage.Content.ReadAsByteArrayAsync ().Result;
            callback.Continue();
            return true;
        }

        protected override void GetResponseHeaders(CefResponse response, out long responseLength, out string redirectUrl)
        {
            response.Status = (int)this.responseMessage.StatusCode;
            response.StatusText = this.responseMessage.ReasonPhrase;
            var headers = new NameValueCollection(StringComparer.InvariantCultureIgnoreCase);
            foreach (var hdr in this.responseMessage.Content.Headers)
                headers.Set(hdr.Key, hdr.Value.FirstOrDefault<string>());
            response.SetHeaderMap(headers);
            responseLength = this.responseData.LongLength;
            redirectUrl = null;
        }

        protected override bool ReadResponse(Stream response, int bytesToRead, out int bytesRead, CefCallback callback)
        {
            if (bytesToRead == 0 || this.pos >= this.responseData.Length)
            {
                bytesRead = 0;
                return false;
            }
            else
            {
                response.Write(this.responseData, this.pos, bytesToRead);
                this.pos += bytesToRead;
                bytesRead = bytesToRead;
                return true;
            }
        }

        protected override bool CanGetCookie(CefCookie cookie)
        {
            return false;
        }

        protected override bool CanSetCookie(CefCookie cookie)
        {
            return false;
        }

        protected override void Cancel()
        {
        }
    }
}
