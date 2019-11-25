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
            long totalSize = 0;
            if (IsDirectory(directory))
            {
                List<string> files = System.IO.Directory.GetFiles(directory).ToList<string>();
                List<string> dirs = System.IO.Directory.GetDirectories(directory).ToList<string>();

                foreach (string dir in dirs)
                {
                    totalSize += ProcessDirectory(dir);
                }
            }

            return totalSize;
		}

        // Process all files in the directory passed in, recurse on any directories 
        // that are found, and process the files they contain.
        public static long ProcessDirectory(string dir, long byteSize = 0)
        {
            Console.WriteLine("Processed directory '{0}'.", dir);

            // Process the list of files found in the directory.
            string[] files = Directory.GetFiles(dir);
            foreach (string fileName in files)
                byteSize += ProcessFile(fileName);

            // Recurse into subdirectories of this directory.
            string[] subDirectories = Directory.GetDirectories(dir);
            foreach (string subdir in subDirectories)
                ProcessDirectory(subdir, byteSize);

            return byteSize;
        }

        public static long ProcessFile(string path)
        {
            Console.Write("Processed file '{0}'.", path);
            FileInfo f = new FileInfo(path);
            Console.Write("File size: {0} bytes.", f.Length);
            Console.WriteLine();
            return f.Length;
        }

        // Return the number of files (not counting directories) below the given directory
        public static int CountFiles(string directory)
		{
			throw new NotImplementedException();
		}

		// Return the nesting depth of the given directory. A directory containing only files (no subdirectories) has a depth of 0.
		public static int GetDepth(string directory)
		{
			throw new NotImplementedException();
		}

		// Get the path and size (in bytes) of the smallest file below the given directory
		public static Tuple<string, long> GetSmallestFile(string directory)
		{
			throw new NotImplementedException();
		}

		// Get the path and size (in bytes) of the largest file below the given directory
		public static Tuple<string, long> GetLargestFile(string directory)
		{
			throw new NotImplementedException();
		}

		// Get all files whose size is equal to the given value (in bytes) below the given directory
		public static IEnumerable<string> GetFilesOfSize(string directory, long size)
		{
			throw new NotImplementedException();
		}
	}
}
