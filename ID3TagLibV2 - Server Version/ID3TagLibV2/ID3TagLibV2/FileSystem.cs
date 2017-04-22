namespace ID3TagLibV2
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    public abstract class FileSystem
    {
        public static void CreateDirectory(string directory)
        {
            try
            {
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory.Trim(IllegalChars.IllegalFileNameChars).Trim(IllegalChars.NonASCII));
            }
            catch (Exception ex)
            {
                ex.Source = "CreateDirectory(" + directory + ")--->" + ex.Source;
                throw;
            }
        }

        private static void OrganizeDirectoryFiles(string source, string destination, char directorySeparatorChar)
        {
            try
            {
                foreach (string file in Directory.GetFiles(source, "*.mp3"))
                {
                    Organize(file, destination, directorySeparatorChar, true, true);
                }
            }
            catch (Exception ex)
            {
                ex.Source = "OrganizeDirectoryFiles(" + source + "," + destination + "," + directorySeparatorChar + ")--->" + ex.Source;
                throw;
            }
        }

        private static void UpdateDirectoryFiles(string path, ID3Tag tag)
        {
            try
            {
                foreach (string file in Directory.GetFiles(path, "*.mp3"))
                {
                    using (ID3Tag t = new ID3Tag())
                    {
                        t.AlbumArtist = tag.AlbumArtist;
                        t.Album = tag.Album;
                        t.Genre = tag.Genre;
                        t.Year = tag.Year;
                        t.AlbumArt = tag.AlbumArt;
                        t.BackCoverImage = tag.BackCoverImage;
                        t.ArtistImage = tag.ArtistImage;
                        t.BandImage = tag.BandImage;
                        t.DiscNum = tag.DiscNum;

                        TagHandler.UpdateFileTags(file, tag);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Source = "UpdateDirectoryFiles(" + path + ",ID3TagLib.ID3Tag)--->" + ex.Source;
                throw;
            }
        }

        public static void OrganizeDirectory(string directoryToSearch, string destination, char directorySeparatorChar)
        {
            try
            {
                Stack<string> searchStack = new Stack<string>();
                searchStack.Push(directoryToSearch);
                while (searchStack.Count > 0)
                {
                    string directory = searchStack.Pop();
                    OrganizeDirectoryFiles(directory, destination, directorySeparatorChar);
                    foreach (string dir in Directory.GetDirectories(directory))
                    {
                        searchStack.Push(dir);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Source = "OrganizeDirectory(" + directoryToSearch + "," + destination + "," + directorySeparatorChar + ")--->" + ex.Source;
                throw;
            }
        }

        public static void UpdateDirectory(string directoryToSearch, ID3Tag id3Tag)
        {
            try
            {
                Stack<string> searchStack = new Stack<string>();
                searchStack.Push(directoryToSearch);
                while (searchStack.Count > 0)
                {
                    string directory = searchStack.Pop();
                    UpdateDirectoryFiles(directory, id3Tag);
                    foreach (string dir in Directory.GetDirectories(directory))
                    {
                        searchStack.Push(dir);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Source = "UpdateDirectory(" + directoryToSearch + ",ID3TagLib.ID3Tag)--->" + ex.Source;
                throw;
            }
        }

        public static string Organize(string source,
                                      string destination,
                                      char DirectorySeparator,
                                      bool moveFile,
                                      bool overwrite)
        {
            try
            {
                using (ID3Tag tag = TagHandler.ReadFileTags(source))
                {
                    if (!destination.EndsWith(DirectorySeparator.ToString()))
                        destination = destination + DirectorySeparator;

                    destination = destination +
                                    IllegalChars.Legalize(tag.AlbumArtist) +
                                    DirectorySeparator +
                                    IllegalChars.Legalize(tag.Album);
                    CreateDirectory(destination);
                }

                if (Directory.Exists(destination))
                {
                    string destFile = destination + DirectorySeparator + Path.GetFileName(source);
                    if (!File.Exists(destFile))
                        if (moveFile)
                            File.Move(source, destFile);
                        else
                            File.Copy(source, destFile, overwrite);
                }
                else
                    throw new Exception("Unable to create destination path: " + destination);

                return destination;
            }
            catch (Exception ex)
            {
                ex.Source = "Organize(" + source + "," + destination + ")--->" + ex.Source;
                throw;
            }
        }

        public static string GetTagPathWithTrack(string filename)
        {
            try
            {
                ID3TagLibV2.ID3Tag tag = ID3TagLibV2.TagHandler.ReadFileTags(filename);

                return IllegalChars.Legalize(tag.AlbumArtist) + System.IO.Path.DirectorySeparatorChar +
                       IllegalChars.Legalize(tag.Album) + System.IO.Path.DirectorySeparatorChar +
                       (tag.TrackNum > 0 ? IllegalChars.Legalize(tag.TrackNum.ToString("D2")) + " - " : string.Empty) +
                       IllegalChars.Legalize(tag.Title) + ".mp3";
            }
            catch (Exception ex)
            {
                ex.Source = "GetTagPathWithTrack(" + filename + ")->" + ex.Source;
                throw;
            }
        }

        public static string GetTagPath(string filename)
        {
            try
            {
                ID3TagLibV2.ID3Tag tag = ID3TagLibV2.TagHandler.ReadFileTags(filename);

                return IllegalChars.Legalize(tag.AlbumArtist) + System.IO.Path.DirectorySeparatorChar +
                       IllegalChars.Legalize(tag.Album) + System.IO.Path.DirectorySeparatorChar +
                       IllegalChars.Legalize(tag.Title) + ".mp3";
            }
            catch (Exception ex)
            {
                ex.Source = "GetTagPath(" + filename + ")->" + ex.Source;
                throw;
            }
        }

        public static bool TagPathMismatch(string filename, out string tagpath)
        {
            try
            {
                bool returnValue = false;
                tagpath = GetTagPath(filename);

                if (filename.ToUpper().EndsWith(tagpath.ToUpper()))
                    returnValue = false;
                else
                {
                    tagpath = GetTagPathWithTrack(filename);
                    if (filename.ToUpper().EndsWith(tagpath.ToUpper()))
                    {
                        returnValue = false;
                    }
                    else
                        returnValue = true;
                }

                return returnValue;
            }
            catch (Exception ex)
            {
                ex.Source = "TagPathMismatch(" + filename + ")->" + ex.Source;
                throw;
            }
        }

        public ID3Tag TagFromPath(string source, string delimiter)
        {
            try
            {
                string[] parts = source.Split(delimiter.ToCharArray());

                if (parts.Length > 2)
                {
                    string title = parts[parts.Length - 1];
                    string album = parts[parts.Length - 2];
                    string artist = parts[parts.Length - 3];

                    using (ID3Tag tag = new ID3Tag())
                    {
                        tag.AlbumArtist = artist;
                        tag.Album = album;
                        tag.Title = title;

                        return tag;
                    }
                }
                else
                    throw new Exception("Unable to parse directory path in: " + source);
            }
            catch (Exception ex)
            {
                ex.Source = "TagFromPath(" + source + ")--->" + ex.Source;
                throw;
            }
        }
    }
}
