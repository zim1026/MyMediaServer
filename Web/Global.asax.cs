namespace Web
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Web;
    using System.Web.Configuration;
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

        protected void Application_End(object sender, EventArgs e)
        {
            EncryptWebConfig();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            EncryptWebConfig();
        }

        protected void Session_Start(Object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["SessionStartTime"] != null)
            {
                HttpContext.Current.Session.Remove("SessionStartTime");
            }
            HttpContext.Current.Session.Add("SessionStartTime", DateTime.Now);

            foreach (FileInfo file in new DirectoryInfo(HttpContext.Current.Server.MapPath("~/Images")).GetFiles("tmp*.jpg"))
            {
                file.Delete();
            }
        }

        internal static void DecryptWebConfig(bool saveFile)
        {
            Configuration webConfig = WebConfigurationManager.OpenWebConfiguration("~/");
            ConfigurationSection webConfigSection = webConfig.GetSection("connectionStrings");

            if (webConfigSection.SectionInformation.IsProtected)
            {
                webConfigSection.SectionInformation.UnprotectSection();
                if (saveFile)
                {
                    string source = HttpContext.Current.Server.MapPath("~/") + "web.config";
                    string dest = HttpContext.Current.Server.MapPath("~/App_Data") + Path.DirectorySeparatorChar + "web.config.bak";

                    webConfig.Save();
                    File.Copy(source, dest, true);
                }
            }
        }

        private void EncryptWebConfig()
        {
            Configuration webConfig = WebConfigurationManager.OpenWebConfiguration("~/");
            ConfigurationSection webConfigSection = webConfig.GetSection("connectionStrings");

            if (!webConfigSection.SectionInformation.IsProtected)
            {
                webConfigSection.SectionInformation.ProtectSection("RSAProtectedConfigurationProvider");
                webConfig.Save();
            }
        }
    }
}