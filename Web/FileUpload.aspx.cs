namespace Web
{
    using System;
    using System.IO;
    using System.Web;
    using System.Web.Services;
    using System.Web.UI.WebControls;

    public partial class FileUpload : BasePage 
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsAuthorizedUser)
            {
                Response.Redirect("AccessDenied.aspx");
            }

            if (!IsAdmin)
            {
                lstUploadedFiles.Enabled = false;
            }
            
            if (!IsPostBack)
            {
                ListUploadedFiles();
            }
        }

        protected void cmdUpload_Click(object sender, EventArgs e)
        {
            foreach (HttpPostedFile file in FileUploader.PostedFiles)
            {
                try
                {
                    if (Path.GetExtension(file.FileName).ToUpper() == ".MP3")
                    {
                        if (file.FileName.Contains("+"))
                            file.SaveAs(GetUploadPath + Path.DirectorySeparatorChar + Path.GetFileName(file.FileName).Replace("+", "-"));
                        else
                            file.SaveAs(GetUploadPath + Path.DirectorySeparatorChar + Path.GetFileName(file.FileName));
                    }
                }
                catch (Exception ex1)
                {
                    ex1.Source = "File: " + file.FileName + " ---> " + ex1.Source;
                    throw;
                }
            }

            ListUploadedFiles();
        }

        protected void ListUploadedFiles()
        {
            lstUploadedFiles.SelectedIndex = -1;
            lstUploadedFiles.Items.Clear();

            foreach (string file in System.IO.Directory.GetFiles(GetUploadPath, "*.mp3"))
            {
                lstUploadedFiles.Items.Add(file);
            }
        }

        private string GetUploadPath
        {
            get
            {
                return Environment.MachineName != "ROCKNBECIT07121" ? Server.MapPath("~/media/Uploads") : Server.MapPath("~/App_Data");
            }
        }
    }
}