namespace MP3Catalog
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.Common;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.UI.WebControls;
    using System.Xml.Linq;

    public static class Common
    {
        private const float MAXHEIGHT = 100;
        private const float MAXWIDTH = 100;
        private static Unit _height;
        private static Unit _width;
        private Catalog.Logic _logic = new Catalog.Logic();

        private const string GeoAPI = "http://ipinfodb.com/ip_query.php?ip={0}";
        public struct GeoInfo
        {
            public string Country;
            public string State;
            public string City;
            public string Zip;
            public decimal Latitude;
            public decimal Longitude;
        }

        public static GeoInfo GetGeoInfo(string IPAddress)
        {
            GeoInfo info = new GeoInfo()
            {
                City = string.Empty,
                Country = string.Empty,
                State = string.Empty,
                Zip = string.Empty,
                Latitude = 0,
                Longitude = 0
            };

            if (!string.IsNullOrWhiteSpace(IPAddress))
            {
                if (IPAddress == "::1")
                    IPAddress = "127.0.0.1";

                string APIUrl = string.Format(GeoAPI, IPAddress);
                HttpWebRequest httpWebReq = HttpWebRequest.Create(APIUrl) as HttpWebRequest;
                try
                {
                    string result = string.Empty;
                    HttpWebResponse response = httpWebReq.GetResponse() as HttpWebResponse;
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        result = reader.ReadToEnd();
                        reader.Close();
                    }
                    info = ProcessGeoAPI(result);
                }
                catch (Exception ex)
                {
                    ex.Source = "GetGeoInfo(string)--->" + ex.Source;
                    //throw;
                }
            }
            return info;
        }

        private static GeoInfo ProcessGeoAPI(string APIResponse)
        {
            try
            {
                StringReader reader = new StringReader(APIResponse);
                XElement element = XElement.Load(reader);

                if (string.Compare(((string)element.Element("Status") as String).ToUpper(), "OK", true) != 0)
                    throw new ApplicationException("GeoIP API Invocation Failure");

                return new GeoInfo()
                {
                    City = (string)element.Element("City"),
                    Country = (string)element.Element("CountryName"),
                    State = (string)element.Element("RegionName"),
                    Zip = (string)element.Element("ZipPostalCode"),
                    Latitude = (decimal)element.Element("Latitude"),
                    Longitude = (decimal)element.Element("Longitude")
                };
            }
            catch (Exception ex)
            {
                ex.Source = "ProcessGeoAPI(string)--->" + ex.Source;
                throw;
            }
        }

        public static Unit Height
        {
            get { return _height; }
            set { _height = value; }
        }

        public static Unit Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public static string GetConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["mp3_user"].ConnectionString.ToString(); }
        }

        public static string GetImageFile(object objImage, string serverMappedPath, float mHeight, float mWidth)
        {
            string returnValue = string.Empty;

            try
            {
                if (objImage != null && objImage != DBNull.Value)
                {
                    byte[] image = null;
                    /*
                    using(ASC_DBI.ODP dbUtil = new ASC_DBI.ODP(GetConnectionString)) {
                        using(OracleCommand oCmd = new OracleCommand()) {
                            oCmd.CommandType = CommandType.StoredProcedure;
                            oCmd.CommandText = "pa_admin.f_get_blob";
                            dbUtil.GetStoredProcParameters(oCmd);
                            oCmd.Parameters["in_song_id"].Value = Convert.ToInt32(objImage);

                            dbUtil.ExecuteNonQuery(oCmd);
                            if(oCmd.Parameters["RETURN_VALUE"].Value != DBNull.Value) {
                                image = (byte[])oCmd.Parameters["RETURN_VALUE"].Value;
                            }
                            else {
                                image = new byte[0];
                            }
                        }
                    }
                    */
                    if (image.Length > 0)
                    {
                        string fileName = System.IO.Path.GetFileName(System.IO.Path.GetTempFileName()).ToLower().Replace(".tmp", ".jpg");

                        using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(image, 0, image.Length))
                        {
                            using (System.Drawing.Image img = System.Drawing.Image.FromStream(memoryStream))
                            {

                                if (img.Width > mWidth || img.Height > mHeight)
                                {
                                    float height = img.Height;
                                    float width = img.Width;
                                    float deltaWidth = width - mWidth;
                                    float deltaHeight = height - mHeight;
                                    float scaleFactor;

                                    if (deltaHeight > deltaWidth)
                                    {
                                        scaleFactor = mHeight / height;
                                    }
                                    else
                                    {
                                        scaleFactor = mWidth / width;
                                    }
                                    Width = (Unit)Math.Floor(img.Width * scaleFactor);
                                    Height = (Unit)Math.Floor(img.Height * scaleFactor);
                                }
                                else
                                {
                                    Width = (Unit)img.Width;
                                    Height = (Unit)img.Height;
                                }

                                Bitmap bitmap = new Bitmap((int)Width.Value, (int)Height.Value);
                                using (Graphics g = Graphics.FromImage(bitmap))
                                {
                                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                                    g.DrawImage(img,
                                                new Rectangle(Point.Empty, new Size((int)Width.Value, (int)Height.Value)),
                                                new Rectangle(Point.Empty, new Size(img.Width, img.Height)),
                                                GraphicsUnit.Pixel);
                                }
                                bitmap.Save(serverMappedPath + fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                                returnValue = "~/media/temp_art/" + fileName;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Source = "GetImageFile()--->" + ex.Source;
            }

            return returnValue;
        }

        public static string GetImageFile(object objImage, string serverMappedPath)
        {
            return GetImageFile(objImage, serverMappedPath, true);
        }

        public static string GetImageFile(object objImage, string serverMappedPath, bool resize)
        {
            string returnValue = string.Empty;

            try
            {
                if (objImage != null && objImage != DBNull.Value)
                {
                    byte[] image = null;

                    if (objImage is byte[])
                        image = (byte[])objImage;
                    else
                    {
                        /*
                        using(ASC_DBI.ODP dbUtil = new ASC_DBI.ODP(GetConnectionString)) {
                            using(OracleCommand oCmd = new OracleCommand()) {
                                oCmd.CommandType = CommandType.StoredProcedure;
                                //oCmd.CommandText = "pa_admin.f_get_blob";
                                oCmd.CommandText = "pa_song.f_get_song_image";
                                dbUtil.GetStoredProcParameters(oCmd);
                                oCmd.Parameters["in_song_id"].Value = objImage;
                                oCmd.Parameters["is_image_type"].Value = "ALBUM ART";

                                dbUtil.ExecuteNonQuery(oCmd);
                                if(oCmd.Parameters["RETURN_VALUE"].Value != DBNull.Value) {
                                    image = (byte[])oCmd.Parameters["RETURN_VALUE"].Value;
                                }
                                else {
                                    image = new byte[0];
                                }
                            }
                        }
                        */
                    }

                    if (image.Length > 0)
                    {
                        if (!resize)
                            return "~/media/temp_art/" + System.IO.Path.GetFileName(ID3TagLibV2.Helpers.CreateImageFile(image, serverMappedPath));
                        else
                        {
                            string fileName = System.IO.Path.GetFileName(System.IO.Path.GetTempFileName()).ToLower().Replace(".tmp", ".jpg");

                            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(image, 0, image.Length))
                            {
                                using (System.Drawing.Image img = System.Drawing.Image.FromStream(memoryStream))
                                {

                                    //Calculate web-friendly image size
                                    if (img.Width > MAXWIDTH || img.Height > MAXHEIGHT)
                                    {
                                        float height = img.Height;
                                        float width = img.Width;
                                        float deltaWidth = width - MAXWIDTH;
                                        float deltaHeight = height - MAXHEIGHT;
                                        float scaleFactor;

                                        if (deltaHeight > deltaWidth)
                                        {
                                            scaleFactor = MAXHEIGHT / height;
                                        }
                                        else
                                        {
                                            scaleFactor = MAXWIDTH / width;
                                        }
                                        Width = (Unit)Math.Floor(img.Width * scaleFactor);
                                        Height = (Unit)Math.Floor(img.Height * scaleFactor);
                                    }
                                    else
                                    {
                                        Width = (Unit)img.Width;
                                        Height = (Unit)img.Height;
                                    }

                                    Bitmap bitmap = new Bitmap((int)Width.Value, (int)Height.Value);
                                    using (Graphics g = Graphics.FromImage(bitmap))
                                    {
                                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                                        g.DrawImage(img,
                                                    new Rectangle(Point.Empty, new Size((int)Width.Value, (int)Height.Value)),
                                                    new Rectangle(Point.Empty, new Size(img.Width, img.Height)),
                                                    GraphicsUnit.Pixel);
                                    }
                                    bitmap.Save(serverMappedPath + fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                                    //bitmap.Save(Server.MapPath("~/media/temp_art/"+fileName), System.Drawing.Imaging.ImageFormat.Jpeg);

                                    /*
                                    //rotate 360 to shed any embedded thumbnails (stupid microsoft)
                                    img.RotateFlip(RotateFlipType.Rotate180FlipNone);
                                    img.RotateFlip(RotateFlipType.Rotate180FlipNone);
                                    using(System.Drawing.Image thumb = img.GetThumbnailImage((int)Width.Value, (int)Height.Value, null, IntPtr.Zero)) {
                                        thumb.Save(Server.MapPath("~/media/temp_art/" + fileName), System.Drawing.Imaging.ImageFormat.Jpeg);
                                    }
                                    */

                                    //img.Save(Server.MapPath("~/media/temp_art/" + fileName), System.Drawing.Imaging.ImageFormat.Jpeg);
                                    returnValue = "~/media/temp_art/" + fileName;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Source = "GetImageFile()--->" + ex.Source;
                throw;
                //HandleException(ex);
            }

            return returnValue;
        }

        public static int BoolToInt(bool value)
        {
            if (value)
                return 1;
            else
                return 0;
        }

        public static bool IntToBool(int value)
        {
            if (value == 1)
                return true;
            else
                return false;
        }
    }
}