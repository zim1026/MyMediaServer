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
    
    public partial class V_SONG_LIST
    {
        public decimal ALBUM_ID { get; set; }
        public decimal ARTIST_ID { get; set; }
        public decimal SONG_ID { get; set; }
        public string ALBUM_NAME { get; set; }
        public string ARTIST_NAME { get; set; }
        public string SONG_TITLE { get; set; }
        public byte[] ALBUM_ART { get; set; }
        public bool ALBUM_ART_FLAG { get; set; }
        public string YEAR { get; set; }
        public string GENRE { get; set; }
        public string TRACK_NUM { get; set; }
        public string ABS_FILE_PATH { get; set; }
        public System.DateTime CREATE_DATE { get; set; }
        public Nullable<decimal> FILE_SIZE { get; set; }
        public string DURATION { get; set; }
        public Nullable<decimal> SAMPLE_RATE { get; set; }
        public Nullable<decimal> BITRATE { get; set; }
        public bool LYRICS_FLAG { get; set; }
    }
}