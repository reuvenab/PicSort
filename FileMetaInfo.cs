using System;
using System.Drawing;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Text.RegularExpressions;

using ExifTags = SixLabors.ImageSharp.MetaData.Profiles.Exif;

namespace PicSort
{
    internal class FileMetaInfo
    {

        public DateTime DT;
        public string DirName;
        //private string fileName;

        private void InitMembers(DateTime dt)
        {
            this.DT = dt;
            //this.DirName = $"{dt.Year:D4}-{dt.Month:D2}-{dt.Day:D2}_{dt.Hour:D2}-{dt.Minute:D2}-{dt.Se:D2}";
            this.DirName = $"{dt.Year:D4}-{dt.Month:D2}";

        }

        public FileMetaInfo(DateTime dt)
        {
            InitMembers(dt);
        }

        public FileMetaInfo(string fileName)
        {
            var dt = Filename2Datetime.MakeDatetime(fileName);
            if (dt != null)
            {
                InitMembers(dt.Value); 
            }
            else 
            {
                Console.WriteLine($"Could not parse {fileName}");
                InitMembers(DateTime.MinValue);
            }
            
        }

        internal static FileMetaInfo MetaInfoFactory(string fullName, string fileName, string fileExt)
        {
            FileMetaInfo mi = null;
            switch (fileExt.ToLower())
            {
                case ".jpg" : 
                    mi = ReadJpegMetaInfo(fullName);
                    if (mi == null)
                    {
                        mi = new FileMetaInfo(fileName);
                    }
                    break;

                case ".mov" :
                    mi = new FileMetaInfo(new FileInfo(fullName).LastWriteTime);
                    break;

                case ".mp4" :
                    mi = new FileMetaInfo(fileName);
                    break;

                case ".mpg" :
                    mi = new FileMetaInfo(new FileInfo(fullName).LastWriteTime);
                    break;
                
                case ".avi" :
                    mi = new FileMetaInfo(new FileInfo(fullName).LastWriteTime);
                    break;
                
                case ".mts" :
                    mi = new FileMetaInfo(new FileInfo(fullName).LastWriteTime);
                    break;
                    
                case ".mkv" :
                    mi = new FileMetaInfo(new FileInfo(fullName).LastWriteTime);
                    break;

/*
.3gp
.mkv
*.mpg
.mts
.gif

 */
            }
            return mi;
        }

       
        private static FileMetaInfo ReadJpegMetaInfo(string fullName)
        {
            ExifTags.ExifValue dtValue;
            using (var image = Image.Load(fullName))
            {
                if (image.MetaData.ExifProfile == null)
                {
                    return null;
                }
                var exif = image.MetaData.ExifProfile;
                
                if (!exif.TryGetValue(ExifTags.ExifTag.DateTime, out dtValue))
                {
                    return null;
                }
            }

            var dtS = (String)dtValue.Value;
            
            var dt = Filename2Datetime.MakeDatetime(dtS, Filename2Datetime.RegexType.Exif);
            if (dt != null)
            {
                return new FileMetaInfo(dt.Value);
            }
            Console.WriteLine($"{fullName} has EXIF, however could not parse {dtS}");
            
            return null;
        }
    }
}