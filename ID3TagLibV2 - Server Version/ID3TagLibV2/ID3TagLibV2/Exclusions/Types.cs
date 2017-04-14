using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ID3TagLibV2 {
    public abstract class Types {

        public struct ID3Tag {
            public string Artist;
            public string Album;
            public string Title;
            public string Lyrics;
            public string Genre;
            public string Year;
            public string AlbumArtFilePath;
            public Image AlbumArtImage;
            public byte[] AlbumArtBytes;

            public string TrackNum;
            public string DiscNum;
            public string Comments;
            
            public string[] Artists;
            public string[] Composers;
            public string[] Performers;

            public string Conductor;

            public string v1Artist;
            public string v1Album;
            public string v1Title;

            public string v2Artist;
            public string v2Album;
            public string v2Title;

            public string FilePath;
        }
    }
}
