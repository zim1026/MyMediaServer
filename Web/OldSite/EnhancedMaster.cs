namespace MP3Catalog
{
    using System;
    using MP3Catalog.Common;

    public partial class EnhancedMaster : System.Web.UI.MasterPage
    {
        private string _baseURL_Prefix = String.Empty;

        public string BaseURL_Prefix
        {
            get { return _baseURL_Prefix; }
            set { _baseURL_Prefix = value; }
        }

        public void foo()
        {

        }
    }
}