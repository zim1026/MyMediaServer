
namespace ID3TagLibV2 {
    public abstract class Types {
        public enum ID3Fields {
            id3v1Artists,
            id3v1Artist,
            AlbumArtists,
            AlbumArtist,
            Album,
            id3v1Album,
            Title,
            id3v1Title,
            Genres,
            Genre,
            Year,
            Lyrics,
            TrackNum,
            DiscNum,
            Comment,
            Conductor,
            Duration,
            Bitrate,
            SampleRate,
            AlbumArt,
            BackCoverImage,
            ArtistImage,
            BandImage,
            Composers,
            Performers
        }

        public struct ArtCollecton {
            public TagLib.PictureType type;
            public byte[] ImageData;
        }
    }
}
