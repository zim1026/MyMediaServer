namespace jQueryTestWeb
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using jQueryTestLibrary;

    public partial class Default2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        /*
        protected void cmdTest_Click(object sender, EventArgs e)
        {
            TestWebServiceReference.TestWebService ts = new TestWebServiceReference.TestWebService();
            
            TestWebServiceReference.Artist artist = new TestWebServiceReference.Artist();
            artist.ArtistName = "Foo Bar";
            artist.Genre = "Christian";
            artist = ts.SaveArtist(artist);
            

            TestWebServiceReference.Album album = new TestWebServiceReference.Album();
            album.ArtistId = artist.ArtistId;
            album.AlbumName = "First Album";
            album = ts.SaveAlbum(album);

            TestWebServiceReference.Song song = new TestWebServiceReference.Song();
            song.AlbumId = album.AlbumId;
            song.Title = "First Song";
            song.TrackNumber = 1;
            song = ts.SaveSong(song);

            foreach (Artist a in ArtistManager.GetArtists())
            {
                foreach (Album al in AlbumManager.GetAlbums(a.ArtistId))
                {
                    foreach (Song s in SongManager.GetSongs(al.AlbumId))
                    {

                    }
                }
            }
        }
        */
    }
}