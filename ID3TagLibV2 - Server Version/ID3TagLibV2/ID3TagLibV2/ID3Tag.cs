namespace ID3TagLibV2 {
    using System;

    public class ID3Tag:IDisposable {
        #region Privates
        private static readonly byte[] empty = new byte[0];
        private string[] _albumArtists;
        private string[] _artists;
        private string[] _composers;
        private string[] _genres;
        private string[] _id3v1artists;
        private string[] _performers;
        private string _id3v1Album;
        private string _id3v1Title;
        #endregion Privates

        public ID3Tag() {
            AlbumArtists = new string[1] { string.Empty };
            Album = string.Empty;
            Title = string.Empty;
            Artists = new string[1] { string.Empty };

            id3v1Artists = new string[1] { string.Empty };
            id3v1Album = string.Empty;
            id3v1Title = string.Empty;

            Performers = new string[1] { string.Empty };
            Composers = new string[1] { string.Empty };
            Genres = new string[1] { string.Empty };
            Lyrics = string.Empty;
            Comment = string.Empty;
            Conductor = string.Empty;
            
            AlbumArt = new byte[]{};
            BackCoverImage = new byte[] { };
            ArtistImage = new byte[] { };
            BandImage = new byte[] { };

            FileName = string.Empty;

            Copyright = string.Empty;

            Year = 0;
            TrackNum = 0;
            DiscNum = 0;
        }
        
        public string[] id3v1Artists {
            /*
            get { id3v1StringArray(ref _id3v1artists);
                  return _id3v1artists; }
            */
            get { return _id3v1artists.Length == 0 ? id3v1StringArray(ref _albumArtists) : id3v1StringArray(ref _id3v1artists); }
            set { _id3v1artists = ( (value == null || value.Length == 0 )? new string[1] {string.Empty} : value); }
        }

        public string id3v1Artist { 
            get { return id3v1String(id3v1Artists[0]).Length == 0 ? id3v1String(AlbumArtist) : id3v1String(id3v1Artists[0]); }
            set { id3v1Artists[0] = id3v1String(value); }
        }
        
        public string[] AlbumArtists { 
            get { return _albumArtists; }
            set { _albumArtists = ( (value == null || value.Length == 0)? new string[1] {string.Empty} : value); }
        }
        public string AlbumArtist { get { return AlbumArtists[0]; } set { AlbumArtists[0] = value; } }

        public string[] Artists
        {
            get { return _artists; }
            set { _artists = ((value == null || value.Length == 0) ? new string[1] { string.Empty } : value); }
        }
        public string Artist { get { return Artists[0]; }  set { Artists[0] = value; } }
        
        public string Album { get; set; }
        public string id3v1Album {
            get { return id3v1String(_id3v1Album).Length == 0 ? id3v1String(Album) : id3v1String(_id3v1Album); }
            set { _id3v1Album = id3v1String(value); }
        }
        
        public string Title { get; set; }
        public string id3v1Title {
            get { return id3v1String(_id3v1Title).Length == 0 ? id3v1String(Title) : id3v1String(_id3v1Title); }
            set { _id3v1Title = id3v1String(value); }
        }

        public string[] Genres {
            get { return _genres; }
            set { _genres = ( (value == null || value.Length == 0)? new string[1] { string.Empty } : value); }
        }
        public string Genre { get { return Genres[0]; } set { Genres[0] = value; } }

        public uint Year { get; set; }
        public string YearString
        {
            get { return Year.ToString(); }
            set { Year = Convert.ToUInt32(value); }
        }
        public uint TrackNum { get; set; }
        public string TrackNumString
        {
            get { return TrackNum.ToString(); }
            set { TrackNum = Convert.ToUInt32(value); }
        }
        public uint DiscNum { get; set; }
        public string DiscNumString
        {
            get { return DiscNum.ToString(); }
            set { DiscNum = Convert.ToUInt32(value); }
        }

        public string Lyrics { get; set; }
        public string Comment { get; set; }
        public string Conductor { get; set; }

        public string Duration { get; set; }
        public int Bitrate { get; set; }
        public int SampleRate { get; set; }

        public byte[] AlbumArt { get; set; }
        public byte[] BackCoverImage { get; set; }
        public byte[] ArtistImage { get; set; }
        public byte[] BandImage { get; set; }

        public string[] Composers {
            get { return _composers; }
            set { _composers = ((value == null || value.Length == 0 )? new string[1] {string.Empty} : value); }
        }
        public string Composer { get { return Composers[0]; } set { Composers[0] = value; } }

        public string[] Performers {
            get { return _performers; }
            set { _performers = ((value == null || value.Length == 0) ? new string[1] { string.Empty } : value); }
        }
        public string Performer { get { return Performers[0]; } set { Performers[0] = value; } }
        
        public string FileNameOnly {
            get { return !String.IsNullOrEmpty(FileName) ? System.IO.Path.GetFileName(FileName) : string.Empty; }
        }
        public string FileName { get; set; }

        public static string id3v1String(string value) {
            if(!string.IsNullOrWhiteSpace(value))
                if(value.Length > 30)
                    return value.Substring(0, 30);
                else
                    return value;
            else
                return String.Empty;
        }

        public static string[] id3v1StringArray(string[] value) {
            string[] _v1Array = new string[value.Length];

            for(int i = 0;i < value.Length;i++)
                _v1Array[i] = id3v1String(value[i]);

            return _v1Array;
        }
        
        public static string[] id3v1StringArray(ref string[] value) {
            string[] _v1Array = new string[value.Length];

            for(int i = 0;i < value.Length;i++)
                _v1Array[i] = id3v1String(value[i]);

            return _v1Array;
        }

        public string Copyright { get; set; }
        
        /*
        public static void id3v1StringArray(ref string[] value) {
            for(int x = 0;x < value.Length;x++)
                value[x] = id3v1String(value[x]);
        }
        */

        void IDisposable.Dispose() {
            AlbumArt = empty;
            BackCoverImage = empty;
            ArtistImage = empty;
            BandImage = empty;
        }
    }
}
