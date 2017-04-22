namespace Web
{
    using System;
    using System.Web;
    using System.Web.SessionState;
    
    /// <summary>
    /// Does exactly what the name implies, keeps the session alive since my site is Ajaxy
    /// </summary>
    public class SessionHeartbeat : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            /* https://seejoelprogram.wordpress.com/2008/11/10/maintaining-aspnet-session-state-in-an-ajax-application/ */
            
            if (context.Session["SessionStartTime"] != null)
            {
                context.Session["SessionStartTime"] = DateTime.Now;
            }
            else
            {
                context.Session.Add("SessionStartTime", DateTime.Now);
            }
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