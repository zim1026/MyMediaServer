namespace MediaLibrary
{
    public class AlbumSongUpdate
    {
        public byte[] AlbumArt { get; set; }
        public string Year { get; set; }
        public string Genre { get; set; }
        public string ArtistName { get; set; }
        public string AlbumName { get; set; }
        public string Composer { get; set; }
        public string Performer { get; set; }
        public string Conductor { get; set; }
        public string id3v1Artist { get; set; }
        public string id3v1Album { get; set; }
        public string id3v1Genre { get; set; }
    }
}
