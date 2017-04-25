namespace MediaLibrary
{
    using System;
    
    public class SearchSummary
    {
        public SearchSummary()
        {
            ArtistCount = 0;
            AlbumCount = 0;
            SongCount = 0;
            GenreCount = 0;
            NewestSongDate = string.Empty;
        }

        public int ArtistCount { get; set; }
        public int AlbumCount { get; set; }
        public int SongCount { get; set; }
        public int GenreCount { get; set; }
        public string NewestSongDate { get; set; }
    }
}
