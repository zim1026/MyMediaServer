using System;
using System.Collections.Generic;
using System.Drawing;

namespace ID3TagLib {
    public class ID3Tag1 {
        #region Privates
        #region Basic
        private string _artist = string.Empty;
        private string _album = string.Empty;
        private string _title = string.Empty;
        private string _year = string.Empty;
        private string _path = string.Empty;
        private string _genre = string.Empty;
        private string _comments = string.Empty;
        private bool _hasAlbumArt;
        private Image _albumArt;
        private string _lyrics = string.Empty;
        private string _track = string.Empty;
        private string _trackCount = string.Empty;
        private string _discNum = string.Empty;
        private string _discCount = string.Empty;        
        #endregion Basic

        #region ID3v1
        private bool _hasID3v1;
        private string _v1Artist = string.Empty;
        private string _v1Album = string.Empty;
        private string _v1Title = string.Empty;
        #endregion ID3v1

        #region ID3v2
        private bool _hasID3v2;
        private string _v2Artist = string.Empty;
        private string _v2Album = string.Empty;
        private string _v2Title = string.Empty;
        #endregion ID3v2

        #region Identifiers
        private string _amazonID = string.Empty;
        private string _mbArtistID = string.Empty;
        private string _mbCountry = string.Empty;
        private string _mbDiscID = string.Empty;
        private string _mbTrackID = string.Empty;
        private string _mbReleaseArtistID = string.Empty;
        private string _mbReleaseID = string.Empty;
        private string _mbReleaseStatus = string.Empty;
        private string _mbReleaseType = string.Empty;
        private string _musicIPID = string.Empty;
        #endregion Identifier

        #region Collections
        private string[] _artists;
        private string[] _composers;
        private string[] _performers;
        private string _conductor;
        private List<string> _errors;
        #endregion Collections

        #region Audio
        private int _bitRate;
        private int _channels;
        private int _sampleRate;
        private int _bitsPerSample;
        private uint _beatsPerMinute;
        private TimeSpan _duration;
        #endregion Audio

        #region Hacks
        string _imageFile = string.Empty;
        #endregion Hacks
        #endregion Privates

        public string ImageFile {
            get { return _imageFile; }
            set { _imageFile = value; }
        }
        
        public string TrackCount {
            get { return _trackCount; }
            set { _trackCount = value; }
        }

        public string DiscCount {
            get { return _discCount; }
            set { _discCount = value; }
        }
        
        public string AmazonID {
            get { return _amazonID; }
            set { _amazonID = value; }
        }

        public string mbArtistID {
            get { return _mbArtistID; }
            set { _mbArtistID = value; }
        }

        public string mbCountry {
            get { return _mbCountry; }
            set { _mbCountry = value; }
        }

        public string mbDiscID {
            get { return _mbDiscID; }
            set { _mbDiscID = value; }
        }

        public string mbTrackID {
            get { return _mbTrackID; }
            set { _mbTrackID = value; }
        }

        public string mbReleaseArtistID {
            get { return _mbReleaseArtistID; }
            set { _mbReleaseArtistID = value; }
        }

        public string mbReleaseID {
            get { return _mbReleaseID; }
            set { _mbReleaseID = value; }
        }

        public string mbReleaseStatus {
            get { return _mbReleaseStatus; }
            set { _mbReleaseStatus = value; }
        }

        public string mbReleaseType {
            get { return _mbReleaseType; }
            set { _mbReleaseType = value; }
        }

        public string MusicIPID {
            get { return _musicIPID; }
            set { _musicIPID = value; }
        }
        
        public TimeSpan Duration {
            get { return _duration; }
            set { _duration = value; }
        }
        
        public uint BeatsPerMinute {
            get { return _beatsPerMinute; }
            set { _beatsPerMinute = value; }
        }
        
        public int BitsPerSample {
            get { return _bitsPerSample; }
            set { _bitsPerSample = value; }
        }

        public int SampleRate {
            get { return _sampleRate; }
            set { _sampleRate =value; }
        }
        
        public int Channels {
            get { return _channels; }
            set { _channels = value; }
        }
        
        public int BitRate {
            get { return _bitRate; }
            set { _bitRate = value; }
        }

        public List<string> errors {
            get { return _errors; }
            set { _errors = value; }
        }

        public string[] artists {
            get { return _artists; }
            set { _artists = value; }
        }

        public string[] composers {
            get { return _composers; }
            set { _composers = value; }
        }

        public string[] performers {
            get { return _performers; }
            set { _performers = value; }
        }

        public string conductor {
            get { return _conductor; }
            set { _conductor = value; }
        }
        
        public string artist {
            get { return _artist; }
            set { _artist = value; }
        }

        public string album {
            get { return _album; }
            set { _album = value; }
        }

        public string title {
            get { return _title; }
            set { _title = value; }
        }

        public string year {
            get { return _year; }
            set { _year = value; }
        }

        public string track {
            get { return _track; }
            set { _track = value; }
        }

        public string discNum {
            get { return _discNum; }
            set { _discNum = value; }
        }

        public string path {
            get { return _path; }
            set { _path = value; }
        }

        public string genre {
            get { return _genre; }
            set { _genre = value; }
        }

        public bool HasID3v1Tag {
            get { return _hasID3v1; }
            set { _hasID3v1 = value; }
        }

        public string v1Artist {
            get { return _v1Artist; }
            set { _v1Artist = value; }
        }

        public string v1Album {
            get { return _v1Album; }
            set { _v1Album = value; }
        }

        public string v1Title {
            get { return _v1Title; }
            set { _v1Title = value; }
        }

        public bool HasID3v2Tag {
            get { return _hasID3v2; }
            set { _hasID3v2 = value; }
        }

        public string v2Artist {
            get { return _v2Artist; }
            set { _v2Artist = value; }
        }

        public string v2Album {
            get { return _v2Album; }
            set { _v2Album = value; }
        }

        public string v2Title {
            get { return _v2Title; }
            set { _v2Title = value; }
        }

        public bool HasAlbumArt {
            get { return _hasAlbumArt; }
            set { _hasAlbumArt = value; }
        }

        public Image albumArt {
            get { return _albumArt; }
            set { _albumArt = value; }
        }

        public string Lyrics {
            get { return _lyrics; }
            set { _lyrics = value; }
        }

        public string Comments {
            get { return _comments; }
            set { _comments = value; }
        }

        public ID3Tag1(string path, string artist, string album, string title) {
            _errors = new List<string>();
            _artist = artist;
            _album = album;
            _title = title;
        }

        public ID3Tag1() {
            _errors = new List<string>();
        }
    }
}
