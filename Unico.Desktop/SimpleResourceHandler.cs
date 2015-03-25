using System;
using Xilium.CefGlue;
using System.Collections.Generic;
using Microsoft.Owin.Hosting.Services;
using Microsoft.Owin.Hosting.Builder;
using Microsoft.Owin.Hosting.Loader;
using System.Threading.Tasks;
using System.Threading;
using Owin.Types;
using System.Collections.Concurrent;
using System.Net;
using System.IO;
using System.Text;
using System.Collections.Specialized;

namespace Unico.Desktop
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    internal sealed class SimpleSchemeHandlerFactory : CefSchemeHandlerFactory
    {
        protected override CefResourceHandler Create(CefBrowser browser, CefFrame frame, string schemeName, CefRequest request)
        {
            return new SimpleResourceHandler();
        }
    }

    public class SimpleResourceHandler : CefResourceHandler
    {
        private byte[] responseData;
        private int pos;

        AppFunc app;
        private IDictionary<string, object> env;

        public SimpleResourceHandler()
        {
            var loader = ServicesFactory.Create().GetService<IAppLoader>();
            var builder = new AppBuilderFactory().Create();

            var startup = loader.Load(null, new List<string>());
            if (startup != null)
            {
                startup.Invoke(builder);
                app = (AppFunc)builder.Build(typeof(AppFunc));
            }
        }

        protected override bool ProcessRequest(CefRequest request, CefCallback callback)
        {
            env = new Dictionary<string, object>();
            IList<Task> asyncTasks = new List<Task>();
            var uri = new Uri(request.Url);
            env[OwinConstants.CallCancelled] = new CancellationToken();
            IDictionary<string, string[]> reqHeaders = new ConcurrentDictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);
            env[OwinConstants.RequestMethod] = request.Method;
            env[OwinConstants.RequestPathBase] = "";
            env[OwinConstants.RequestPath] = uri.AbsolutePath;
            env[OwinConstants.RequestQueryString] = uri.Query;
            env[OwinConstants.RequestScheme] = uri.Scheme;
            IDictionary<string, string[]> resHeaders = new ConcurrentDictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);
            env[OwinConstants.ResponseHeaders] = resHeaders;
            var headerMap = request.GetHeaderMap();
            foreach (var k in headerMap.AllKeys)
                reqHeaders.Add(k, headerMap.GetValues(k));
            env[OwinConstants.RequestHeaders] = reqHeaders;
            env[OwinConstants.ResponseStatusCode] = (int)HttpStatusCode.OK;

            byte[] body = Encoding.UTF8.GetBytes(request.PostData != null ? request.PostData.ToString() : "");
            env[OwinConstants.RequestBody] = body.Length > 0 ? new MemoryStream(body) : Stream.Null;
            env["owin.ResponseBody"] = new MemoryStream(65536);
            env[OwinConstants.OwinVersion] = "1.0";
            var task = app(env);
            task.Wait();

            var stream = env["owin.ResponseBody"] as MemoryStream;
            //          stream.GetBuffer ();
            //          var buf = new byte[65536] ();
            //          var thiss = new MemoryStream (buf);
            //          stream.CopyTo (thiss);

            responseData = stream.GetBuffer();

            callback.Continue();
            return true;
        }

        protected override void GetResponseHeaders(CefResponse response, out long responseLength, out string redirectUrl)
        {
            response.Status = (int)env[OwinConstants.ResponseStatusCode];
            //response.StatusText = (string)env [OwinConstants.ResponseReasonPhrase];
            var headers = new NameValueCollection(StringComparer.InvariantCultureIgnoreCase);
            var resHeaders = env[OwinConstants.ResponseHeaders] as IDictionary<string, string[]>;
            foreach (var k in resHeaders.Keys)
            {
                string[] values;
                if (!resHeaders.TryGetValue(k, out values))
                    values = new string[] { "" };
                headers.Set(k, values[0]);
            }
            response.SetHeaderMap(headers);

            responseLength = responseData.LongLength;
            redirectUrl = null;
        }

        protected override bool ReadResponse(Stream response, int bytesToRead, out int bytesRead, CefCallback callback)
        {
            if (bytesToRead == 0 || pos >= responseData.Length)
            {
                bytesRead = 0;
                return false;
            }
            else
            {
                response.Write(responseData, pos, bytesToRead);
                pos += bytesToRead;
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
