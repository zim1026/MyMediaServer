namespace MediaLibrary
{

    public partial class V_SONG_LIST
    {
        public V_SONG_LIST()
        {
            AlbumArtFileName = string.Empty;
            AlbumArtRelativeFileName = string.Empty;
        }

        public string AlbumArtFileName { get; set; }
        public string AlbumArtRelativeFileName { get; set; }
    }
}
