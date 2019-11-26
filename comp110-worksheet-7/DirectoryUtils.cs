using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comp110_worksheet_7
{
    public static class DirectoryUtils
    {
        private static int depthCount = 0;

        private static List<FileInformation> fileinfos = new List<FileInformation>();
        
        /// <summary>
        /// Utilisies the FileInformation custom class that I created.
        /// This methods searches through all the directories and sub-
        /// directories and adds all of the files it finds to the list
        /// 'fileinfos'. This list is used in most methods to easily
        /// return the correct answer, such as the number of files.
        /// </summary>
        /// <param name="path"></param>
        public static void SetFileInformation(string path)
        {
            // Process the list of files found in the directory.
            string[] files = Directory.GetFiles(path);
            foreach (string fileName in files)
                fileinfos.Add(new FileInformation(fileName, GetFileSize(fileName)));

            // Recurse into subdirectories of this directory.
            string[] subDirectories = Directory.GetDirectories(path);
            foreach (string subdir in subDirectories)
                SetFileInformation(subdir);
        }

        // Return the size, in bytes, of the given file
        public static long GetFileSize(string filePath)
        {
            return new FileInfo(filePath).Length;
        }

        // Return true if the given path points to a directory, false if it points to a file
        public static bool IsDirectory(string path)
        {
            return File.GetAttributes(path).HasFlag(FileAttributes.Directory);
        }

        // Return the total size, in bytes, of all the files below the given directory
        public static long GetTotalSize(string directory)
        {
            long count = 0;
            SetFileInformation(directory);
            foreach (FileInformation file in fileinfos)
            {
                Console.WriteLine("FILE:: {0}, SIZE:: {1}", file.filepath, file.filesize);
                count += file.filesize;
            }
            Console.WriteLine("TOTAL FILE SIZE:: {0}", count);
            return count;
        }

        // Return the number of files (not counting directories) below the given directory
        public static int CountFiles(string directory)
        {
            SetFileInformation(directory);
            return fileinfos.Count;
        }

        // Return the nesting depth of the given directory. A directory containing only files (no subdirectories) has a depth of 0.
        public static int GetDepth(string directory)
        {
            string[] subDirectories = Directory.GetDirectories(directory);
            foreach (string subdir in subDirectories)
            {
                GetDepth(subdir);
                depthCount++;
            }

            return depthCount;
            //throw new NotImplementedException();
        }

        // Get the path and size (in bytes) of the smallest file below the given directory
        public static Tuple<string, long> GetSmallestFile(string directory)
        {
            SetFileInformation(directory);
            Tuple<string, long> smallestFile = new Tuple<string, long>("", 10000);
            foreach (FileInformation file in fileinfos)
            {
                if (file.filesize < smallestFile.Item2)
                {
                    smallestFile = new Tuple<string, long>(file.filepath, file.filesize);
                }
            }

            return smallestFile;
        }

        // Get the path and size (in bytes) of the largest file below the given directory
        public static Tuple<string, long> GetLargestFile(string directory)
        {
            SetFileInformation(directory);
            Tuple<string, long> largestFile = new Tuple<string, long>("", 0);
            foreach (FileInformation file in fileinfos)
            {
                if (file.filesize > largestFile.Item2)
                {
                    largestFile = new Tuple<string, long>(file.filepath, file.filesize);
                }
            }
            return largestFile;
        }

        // Get all files whose size is equal to the given value (in bytes) below the given directory
        public static IEnumerable<string> GetFilesOfSize(string directory, long size)
        {
            SetFileInformation(directory);
            List<string> matchingFileSizes = new List<string>();

            foreach (FileInformation file in fileinfos)
            {
                if (file.filesize == size && !matchingFileSizes.Contains(file.filepath))
                {
                    matchingFileSizes.Add(file.filepath);
                }
            }

            return matchingFileSizes.ToArray();
        }

    }

    public class FileInformation
    {
        public string filepath;
        public long filesize;

        public FileInformation(string path, long size)
        {
            filepath = path;
            filesize = size;
        }
    }
}
