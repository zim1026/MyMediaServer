namespace MediaLibrary
{
    using System;

    public partial class SONG
    {
        public SONG()
        {
            CREATE_DATE = DateTime.Now;
            ALBUM_ART_FLAG = false;
            FAVORITES_FLAG = false;
            RECOMMEND_FLAG = false;
            ID3V1_FLAG = true;
            ID3V2_FLAG = true;
            UPDATE_FILE_FLAG = false;
            UPDATE_TAGS_FLAG = false;
            LYRICS_FLAG = false;
            ITUNES_FLAG = false;
            PLAY_COUNT = 0;
            DOWNLOAD_COUNT = 0;
        }
    }
}
