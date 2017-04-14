namespace MediaLibrary
{
    public class SearchSummary
    {
        public SearchSummary()
        {
            ArtistCount = 0;
            AlbumCount = 0;
            SongCount = 0;
            GenreCount = 0;
        }

        public int ArtistCount { get; set; }
        public int AlbumCount { get; set; }
        public int SongCount { get; set; }
        public int GenreCount { get; set; }
    }
}
