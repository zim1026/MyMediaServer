namespace ID3TagLibV2{
    using System;
    using System.Collections.Generic;
    public abstract class TagHandler : Helpers
    {
        #region Privates
        private static List<ArtCollecton> GetImages(TagLib.IPicture[] pics) {
            List<ArtCollecton> images = new List<ArtCollecton>();
            foreach(TagLib.IPicture pic in pics)
                if(ValidData(pic.Data.Data, true))
                    images.Add(new ArtCollecton() { type = pic.Type, ImageData = pic.Data.Data });
            return images;
        }

        private static List<ArtCollecton> GetImages(ID3TagLibV2.ID3Tag tag) {
            List<ArtCollecton> ac = new List<ArtCollecton>();
            if(tag.ArtistImage != null && tag.ArtistImage.Length > 0)
                ac.Add(new ArtCollecton() { ImageData = tag.ArtistImage, type = TagLib.PictureType.Artist });
            if(tag.AlbumArt != null && tag.AlbumArt.Length > 0)
                ac.Add(new ArtCollecton() { ImageData = tag.AlbumArt, type = TagLib.PictureType.FrontCover });
            if(tag.BackCoverImage != null && tag.BackCoverImage.Length > 0)
                ac.Add(new ArtCollecton() { ImageData = tag.BackCoverImage, type = TagLib.PictureType.BackCover });
            if(tag.BandImage != null && tag.BandImage.Length > 0)
                ac.Add(new ArtCollecton() { ImageData = tag.BandImage, type = TagLib.PictureType.Band });
            return ac;
        }

        private static TagLib.IPicture[] ProcessPictures(List<ArtCollecton> Images) {
            List<TagLib.IPicture> ipics = new List<TagLib.IPicture>();

            foreach(ArtCollecton ac in Images) {
                ipics.Add(PicFrame(ac.ImageData, ac.type));
            }

            return ipics.ToArray();
        }
        #endregion Privates

        public static ID3Tag ReadFileTags(string filename) {
            try {
                ID3Tag tag = new ID3Tag();
                using(TagLib.File tlf = TagLib.File.Create(filename)) {
                    tlf.Mode = TagLib.File.AccessMode.Read;

                    TagLib.Id3v1.Tag id3v1 = (tlf.GetTag(TagLib.TagTypes.Id3v1) as TagLib.Id3v1.Tag);
                    TagLib.Id3v2.Tag id3v2 = (tlf.GetTag(TagLib.TagTypes.Id3v2) as TagLib.Id3v2.Tag);
                    
                    tag.FileName = filename;
                    tag.Bitrate = tlf.Properties.AudioBitrate;
                    tag.SampleRate = tlf.Properties.AudioSampleRate;
                    tag.Duration = 
                        tlf.Properties.Duration.Hours > 0 ? tlf.Properties.Duration.Hours.ToString("00") + ":" : string.Empty
                            + tlf.Properties.Duration.Minutes.ToString("00") + ":" 
                            + tlf.Properties.Duration.Seconds.ToString("00");

                    if(id3v2 != null) {
                        if (id3v2.AlbumArtists.Length > 0)
                            tag.AlbumArtists = id3v2.AlbumArtists;
                        else if (id3v2.Artists.Length > 0)
                            tag.AlbumArtists = id3v2.Artists;
                        else if (id3v2.Performers.Length > 0)
                            tag.AlbumArtists = id3v2.Performers;

                        if (id3v2.Artists.Length > 0)
                            tag.Artists = id3v2.Artists;
                        else if (id3v2.AlbumArtists.Length > 0)
                            tag.Artists = id3v2.AlbumArtists;
                        else if (id3v2.Performers.Length > 0)
                            tag.Artists = id3v2.Performers;

                        tag.Album = string.IsNullOrWhiteSpace(id3v2.Album) ? string.Empty : id3v2.Album;
                        tag.Title = string.IsNullOrWhiteSpace(id3v2.Title) ? string.Empty : id3v2.Title;
                        tag.Performers = id3v2.Performers;
                        tag.Composers = id3v2.Composers;
                        tag.Genres = id3v2.Genres;
                        tag.Year = id3v2.Year;
                        tag.TrackNum = id3v2.Track;
                        tag.DiscNum = id3v2.Disc;
                        tag.Comment = string.IsNullOrWhiteSpace(id3v2.Comment) ? string.Empty : id3v2.Comment;
                        tag.Copyright = string.IsNullOrWhiteSpace(id3v2.Copyright) ? string.Empty : id3v2.Copyright;
                        tag.Lyrics = string.IsNullOrWhiteSpace(id3v2.Lyrics) ? string.Empty : id3v2.Lyrics;
                        tag.Conductor = string.IsNullOrWhiteSpace(id3v2.Conductor) ? string.Empty : id3v2.Conductor;

                        foreach(ArtCollecton c in GetImages(id3v2.Pictures)) {
                            if(c.type == TagLib.PictureType.FrontCover)
                                tag.AlbumArt = c.ImageData;
                            else if(c.type == TagLib.PictureType.BackCover)
                                tag.BackCoverImage = c.ImageData;
                            else if(c.type == TagLib.PictureType.Artist)
                                tag.ArtistImage = c.ImageData;
                            else if(c.type == TagLib.PictureType.Band)
                                tag.BandImage = c.ImageData;
                        }
                    }

                    if(id3v1 != null) {
                        tag.id3v1Artists = id3v1.Artists;
                        tag.id3v1Album = id3v1.Album;
                        tag.id3v1Title = id3v1.Title;
                    }

                    //If no ID3v2 values, check for and use ID3v1 balues
                    if(tag.AlbumArtist.Length == 0 && tag.id3v1Artist.Length > 0)
                        tag.AlbumArtists = tag.id3v1Artists;
                    if(String.IsNullOrWhiteSpace(tag.Album) && !String.IsNullOrWhiteSpace(tag.id3v1Album))
                        tag.Album = tag.id3v1Album;
                    if(String.IsNullOrWhiteSpace(tag.Title) && !String.IsNullOrWhiteSpace(tag.id3v1Title))
                        tag.Title = tag.id3v1Title;

                    //If no ID3v1 values, check for and use ID3v2 values
                    if(String.IsNullOrWhiteSpace(tag.id3v1Artist) && !String.IsNullOrWhiteSpace(tag.AlbumArtist))
                        tag.id3v1Artists = ID3Tag.id3v1StringArray(tag.AlbumArtists);
                    if(String.IsNullOrWhiteSpace(tag.id3v1Album) && !String.IsNullOrWhiteSpace(tag.Album))
                        tag.id3v1Album = ID3Tag.id3v1String(tag.Album);
                    if(String.IsNullOrWhiteSpace(tag.id3v1Title) && !String.IsNullOrWhiteSpace(tag.Title))
                        tag.id3v1Title = ID3Tag.id3v1String(tag.Title);

                    tlf.Mode = TagLib.File.AccessMode.Closed;
                }

                if (tag.AlbumArtists[0] != tag.Artists[0])
                {
                    string commentsAddendum = "AlbumArtists/Artists mismatch: ";

                    if (tag.Performers.Length > 0)
                    {
                        if (tag.Artists[0] == tag.Performers[0])
                        {
                            commentsAddendum = commentsAddendum + "Set AlbumArtists equal to Performers.";
                            tag.AlbumArtists = tag.Performers;
                        }
                        else if (tag.AlbumArtists[0] == tag.Performers[0])
                        {
                            commentsAddendum = commentsAddendum + "Set Artists equal to Performers.";
                            tag.Artists = tag.Performers;
                        }
                    }
                    else
                    {
                        commentsAddendum = commentsAddendum + "Performers not found, set AlbumArtists equal to Artists. ";
                        tag.AlbumArtists = tag.Artists;
                    }

                    tag.Comment = commentsAddendum + (tag.Comment.Length > 0 ? " " : string.Empty) + tag.Comment;
                    // UpdateFileTags(filename, tag);
                }

                return tag;
            } catch(Exception ex) {
                ex.Source = System.Reflection.Assembly.GetExecutingAssembly() + ".ReadFileTags(" + filename + ")->" + ex.Source;
                throw;
            }
        }

        public static void UpdateFileTags(string filename, ID3Tag tag) {
            try {
                using(TagLib.File tlf = TagLib.File.Create(filename)) {
                    tlf.Mode = TagLib.File.AccessMode.Write;

                    TagLib.Id3v1.Tag id3v1 = (tlf.GetTag(TagLib.TagTypes.Id3v1,
                                                         !(tlf.GetTag(TagLib.TagTypes.Id3v1) as TagLib.Id3v1.Tag).IsEmpty) as TagLib.Id3v1.Tag);
                    TagLib.Id3v2.Tag id3v2 = (tlf.GetTag(TagLib.TagTypes.Id3v2,
                                                         !(tlf.GetTag(TagLib.TagTypes.Id3v2) as TagLib.Id3v2.Tag).IsEmpty) as TagLib.Id3v2.Tag);

                    List<TagLib.Id3v2.Frame> killFrameList = new List<TagLib.Id3v2.Frame>();
                    foreach(TagLib.Id3v2.Frame f in id3v2.GetFrames()) {
                        if(f.ToString().ToLower().Contains("privateframe"))
                            killFrameList.Add(f);
                    }

                    if(killFrameList.Count > 0) {
                        foreach(TagLib.Id3v2.Frame f in killFrameList) {
                            id3v2.RemoveFrame(f);
                        }
                    }

                    id3v1.Artists = tag.id3v1Artists;
                    id3v1.Album = tag.id3v1Album;
                    id3v1.Title = tag.id3v1Title;

                    id3v2.Artists = tag.Artists;
                    id3v2.AlbumArtists = tag.AlbumArtists;
                    id3v2.Performers = tag.Performers;
                    id3v2.Album = tag.Album;
                    id3v2.Title = tag.Title;
                    id3v2.Genres = tag.Genres;
                    id3v2.Year = tag.Year;
                    id3v2.Track = tag.TrackNum;
                    id3v2.Disc = tag.DiscNum;
                    id3v2.Conductor = tag.Conductor;
                    id3v2.Lyrics = tag.Lyrics;
                    id3v2.Composers = tag.Composers;

                    id3v2.Copyright = string.Empty;
                    id3v2.Comment = String.IsNullOrWhiteSpace(tag.Comment) ?
                                        String.Empty : tag.Comment.ToUpper().Contains("AMAZON") ?
                                            string.Empty : tag.Comment;                    
                    
                    id3v2.Pictures = ProcessPictures(GetImages(tag));

                    tlf.Save();
                    tlf.Mode = TagLib.File.AccessMode.Closed;
                }
            } catch(Exception ex) {
                ex.Source = System.Reflection.Assembly.GetExecutingAssembly() + ".UpdateFileTags(" + filename + ")->" + ex.Source;
                throw;
            }
        }

        public static void PartialUpdate(string filename, ID3Tag tag, List<ID3Fields> Updates) {
            try {
                using(TagLib.File tlf = TagLib.File.Create(filename)) {
                    tlf.Mode = TagLib.File.AccessMode.Write;

                    TagLib.Id3v1.Tag id3v1 = (tlf.GetTag(TagLib.TagTypes.Id3v1,
                                                         !(tlf.GetTag(TagLib.TagTypes.Id3v1) as TagLib.Id3v1.Tag).IsEmpty) as TagLib.Id3v1.Tag);
                    TagLib.Id3v2.Tag id3v2 = (tlf.GetTag(TagLib.TagTypes.Id3v2,
                                                         !(tlf.GetTag(TagLib.TagTypes.Id3v2) as TagLib.Id3v2.Tag).IsEmpty) as TagLib.Id3v2.Tag);

                    foreach(ID3Fields update in Updates) {
                        switch(update) {
                            case ID3Fields.id3v1Artists:
                                id3v1.Artists = tag.id3v1Artists;
                                break;
                            case ID3Fields.id3v1Artist:
                                if(id3v1.Artists.Length == 0)
                                    id3v1.Artists = new string[1] { tag.id3v1Artist };
                                else
                                    id3v1.Artists[0] = tag.id3v1Artist;
                                break;
                            case ID3Fields.AlbumArtists:
                                id3v2.AlbumArtists = tag.AlbumArtists;
                                break;
                            case ID3Fields.AlbumArtist:
                                if(id3v2.AlbumArtists.Length == 0)
                                    id3v2.AlbumArtists = new string[1] { tag.AlbumArtist };
                                else
                                    id3v2.AlbumArtists[0] = tag.AlbumArtist;
                                break;
                            case ID3Fields.Album:
                                id3v2.Album = tag.Album;
                                break;
                            case ID3Fields.id3v1Album:
                                id3v1.Album = ID3Tag.id3v1String(tag.id3v1Album);
                                break;
                            case ID3Fields.Title:
                                id3v2.Title = tag.Title;
                                break;
                            case ID3Fields.id3v1Title:
                                id3v1.Title = ID3Tag.id3v1String(tag.id3v1Title);
                                break;
                            case ID3Fields.Genres:
                                id3v2.Genres = tag.Genres;
                                break;
                            case ID3Fields.Genre:
                                if(id3v2.Genres.Length == 0)
                                    id3v2.Genres = new string[1] { tag.Genre };
                                else
                                    id3v2.Genres[0] = tag.Genre;
                                break;
                            case ID3Fields.Year:
                                id3v2.Year = tag.Year;
                                break;
                            case ID3Fields.Lyrics:
                                id3v2.Lyrics = tag.Lyrics;
                                break;
                            case ID3Fields.TrackNum:
                                id3v2.Track = tag.TrackNum;
                                break;
                            case ID3Fields.DiscNum:
                                id3v2.Disc = tag.DiscNum;
                                break;
                            case ID3Fields.Comment:
                                //id3v2.Comment = tag.Comment;
                                id3v2.Comment = String.IsNullOrWhiteSpace(tag.Comment) ? 
                                                    String.Empty : tag.Comment.ToUpper().Contains("AMAZON") ? 
                                                        string.Empty : tag.Comment;
                                break;
                            case ID3Fields.Conductor:
                                id3v2.Conductor = tag.Conductor;
                                break;
                            case ID3Fields.Composers:
                                id3v2.Composers = tag.Composers;
                                break;
                            case ID3Fields.Performers:
                                id3v2.Performers = tag.Performers;
                                break;
                            case ID3Fields.AlbumArt:
                                id3v2.Pictures = ProcessPictures(GetImages(tag));
                                break;
                            case ID3Fields.BackCoverImage:
                                id3v2.Pictures = ProcessPictures(GetImages(tag));
                                break;
                            case ID3Fields.ArtistImage:
                                id3v2.Pictures = ProcessPictures(GetImages(tag));
                                break;
                            case ID3Fields.BandImage:
                                id3v2.Pictures = ProcessPictures(GetImages(tag));
                                break;
                        }
                    }

                    List<TagLib.Id3v2.Frame> killFrameList = new List<TagLib.Id3v2.Frame>();
                    foreach(TagLib.Id3v2.Frame f in id3v2.GetFrames()) {
                        if(f.ToString().ToLower().Contains("privateframe"))
                            killFrameList.Add(f);
                    }

                    if(killFrameList.Count > 0) {
                        foreach(TagLib.Id3v2.Frame f in killFrameList) {
                            id3v2.RemoveFrame(f);
                        }
                    }

                    tlf.Save();
                    tlf.Mode = TagLib.File.AccessMode.Closed;
                }
            } catch(Exception ex) {
                ex.Source = System.Reflection.Assembly.GetExecutingAssembly() + ".PartialUpdate(" + filename + ")->" + ex.Source;
                throw;
            }
        }

        public static void RemoveAllTags(string filename) {
            try {
                using(TagLib.File tlf = TagLib.File.Create(filename)) {
                    tlf.Mode = TagLib.File.AccessMode.Write;
                    tlf.RemoveTags(TagLib.TagTypes.AllTags);
                    tlf.Save();
                    tlf.Mode = TagLib.File.AccessMode.Closed;
                }
            } catch(Exception ex) {
                ex.Source = System.Reflection.Assembly.GetExecutingAssembly() + ".RemoveAllTags(" + filename + ")->" + ex.Source;
                throw;
            }
        }

        public static bool DRM_Found(string filename) {
            try {
                bool returnValue = false;

                using(TagLib.File tlf = TagLib.File.Create(filename)) {
                    tlf.Mode = TagLib.File.AccessMode.Read;

                    TagLib.Id3v1.Tag id3v1 = (tlf.GetTag(TagLib.TagTypes.Id3v1,
                                                !(tlf.GetTag(TagLib.TagTypes.Id3v1) as 
                                                    TagLib.Id3v1.Tag).IsEmpty) as TagLib.Id3v1.Tag);

                    TagLib.Id3v2.Tag id3v2 = (tlf.GetTag(TagLib.TagTypes.Id3v2,
                                                !(tlf.GetTag(TagLib.TagTypes.Id3v2) as
                                                    TagLib.Id3v2.Tag).IsEmpty) as TagLib.Id3v2.Tag);

                    List<TagLib.Id3v2.Frame> killFrameList = new List<TagLib.Id3v2.Frame>();
                    foreach(TagLib.Id3v2.Frame f in id3v2.GetFrames()) {
                        if(f.ToString().ToLower().Contains("privateframe")) {
                            killFrameList.Add(f);
                            break;
                        }
                    }

                    if(killFrameList.Count > 0)
                        returnValue = true;

                    if(!returnValue) {  //check comments field
                        if(id3v2.Comment != null) {
                            if(id3v2.Comment.Length > 0) {
                                if(id3v2.Comment.ToUpper().Contains("AMAZON"))
                                    returnValue = true;
                            }
                        }
                    }

                    tlf.Mode = TagLib.File.AccessMode.Closed;
                }
                return returnValue;
            } catch(Exception ex) {
                ex.Source = "DRM_Found(" + filename + ")->" + ex.Source;
                throw;
            }
        }

        #region Images
        public static TagLib.Id3v2.AttachedPictureFrame PicFrame(byte[] image, TagLib.PictureType PicType) {
            TagLib.Picture pic = new TagLib.Picture(image);
            TagLib.Id3v2.AttachedPictureFrame apf = new TagLib.Id3v2.AttachedPictureFrame(pic);

            using(System.IO.MemoryStream ms = new System.IO.MemoryStream(image)) {
                using(System.Drawing.Image i = System.Drawing.Image.FromStream(ms)) {
                    apf.MimeType = GetMIMEType(i);
                }
                ms.Close();
            }
            apf.Type = PicType;
            return apf;
        }

        public static TagLib.IPicture[] ProcessPicture(byte[] image, TagLib.PictureType PicType) {
            return new TagLib.IPicture[1] { PicFrame(image, PicType) };
        }

        public static TagLib.Id3v2.AttachedPictureFrame PicFrame(string ImageFile, TagLib.PictureType PicType) {
            TagLib.Picture pic = new TagLib.Picture(ImageFile);
            TagLib.Id3v2.AttachedPictureFrame apf = new TagLib.Id3v2.AttachedPictureFrame(pic);
            apf.MimeType = GetMIMEType(GetFileExtension(ImageFile));
            apf.Type = PicType;
            return apf;
        }

        public static TagLib.IPicture[] ProcessPicture(string ImageFile, TagLib.PictureType PicType) {
            return new TagLib.IPicture[1] { PicFrame(ImageFile, PicType) };
        }

        public static byte[] GetAlbumArt(TagLib.IPicture[] pics, TagLib.PictureType type) {
            foreach(TagLib.IPicture pic in pics) {
                if(pic.Type == type && ValidData(pic.Data.Data, true))
                    return pic.Data.Data;
            }
            return new byte[0];
        }
        #endregion Images

        #region Discrepancies
        public static void FixTagDifferences(string SourceFile, string TargetFile) {
            FixTagDifferences(ReadFileTags(SourceFile),TargetFile);
        }
        
        public static void FixTagDifferences(ID3Tag SourceTag, string TargetFile) {
            UpdateFileTags(TargetFile, SourceTag);
        }
        #endregion Discrepancies

        #region Verification
        public static bool MatchingTags(string File1, string File2) {
            return MatchingTags(ReadFileTags(File1), ReadFileTags(File2));
        }

        public static bool MatchingTags(string File1, string File2, List<ID3Fields> FieldsToCompare) {
            return MatchingTags(ReadFileTags(File1), ReadFileTags(File2), FieldsToCompare);
        }
        
        public static bool MatchingTags(ID3Tag Tag1, ID3Tag Tag2) {
            List<ID3Fields> fields = new List<ID3Fields>();
            foreach(ID3Fields f in Enum.GetValues(typeof(ID3Fields))){
                fields.Add(f);
            }

            return MatchingTags(Tag1, Tag2, fields);
        }
        
        public static bool MatchingTags(ID3Tag Tag1, ID3Tag Tag2, List<ID3Fields> FieldsToCompare) {
            bool match = false;

            foreach(ID3Fields f in FieldsToCompare) {
                switch(f) {
                    case ID3Fields.id3v1Artists:
                        match = (Tag1.id3v1Artists == Tag2.id3v1Artists);
                        if(!match)
                            return false;
                        break;
                    case ID3Fields.id3v1Artist:
                        match = (Tag1.id3v1Artist == Tag2.id3v1Artist);
                        if(!match)
                            return false;
                        break;
                    case ID3Fields.AlbumArtists:
                        match = (Tag1.AlbumArtists == Tag2.AlbumArtists);
                        if(!match)
                            return false;
                        break;
                    case ID3Fields.AlbumArtist:
                        match = (Tag1.AlbumArtist == Tag2.AlbumArtist);
                        if(!match)
                            return false;
                        break;
                    case ID3Fields.Album:
                        match = (Tag1.Album == Tag2.Album);
                        if(!match)
                            return false;
                        break;
                    case ID3Fields.id3v1Album:
                        match = (Tag1.id3v1Album == Tag2.id3v1Album);
                        if(!match)
                            return false;
                        break;
                    case ID3Fields.Title:
                        match = (Tag1.Title == Tag2.Title);
                        if(!match)
                            return false;
                        break;
                    case ID3Fields.id3v1Title:
                        match = (Tag1.id3v1Title == Tag2.id3v1Title);
                        if(!match)
                            return false;
                        break;
                    case ID3Fields.Genres:
                        match = (Tag1.Genres == Tag2.Genres);
                        if(!match)
                            return false;
                        break;
                    case ID3Fields.Genre:
                        match = (Tag1.Genre == Tag2.Genre);
                        if(!match)
                            return false;
                        break;
                    case ID3Fields.Year:
                        match = (Tag1.Year == Tag2.Year);
                        if(!match)
                            return false;
                        break;
                    case ID3Fields.Lyrics:
                        match = (Tag1.Lyrics == Tag2.Lyrics);
                        if(!match)
                            return false;
                        break;
                    case ID3Fields.TrackNum:
                        match = (Tag1.TrackNum == Tag2.TrackNum);
                        if(!match)
                            return false;
                        break;
                    case ID3Fields.DiscNum:
                        match = (Tag1.DiscNum == Tag2.DiscNum);
                        if(!match)
                            return false;
                        break;
                    case ID3Fields.Comment:
                        match = (Tag1.Comment == Tag2.Comment);
                        if(!match)
                            return false;
                        break;
                    case ID3Fields.Conductor:
                        match = (Tag1.Conductor == Tag2.Conductor);
                        if(!match)
                            return false;
                        break;
                    case ID3Fields.Duration:
                        match = (Tag1.Duration == Tag2.Duration);
                        if(!match)
                            return false;
                        break;
                    case ID3Fields.Bitrate:
                        match = (Tag1.Bitrate == Tag2.Bitrate);
                        if(!match)
                            return false;
                        break;
                    case ID3Fields.SampleRate:
                        match = (Tag1.SampleRate == Tag2.SampleRate);
                        if(!match)
                            return false;
                        break;
                    case ID3Fields.AlbumArt:
                        match = (Tag1.AlbumArt == Tag2.AlbumArt);
                        if(!match)
                            return false;
                        break;
                    case ID3Fields.BackCoverImage:
                        match = (Tag1.BackCoverImage == Tag2.BackCoverImage);
                        if(!match)
                            return false;
                        break;
                    case ID3Fields.ArtistImage:
                        match = (Tag1.ArtistImage == Tag2.ArtistImage);
                        if(!match)
                            return false;
                        break;
                    case ID3Fields.BandImage:
                        match = (Tag1.BandImage == Tag2.BandImage);
                        if(!match)
                            return false;
                        break;
                    case ID3Fields.Composers:
                        match = (Tag1.Composers == Tag2.Composers);
                        if(!match)
                            return false;
                        break;
                    case ID3Fields.Performers:
                        match = (Tag1.Performers == Tag2.Performers);
                        if(!match)
                            return false;
                        break;
                }
            }
            return match;
        }
        #endregion Verification
    }
}
