using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace ID3TagLib {

    public class ID3Info:IDisposable {
        public enum ErrorMode {
            Permissive,
            Strict
        }
        
        #region Privates Variables
        private bool _invalidAlbumArt;
        private bool _disposed=false;
        private string _filename = null;
        private bool _changesToSave=false;
        private Image _albumArt;
        private static string _errorBase = "ID3TagLib.ID3Info.";
        private MemoryStream _memStream = null;
        private TagLib.File _tagLib = null;
        private TagLib.Id3v2.Tag _id3v2;
        private TagLib.Id3v1.Tag _id3v1;
        private static Error _error;
        private ErrorMode _errorMode;
        #endregion Privates

        #region Private Methods
        private string GetFrameID(string item) {
            switch(item.Trim().ToUpper()) {
                case ("ARTIST"):
                    return "TPE1";
                case ("ALBUM"):
                    return "TALB";
                case ("TITLE"):
                    return "TIT2";
                case ("TRACK"):
                    return "TRCK";
                case ("YEAR"):
                    return "TYER";
                case ("GENRE"):
                    return "TCON";
                case ("COMMENT"):
                    return "COMM";
                case ("ART"):
                    return "APIC";
                case ("DISC"):
                    return "TPOS";
                case ("SET"):
                    return "TPOS";
                case ("LYRICS"):
                    return "USLT";
                default:
                    throw new NotImplementedException(item + " is not implemented.");
            }
        }

        private MemoryStream MemStream {
            get { return _memStream; }
            set { _memStream = value; }
        }

        private static string GetMIMEType(string Extension) {
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(Extension);
            if(regKey == null)
                return "image/unknown";
            else
                return regKey.GetValue("Content Type").ToString();
        }

        private static string GetMIMEType(Image image) {
            foreach(System.Drawing.Imaging.ImageCodecInfo codec in System.Drawing.Imaging.ImageCodecInfo.GetImageDecoders()) {
                if(codec.FormatID == image.RawFormat.Guid)
                    return codec.MimeType;
            }

            return "image/unknown";
        }

        private void LogError(ref ID3TagLib.ID3Tag tag, ref Exception ex) {
            tag.errors.Add(ex.Source + " threw: " + ex.Message);
            _error.Add(ex);
        }

        private ErrorMode errorMode {
            get { return _errorMode; }
            set { _errorMode = errorMode; }
        }
        #endregion

        #region Constructor
        public ID3Info(string file, ErrorMode tolerance) {
            _error = new Error(file);
            errorMode = tolerance;
            try {
                MemStream = new MemoryStream();
                _filename = file;
                _changesToSave=false;
                _tagLib = TagLib.File.Create(file);
                _id3v2 = (TagLib.Id3v2.Tag)_tagLib.GetTag(TagLib.TagTypes.Id3v2);
                _id3v1 = (TagLib.Id3v1.Tag)_tagLib.GetTag(TagLib.TagTypes.Id3v1);
            } catch(Exception ex) {
                ex.Source = _errorBase + "Constructor()--->"+ex.Source;
                if(!ex.Message.ToUpper().Contains("MPEG HEADER")) {
                    _error.Add(ex);
                    if(errorMode == ErrorMode.Strict)
                        throw;
                }
            }
        }
        #endregion

        #region Utility Methods
        public void RemoveEmbeddedArt(bool RemoveAlbumArtOnly) {
            try {
                foreach(TagLib.IPicture pic in _tagLib.Tag.Pictures) {
                    if(RemoveAlbumArtOnly) {
                        if(pic.Type == TagLib.PictureType.FrontCover)
                            pic.Data.Clear();
                    }
                    else
                        pic.Data.Clear();
                }
                _changesToSave = true;
            } catch(Exception ex) {
                ex.Source = _errorBase + "RemoveEmbeddedArt("+RemoveAlbumArtOnly.ToString()+")--->"+ex.Source;
                _error.Add(ex);
                if(errorMode == ErrorMode.Strict)
                    throw;
            }
        }

        public void AddAlbumArt(string imageFile) {
            try {
                TagLib.Picture pic = new TagLib.Picture(imageFile);
                TagLib.Id3v2.AttachedPictureFrame apf = new TagLib.Id3v2.AttachedPictureFrame(pic);
                apf.MimeType = GetMIMEType(Path.GetExtension(imageFile));
                apf.Type = TagLib.PictureType.FrontCover;
                TagLib.IPicture[] ipic = new TagLib.IPicture[1];
                ipic[0] = apf;
                _tagLib.Tag.Pictures = ipic;
                _changesToSave = true;
            } catch(Exception ex) {
                ex.Source = _errorBase + "AddAlbumArt("+imageFile+")--->"+ex.Source;
                _error.Add(ex);
                if(errorMode == ErrorMode.Strict)
                    throw;
            }
        }
        
        public ID3Tag GetAllTags(bool GetArt) {
            ID3Tag _tag = new ID3Tag();
            try {
                try {
                    _tag.Comments = Comments;
                } catch(Exception ex) {
                    LogError(ref _tag, ref ex);
                }
                try {
                    _tag.artist = Artist;
                } catch(Exception ex) {
                    LogError(ref _tag, ref ex);
                }
                try {
                    _tag.album = Album;
                } catch(Exception ex) {
                    LogError(ref _tag, ref ex);
                }
                try {
                    _tag.title = Title;
                } catch(Exception ex) {
                    LogError(ref _tag, ref ex);
                }
                try {
                    _tag.path = _filename;
                } catch(Exception ex) {
                    LogError(ref _tag, ref ex);
                }
                try {
                    _tag.year = Year;
                } catch(Exception ex) {
                    LogError(ref _tag, ref ex);
                }
                try {
                    _tag.genre = Genre;
                } catch(Exception ex) {
                    LogError(ref _tag, ref ex);
                }
                try {
                    _tag.Lyrics = Lyrics;
                } catch(Exception ex) {
                    LogError(ref _tag, ref ex);
                }
                try {
                    _tag.track = Track;
                } catch(Exception ex) {
                    LogError(ref _tag, ref ex);
                }
                try {
                    _tag.discNum = Disc;
                } catch(Exception ex) {
                    LogError(ref _tag, ref ex);
                }

                try {
                    _tag.HasID3v1Tag = HasID3v1Tag;
                    if(HasID3v1Tag) {
                        try {
                            _tag.v1Artist = Id3v1_Artist;
                        } catch(Exception ex) {
                            LogError(ref _tag, ref ex);
                        }
                        try {
                            _tag.v1Album = Id3v1_Album;
                        } catch(Exception ex) {
                            LogError(ref _tag, ref ex);
                        }
                        try {
                            _tag.v1Title = Id3v1_Title;
                        } catch(Exception ex) {
                            LogError(ref _tag, ref ex);
                        }
                    }
                } catch(Exception Exception) {
                    Exception.Source = "ID3v1 Read()--->"+Exception.Source;
                    _tag.HasID3v1Tag = false;
                    LogError(ref _tag, ref Exception);
                }

                try {
                    _tag.HasID3v2Tag = HasID3v2Tag;
                    if(HasID3v2Tag) {
                        try {
                            _tag.v2Artist = Id3v2_Artist;
                        } catch(Exception ex) {
                            LogError(ref _tag, ref ex);
                        }

                        try {
                            _tag.v2Album = Id3v2_Album;
                        } catch(Exception ex) {
                            LogError(ref _tag, ref ex);
                        }

                        try {
                            _tag.v2Title = Id3v2_Title;
                        } catch(Exception ex) {
                            LogError(ref _tag, ref ex);
                        }

                        try {
                            _tag.HasAlbumArt = HasAlbumArt;
                        } catch(Exception ex) {
                            LogError(ref _tag, ref ex);
                        }

                        if(_tag.HasAlbumArt && GetArt) {
                            try {
                                _tag.albumArt = AlbumArt;
                                if(InvalidAlbumArt) {
                                    _tag.HasAlbumArt = false;
                                }
                            } catch(Exception ex) {
                                _tag.HasAlbumArt = false;
                                LogError(ref _tag, ref ex);
                            }
                        }
                    }
                } catch(Exception ex) {
                    ex.Source = "ID3v2 Read()--->" + ex.Source;
                    _tag.HasID3v2Tag = false;
                    LogError(ref _tag, ref ex);
                }

                try {
                    _tag.TrackCount = TrackCount;
                } catch(Exception ex) {
                    ex.Source = "TrackCount--->"+ex.Source;
                    LogError(ref _tag, ref ex);
                }

                try {
                    _tag.DiscCount = DiscCount;
                } catch(Exception ex) {
                    ex.Source = "DiscCount--->"+ex.Source;
                    LogError(ref _tag, ref ex);
                }
                try {
                    _tag.AmazonID = AmazonID;
                } catch(Exception ex) {
                    ex.Source = "AmazonID--->"+ex.Source;
                    LogError(ref _tag, ref ex);
                }
                try {
                    _tag.mbArtistID = mbArtistID;
                } catch(Exception ex) {
                    ex.Source = "mbArtistID--->"+ex.Source;
                    LogError(ref _tag, ref ex);
                }
                try {
                    _tag.mbDiscID = mbDiscID;
                } catch(Exception ex) {
                    ex.Source = "mbDiscID--->"+ex.Source;
                    LogError(ref _tag, ref ex);
                }
                try {
                    _tag.mbTrackID = mbTrackID;
                } catch(Exception ex) {
                    ex.Source = "mbTrackID--->"+ex.Source;
                    LogError(ref _tag, ref ex);
                }
                try {
                    _tag.mbReleaseArtistID = mbReleaseArtistID;
                } catch(Exception ex) {
                    ex.Source = "mbReleaseArtistID--->"+ex.Source;
                    LogError(ref _tag, ref ex);
                }
                try {
                    _tag.mbCountry = mbCountry;
                } catch(Exception ex) {
                    ex.Source = "mbCountry--->"+ex.Source;
                    LogError(ref _tag, ref ex);
                }
                try {
                    _tag.mbReleaseID = mbReleaseID;
                } catch(Exception ex) {
                    ex.Source = "mbReleaseID--->"+ex.Source;
                    LogError(ref _tag, ref ex);
                }
                try {
                    _tag.mbReleaseStatus = mbReleaseStatus;
                } catch(Exception ex) {
                    ex.Source = "mbReleaseStatus--->"+ex.Source;
                    LogError(ref _tag, ref ex);
                }
                try {
                    _tag.mbReleaseType = mbReleaseType;
                } catch(Exception ex) {
                    ex.Source = "mbReleaseType--->"+ex.Source;
                    LogError(ref _tag, ref ex);
                }
                try {
                    _tag.MusicIPID = MusicIPID;
                } catch(Exception ex) {
                    ex.Source = "MusicIPID--->"+ex.Source;
                    LogError(ref _tag, ref ex);
                }
                try {
                    _tag.BitRate = BitRate;
                } catch(Exception ex) {
                    ex.Source = "BitRate--->"+ex.Source;
                    LogError(ref _tag, ref ex);
                }
                try {
                    _tag.Channels = Channels;
                } catch(Exception ex) {
                    ex.Source = "Channels--->"+ex.Source;
                    LogError(ref _tag, ref ex);
                }
                try {
                    _tag.SampleRate = SampleRate;
                } catch(Exception ex) {
                    ex.Source = "SampleRate--->"+ex.Source;
                    LogError(ref _tag, ref ex);
                }
                try {
                    _tag.BitsPerSample = BitsPerSample;
                } catch(Exception ex) {
                    ex.Source = "BitsPerSample--->"+ex.Source;
                    LogError(ref _tag, ref ex);
                }
                try {
                    _tag.BeatsPerMinute = BeatsPerMinute;
                } catch(Exception ex) {
                    ex.Source = "BeatsPerMinute--->"+ex.Source;
                    LogError(ref _tag, ref ex);
                }
                try {
                    _tag.Duration = Duration;
                } catch(Exception ex) {
                    ex.Source = "Duration--->"+ex.Source;
                    LogError(ref _tag, ref ex);
                }
                try {
                    _tag.artists = Artists;
                } catch(Exception ex) {
                    ex.Source = "Artists--->"+ex.Source;
                    LogError(ref _tag, ref ex);
                }
                try {
                    _tag.composers = Composers;
                } catch(Exception ex) {
                    ex.Source = "Composers--->"+ex.Source;
                    LogError(ref _tag, ref ex);
                }
                try {
                    _tag.performers = Performers;
                } catch(Exception ex) {
                    ex.Source = "Performers--->"+ex.Source;
                    LogError(ref _tag, ref ex);
                }
                try {
                    _tag.conductor = Conductor;
                } catch(Exception ex) {
                    ex.Source = "Conductor--->"+ex.Source;
                    LogError(ref _tag, ref ex);
                }

                return _tag;
            } catch(Exception mex) {
                mex.Source = "Main read()--->"+ mex.Source;
                throw;
            }
        }

        public void RemoveID3v2Tags() {
            try {
                if(HasID3v2Tag) {
                    _tagLib.RemoveTags(TagLib.TagTypes.Id3v2);
                    _changesToSave = true;
                }
            } catch(Exception ex) {
                ex.Source = _errorBase + "RemoveID3v2Tags(" + _filename + ")--->" + ex.Source;
                _error.Add(ex);
                if(errorMode == ErrorMode.Strict)
                    throw;
            }
        }

        public void RemoveID3v1Tags() {
            try {
                if(HasID3v1Tag) {
                    _tagLib.RemoveTags(TagLib.TagTypes.Id3v1);
                    _changesToSave=true;
                }
            } catch(Exception ex) {
                ex.Source = _errorBase + "RemoveID3v1Tags("+_filename+")--->"+ex.Source;
                _error.Add(ex);
                if(errorMode == ErrorMode.Strict)
                    throw;
            }
        }
        #endregion

        #region Audio Properties
        public int BitRate {
            get {
                try {
                    return _tagLib.Properties.AudioBitrate;
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Get_BitRate()--->"+ex.Source;
                    throw;
                }
            }
        }

        public int SampleRate {
            get {
                try {
                    return _tagLib.Properties.AudioSampleRate;
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Get_SampleRate()--->"+ex.Source;
                    throw;
                }
            }
        }

        public int BitsPerSample {
            get {
                try {
                    return _tagLib.Properties.BitsPerSample;
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Get_BitsPerSample()--->"+ex.Source;
                    throw;
                }
            }
        }

        public int Channels {
            get {
                try {
                    return _tagLib.Properties.AudioChannels;
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Get_Channels()--->"+ex.Source;
                    throw;
                }
            }
        }

        public uint BeatsPerMinute {
            get {
                try {
                    return _tagLib.Tag.BeatsPerMinute;
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Get_BeatsPerMinute()--->"+ex.Source;
                    throw;
                }
            }
        }

        public TimeSpan Duration {
            get {
                try {
                    return _tagLib.Properties.Duration;
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Get_Duration()--->"+ex.Source;
                    throw;
                }
            }
        }
        #endregion Audio Properties

        public Error Errors {
            get { return _error; }
        }

        public bool HadErrors {
            get {
                if(Errors.Count > 0)
                    return true;
                else
                    return false;
            }
        }

        public string[] Artists {
            get {
                try {
                    return _tagLib.Tag.AlbumArtists;
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Get_Artists()--->"+ex.Source;
                    _error.Add(ex);
                    if(errorMode == ErrorMode.Strict)
                        throw;
                    else
                        return new string[] { "AlbumArtists Read Failure" };
                }
            }
            set {
                try {
                    _tagLib.Tag.AlbumArtists = value;
                    _changesToSave = true;
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Set_Artists()--->"+ex.Source;
                    _error.Add(ex);
                    if(errorMode == ErrorMode.Strict)
                        throw;
                }
            }
        }

        public string[] Composers {
            get {
                try {
                    return _tagLib.Tag.Composers;
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Get_Composers()--->"+ex.Source;
                    _error.Add(ex);
                    if(errorMode == ErrorMode.Strict)
                        throw;
                    else
                        return new string[] { "Composers Read Failure" };
                }
            }
            set {
                try {
                    _tagLib.Tag.Composers = value;
                    _changesToSave = true;
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Set_Composers()--->"+ex.Source;
                    _error.Add(ex);
                    if(errorMode == ErrorMode.Strict)
                        throw;
                }
            }
        }

        public string[] Performers {
            get {
                try {
                    return _tagLib.Tag.Performers;
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Get_Performers()--->"+ex.Source;
                    _error.Add(ex);
                    if(errorMode == ErrorMode.Strict)
                        throw;
                    else
                        return new string[] { "Performers Read Failure" };
                }
            }
            set {
                try {
                    _tagLib.Tag.Performers = value;
                    _changesToSave = true;
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Set_Performers()--->"+ex.Source;
                    _error.Add(ex);
                    if(errorMode == ErrorMode.Strict)
                        throw;
                }

            }
        }

        public string Conductor {
            get {
                try {
                    return _tagLib.Tag.Conductor;
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Get_Conductor()--->"+ex.Source;
                    _error.Add(ex);
                    if(errorMode == ErrorMode.Strict)
                        throw;
                    else
                        return "Conductor Read Failure";
                }
            }
            set {
                try {
                    _tagLib.Tag.Conductor = value;
                    _changesToSave = true;
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Set_Conductor()--->"+ex.Source;
                    _error.Add(ex);
                    if(errorMode == ErrorMode.Strict)
                        throw;
                }
            }
        }

        #region Identifiers
        public string AmazonID {
            get {
                try {
                    return _tagLib.Tag.AmazonId;
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Get_AmazonID()--->" + ex.Source;
                    _error.Add(ex);
                    if(errorMode == ErrorMode.Strict)
                        throw;
                    else
                        return "AmazonID Read Failure";
                }
            }
            set {
                try {
                    _tagLib.Tag.AmazonId = value;
                    _changesToSave = true;
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Set_AmazonID()--->"+ex.Source;
                    _error.Add(ex);
                    if(errorMode == ErrorMode.Strict)
                        throw;
                }
            }
        }

        public string mbArtistID {
            get {
                try {
                    return _tagLib.Tag.MusicBrainzArtistId;
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Get_MusicBrainz_ArtistID()--->"+ex.Source;
                    _error.Add(ex);
                    if(errorMode == ErrorMode.Strict)
                        throw;
                    else
                        return "mbArtistID Read Failure";
                }
            }
            set {
                try {
                    _tagLib.Tag.MusicBrainzArtistId = value;
                    _changesToSave = true;
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Set_MusicBrainz_ArtistID()--->"+ex.Source;
                    _error.Add(ex);
                    if(errorMode == ErrorMode.Strict)
                        throw;
                }
            }
        }

        public string mbDiscID {
            get {
                try {
                    return _tagLib.Tag.MusicBrainzDiscId;
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Get_MusicBrainz_DiscID()--->"+ex.Source;
                    _error.Add(ex);
                    if(errorMode == ErrorMode.Strict)
                        throw;
                    else
                        return "mbDiscID Read Failure";
                }
            }
            set {
                try {
                    _tagLib.Tag.MusicBrainzDiscId = value;
                    _changesToSave = true;
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Set_MusicBrainz_DiscID()--->"+ex.Source;
                    _error.Add(ex);
                    if(errorMode == ErrorMode.Strict)
                        throw;
                }
            }
        }

        public string mbTrackID {
            get {
                try {
                    return _tagLib.Tag.MusicBrainzTrackId;
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Get_MusicBrainz_TrackID()--->"+ex.Source;
                    _error.Add(ex);
                    if(errorMode == ErrorMode.Strict)
                        throw;
                    else
                        return "mbTrackID read failure";
                }
            }
            set {
                try {
                    _tagLib.Tag.MusicBrainzTrackId = value;
                    _changesToSave = true;
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Set_MusicBrainz_TrackID()--->"+ex.Source;
                    _error.Add(ex);
                    if(errorMode == ErrorMode.Strict)
                        throw;
                }
            }
        }

        public string mbReleaseArtistID {
            get {
                try {
                    return _tagLib.Tag.MusicBrainzReleaseArtistId;
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Get_MusicBrainz_ReleaseArtistID()--->"+ex.Source;
                    _error.Add(ex);
                    if(errorMode == ErrorMode.Strict)
                        throw;
                    else
                        return "mbReleaseArtistID Read Failure";
                }
            }
            set {
                try {
                    _tagLib.Tag.MusicBrainzReleaseArtistId = value;
                    _changesToSave = true;
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Set_MusicBrainz_ReleaseArtistID()--->"+ex.Source;
                    _error.Add(ex);
                    if(errorMode == ErrorMode.Strict)
                        throw;
                }
            }
        }

        public string mbCountry {
            get {
                try {
                    return _tagLib.Tag.MusicBrainzReleaseCountry;
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Get_MusicBrainz_Country()--->"+ex.Source;
                    _error.Add(ex);
                    if(errorMode == ErrorMode.Strict)
                        throw;
                    else
                        return "mbCountry Read Failure";
                }
            }
            set {
                try {
                    _tagLib.Tag.MusicBrainzReleaseCountry = value;
                    _changesToSave = true;
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Set_MusicBrainz_Country()--->"+ex.Source;
                    _error.Add(ex);
                    if(errorMode == ErrorMode.Strict)
                        throw;
                }
            }
        }

        public string mbReleaseID {
            get {
                try {
                    return _tagLib.Tag.MusicBrainzReleaseId;
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Get_MusicBrainz_ReleaseID()--->"+ex.Source;
                    _error.Add(ex);
                    if(errorMode == ErrorMode.Strict)
                        throw;
                    else
                        return "mbReleaseID Read Failure";
                }
            }
            set {
                try {
                    _tagLib.Tag.MusicBrainzReleaseId = value;
                    _changesToSave = true;
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Set_MusicBrainz_ReleaseID()--->"+ex.Source;
                    _error.Add(ex);
                    if(errorMode == ErrorMode.Strict)
                        throw;
                }
            }
        }

        public string mbReleaseStatus {
            get {
                try {
                    return _tagLib.Tag.MusicBrainzReleaseStatus;
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Get_MusicBrainz_ReleaseStatus()--->"+ex.Source;
                    _error.Add(ex);
                    if(errorMode == ErrorMode.Strict)
                        throw;
                    else
                        return "mbReleaseStatus Read Failure";
                }
            }
            set {
                try {
                    _tagLib.Tag.MusicBrainzReleaseStatus =value;
                    _changesToSave = true;
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Set_MusicBrainz_ReleaseStatus()--->"+ex.Source;
                    _error.Add(ex);
                    if(errorMode == ErrorMode.Strict)
                        throw;
                }
            }
        }

        public string mbReleaseType {
            get {
                try {
                    return _tagLib.Tag.MusicBrainzReleaseType;
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Get_MusicBrainz_ReleaseType()--->"+ex.Source;
                    _error.Add(ex);
                    if(errorMode == ErrorMode.Strict)
                        throw;
                    else
                        return "mbReleaseType Read Failure";
                }
            }
            set {
                try {
                    _tagLib.Tag.MusicBrainzReleaseType = value;
                    _changesToSave = true;
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Set_MusicBrainz_ReleaseType()--->"+ex.Source;
                    _error.Add(ex);
                    if(errorMode == ErrorMode.Strict)
                        throw;
                }
            }
        }

        public string MusicIPID {
            get {
                try {
                    return _tagLib.Tag.MusicIpId;
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Get_MusicIPID()--->"+ex.Source;
                    _error.Add(ex);
                    if(errorMode == ErrorMode.Strict)
                        throw;
                    else
                        return "MusicIPID Read Failure";
                }
            }
            set {
                try {
                    _tagLib.Tag.MusicIpId = value;
                    _changesToSave = true;
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Set_MusicIPID()--->"+ex.Source;
                    _error.Add(ex);
                    if(errorMode == ErrorMode.Strict)
                        throw;
                }
            }
        }
        #endregion Identifiers

        public string TrackCount {
            get {
                try {
                    return _tagLib.Tag.TrackCount.ToString();
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Get_TrackCount()--->"+ex.Source;
                    _error.Add(ex);
                    if(errorMode == ErrorMode.Strict)
                        throw;
                    else
                        return "TrackCount Read Failure";
                }
            }
            set {
                try {
                    uint _count = 1026;
                    try {
                        _count = Convert.ToUInt32(value);
                    } catch { }
                    if(_count != 1026) {
                        _tagLib.Tag.TrackCount = _count;
                        _changesToSave = true;
                    }
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Set_TrackCount()--->"+ex.Source;
                    _error.Add(ex);
                    if(errorMode == ErrorMode.Strict)
                        throw;
                }
            }
        }

        public string DiscCount {
            get {
                try {
                    return _tagLib.Tag.DiscCount.ToString();
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Get_DiscCount()--->"+ex.Source;
                    _error.Add(ex);
                    if(errorMode == ErrorMode.Strict)
                        throw;
                    else
                        return "DiscCount Read Failure";
                }
            }
            set {
                try {
                    uint _count = 1026;
                    try {
                        _count = Convert.ToUInt32(value);
                    } catch { }
                    if(_count != 1026) {
                        _tagLib.Tag.DiscCount = _count;
                        _changesToSave = true;
                    }
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Set_DiscCount()--->"+ex.Source;
                    _error.Add(ex);
                    if(errorMode == ErrorMode.Strict)
                        throw;
                }
            }
        }

        public string Artist {
            get {
                try {
                    if(HasID3v2Tag) {
                        if(_id3v2.AlbumArtists.Length > 0)
                            return _id3v2.AlbumArtists[0];
                        else if(_id3v2.Performers.Length > 0)
                            return _id3v2.Performers[0];
                        else if(_id3v2.Artists.Length > 0)
                            return _id3v2.Artists[0];
                    }
                    if(HasID3v1Tag) {
                        if(_id3v1.AlbumArtists.Length > 0)
                            return _id3v1.AlbumArtists[0];
                        else if(_id3v1.Performers.Length > 0)
                            return _id3v1.Performers[0];
                        else if(_id3v1.Artists.Length > 0)
                            return _id3v1.Artists[0];
                    }
                    //execution should NEVER reach this next IF-BLOCK;
                    //something has gone terribly awry if it does...
                    if(_tagLib.Tag.AlbumArtists.Length > 0)
                        return _tagLib.Tag.AlbumArtists[0];
                    else if(_tagLib.Tag.Performers.Length > 0)
                        return _tagLib.Tag.Performers[0];
                    else if(_tagLib.Tag.Artists.Length > 0)
                        return _tagLib.Tag.Artists[0];
                    
                    return string.Empty;
                } catch(Exception ex) {
                    ex.Source = _errorBase + ".Get_Artist()--->"+ex.Source;
                    _error.Add(ex);
                    if(errorMode == ErrorMode.Strict)
                        throw;
                    else
                        return "Artist Read Failure";
                }
            }
            set {
                try {
                    string[] _artists = { value };
                    _tagLib.Tag.AlbumArtists = _artists;
                    _tagLib.Tag.Performers = _artists;
                    _tagLib.Tag.Artists = _artists;
                    _changesToSave = true;
                } catch(Exception ex) {
                    ex.Source = _errorBase + ".Set_Artist()--->" + ex.Source;
                    _error.Add(ex);
                    if(errorMode == ErrorMode.Strict)
                        throw;
                }
            }
        }

        public string Album {
            get {
                try {
                    return _tagLib.Tag.Album;
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Get_Album()--->"+ex.Source;
                    _error.Add(ex);
                    if(errorMode == ErrorMode.Strict)
                        throw;
                    else
                        return "Album Read Failure";
                }
            }
            set {
                try {
                    _tagLib.Tag.Album = value;
                    _changesToSave = true;
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Set_Album()--->"+ex.Source;
                    _error.Add(ex);
                    if(errorMode == ErrorMode.Strict)
                        throw;
                }
            }
        }

        public string Title {
            get {
                try {
                    return _tagLib.Tag.Title;
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Get_Title()--->"+ex.Source;
                    _error.Add(ex);
                    if(errorMode == ErrorMode.Strict)
                        throw;
                    else
                        return "Title Read Failure";
                }
            }
            set {
                try {
                    _tagLib.Tag.Title = value;
                    _changesToSave = true;
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Set_Title()--->"+ex.Source;
                    _error.Add(ex);
                    if(errorMode == ErrorMode.Strict)
                        throw;
                }
            }
        }

        public bool HasAlbumArt {
            get {
                try {
                    foreach(TagLib.IPicture ipic in _tagLib.Tag.Pictures) {
                        if(ipic.Type == TagLib.PictureType.FrontCover)
                            return true;
                    }
                    return false;
                } catch(Exception ArtCheck) {
                    ArtCheck.Source = "Checking for art--->" + ArtCheck.Source;
                    _error.Add(ArtCheck);
                    if(errorMode == ErrorMode.Strict)
                        throw;
                    else
                        return false;
                }
            }
        }

        public bool InvalidAlbumArt {
            get {
                return _invalidAlbumArt;
            }
            set {
                _invalidAlbumArt = value;
            }
        }

        public Image AlbumArt {
            get {
                InvalidAlbumArt = false;
                _albumArt = null;
                try {
                    foreach(TagLib.IPicture ipic in _tagLib.Tag.Pictures) {
                        string picType = String.Empty;
                        if(ipic.Type == TagLib.PictureType.FrontCover ||
                           ipic.Type == TagLib.PictureType.BackCover ||
                           ipic.Type == TagLib.PictureType.Other) {

                            try {
                                if(ipic.Data.Data.Length > 0) {
                                    string tempArt = System.IO.Path.GetTempFileName();
                                    if(tempArt.Length >0) {
                                        try {
                                            MemStream = new MemoryStream(ipic.Data.Data);
                                            System.Drawing.Image img = Image.FromStream(MemStream);
                                            img.Save(tempArt);
                                            _albumArt = img;
                                            try {
                                                if(System.IO.File.Exists(tempArt))
                                                    System.IO.File.Delete(tempArt);
                                            } catch(Exception cleanup) {
                                                cleanup.Source = "Cleanup("+tempArt+")--->"+cleanup.Source;
                                            }
                                        } catch(Exception imgCheck) {
                                            imgCheck.Source = "Verifying Image--->"+imgCheck.Source;
                                            InvalidAlbumArt=true;
                                            _error.Add(imgCheck);
                                            if(errorMode == ErrorMode.Strict)
                                                throw;
                                        }
                                    }
                                }
                                else { //atp.Data.Length>0
                                    InvalidAlbumArt = true;
                                }
                            } catch(Exception getArt) {
                                getArt.Source = "Get Art--->"+getArt.Source;
                                _error.Add(getArt);
                                if(errorMode == ErrorMode.Strict)
                                    throw;
                            }
                        }
                        else { //no picture frame, or incorrect picture frame types
                            InvalidAlbumArt = true;
                        }
                    }
                } catch(Exception ex) {
                    InvalidAlbumArt = true;
                    ex.Source = _errorBase + "Get_AlbumArt()--->"+ex.Source;
                    _error.Add(ex);
                    if(errorMode == ErrorMode.Strict)
                        throw;
                }
                return _albumArt;
            }
            set {
                try {
                    string tempArt = System.IO.Path.GetTempFileName();
                    if(tempArt.Length > 0) {
                        TagLib.Picture pic = new TagLib.Picture(tempArt);
                        TagLib.Id3v2.AttachedPictureFrame apf = new TagLib.Id3v2.AttachedPictureFrame(pic);
                        apf.MimeType = GetMIMEType(value);
                        apf.Type = TagLib.PictureType.FrontCover;
                        TagLib.IPicture[] ipic = new TagLib.IPicture[1];
                        ipic[0] = apf;
                        _tagLib.Tag.Pictures = ipic;
                        _changesToSave = true;
                    }
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Set_AlbumArt()--->"+ex.Source;
                    _error.Add(ex);
                    if(errorMode == ErrorMode.Strict)
                        throw;
                }
            }
        }

        public string Year {
            get {
                try {
                    return _tagLib.Tag.Year.ToString();
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Get_Year()--->"+ex.Source;
                    _error.Add(ex);
                    if(errorMode == ErrorMode.Strict)
                        throw;
                    else
                        return "Year Read Failure";
                }
            }
            set {
                try {
                    UInt32 _year = 1026;
                    try {
                        _year = Convert.ToUInt32(value);
                    } catch { }
                    if(_year != 1026) {
                        _tagLib.Tag.Year = _year;
                        _changesToSave = true;
                    }
                } catch(Exception ex) {
                    ex.Source = "Set_Year()--->"+ex.Source;
                    _error.Add(ex);
                    if(errorMode == ErrorMode.Strict)
                        throw;
                }
            }
        }

        public string Track {
            get {
                try {
                    return _tagLib.Tag.Track.ToString();
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Get_Track()--->"+ex.Source;
                    _error.Add(ex);
                    if(errorMode == ErrorMode.Strict)
                        throw;
                    else
                        return "Track Read Failure";
                }
            }
            set {
                try {
                    uint _track = 1026;
                    try {
                        _track = Convert.ToUInt32(value);
                    } catch { }
                    if(_track != 1026) {
                        _tagLib.Tag.Track = _track;
                        _changesToSave = true;
                    }
                } catch(Exception ex) {
                    ex.Source = _errorBase+"Set_Track()--->"+ex.Source;
                    _error.Add(ex);
                    if(errorMode == ErrorMode.Strict)
                        throw;
                }
            }
        }

        public string Disc {
            get {
                try {
                    return _tagLib.Tag.Disc.ToString();
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Get_Disc()--->"+ex.Source;
                    _error.Add(ex);
                    if(errorMode == ErrorMode.Strict)
                        throw;
                    else
                        return "Disc Read Failure";
                }
            }
            set {
                try {
                    uint _disc = 1026;
                    try {
                        _disc = Convert.ToUInt32(value);
                    } catch { }
                    if(_disc != 1026) {
                        _tagLib.Tag.Disc = _disc;
                        _changesToSave = true;
                    }
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Set_Disc()--->"+ex.Source;
                    _error.Add(ex);
                    if(errorMode == ErrorMode.Strict)
                        throw;
                }
            }
        }

        public string Genre {
            get {
                try {
                    if(_tagLib.Tag.Genres.Length > 0)
                        return _tagLib.Tag.Genres[0];
                    else
                        return string.Empty;
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Get_Genre()--->"+ex.Source;
                    _error.Add(ex);
                    if(errorMode == ErrorMode.Strict)
                        throw;
                    else
                        return "Genre Read Failure";
                }
            }
            set {
                try {
                    string[] genre = new string[1];
                    genre[0] = value;
                    _tagLib.Tag.Genres = genre;
                    /*
                    if(_tagLib.Tag.Genres.Length > 0) {
                        string[] _t = new string[1];
                        _t[0] = value;
                        //_tagLib.Tag.Genres[0] = _t;
                        _tagLib.Tag.Genres = _t;
                    }
                    else {
                        string[] _genres = { value };
                        _tagLib.Tag.Genres = _genres;
                    }
                    */
                    _changesToSave = true;
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Set_Genre()--->"+ex.Source;
                    _error.Add(ex);
                    if(errorMode == ErrorMode.Strict)
                        throw;
                }
            }
        }

        public string Lyrics {
            get {
                try {
                    return _tagLib.Tag.Lyrics;
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Get_Lyrics()--->"+ex.Source;
                    _error.Add(ex);
                    if(errorMode == ErrorMode.Strict)
                        throw;
                    else
                        return "Lyrics Read Failure";
                }
            }
            set {
                try {
                    _tagLib.Tag.Lyrics = value;
                    _changesToSave = true;
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Set_Lyrics()--->"+ex.Source;
                    _error.Add(ex);
                    if(errorMode == ErrorMode.Strict)
                        throw;
                }
            }
        }

        public string Comments {
            get {
                try {
                    return _tagLib.Tag.Comment;
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Get_Comments()--->"+ex.Source;
                    _error.Add(ex);
                    if(errorMode == ErrorMode.Strict)
                        throw;
                    else
                        return "Comments Read Failure";
                }
            }
            set {
                try {
                    _tagLib.Tag.Comment = value;
                    _changesToSave = true;
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Set_Comments()--->"+ex.Source;
                    _error.Add(ex);
                    if(errorMode == ErrorMode.Strict)
                        throw;
                }
            }
        }

        #region ID3v2 Direct Methods
        public bool HasID3v2Tag {
            get {
                return !_id3v2.IsEmpty;
            }
        }

        public string Id3v2_Artist {
            get {
                try {
                    if(HasID3v2Tag) {
                        if(_id3v2.AlbumArtists.Length > 0)
                            return _id3v2.AlbumArtists[0];
                        else if(_id3v2.Performers.Length > 0)
                            return _id3v2.Performers[0];
                        else if(_id3v2.Artists.Length > 0)
                            return _id3v2.Artists[0];
                        else
                            return string.Empty;
                    }
                    else
                        return string.Empty;
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Get_Id3v2_Artist()--->"+ex.Source;
                    throw;
                }
            }
            set {
                try {
                    string[] artists = { value };
                    _id3v2.AlbumArtists = artists;
                    _changesToSave = true;
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Set_Id3v2_Artist()--->"+ex.Source;
                    throw;
                }
            }
        }

        public string Id3v2_Album {
            get {
                try {
                    if(HasID3v2Tag) {
                        return _id3v2.Album;
                    }
                    else
                        return string.Empty;
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Get_Id3v2_Album()--->"+ex.Source;
                    throw;
                }
            }
            set {
                try {
                    _id3v2.Album = value;
                    _changesToSave = true;
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Set_Id3v2_Album()--->"+ex.Source;
                    throw;
                }
            }
        }

        public string Id3v2_Title {
            get {
                try {
                    if(HasID3v2Tag) {
                        return _id3v2.Title;
                    }
                    else
                        return string.Empty;
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Get_Id3v2_Title()--->"+ex.Source;
                    throw;
                }
            }
            set {
                _id3v2.Title = value;
                _changesToSave = true;
            }
        }
        #endregion

        #region ID3v1 Direct Methods
        public bool HasID3v1Tag {
            get {
                return !_id3v1.IsEmpty;
            }
        }

        public string Id3v1_Artist {
            get {
                try {
                    if(HasID3v1Tag) {
                        if(_id3v1.AlbumArtists.Length > 0)
                            return _id3v1.AlbumArtists[0];
                        else if(_id3v1.Performers.Length > 0)
                            return _id3v1.FirstPerformer;
                        else if(_id3v1.Artists.Length > 0)
                            return _id3v1.Artists[0];
                        else
                            return string.Empty;
                    }
                    else
                        return string.Empty;
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Get_Id3v1_Artist()--->"+ex.Source;
                    throw;
                }
            }
            set {
                try {
                    string[] artists = { value };
                    if(artists[0].Length > 30)
                        artists[0] = artists[0].Substring(0, 29);
                    _id3v1.AlbumArtists = artists;
                    _id3v1.Performers = artists;
                    _id3v1.Artists = artists;
                    _changesToSave = true;
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Set_Id3v1_Artist()--->"+ex.Source;
                    throw;
                }
            }
        }

        public string Id3v1_Album {
            get {
                try {
                    if(HasID3v1Tag)
                        return _id3v1.Album;
                    else
                        return string.Empty;
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Get_Id3v1_Album()--->"+ex.Source;
                    throw;
                }
            }
            set {
                try {
                    if(value.Length > 30)
                        _id3v1.Album = value.Substring(0, 29);
                    else
                        _id3v1.Album = value;
                    _changesToSave = true;
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Set_Id3v1_Album()--->"+ex.Source;
                    throw;
                }
            }
        }

        public string Id3v1_Title {
            get {
                try {
                    if(HasID3v1Tag) {
                        return _id3v1.Title;
                    }
                    else
                        return string.Empty;
                } catch(Exception ex) {
                    ex.Source = _errorBase + "Get_Id3v1_Title()--->"+ex.Source;
                    throw;
                }
            }
            set {
                try {
                    if(value.Length > 30)
                        _id3v1.Title = value.Substring(0, 29);
                    else
                        _id3v1.Title = value;
                    _changesToSave = true;
                } catch(Exception ex) {
                    ex.Source = "Set_Id3v1_Title()--->"+ex.Source;
                    throw;
                }
            }
        }
        #endregion

        #region IDisposable Members
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing) {
            if(!this._disposed) {
                if(disposing) {
                    if(_tagLib != null) {
                        if(_changesToSave)
                            _tagLib.Save();
                    }
                }
                _disposed=true;
            }
        }
        #endregion
    }
}