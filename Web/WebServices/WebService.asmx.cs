namespace Web.WebServices
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Script.Services;
    using System.Web.Services;
    using System.Web.UI.WebControls;
    using MediaLibrary;
    using MediaLibrary.Utilities.Web.JQuery;
    
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class WebService : System.Web.Services.WebService
    {
        #region Artist WebMethods
        [WebMethod]
        public ARTIST GetEmptyArtist()
        {
            return new ARTIST();
        }

        [WebMethod]
        public ARTIST UpdateArtist(int artistID, string artistName, string filename)
        {
            ARTIST artist = ArtistManager.GetArtist(artistID);
            artist.ARTIST_NAME = artistName;
            artist = ArtistManager.Save(artist);

            if (!File.Exists(filename))
            {
                filename = Server.MapPath("~/" + filename);
            }
            
            ID3TagLibV2.ID3Tag tag = GetID3Tag(filename);

            tag.Artist = artist.ARTIST_NAME;
            tag.AlbumArtist = artist.ARTIST_NAME;
            tag.Performer = artist.ARTIST_NAME;
            tag.Composer = artist.ARTIST_NAME;

            tag.id3v1Artist = ID3TagLibV2.ID3Tag.id3v1String(artist.ARTIST_NAME);

            ID3TagLibV2.TagHandler.UpdateFileTags(filename, tag);

            return artist;
        }

        [WebMethod]
        public ARTIST SaveArtist(ARTIST artist)
        {
            return ArtistManager.Save(artist);
        }

        [WebMethod]
        public ARTIST GetArtist(int artistId)
        {
            return ArtistManager.GetArtist(artistId);
        }

        [WebMethod]
        public Collection<ARTIST> GetArtists()
        {
            return ArtistManager.GetArtists();
        }
        #endregion Artist WebMethods

        #region Album WebMethods
        [WebMethod]
        public ALBUM GetEmptyAlbum()
        {
            return new ALBUM();
        }

        [WebMethod]
        public ALBUM UpdateAlbum(int albumID, string albumName, string filename)
        {
            ALBUM album = AlbumManager.GetAlbum(albumID);
            album.ALBUM_NAME = albumName;
            album = AlbumManager.Save(album);

            if (!File.Exists(filename))
            {
                filename = Server.MapPath("~/" + filename);
            }
            
            ID3TagLibV2.ID3Tag tag = GetID3Tag(filename);

            tag.Album = album.ALBUM_NAME;
            tag.id3v1Album = ID3TagLibV2.ID3Tag.id3v1String(album.ALBUM_NAME);
            
            ID3TagLibV2.TagHandler.UpdateFileTags(filename, tag);

            return album;
        }
        
        [WebMethod]
        public ALBUM SaveAlbum(ALBUM a)
        {
            return AlbumManager.Save(a);
        }

        [WebMethod]
        public ALBUM GetAlbum(int albumId)
        {
            return AlbumManager.GetAlbum(albumId);
        }

        [WebMethod]
        public Collection<ALBUM> GetAlbums(int artistId)
        {
            return AlbumManager.GetAlbums(artistId);
        }
        #endregion Album WebMethods

        #region Song WebMethods
        [WebMethod]
        public SONG GetEmptySong()
        {
            return new SONG();
        }

        [WebMethod]
        public SONG SaveSong(SONG s)
        {
            return SongManager.Save(s);
        }

        [WebMethod]
        public SONG GetSong(int songId)
        {
            return SongManager.GetSong(songId);
        }

        [WebMethod]
        public Collection<SONG> GetSongs(int albumId)
        {
            return SongManager.GetSongs(albumId);
        }
        #endregion Song WebMethods

        #region Search
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetSongList(string artistSearch, string albumSearch, string songSearch, string genreSearch, string dateAddedSearch)
        {
            if (this.Context.Session["user"] != null)
            {
                ResetSessionStartTime(this.Context);

                decimal userSecurityID = (this.Context.Session["user"] as USER_SECURITY).USER_SECURITY_ID;

                RequestData requestData = DataTablesHelper.ParseSentRequestParameters(this.Context);
                string imageFolder = GetImageFolderPath(this.Context);

                SearchCriteria criteria = BuildSearchCriteria(artistSearch, albumSearch, songSearch, genreSearch, dateAddedSearch);

                IList<V_SONG_LIST> songList = SongListManager.GetSongList(criteria, requestData);

                IList<USER_PLAYLIST> playlist = UserPlaylistManager.GetUserPlaylist(userSecurityID);

                object[] data = songList
                    .Select(X => new
                    {
                        DT_RowId = X.SONG_ID,
                        SONG_ID = X.SONG_ID,
                        ARTIST_ID = X.ARTIST_ID,
                        ARTIST_NAME = X.ARTIST_NAME,
                        ALBUM_ID = X.ALBUM_ID,
                        ALBUM_NAME = X.ALBUM_NAME,
                        ART = GetRelativeImageFile(GetImageFileFromImageBytes(X.ALBUM_ART, imageFolder)),
                        SONG_TITLE = X.SONG_TITLE,
                        DURATION = X.DURATION,
                        YEAR = X.YEAR,
                        GENRE = X.GENRE,
                        CREATE_DATE = X.CREATE_DATE.ToShortDateString() + " " + X.CREATE_DATE.ToShortTimeString(),
                        AFP = ResolveFilePath(this.Context, X.ABS_FILE_PATH),
                        AudioFile = GetAudioPlaybackFile(this.Context, ResolveFilePath(this.Context, X.ABS_FILE_PATH)),
                        Playlisted = playlist.Where(x => x.USER_SECURITY_ID == userSecurityID && x.SONG_ID == X.SONG_ID).FirstOrDefault() != null
                    })
                    .ToArray();

                string json = string.Empty;
                string error = string.Empty;

                try
                {
                    json = DataTablesHelper.ConvertToJson(requestData, requestData.TotalRecords, data);
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                }

                HttpContext.Current.Response.Write(json);
            }
            else
            {
                throw new Exception("Unable to retrieve user information from current session.");
            }
        }
        
        [WebMethod]
        public SearchSummary GetSearchResultsSummary(string artistSearch, string albumSearch, string songSearch, string genreSearch, string dateAddedSearch)
        {
            SearchCriteria criteria = BuildSearchCriteria(artistSearch, albumSearch, songSearch, genreSearch, dateAddedSearch);

            return SongListManager.GetSearchSummary(criteria);
        }

        /*
        [WebMethod]
        public string GetNewestSongDate()
        {
            return SongListManager.GetNewestSongDate();
        }
        */
        #endregion Search

        #region File Management
        [WebMethod]
        public void UploadFile()
        {
            string returnValue = string.Empty;

            try
            {
                string tagFile = string.Empty;
                if (this.Context.Request.Form.AllKeys.Count() > 0 && this.Context.Request.Files.Count > 0)
                {
                    var formDataKeys = this.Context.Request.Form.GetValues("tagFile");
                    tagFile = formDataKeys[0];

                    foreach (string file in this.Context.Request.Files)
                    {
                        var fileContent = this.Context.Request.Files[file];
                        if (fileContent != null && fileContent.ContentLength > 0)
                        {
                            var stream = fileContent.InputStream;
                            string fileName = Path.GetFileName(file);
                            string path = Path.Combine(Server.MapPath("~/Images"), fileName);

                            if (File.Exists(path))
                            {
                                File.Delete(path);
                            }
                            
                            using (var fileStream = System.IO.File.Create(path))
                            {
                                stream.CopyTo(fileStream);

                                returnValue = "success";

                                //this.Context.Response.StatusCode = (int)HttpStatusCode.OK;
                            }
                        }
                    }
                }
            }
            catch
            {
                //this.Context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                //returnValue = ex.Message;
                throw;
            }

            //return returnValue;
        }

        [WebMethod]
        public bool ProcessFile(string file)
        {
            bool returnValue = false;
            try
            {
                /*
                // Works, but not very efficient due to Reflection Overhead and no need to unnecessarily generate image files
                var res = GetID3TagInfo(file);

                Type t = res.GetType();
                PropertyInfo p = t.GetProperty("tag");
                ID3TagLibV2.ID3Tag tag = (ID3TagLibV2.ID3Tag)p.GetValue(res, null);
                */

                ID3TagLibV2.ID3Tag tag = GetID3Tag(file);

                if (tag != null)
                {
                    string newPath = 
                        (!Environment.MachineName.ToUpper().Contains("ROCK") ? @"F:\lib\iTunes\" : Server.MapPath("~/App_Data") + Path.DirectorySeparatorChar) +
                        GetSafeFilename(tag.Artist) + Path.DirectorySeparatorChar +
                        GetSafeFilename(tag.Album) + Path.DirectorySeparatorChar +
                        Path.GetFileName(file);

                    Directory.CreateDirectory(Path.GetDirectoryName(newPath));
                    File.Copy(file, newPath, true);

                    if (File.Exists(newPath))
                    {
                        string absFilePath = string.Empty;
                        
                        if(newPath.Contains(@"F:\lib\"))
                        {
                            absFilePath = newPath.Replace(@"F:\lib\", "media/").Replace(@"\", "/");
                        }
                        else
                        {
                            absFilePath = newPath.Replace(Server.MapPath("~/App_Data/"), "App_Data/").Replace(@"\", "/");
                        }

                        ARTIST artist = ArtistManager.GetArtist(ArtistManager.ExistingArtist(tag.Artist));
                        if (artist == null)
                        {
                            artist = new ARTIST();
                            artist.ARTIST_NAME = tag.Artist;
                            artist = ArtistManager.Save(artist);
                        }

                        ALBUM album = AlbumManager.GetAlbum(AlbumManager.ExistingAlbum(artist.ARTIST_ID, tag.Album));
                        if (album == null)
                        {
                            album = new ALBUM();
                            album.ARTIST_ID = artist.ARTIST_ID;
                            album.ALBUM_NAME = tag.Album;

                            album = AlbumManager.Save(album);
                        }

                        SONG song = SongManager.GetSong(SongManager.ExistingSong(album.ALBUM_ID, tag.Title));
                        if (song == null)
                        {
                            song = new SONG();
                        }

                        song.ARTIST_ID = artist.ARTIST_ID;
                        song.ALBUM_ID = album.ALBUM_ID;
                        song.SONG_TITLE = tag.Title;
                        song.GENRE = tag.Genre;

                        song.TRACK_NUM = Convert.ToString(tag.TrackNum);
                        song.YEAR = Convert.ToString(tag.Year);

                        song.ALBUM_ART = ((tag.AlbumArt != null && tag.AlbumArt.Length > 0) ? tag.AlbumArt : null);
                        song.ALBUM_ART_FLAG = (tag.AlbumArt != null && tag.AlbumArt.Length > 0);
                        song.ARTIST_IMAGE = ((tag.ArtistImage != null && tag.ArtistImage.Length > 0) ? tag.ArtistImage : null);
                        song.BAND = ((tag.BandImage != null && tag.BackCoverImage.Length > 0) ? tag.BandImage : null);
                        song.BACK_COVER = ((tag.BackCoverImage != null && tag.BackCoverImage.Length > 0) ? tag.BackCoverImage : null);

                        song.ITUNES_FLAG = true;

                        song.COMMENTS = (string.IsNullOrWhiteSpace(tag.Comment) ? null : tag.Comment);
                        song.COMPOSER = (string.IsNullOrWhiteSpace(tag.Composers[0]) ? null : tag.Composers[0]);
                        song.CONDUCTOR = (string.IsNullOrWhiteSpace(tag.Conductor) ? null : tag.Conductor);
                        song.DISC_NUM = Convert.ToString(tag.DiscNum);

                        System.IO.FileInfo fi = new System.IO.FileInfo(newPath);
                        song.FILE_SIZE = fi.Length;

                        song.FILENAME = Path.GetFileName(newPath);
                        song.ABS_FILE_PATH = absFilePath;
                        song.REL_FILE_PATH = "~/" + absFilePath;

                        song.ID3V1_ALBUM = tag.id3v1Album;
                        song.ID3V1_ARTIST = tag.id3v1Artist;
                        song.ID3V1_TITLE = tag.id3v1Title;
                        song.ID3V1_FLAG = true;

                        song.ID3V2_ALBUM = tag.Album;
                        song.ID3V2_ARTIST = tag.Artist;
                        song.ID3V2_TITLE = tag.Title;
                        song.ID3V2_FLAG = true;

                        song.LYRICS = (string.IsNullOrWhiteSpace(tag.Lyrics) ? null : tag.Lyrics);
                        song.LYRICS_FLAG = string.IsNullOrWhiteSpace(tag.Lyrics);

                        song.DURATION = tag.Duration;
                        song.BITRATE = tag.Bitrate;
                        song.SAMPLE_RATE = tag.SampleRate;

                        song = SongManager.Save(song);

                        returnValue = true;
                    }
                }
                else
                {
                    returnValue = false;
                }
            }
            catch
            {
                returnValue = false;
                throw;
            }

            if (returnValue)
            {
                File.Delete(file);
            }
            
            return returnValue;
        }

        [WebMethod]
        public void UpdateFileTag(string file, ID3TagLibV2.ID3Tag tag, string songID, string albumArtFilename)
        {
            if (!File.Exists(file))
            {
                file = Server.MapPath("~/" + file);
            }

            if (!string.IsNullOrWhiteSpace(albumArtFilename))
            {
                if (!File.Exists(albumArtFilename))
                {
                    albumArtFilename = GetImageFolderPath(this.Context) + albumArtFilename;
                }

                if (File.Exists(albumArtFilename))
                {
                    tag.AlbumArt = File.ReadAllBytes(albumArtFilename);
                }
            }
            
            try
            {
                ID3TagLibV2.TagHandler.UpdateFileTags(file, tag);
            }
            catch (Exception ex)
            {
                throw new Exception("Updating file: " + file + " >>> " + ex.Message);
            }

            try
            {
                SONG song = SongManager.GetSong(Convert.ToDecimal(songID));
                if (song != null)
                {
                    song.SONG_TITLE = tag.Title;
                    song.COMMENTS = tag.Comment;
                    song.COMPOSER = tag.Composer;
                    song.CONDUCTOR = tag.Conductor;
                    song.DISC_NUM = tag.DiscNumString;
                    song.GENRE = tag.Genre;
                    song.ID3V1_ALBUM = tag.id3v1Album;
                    song.ID3V1_ARTIST = tag.id3v1Artist;
                    song.ID3V1_TITLE = tag.id3v1Title;
                    song.LYRICS = tag.Lyrics;
                    song.LYRICS_FLAG = !string.IsNullOrWhiteSpace(tag.Lyrics);
                    song.TRACK_NUM = tag.TrackNumString;
                    song.YEAR = tag.YearString;
                    song.ALBUM_ART = tag.AlbumArt;
                    song.ALBUM_ART_FLAG = tag.AlbumArt != null && tag.AlbumArt.Length > 0;

                    SongManager.Save(song);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("updating song entity >>> " + ex.Message);
            }
        }
        #endregion File Management

        #region Tag Management
        [WebMethod]
        public object GetID3TagInfo(string file)
        {
            string audioFile = GetAudioPlaybackFile(this.Context, file);    //GetAudioPlaybackFile expects a RELATIVE file path, not a physical one

            if (!File.Exists(file))
            {
                file = Server.MapPath("~/" + file);
            }

            ID3TagLibV2.ID3Tag tag = GetID3Tag(file);

            if (tag != null)
            {
                string albumArtPhysicalPath = GetAlbumArtImageFile(tag, GetImageFolderPath(this.Context));
                string albumArtRelativePath = "Images/" + System.IO.Path.GetFileName(albumArtPhysicalPath);
                decimal artistId = ArtistManager.ExistingArtist(tag.AlbumArtist);
                if (artistId == -1)
                    artistId = ArtistManager.ExistingArtist(tag.Artist);
                decimal albumId = AlbumManager.ExistingAlbum(artistId, tag.Album);
                decimal songId = SongManager.ExistingSong(albumId, tag.Title);

                return new
                {
                    artistId = artistId,
                    albumId = albumId,
                    songId = songId,
                    tag = tag,
                    albumArtPhysicalPath = albumArtPhysicalPath,
                    albumArtRelativePath = albumArtRelativePath,
                    audioFile
                };
            }
            else
            {
                throw new ApplicationException("Unable to open file: " + file);
            }
        }

        [WebMethod]
        public ID3TagLibV2.ID3Tag GetEmptyID3Tag()
        {
            return new ID3TagLibV2.ID3Tag();
        }
        #endregion Tag Management

        #region Playlist Management
        [WebMethod(EnableSession = true)]
        public bool IsPlaylistedSong(string songID)
        {
            if (this.Context.Session["user"] != null)
            {
                decimal userSecurityID = (this.Context.Session["user"] as USER_SECURITY).USER_SECURITY_ID;

                return UserPlaylistManager.GetUserPlaylistEntry(Convert.ToDecimal(songID), userSecurityID) != null;
            }
            else
            {
                throw new Exception("Unable to retrieve user information from current session.");
            }
        }
        
        [WebMethod(EnableSession=true)]
        public void ManageUserPlaylist(string songID, string add)
        {
            if (this.Context.Session["user"] != null)
            {
                decimal userSecurityID = (this.Context.Session["user"] as USER_SECURITY).USER_SECURITY_ID;

                if (Convert.ToBoolean(add))
                {
                    UserPlaylistManager.AddSong(Convert.ToDecimal(songID), Convert.ToDecimal(userSecurityID));
                }
                else
                {
                    UserPlaylistManager.RemoveSong(Convert.ToDecimal(songID), Convert.ToDecimal(userSecurityID));
                }
            }
            else
            {
                throw new Exception("Unable to retrieve user information from current session.");
            }
        }

        [WebMethod(EnableSession = true)]
        public object GetUserPlaylist()
        {
            if (this.Context.Session["user"] != null)
            {
                ResetSessionStartTime(this.Context);

                decimal userSecurityID = (this.Context.Session["user"] as USER_SECURITY).USER_SECURITY_ID;

                IList<USER_PLAYLIST> pl = UserPlaylistManager.GetUserPlaylist(Convert.ToDecimal(userSecurityID));

                TimeSpan runtime = new TimeSpan();

                foreach (USER_PLAYLIST upl in pl)
                {
                    runtime = runtime.Add(GetTimespanFromDuration(upl.SONG.DURATION));
                }

                object[] data = pl
                    .Select(x => new
                    {
                        SONG_ID = x.SONG_ID,
                        AudioFile = GetAudioPlaybackFile(this.Context, ResolveFilePath(this.Context, x.SONG.ABS_FILE_PATH)),
                        DisplayName = x.SONG.ID3V2_TITLE + " (" + x.SONG.DURATION + ")",
                        Duration = x.SONG.DURATION,
                        RunTime = runtime.ToString(),
                        Artist = x.SONG.ID3V2_ARTIST,
                        Album = x.SONG.ID3V2_ALBUM,
                        Title = x.SONG.ID3V2_TITLE,
                        CreateDate = x.CREATE_DATE,
                        PlaylistName = x.NAME
                    })
                    .OrderByDescending(x => x.CreateDate)
                    .ToArray();

                return data;
            }
            else
            {
                throw new Exception("Unable to retrieve user information from current session.");
            }
        }
        #endregion Playlist Management

        #region User Management
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetUsers()
        {
            RequestData requestData = DataTablesHelper.ParseSentRequestParameters(this.Context);

            IList<USER_SECURITY> userList = UserSecurityManager.GetUsers(requestData);

            object[] data = userList
                .Select(x => new
                {
                    DT_RowId = x.USER_SECURITY_ID,
                    
                    USER_SECURITY_ID = x.USER_SECURITY_ID,
                    USERNAME = x.USERNAME,
                    PASSWORD = x.PASSWORD,
                    LOGIN_FAILURE_COUNT = x.LOGIN_FAILURE_COUNT,

                    LOCKED_FLAG = x.LOCKED_FLAG,
                    ACTIVE_FLAG = x.ACTIVE_FLAG,
                    ADMIN_FLAG = x.ADMIN_FLAG,

                    CREATE_DATE = x.CREATE_DATE.ToShortDateString(),

                    LAST_FAILURE_DATE = (x.LAST_FAILURE_DATE.HasValue ? x.LAST_FAILURE_DATE.Value.ToShortDateString() : null),
                    LAST_LOGIN_DATE = (x.LAST_LOGIN_DATE.HasValue ? x.LAST_LOGIN_DATE.Value.ToShortDateString() : null),
                    PREV_TO_LAST_LOGIN_DATE = (x.PREV_TO_LAST_LOGIN_DATE.HasValue ? x.PREV_TO_LAST_LOGIN_DATE.Value.ToShortDateString() : null),
                    UPDATE_DATE = (x.UPDATE_DATE.HasValue ? x.UPDATE_DATE.Value.ToShortDateString() : null)
                })
                .ToArray();

            string json = string.Empty;
            string error = string.Empty;

            try
            {
                json = DataTablesHelper.ConvertToJson(requestData, requestData.TotalRecords, data);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            HttpContext.Current.Response.Write(json);
        }

        [WebMethod]
        public object AuthorizedUser(string username, string password)
        {
            USER_SECURITY user = GetUserByName(username);

            bool isAdmin = user.ADMIN_FLAG;
            bool isAuthorized = false;
            
            if (password.ToUpper() == user.PASSWORD.ToUpper())
            {
                isAuthorized = user.ACTIVE_FLAG && !user.LOCKED_FLAG;
            }

            if (isAuthorized)
            {
                user.PREV_TO_LAST_LOGIN_DATE = user.LAST_LOGIN_DATE;
                user.LAST_LOGIN_DATE = DateTime.Now;
            }
            else
            {
                user.LAST_FAILURE_DATE = DateTime.Now;
                user.LOGIN_FAILURE_COUNT = user.LOGIN_FAILURE_COUNT + 1;
            }

            SaveUser(user);

            return new
            {
                user = user,
                isAdmin = isAdmin,
                isAuthorized = isAuthorized
            };
        }

        [WebMethod]
        public USER_SECURITY GetUserByName(string username)
        {
            return UserSecurityManager.GetUser(username);
        }

        [WebMethod]
        public USER_SECURITY GetUserByID(int userSecurityID)
        {
            return UserSecurityManager.GetUser(userSecurityID);
        }

        [WebMethod]
        public USER_SECURITY GetEmptyUser()
        {
            return new USER_SECURITY();
        }

        [WebMethod]
        public USER_SECURITY SaveUser(USER_SECURITY user)
        {
            return UserSecurityManager.Save(user);
        }

        [WebMethod(EnableSession = true)]
        public bool IsAdminUser()
        {
            if(Session["user"] != null){
                return ((USER_SECURITY)Session["user"]).ADMIN_FLAG;
            }
            else
            {
                throw new Exception("Unable to retrieve user information from current session.");
            }
        }
        #endregion User Management

        #region Session Management
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string KeepAlive()
        {
            ResetSessionStartTime(this.Context);

            return "OK";
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public DateTime GetSessionStart()
        {
            if (this.Context.Session["SessionStartTime"] != null)
            {
                return (DateTime)this.Context.Session["SessionStartTime"];
            }

            throw new ApplicationException("GetSessionStart() encountered a NULL session");
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public double GetSessionIdleTime()
        {
            TimeSpan span = DateTime.Now - (DateTime)this.Context.Session["SessionStartTime"];
            return span.TotalSeconds;
        }
        #endregion Session Management

        #region Privates
        private static void ResetSessionStartTime(HttpContext context)
        {
            if (context.Session["SessionStartTime"] != null)
            {
                context.Session["SessionStartTime"] = DateTime.Now;
            }
            else
            {
                context.Session.Add("SessionStartTime", DateTime.Now);
            }
        }
        
        private static TimeSpan GetTimespanFromDuration(string songDuration)
        {
            TimeSpan ts = new TimeSpan(0, 0, 0);

            if (songDuration.Contains(':'))
            {
                //HH:MI:SS
                string[] splits = songDuration.Split(new char[] { ':' });

                if (splits.Length > 2)
                {
                    ts = new TimeSpan(Convert.ToInt32(splits[0]), Convert.ToInt32(splits[1]), Convert.ToInt32(splits[2]));
                }
                else
                {
                    ts = new TimeSpan(0, Convert.ToInt32(splits[0]), Convert.ToInt32(splits[1]));
                }
            }

            return ts;
        }
        
        private static SearchCriteria BuildSearchCriteria(string artistSearch, string albumSearch, string songSearch, string genreSearch, string dateAddedSearch)
        {
            SearchCriteria criteria = new SearchCriteria();
            
            Nullable<DateTime> dateAdded = null;
            if (!string.IsNullOrWhiteSpace(dateAddedSearch))
                dateAdded = Convert.ToDateTime(dateAddedSearch);

            criteria.AlbumName = albumSearch;
            criteria.AlbumSearchType = SearchCriteria.SearchType.Contains;

            criteria.ArtistName = artistSearch;
            criteria.ArtistSearchType = SearchCriteria.SearchType.Contains;

            criteria.SongTitle = songSearch;
            criteria.SongSearchType = SearchCriteria.SearchType.Contains;

            criteria.Genre = genreSearch;
            criteria.GenreSearchType = SearchCriteria.SearchType.Contains;

            criteria.CreateDate = dateAdded;

            return criteria;
        }
        
        private static string ResolveFilePath(HttpContext context, string filepath)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(filepath))
                {
                    if (filepath.StartsWith("~"))
                    {
                        return context.Server.MapPath(filepath);
                    }
                    else
                    {
                        return filepath;
                    }
                }
                else
                {
                    return string.Empty;
                }
                
            }
            catch
            {
                return string.Empty;
            }
        }

        private static string GetImageFolderPath(HttpContext context)
        {
            return context.Server.MapPath("~/Images") + System.IO.Path.DirectorySeparatorChar;
        }

        private static string GetImageFileFromImageBytes(byte[] imageBytes, string imageFolderPath)
        {
            string returnValue = string.Empty;
            if(imageBytes != null && imageBytes.Length > 0)
            {
                string fileName = System.IO.Path.GetFileName(System.IO.Path.GetTempFileName()).ToLower().Replace(".tmp", ".jpg");

                using (System.IO.MemoryStream ms = new System.IO.MemoryStream(imageBytes, 0, imageBytes.Length))
                {
                    using (System.Drawing.Image img = System.Drawing.Image.FromStream(ms))
                    {
                        float mHeight = (float)60;
                        float mWidth = (float)60;
                        Unit Height;
                        Unit Width;

                        if (img.Width > mWidth || img.Height > mHeight)
                        {
                            float height = img.Height;
                            float width = img.Width;
                            float deltaWidth = width - mWidth;
                            float deltaHeight = height - mHeight;
                            float scaleFactor;

                            if (deltaHeight > deltaWidth)
                            {
                                scaleFactor = mHeight / height;
                            }
                            else
                            {
                                scaleFactor = mWidth / width;
                            }
                            Width = (Unit)Math.Floor(img.Width * scaleFactor);
                            Height = (Unit)Math.Floor(img.Height * scaleFactor);
                        }
                        else
                        {
                            Width = (Unit)img.Width;
                            Height = (Unit)img.Height;
                        }

                        Bitmap bitmap = new Bitmap((int)Width.Value, (int)Height.Value);
                        using (Graphics g = Graphics.FromImage(bitmap))
                        {
                            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                            g.DrawImage(img,
                                        new Rectangle(Point.Empty, new Size((int)Width.Value, (int)Height.Value)),
                                        new Rectangle(Point.Empty, new Size(img.Width, img.Height)),
                                        GraphicsUnit.Pixel);
                        }
                        bitmap.Save(imageFolderPath + fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                        returnValue = imageFolderPath + fileName;
                    }
                }

                return returnValue;
            }
            else
            {
                return string.Empty;
            }
        }

        private static string GetRelativeImageFile(string file)
        {
            return string.IsNullOrWhiteSpace(file) ? string.Empty : "Images/" + System.IO.Path.GetFileName(file);
        }
        
        private static ID3TagLibV2.ID3Tag GetID3Tag(string file)
        {
            return ID3TagLibV2.TagHandler.ReadFileTags(file);
        }

        private static string GetAlbumArtImageFile(ID3TagLibV2.ID3Tag tag, string destinationFolder)
        {
            if (!destinationFolder.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
                destinationFolder = destinationFolder + System.IO.Path.DirectorySeparatorChar;

            return ID3TagLibV2.Helpers.CreateImageFile(tag.AlbumArt, destinationFolder);
        }

        private static string GetAudioPlaybackFile(HttpContext context, string filename)
        {
            if (filename.ToLower().Contains("app_data/"))
            {
                string playbackPath = filename.ToLower().Replace("app_data/", "AudioTest/");

                Directory.CreateDirectory(Path.GetDirectoryName(context.Server.MapPath("~/" + playbackPath)));
                System.IO.File.Copy(context.Server.MapPath("~/" + filename), context.Server.MapPath("~/" + playbackPath), true);

                return playbackPath;
            }
            else
            {
                return filename.ToLower().Replace("f:\\lib", "media").Replace("\\", "/");
            }
        }

        private static string GetSafeFilename(string filename)
        {
            return string.Join("_", filename.Split(Path.GetInvalidFileNameChars()));
        }
        #endregion Privates
    }
}
