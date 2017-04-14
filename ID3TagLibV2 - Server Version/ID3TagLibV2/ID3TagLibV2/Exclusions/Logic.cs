using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Catalog {
    public class Logic {
        private ID3TagLib.Error _errorList;

        #region Private Methods
        private void CreateDirectory(string directory) {
            try {
                if(!Directory.Exists(directory))
                    Directory.CreateDirectory(directory.Trim(ID3TagLib.IllegalChars.IllegalFileNameChars).Trim(ID3TagLib.IllegalChars.NonASCII));
            }
            catch(Exception ex) {
                ex.Source = "CreateDirectory("+directory+")--->"+ex.Source;
                throw;
            }
        }

        private bool ValidTags(string source){
            bool valid = true;
            string artist = string.Empty;
            string album = string.Empty;
            string title = string.Empty;
            using(ID3TagLib.ID3Info _id3 = new ID3TagLib.ID3Info(source, ID3TagLib.ID3Info.ErrorMode.Strict)) {
                artist = ID3TagLib.IllegalChars.Legalize(_id3.Artist);
                if(artist == string.Empty || artist.Length <= 0)
                    valid = false;
                
                if(valid){
                    album = ID3TagLib.IllegalChars.Legalize(_id3.Album);
                    if(album == string.Empty || album.Length <=0)
                        valid = false;
                    if(valid){
                        title = ID3TagLib.IllegalChars.Legalize(_id3.Title);
                        if(title == string.Empty || title.Length <=0)
                            valid = false;
                        if(valid) {
                            string testDir = Path.GetTempPath();
                            testDir = testDir + Path.DirectorySeparatorChar +
                                        artist + Path.DirectorySeparatorChar +
                                        album + Path.DirectorySeparatorChar;
                            try {
                                CreateDirectory(testDir);
                            } catch(Exception ex) {
                                valid = false;
                            }
                            if(!Directory.Exists(testDir))
                                valid = false;
                            else {
                                try {
                                    Directory.Delete(testDir);
                                } catch { }
                            }
                        }
                    }
                }
            }
            return valid;
        }

        private void OrganizeDirectoryFiles(string source, string destination, char directorySeparatorChar) {
            try {
                foreach(string file in Directory.GetFiles(source, "*.mp3")) {
                    if(ValidTags(file))
                        Organize(file, destination, directorySeparatorChar,true);
                }
            }
            catch(Exception ex) {
                ex.Source = "OrganizeDirectoryFiles("+source+","+destination+","+directorySeparatorChar+")--->"+ex.Source;
                throw;
            }
        }

        private void UpdateDirectoryFiles(string path, ID3TagLib.ID3Tag id3Tag) {
            try {
                foreach(string file in Directory.GetFiles(path, "*.mp3")) {
                    using(ID3TagLib.ID3Info _id3Info = new ID3TagLib.ID3Info(file, ID3TagLib.ID3Info.ErrorMode.Strict)) {
                        _id3Info.Artist = id3Tag.artist;
                        _id3Info.Album = id3Tag.album;
                        _id3Info.Title = id3Tag.title;
                        _id3Info.Genre = id3Tag.genre;
                        _id3Info.Year = id3Tag.year;

                        if(id3Tag.albumArt != null)
                            _id3Info.AlbumArt = id3Tag.albumArt;

                        _ErrorList = _id3Info.Errors;
                    }
                }
            }
            catch(Exception ex) {
                ex.Source = "UpdateDirectoryFiles("+path+",ID3TagLib.ID3Tag)--->"+ex.Source;
                throw;
            }
        }

        private ID3TagLib.Error _ErrorList {
            set { _errorList = value; }
        }
        #endregion

        public ID3TagLib.Error ErrorList {
            get { return _errorList; }
        }

        public ID3TagLib.ID3Tag HandleFile(string fileName) {
            try {
                ID3TagLib.ID3Tag _tag = new ID3TagLib.ID3Tag();
                using(ID3TagLib.ID3Info _id3 = new ID3TagLib.ID3Info(fileName, ID3TagLib.ID3Info.ErrorMode.Strict)) {
                    _tag = _id3.GetAllTags(true);
                    _ErrorList = _id3.Errors;
                }
                return _tag;
            }
            catch(Exception ex) {
                ex.Source = "HandleFile("+fileName+")--->"+ex.Source;
                throw;
            }
        }

        public void OrganizeDirectory(string directoryToSearch, string destination, char directorySeparatorChar) {
            try {
                Stack<string> searchStack = new Stack<string>();
                searchStack.Push(directoryToSearch);
                while(searchStack.Count > 0) {
                    string directory = searchStack.Pop();
                    OrganizeDirectoryFiles(directory, destination, directorySeparatorChar);
                    foreach(string dir in Directory.GetDirectories(directory)) {
                        searchStack.Push(dir);
                    }
                }
            }
            catch(Exception ex) {
                ex.Source = "OrganizeDirectory("+directoryToSearch+","+destination+","+directorySeparatorChar+")--->"+ex.Source;
                throw;
            }
        }
        
        public void UpdateDirectory(string directoryToSearch, ID3TagLib.ID3Tag id3Tag) {
            try {
                Stack<string> searchStack = new Stack<string>();
                searchStack.Push(directoryToSearch);
                while(searchStack.Count > 0) {
                    string directory = searchStack.Pop();
                    UpdateDirectoryFiles(directory, id3Tag);
                    foreach(string dir in Directory.GetDirectories(directory)) {
                        searchStack.Push(dir);
                    }
                }
            }
            catch(Exception ex) {
                ex.Source = "UpdateDirectory("+directoryToSearch+",ID3TagLib.ID3Tag)--->"+ex.Source;
                throw;
            }
        }
        
        public void RemoveID3v1Tags(string fileName) {
            try {
                using(ID3TagLib.ID3Info _id3 = new ID3TagLib.ID3Info(fileName, ID3TagLib.ID3Info.ErrorMode.Strict)){
                    _id3.RemoveID3v1Tags();
                    _ErrorList = _id3.Errors;
                }
            }
            catch(Exception ex) {
                ex.Source = "RemoveID3v1Tags("+fileName+")--->"+ex.Source;
                throw;
            }
        }

        public void RemoveID3v2Tags(string fileName) {
            try {
                using(ID3TagLib.ID3Info _id3 = new ID3TagLib.ID3Info(fileName, ID3TagLib.ID3Info.ErrorMode.Strict)){
                    _id3.RemoveID3v2Tags();
                    _ErrorList = _id3.Errors;
                }
            }
            catch(Exception ex) {
                ex.Source = "RemoveID3v2Tags(" + fileName + ")--->"+ex.Source;
                throw;
            }
        }

        public void RemoveAllTags(string fileName) {
            try {
                using(ID3TagLib.ID3Info _id3 = new ID3TagLib.ID3Info(fileName, ID3TagLib.ID3Info.ErrorMode.Strict)) {
                    _id3.RemoveID3v1Tags();
                    _id3.RemoveID3v2Tags();
                    _ErrorList = _id3.Errors;
                }
            }
            catch(Exception ex) {
                ex.Source = "RemoveAllTags("+fileName+")--->"+ex.Source;
                throw;
            }
        }
        
        public string Organize(string source, string destination, char DirectorySeparator, bool moveFile) {
            try {
                using(ID3TagLib.ID3Info _id3Info = new ID3TagLib.ID3Info(source, ID3TagLib.ID3Info.ErrorMode.Strict)) {
                    if(!destination.EndsWith(DirectorySeparator.ToString()))
                        destination = destination + DirectorySeparator;

                    destination = destination + 
                                    ID3TagLib.IllegalChars.Legalize(_id3Info.Artist) +
                                    DirectorySeparator + 
                                    ID3TagLib.IllegalChars.Legalize(_id3Info.Album);
                    CreateDirectory(destination);
                    _ErrorList = _id3Info.Errors;
                }
                if(Directory.Exists(destination)) {
                    string destFile = destination + DirectorySeparator + Path.GetFileName(source);
                    if(!File.Exists(destFile))
                        if(moveFile)
                            File.Move(source, destFile);
                        else
                            File.Copy(source, destFile);
                }

                return destination;
            }
            catch(Exception ex) {
                ex.Source = "Organize("+source+","+destination+")--->"+ex.Source;
                throw;
            }
        }

        public ID3TagLib.ID3Tag TagFromPath(string source, string delimiter) {
            try {
                ID3TagLib.ID3Tag _tag = new ID3TagLib.ID3Tag();
                string[] parts = source.Split(delimiter.ToCharArray());

                if(parts.Length > 2) {
                    _tag.title = parts[parts.Length-1];
                    _tag.album = parts[parts.Length-2];
                    _tag.artist = parts[parts.Length-3];                    
                    
                    using(ID3TagLib.ID3Info _id3Info = new ID3TagLib.ID3Info(source, ID3TagLib.ID3Info.ErrorMode.Strict)) {
                        _id3Info.Artist = _tag.artist;
                        _id3Info.Album = _tag.album;
                        _id3Info.Title = _tag.title;

                        _ErrorList = _id3Info.Errors;
                    }
                }
                return _tag;
            }
            catch(Exception ex) {
                ex.Source = "TagFromPath("+source+")--->"+ex.Source;
                throw;
            }
        }

        public void SetTagValues(string fileName, ID3TagLib.ID3Tag tag) {
            try {
                using(ID3TagLib.ID3Info _id3Info = new ID3TagLib.ID3Info(fileName, ID3TagLib.ID3Info.ErrorMode.Strict)) {
                    _id3Info.Album = tag.album;
                    _id3Info.Artist = tag.artist;
                    _id3Info.Artists = tag.artists;
                    _id3Info.Title = tag.title;
                    
                    _id3Info.Lyrics = tag.Lyrics;

                    _id3Info.Year = tag.year;
                    _id3Info.Track = tag.track;
                    _id3Info.Disc = tag.discNum;
                    _id3Info.Genre = tag.genre;
                    _id3Info.Comments = tag.Comments;

                    _id3Info.Composers = tag.composers;
                    _id3Info.Performers = tag.performers;
                    _id3Info.Conductor = tag.conductor;

                    if(tag.albumArt != null) {
                        if(tag.ImageFile != string.Empty)
                            _id3Info.AddAlbumArt(tag.ImageFile);
                    }
                    /*
                    else {
                        if(_id3Info.HasAlbumArt)
                            _id3Info.RemoveEmbeddedArt(false);
                    }
                    */

                    //if(tag.v1Album != string.Empty)
                        _id3Info.Id3v1_Album = tag.v1Album;
                    //if(tag.v1Artist != string.Empty)
                        _id3Info.Id3v1_Artist = tag.v1Artist;
                    //if(tag.v1Title != string.Empty)
                        _id3Info.Id3v1_Title = tag.v1Title;
                    //if(tag.v2Album != string.Empty)
                        _id3Info.Id3v2_Album = tag.v2Album;
                    //if(tag.v2Artist != string.Empty)
                        _id3Info.Id3v2_Artist = tag.v2Artist;
                    //if(tag.v2Title != string.Empty)
                        _id3Info.Id3v2_Title = tag.v2Title;

                    _ErrorList = _id3Info.Errors;
                }
            }
            catch(Exception ex) {
                ex.Source = "SetTagValues("+fileName+")--->"+ex.Source;
                throw;
            }
        }
    }
}
