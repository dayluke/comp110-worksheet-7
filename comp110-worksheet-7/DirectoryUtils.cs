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
        //private List<FileInformation> fileinfos = new List<FileInformation>();
        private static int depthCount = 0;
        public static FileArray fileinfos;
        
        /// <summary>
        /// Utilisies the FileInformation and FileArray custom classes
        /// that I created. This methods checks if the 'fileinfos'
        /// instance of the FileArry class is equal to null (e.g. it is
        /// empty and has not been set yet). If it is null, then it calls
        /// the constructor class in the FileArry class - assigning all
        /// the files found to the public array in the class.
        /// the number of files.
        /// </summary>
        /// <param name="path"></param>
        public static void SetFileInformation(string path)
        {
            if (fileinfos == null)
            {
                fileinfos = new FileArray(path);
            }
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
            foreach (FileInformation file in fileinfos.fileList)
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
            return fileinfos.fileList.Count;
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
            foreach (FileInformation file in fileinfos.fileList)
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
            foreach (FileInformation file in fileinfos.fileList)
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

            foreach (FileInformation file in fileinfos.fileList)
            {
                if (file.filesize == size && !matchingFileSizes.Contains(file.filepath))
                {
                    matchingFileSizes.Add(file.filepath);
                }
            }

            return matchingFileSizes.ToArray();
        }

    }

    // Stores the path and size of a file.
    public class FileInformation
    {
        public string filepath;
        public long filesize;

        // Constructor used to set the public variables of the file.
        public FileInformation(string path, long size)
        {
            filepath = path;
            filesize = size;
        }
    }

    public class FileArray
    {
        public List<FileInformation> fileList = new List<FileInformation>();

        // Constructor simply calls the SetFileList to add all of the files into the 'fileList'.
        public FileArray(string path)
        {
            SetFileList(path);
        }

        private void SetFileList(string path)
        {
            string[] files = Directory.GetFiles(path);
            foreach (string fileName in files)
                // Adds the file found to the fileList list.
                fileList.Add(new FileInformation(fileName, new FileInfo(fileName).Length));

            string[] subDirectories = Directory.GetDirectories(path);
            foreach (string subdir in subDirectories)
                SetFileList(subdir); // Recursives into sub-directories.
        }
    }

}
