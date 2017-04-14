namespace MediaLibrary
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;

    public static class ArtistManager
    {
        public static decimal ExistingArtist(string artistName)
        {
            decimal artistId = -1;

            if (!string.IsNullOrWhiteSpace(artistName))
            {
                using (var ctx = NewArtistContext())
                {
                    ARTIST currentArtist = ctx.ARTISTS.Where(l => l.ARTIST_NAME.ToUpper() == artistName.ToUpper()).FirstOrDefault();

                    if (currentArtist != null)
                    {
                        artistId = currentArtist.ARTIST_ID;
                    }
                }
            }

            return artistId;
        }

        public static ARTIST Save(ARTIST artist)
        {
            using (var ctx = NewArtistContext())
            {
                if (artist == null)
                {
                    throw new ArgumentNullException("artist");
                }

                if (artist.ARTIST_ID == 0)
                {
                    ctx.ARTISTS.Add(artist);
                }
                else
                {
                    ARTIST currentArtist = ctx.ARTISTS.Where(l => l.ARTIST_ID == artist.ARTIST_ID).FirstOrDefault();

                    currentArtist.ARTIST_NAME = artist.ARTIST_NAME;
                    //currentArtist.CREATE_DATE = artist.CREATE_DATE;
                    currentArtist.UPDATE_DATE = DateTime.Now;
                }

                ctx.SaveChanges();

                return ctx.ARTISTS
                    .Where(q => q.ARTIST_ID == artist.ARTIST_ID)
                    .FirstOrDefault();
            }
        }

        public static ARTIST GetArtist(decimal artist_Id)
        {
            using (var ctx = NewArtistContext())
            {
                return ctx.ARTISTS
                    .Where(q => q.ARTIST_ID == artist_Id)
                    .FirstOrDefault();
            }
        }

        public static Collection<ARTIST> GetArtists()
        {
            using (var ctx = NewArtistContext())
            {
                return new Collection<ARTIST>(ctx.ARTISTS.OrderBy(a => a.ARTIST_NAME).ToList());
            }
        }

        private static Entities NewArtistContext()
        {
            return new Entities();
        }
    }
}
