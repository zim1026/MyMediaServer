namespace MediaLibrary
{
    using System;

    public class SearchCriteria
    {
        public SearchCriteria()
        {
            ArtistSearchType = SearchType.Contains;
            AlbumSearchType = SearchType.Contains;
            SongSearchType = SearchType.Contains;
            GenreSearchType = SearchType.Contains;
        }

        public bool IsEmpty
        {
            get
            {
                return
                    string.IsNullOrWhiteSpace(ArtistName) &&
                    string.IsNullOrWhiteSpace(AlbumName) &&
                    string.IsNullOrWhiteSpace(SongTitle) &&
                    string.IsNullOrWhiteSpace(Genre) &&
                    !CreateDate.HasValue;
            }
        }
        public Nullable<DateTime> CreateDate { get; set; }
        
        public string ArtistName { get; set; }
        public SearchType ArtistSearchType { get; set; }
        
        public string AlbumName { get; set; }
        public SearchType AlbumSearchType { get; set; }
        
        public string SongTitle { get; set; }
        public SearchType SongSearchType { get; set; }
        
        public string Genre { get; set; }
        public SearchType GenreSearchType { get; set; }
        
        public enum SearchType
        {
            Exact,
            Contains,
            StartsWith,
            EndsWith
        }
    }
}
