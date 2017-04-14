namespace MediaLibrary
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;

    public static class AlbumManager
    {
        public static decimal ExistingAlbum(decimal artistId, string albumName)
        {
            decimal albumId = -1;

            if (artistId > 0 && !string.IsNullOrWhiteSpace(albumName))
            {
                using (var ctx = NewAlbumContext())
                {
                    ALBUM currentAlbum = ctx.ALBUMS.Where
                        (
                            l =>
                                l.ARTIST_ID == artistId
                                && l.ALBUM_NAME.ToUpper() == albumName.ToUpper()
                        ).FirstOrDefault();

                    if (currentAlbum != null)
                    {
                        albumId = currentAlbum.ALBUM_ID;
                    }
                }
            }
            return albumId;
        }

        public static ALBUM Save(ALBUM album)
        {
            if (album == null)
                throw new ArgumentNullException("album");

            using (var ctx = NewAlbumContext())
            {

                if (album.ALBUM_ID == 0)
                {
                    ctx.ALBUMS.Add(album);
                }
                else
                {
                    ALBUM currentAlbum = ctx.ALBUMS.Where(l => l.ALBUM_ID == album.ALBUM_ID).FirstOrDefault();

                    //currentAlbum.CREATE_DATE = album.CREATE_DATE;
                    currentAlbum.UPDATE_DATE = DateTime.Now;
                    currentAlbum.ARTIST_ID = album.ARTIST_ID;
                    currentAlbum.ALBUM_NAME = album.ALBUM_NAME;
                }

                ctx.SaveChanges();

                return ctx.ALBUMS
                    .Where(q => q.ALBUM_ID == album.ALBUM_ID)
                    .FirstOrDefault();
            }
        }

        public static ALBUM GetAlbum(decimal album_Id)
        {
            using (var ctx = NewAlbumContext())
            {
                return ctx.ALBUMS
                    .Where(q => q.ALBUM_ID == album_Id)
                    .FirstOrDefault();
            }
        }

        public static Collection<ALBUM> GetAlbums(decimal artist_Id)
        {
            using (var ctx = NewAlbumContext())
            {
                return new Collection<ALBUM>
                    (
                        ctx.ALBUMS
                            .Where(q => q.ARTIST_ID == artist_Id)
                            .OrderBy(a => a.ALBUM_NAME)
                            .ToList()
                    );
            }
        }

        private static Entities NewAlbumContext()
        {
            return new Entities();
        }
    }
}
