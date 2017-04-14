namespace MediaLibrary
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;

    public static class SongManager
    {
        public static decimal ExistingSong(decimal albumId, string title)
        {
            decimal songId = -1;

            if (albumId > 0 && !string.IsNullOrWhiteSpace(title))
            {
                using (var ctx = NewSongContext())
                {
                    SONG currentSong = ctx.SONGS.Where
                        (
                            l =>
                            l.ALBUM_ID == albumId &&
                            l.SONG_TITLE.ToUpper() == title.ToUpper()
                        ).FirstOrDefault();

                    if (currentSong != null)
                    {
                        songId = currentSong.SONG_ID;
                    }
                }
            }
            return songId;
        }

        public static SONG Save(SONG song)
        {
            if (song == null)
                throw new ArgumentNullException("song");

            using (var ctx = NewSongContext())
            {

                if (song.SONG_ID == 0)
                {
                    ctx.SONGS.Add(song);
                }
                else
                {
                    SONG currentSong = ctx.SONGS.Where(l => l.SONG_ID == song.SONG_ID).FirstOrDefault();

                    //currentSong.CREATE_DATE = song.CREATE_DATE;
                    currentSong.UPDATE_DATE = DateTime.Now;
                    currentSong.ARTIST_ID = song.ARTIST_ID;
                    currentSong.ALBUM_ID = song.ALBUM_ID;
                    currentSong.ABS_FILE_PATH = song.ABS_FILE_PATH;
                    currentSong.ALBUM_ART = song.ALBUM_ART;
                    currentSong.ALBUM_ART_FLAG = song.ALBUM_ART_FLAG;
                    currentSong.ARTIST_IMAGE = song.ARTIST_IMAGE;
                    currentSong.BACK_COVER = song.BACK_COVER;
                    currentSong.BAND = song.BAND;
                    currentSong.BITRATE = song.BITRATE;
                    currentSong.COMMENTS = song.COMMENTS;
                    currentSong.COMPOSER = song.COMPOSER;
                    currentSong.CONDUCTOR = song.CONDUCTOR;
                    currentSong.DISC_NUM = song.DISC_NUM;
                    currentSong.DOWNLOAD_COUNT = song.DOWNLOAD_COUNT;
                    currentSong.DURATION = song.DURATION;
                    currentSong.FAVORITES_FLAG = song.FAVORITES_FLAG;
                    currentSong.FILENAME = song.FILENAME;
                    currentSong.FILE_SIZE = song.FILE_SIZE;
                    currentSong.GENRE = song.GENRE;
                    currentSong.ID3V1_ALBUM = song.ID3V1_ALBUM;
                    currentSong.ID3V1_ARTIST = song.ID3V1_ARTIST;
                    currentSong.ID3V1_FLAG = song.ID3V1_FLAG;
                    currentSong.ID3V1_TITLE = song.ID3V1_TITLE;
                    currentSong.ID3V2_ALBUM = song.ID3V2_ALBUM;
                    currentSong.ID3V2_ARTIST = song.ID3V2_ARTIST;
                    currentSong.ID3V2_FLAG = song.ID3V2_FLAG;
                    currentSong.ID3V2_TITLE = song.ID3V2_TITLE;
                    currentSong.ITUNES_FLAG = song.ITUNES_FLAG;
                    currentSong.LYRICS = song.LYRICS;
                    currentSong.LYRICS_FLAG = song.LYRICS_FLAG;
                    currentSong.NEW_ALBUM = song.NEW_ALBUM;
                    currentSong.NEW_ARTIST = song.NEW_ARTIST;
                    currentSong.NEW_GENRE = song.NEW_GENRE;
                    currentSong.NEW_TITLE = song.NEW_TITLE;
                    currentSong.NEW_TRACK_NUM = song.NEW_TRACK_NUM;
                    currentSong.OLD_ART = song.OLD_ART;
                    currentSong.PLAY_COUNT = song.PLAY_COUNT;
                    currentSong.RECOMMEND_FLAG = song.RECOMMEND_FLAG;
                    currentSong.REL_FILE_PATH = song.REL_FILE_PATH;
                    currentSong.SAMPLE_RATE = song.SAMPLE_RATE;
                    currentSong.SONG_TITLE = song.SONG_TITLE;
                    currentSong.TRACK_NUM = song.TRACK_NUM;
                    currentSong.UPDATE_FILE_FLAG = song.UPDATE_FILE_FLAG;
                    currentSong.UPDATE_TAGS_FLAG = song.UPDATE_TAGS_FLAG;
                    currentSong.VIDEO_PATH = song.VIDEO_PATH;
                    currentSong.YEAR = song.YEAR;
                }

                ctx.SaveChanges();

                return ctx.SONGS
                    .Where(q => q.SONG_ID == song.SONG_ID)
                    .FirstOrDefault();
            }
        }

        public static SONG GetSong(decimal song_Id)
        {
            using (var ctx = NewSongContext())
            {
                return ctx.SONGS
                    .Where(q => q.SONG_ID == song_Id)
                    .FirstOrDefault();
            }
        }

        public static Collection<SONG> GetSongs(decimal album_Id)
        {
            using (var ctx = NewSongContext())
            {
                return new Collection<SONG>
                    (
                        ctx.SONGS
                            .Where(q => q.ALBUM_ID == album_Id)
                            .ToList()
                    );
            }
        }

        private static Entities NewSongContext()
        {
            return new Entities();
        }
    }
}
