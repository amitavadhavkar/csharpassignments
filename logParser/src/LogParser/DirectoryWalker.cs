using System;

namespace LogToCsv
{
    public class DirectoryWalker
    {
        public static bool WalkDirectoryTree(System.IO.DirectoryInfo root, bool appendContent)
        {
            System.IO.FileInfo[] files = null;
            System.IO.DirectoryInfo[] subDirs = null;
            bool canWalk = true;
            // First, process all the files directly under this folder
            try
            {
                files = root.GetFiles("*.*");
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine(e.Message);
                canWalk = false;
                return canWalk;
            }

            catch (System.IO.DirectoryNotFoundException e)
            {
                Console.WriteLine(e.Message);
                canWalk = false;
                return canWalk;
            }

            if (files != null)
            {
                foreach (System.IO.FileInfo fi in files)
                {
                    // Console.WriteLine(fi.FullName);
                    if (appendContent)
                    {
                        LogToCsvConverter.appendCsv(fi.FullName);
                    }
                }

                // Now find all the subdirectories under this directory.
                subDirs = root.GetDirectories();

                foreach (System.IO.DirectoryInfo dirInfo in subDirs)
                {
                    // Resursive call for each subdirectory.
                    WalkDirectoryTree(dirInfo, appendContent);
                }
            }
            return canWalk;
        }
    }
}