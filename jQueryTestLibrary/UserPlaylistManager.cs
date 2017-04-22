namespace MediaLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class UserPlaylistManager
    {
        public static List<USER_PLAYLIST> GetUserPlaylist(decimal userSecurityID)
        {
            using (var ctx = NewUserPlaylistContext())
            {
                return ctx.USER_PLAYLISTS
                    .Include("SONG")
                    .Where(q => q.USER_SECURITY_ID == userSecurityID)
                    .ToList();
            }
        }

        public static USER_PLAYLIST GetUserPlaylistEntry(decimal songID, decimal userSecurityID)
        {
            using (var ctx = NewUserPlaylistContext())
            {
                return ctx.USER_PLAYLISTS
                    .Where(x => x.SONG_ID == songID && x.USER_SECURITY_ID == userSecurityID)
                    .FirstOrDefault();
            }
        }

        public static USER_PLAYLIST Save(USER_PLAYLIST playlist)
        {
            if (playlist == null)
                throw new ArgumentNullException("playlist");

            using (var ctx = NewUserPlaylistContext())
            {
                USER_PLAYLIST pl = ctx.USER_PLAYLISTS
                    .Where(q => q.SONG_ID == playlist.SONG_ID && q.USER_SECURITY_ID == playlist.USER_SECURITY_ID)
                    .FirstOrDefault();

                if (pl == null) // && pl.SONG_ID != playlist.SONG_ID && pl.USER_SECURITY_ID != playlist.USER_SECURITY_ID)
                {
                    ctx.USER_PLAYLISTS.Add(playlist);
                    ctx.SaveChanges();
                }

                return ctx.USER_PLAYLISTS
                    .Include("SONG")
                    .Where(q => q.USER_SECURITY_ID == playlist.USER_SECURITY_ID && q.SONG_ID == playlist.SONG_ID)
                    .FirstOrDefault();
            }
        }

        public static USER_PLAYLIST Remove(USER_PLAYLIST playlist)
        {
            if (playlist == null)
                throw new ArgumentNullException("playlist");

            using (var ctx = NewUserPlaylistContext())
            {
                USER_PLAYLIST pl = ctx.USER_PLAYLISTS
                    .Where(q => q.SONG_ID == playlist.SONG_ID && q.USER_SECURITY_ID == playlist.USER_SECURITY_ID)
                    .FirstOrDefault();

                if (pl != null)
                {
                    ctx.USER_PLAYLISTS.Remove(pl);
                    ctx.SaveChanges();
                }

                return ctx.USER_PLAYLISTS
                    .Include("SONG")
                    .Where(q => q.USER_SECURITY_ID == playlist.USER_SECURITY_ID && q.SONG_ID == playlist.SONG_ID)
                    .FirstOrDefault();
            }
        }

        public static USER_PLAYLIST RemoveSong(decimal songID, decimal userSecurityID)
        {
            USER_PLAYLIST pl = new USER_PLAYLIST();
            pl.SONG_ID = songID;
            pl.USER_SECURITY_ID = userSecurityID;

            return Remove(pl);
        }

        public static USER_PLAYLIST AddSong(decimal songID, decimal userSecurityID)
        {
            USER_PLAYLIST pl = new USER_PLAYLIST();
            pl.SONG_ID = songID;
            pl.USER_SECURITY_ID = userSecurityID;

            return Save(pl);
        }

        private static Entities NewUserPlaylistContext()
        {
            return new Entities();
        }
    }
}
