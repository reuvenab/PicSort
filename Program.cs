using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace PicSort
{
    /// Goal to have all files distributed in the directories per month
    /// 2011-10
    ///    `----- 2011-10-03_15-12-13.jpg


    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length != 3)
            {
                Console.WriteLine("{srcDir} {dstDir} {fileFilter}");
                return;
            }
            var srcDir = args[0];
            var dstDir = args[1];
            var fileFilter = args[2]; // : "*.jpg";
            Console.WriteLine($"FROM: [{srcDir}] TO: [{dstDir}] FILTER: [{fileFilter}]");
            Process(srcDir, dstDir, fileFilter);
        }

        // Directory confirms format YYYY-MM
        // Therefore already was processed by the tool
        private static bool SkipFile(string lastDir)
        {
            // YYYY-MM
            if ((lastDir.Length == 7) && _yyyyMM.IsMatch(lastDir))
            {
                return true;
            }

            return false;
        }

        private static Regex _yyyyMM = new Regex(@"\d{4}-\d{2}", 
                    RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static void Process(string srcDir, string dstDir, string fileFilter)
        {
            HashSet<string> newDirs =  new HashSet<string>();
            try
            {
                var files = Directory.EnumerateFiles(srcDir, fileFilter, 
                                        SearchOption.AllDirectories);

                int ind = 0; 
                bool inputIs0000_01 = Path.GetFileName(srcDir) == "0001-01";               
                
                foreach (string fullName in files)
                {
                    ++ind;
                    Console.WriteLine($"Start processing [{ind}] {fullName}");
                    var fullDirName = Path.GetDirectoryName(fullName);
                    var lastDir = Path.GetFileName(fullDirName);

                    if (!inputIs0000_01 && SkipFile(lastDir))
                    {
                        continue;
                    }
                    
                    var fileExt = Path.GetExtension(fullName).ToLower();
                    var fileName = Path.GetFileName(fullName);
                    var fileInfo = ExtractFileMetaInfo(fileName, fullName,  fileExt);
                    if (fileInfo == null)
                    {
                        Console.WriteLine($"Could not handle {fullName}");
                        continue;
                    }
                    

                    var fileDstDir = Path.Combine(dstDir, fileInfo.DirName);
                    if ( !newDirs.Contains(fileInfo.DirName) )
                    {
                        if (!Directory.Exists(fileDstDir))
                        {
                            Directory.CreateDirectory(fileDstDir);
                        }
                        newDirs.Add(fileInfo.DirName);                     
                    }
                   
                    var newFullName = Path.Combine(fileDstDir, fileName);
                    Console.WriteLine($"Moving {fullName} to {newFullName}");
                    if (!File.Exists(newFullName))
                    {
                        File.Move(fullName, newFullName);
                    } 
                   
                    // string fileName = currentFile.Substring(sourceDirectory.Length + 1);
                    // Directory.Move(currentFile, Path.Combine(archiveDirectory, fileName));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static FileMetaInfo ExtractFileMetaInfo(string fileName, string fullName, string fileExt)
        {
            return FileMetaInfo.MetaInfoFactory(fullName, fileName, fileExt);
        }
    }
}
