using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mushi.Handlers
{
    /// <summary>
    /// Summary description for RegisterExternalLoginHandler
    /// </summary>
    public class RegisterExternalLoginHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}