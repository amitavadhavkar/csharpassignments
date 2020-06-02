using System.Collections.Generic;

namespace LogToCsv
{
    public static class InputArguments
    {
        public static string logDir;
        public static string csvDir;
        public static string absoluteCsvFile;
        public static HashSet<string> logLevels = new HashSet<string>();

        public static string filterLogLevel;
    }
}