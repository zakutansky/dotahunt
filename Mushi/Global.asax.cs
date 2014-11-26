using System;
using System.Web;
using Mushi.Context;

namespace Mushi
{
    public class Global : HttpApplication
    {
        protected void Application_Start(Object sender, EventArgs e)
        {
            Steam.RequestCounter.Run();
        }

        protected void Application_End(Object sender, EventArgs e)
        {
            Steam.RequestCounter.Flush();
        }

        void Application_Error(object sender, EventArgs e)
        {
            // Get last error from the server
            var exc = Server.GetLastError();

            if (exc.InnerException != null)
            {
                exc = new Exception(exc.InnerException.Message);
            }
            Server.Transfer("Sites/ErrorPage.aspx?handler=Application_Error%20-%20Global.asax",
                 true);
        }
    }
}