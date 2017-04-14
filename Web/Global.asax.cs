namespace Web
{
    using System;
    using System.IO;
    using System.Web;
    using System.Web.UI;
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            string JQueryVer = "3.1.1";
            ScriptManager.ScriptResourceMapping.AddDefinition("jquery", new ScriptResourceDefinition
            {
                Path = "~/Scripts/jquery-" + JQueryVer + ".min.js",
                DebugPath = "~/Scripts/jquery-" + JQueryVer + ".js",
                CdnPath = "http://ajax.aspnetcdn.com/ajax/jQuery/jquery-" + JQueryVer + ".min.js",
                CdnDebugPath = "http://ajax.aspnetcdn.com/ajax/jQuery/jquery-" + JQueryVer + ".js",
                CdnSupportsSecureConnection = true,
                LoadSuccessExpression = "window.jQuery"
            });
        }

        protected void Session_Start(Object sender, EventArgs e)
        {
            foreach (FileInfo file in new DirectoryInfo(HttpContext.Current.Server.MapPath("~/Images")).GetFiles("tmp*.jpg"))
            {
                file.Delete();
            }
        }
    }
}