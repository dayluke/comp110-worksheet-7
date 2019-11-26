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
        public static long totalFileSize = 0;
        public static int fileCount = 0;
        public static int depthCount = 0;
        public static Tuple<string, long> smallestFile = new Tuple<string, long>("", 10000);
        public static Tuple<string, long> largestFile = new Tuple<string, long>("", 0);

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

            // Process the list of files found in the directory.
            string[] files = Directory.GetFiles(directory);
            foreach (string fileName in files)
                totalFileSize += GetFileSize(fileName);

            // Recurse into subdirectories of this directory.
            string[] subDirectories = Directory.GetDirectories(directory);
            foreach (string subdir in subDirectories)
                GetTotalSize(subdir);

            return totalFileSize;
        }

        // Return the number of files (not counting directories) below the given directory
        public static int CountFiles(string directory)
        {
            string[] files = Directory.GetFiles(directory);
            fileCount += files.Length;

            // Recurse into subdirectories of this directory.
            string[] subDirectories = Directory.GetDirectories(directory);
            foreach (string subdir in subDirectories)
                CountFiles(subdir);

            return fileCount;
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
            string[] files = Directory.GetFiles(directory);
            foreach (string fileName in files)
                if (GetFileSize(fileName) < smallestFile.Item2)
                {
                    smallestFile = new Tuple<string, long>(fileName, GetFileSize(fileName));
                }

            // Recurse into subdirectories of this directory.
            string[] subDirectories = Directory.GetDirectories(directory);
            foreach (string subdir in subDirectories)
                GetSmallestFile(subdir);

            return smallestFile;
        }

        // Get the path and size (in bytes) of the largest file below the given directory
        public static Tuple<string, long> GetLargestFile(string directory)
        {
            string[] files = Directory.GetFiles(directory);
            foreach (string fileName in files)
                if (GetFileSize(fileName) > largestFile.Item2)
                {
                    largestFile = new Tuple<string, long>(fileName, GetFileSize(fileName));
                }

            // Recurse into subdirectories of this directory.
            string[] subDirectories = Directory.GetDirectories(directory);
            foreach (string subdir in subDirectories)
                GetLargestFile(subdir);

            return largestFile;
        }

        // Get all files whose size is equal to the given value (in bytes) below the given directory
        public static IEnumerable<string> GetFilesOfSize(string directory, long size)
        {
            throw new NotImplementedException();
        }
    }
}
