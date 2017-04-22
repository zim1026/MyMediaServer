namespace ID3TagLibV2
{
    using System;
    public static class IllegalChars
    {
        public static char[] IllegalFileNameChars
        {
            get { return System.IO.Path.GetInvalidFileNameChars(); }
        }

        public static char[] IllegalPathChars
        {
            get { return System.IO.Path.GetInvalidPathChars(); }
        }

        public static char[] NonASCII
        {
            get
            {
                char[] _trimChars = new char[65536];
                for (int i = 128; i < 65536; i++)
                {
                    _trimChars[i] = (char)i;
                }
                return _trimChars;
            }
        }

        public static string Legalize(string value)
        {
            if (value != String.Empty && value != null)
            {
                string returnValue = value;

                foreach (char c in IllegalFileNameChars)
                    returnValue = returnValue.Replace(c.ToString(), String.Empty);

                return returnValue.Trim();
            }
            else
                return string.Empty;
        }
    }
}
