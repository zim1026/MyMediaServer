//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MediaLibrary
{
    using System;
    using System.Collections.Generic;
    
    public partial class SONG
    {
        public decimal SONG_ID { get; set; }
        public Nullable<decimal> ARTIST_ID { get; set; }
        public Nullable<decimal> ALBUM_ID { get; set; }
        public string SONG_TITLE { get; set; }
        public byte[] ALBUM_ART { get; set; }
        public string LYRICS { get; set; }
        public string YEAR { get; set; }
        public string GENRE { get; set; }
        public string TRACK_NUM { get; set; }
        public string DISC_NUM { get; set; }
        public bool ALBUM_ART_FLAG { get; set; }
        public bool LYRICS_FLAG { get; set; }
        public bool ITUNES_FLAG { get; set; }
        public bool ID3V1_FLAG { get; set; }
        public string ID3V1_ARTIST { get; set; }
        public string ID3V1_ALBUM { get; set; }
        public string ID3V1_TITLE { get; set; }
        public bool ID3V2_FLAG { get; set; }
        public string ID3V2_ARTIST { get; set; }
        public string ID3V2_ALBUM { get; set; }
        public string ID3V2_TITLE { get; set; }
        public bool UPDATE_TAGS_FLAG { get; set; }
        public string NEW_ALBUM { get; set; }
        public string NEW_TITLE { get; set; }
        public Nullable<decimal> NEW_TRACK_NUM { get; set; }
        public string NEW_GENRE { get; set; }
        public string FILENAME { get; set; }
        public string ABS_FILE_PATH { get; set; }
        public string REL_FILE_PATH { get; set; }
        public System.DateTime CREATE_DATE { get; set; }
        public Nullable<System.DateTime> UPDATE_DATE { get; set; }
        public bool RECOMMEND_FLAG { get; set; }
        public bool UPDATE_FILE_FLAG { get; set; }
        public byte[] OLD_ART { get; set; }
        public bool FAVORITES_FLAG { get; set; }
        public string VIDEO_PATH { get; set; }
        public Nullable<decimal> FILE_SIZE { get; set; }
        public string DURATION { get; set; }
        public Nullable<decimal> SAMPLE_RATE { get; set; }
        public Nullable<decimal> BITRATE { get; set; }
        public byte[] BACK_COVER { get; set; }
        public byte[] ARTIST_IMAGE { get; set; }
        public byte[] BAND { get; set; }
        public decimal PLAY_COUNT { get; set; }
        public decimal DOWNLOAD_COUNT { get; set; }
        public string NEW_ARTIST { get; set; }
        public string COMMENTS { get; set; }
        public string COMPOSER { get; set; }
        public string CONDUCTOR { get; set; }
    }
}
