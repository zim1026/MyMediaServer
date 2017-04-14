using System;
using System.Drawing;
using System.IO;

namespace ID3TagLibV2 {
    public abstract class Helpers:Types {
        public static string GetMIMEType(string Extension) {
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(Extension);
            if(regKey == null)
                return "image/unknown";
            else
                return regKey.GetValue("Content Type").ToString();
        }

        public static string GetMIMEType(byte[] image) {
            using(MemoryStream ms = new MemoryStream(image)) {
                using(System.Drawing.Image i = System.Drawing.Image.FromStream(ms)) {
                    return GetMIMEType(i);
                }
                // ms.Close();
            }
        }
        
        public static string GetMIMEType(System.Drawing.Image image) {
            foreach(System.Drawing.Imaging.ImageCodecInfo codec in System.Drawing.Imaging.ImageCodecInfo.GetImageDecoders()) {
                if(codec.FormatID == image.RawFormat.Guid)
                    return codec.MimeType;
            }

            return "image/unknown";
        }

        public static string GetFileExtension(string value) {
            return System.IO.Path.GetExtension(value);
        }

        public static string GetFileName(string value) {
            return System.IO.Path.GetFileName(value);
        }

        public static string GetFileNameNoExtension(string value) {
            return System.IO.Path.GetFileNameWithoutExtension(value);
        }

        public static string GetTemporaryFileName() {
            return GetFileName(System.IO.Path.GetTempFileName());
        }

        public static string CreateImageFile(byte[] value, string PhysicalDestinationPath) {
            return CreateImageFile(value, PhysicalDestinationPath, false, 0, 0);
        }

        public static string CreateImageFile(byte[] value,
                                             string PhysicalDestinationPath,
                                             bool ResizeImage,
                                             int MaxWidth, int MaxHeight) {
            if(!PhysicalDestinationPath.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
                PhysicalDestinationPath = PhysicalDestinationPath + System.IO.Path.DirectorySeparatorChar;

            string ImageFile = (PhysicalDestinationPath + GetTemporaryFileName()).Replace(".tmp", ".jpg");
            using(System.IO.MemoryStream ms = new System.IO.MemoryStream(value, 0, value.Length)) {
                if(ValidImage(value)) {
                    using(System.Drawing.Image img = System.Drawing.Image.FromStream(ms)) {
                        if(ResizeImage)
                            CreateScaledImageFile(img, MaxWidth, MaxHeight).Save(ImageFile, System.Drawing.Imaging.ImageFormat.Jpeg);
                        else
                            img.Save(ImageFile, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                }
                ms.Close();
            }
            return ImageFile;
        }

        private static Bitmap CreateScaledImageFile(System.Drawing.Image image, int MaxWidth, int MaxHeight) {
            int width, height;

            if(image.Width > MaxWidth || image.Height > MaxHeight) {
                float givenHeight = image.Height;
                float givenWidth = image.Width;
                float deltaWidth = givenWidth - MaxWidth;
                float deltaHeight = givenHeight - MaxHeight;
                float scaleFactor;

                if(deltaHeight > deltaWidth)
                    scaleFactor = MaxHeight / givenHeight;
                else
                    scaleFactor = MaxWidth / givenWidth;

                width = Convert.ToInt32(Math.Floor(image.Width * scaleFactor));
                height = Convert.ToInt32(Math.Floor(image.Height * scaleFactor));
            }
            else {
                width = image.Width;
                height = image.Height;
            }

            Bitmap bitmap = new Bitmap(width, height);
            using(Graphics g = Graphics.FromImage(bitmap)) {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(image,
                            new Rectangle(Point.Empty, new Size(width, height)),
                            new Rectangle(Point.Empty, new Size(image.Width, image.Height)), GraphicsUnit.Pixel);
            }
            return bitmap;
        }

        public static bool ValidData(byte[] value, bool ValidateImage) {
            bool result = false;

            if(value != null)
                if(value.Length > 0)
                    result = true;
                else
                    result = false;
            else
                result = false;

            if(result && ValidateImage)
                return ValidImage(value);
            else
                return result;
        }

        public static bool ValidImage(byte[] value) {
            try {
                bool result = false;
                using(MemoryStream s = new MemoryStream(value)) {
                    using(System.Drawing.Image i = System.Drawing.Image.FromStream(s)) {
                        result = true;
                    }
                    s.Close();
                }
                return result;
            } catch { 
                return false; 
            }
        }

        public static string[] StringToStringArray(string value) {
            return new string[1] { String.IsNullOrWhiteSpace(value) ? string.Empty:value };
        }
    }
}
