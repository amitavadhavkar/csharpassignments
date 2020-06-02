using System;
using System.Collections.Generic;
using System.IO;
using CommandLine;

namespace LogToCsv
{
    public class Program
    {
        enum LOG_LEVEL
        {
            INFO, WARN, ERROR, DEBUG, TRACE, EVENT
        }
        
        class Options
        {
            [Option('c', "csv", Required = true, HelpText = "Out file path.")]
            public String CsvDir { get; set; }

            [Option('d', "log-dir", Required = true, HelpText = "Log dir path.")]
            public String LogDir { get; set; }

            [Option('d', "log-level", Required = true, HelpText = "Log level.")]
            public IEnumerable<string> LogLevel { get; set; }

            [Option('h', "help", Required = false, HelpText = "Usage.")]
            public String Help { get; set; }
        }

        static void Main(string[] args)
        {
            try
            {
                CommandLine.Parser.Default.ParseArguments<Options>(args)
                .WithParsed(RunOptions)
                .WithNotParsed(errs => HandleParseError(errs));

                DirectoryInfo rootDirInfo = new DirectoryInfo(InputArguments.logDir);
                DirectoryWalker.WalkDirectoryTree(rootDirInfo);
                Console.WriteLine("Generated csv: " + InputArguments.absoluteCsvFile);
            }
            catch (Exception e)
            {
                Console.Write(e.StackTrace);
                Environment.Exit(-1);
            }
        }
        static void RunOptions(Options o)
        {
            if (o.CsvDir != null)
            {
                InputArguments.csvDir = o.CsvDir;
                DirectoryInfo csvDirInfo = null;
                String requiredCsvPath = "../../out/" + o.CsvDir;
                if (!Directory.Exists(requiredCsvPath))
                {
                    csvDirInfo = Directory.CreateDirectory(requiredCsvPath);
                }
                else
                {
                    csvDirInfo = new DirectoryInfo(requiredCsvPath);
                }
                Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                InputArguments.absoluteCsvFile = csvDirInfo.FullName + "/csv-"
                + unixTimestamp.ToString() + ".csv";
                using (System.IO.StreamWriter csvFile = new System.IO.StreamWriter(InputArguments.absoluteCsvFile))
                {
                    csvFile.WriteLine("No,Level,Date,Time,Text");
                }
            }
            if (o.LogDir != null)
            {
                InputArguments.logDir = o.LogDir;
                if (!Directory.Exists(o.LogDir))
                {
                    Console.WriteLine("log-dir does not exist");
                    Environment.Exit(-1);
                }
            }
            if (o.LogLevel != null)
            {
                string[] names = Enum.GetNames(typeof(LOG_LEVEL));

                foreach (var logLevel in o.LogLevel)
                {
                    bool isFound = false;
                    foreach (string name in names)
                    {
                        if (name == logLevel)
                        {
                            isFound = true;
                        }
                    }
                    if (!isFound)
                    {
                        Console.WriteLine("Invalid log-level: " + logLevel);
                        Environment.Exit(-1);
                    }
                    else
                    {
                        InputArguments.logLevels.Add(logLevel);
                    }
                }
                string[] levelArray = new string[InputArguments.logLevels.Count];
                InputArguments.logLevels.CopyTo(levelArray);
                InputArguments.filterLogLevel = string.Join("|", levelArray);
            }
            if (o.Help != null)
            {
                Console.WriteLine(
                "Usage: logParser --log-dir <dir> --log-level <level> --csv <out> "
                  + System.Environment.NewLine
                    + "--log-dir   Directory to parse recursively for .log files "
                    + System.Environment.NewLine
                  + "--csv Out file-path (absolute/relative)"
                );
            }
        }
        static void HandleParseError(IEnumerable<Error> errs)
        {
            Console.WriteLine(
                "Usage: logParser --log-dir <dir> --log-level <level> --csv <out> "
                  + System.Environment.NewLine
                    + "--log-dir   Directory to parse recursively for .log files "
                    + System.Environment.NewLine
                  + "--csv Out file-path (absolute/relative)"
            );
        }
    }
}