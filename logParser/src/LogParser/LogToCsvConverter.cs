using System;
using System.IO;
using System.Text.RegularExpressions;

namespace LogToCsv
{
    public class LogToCsvConverter
    {
        public static Int32 lineNum = 1;
        const string pattern = @"\d+";
        static string patternLevel = @"(" + InputArguments.filterLogLevel + ")";
        static string patternLinePart = @"(" + InputArguments.filterLogLevel + @")\s+[:]";
        static Regex rg = new Regex(pattern);
        static Regex rgLevel = new Regex(patternLevel);
        public static bool appendCsv(String fileName)
        {
            string csvFileName = InputArguments.absoluteCsvFile;
            string monthStr, dateStr, hourStr, minuteStr, secondStr, logLevel, linePart;
            string logLine;

            try
            {
                System.IO.StreamReader file = new System.IO.StreamReader(fileName);
                using (System.IO.StreamWriter csvFile = File.AppendText(csvFileName))
                {
                    while ((logLine = file.ReadLine()) != null)
                    {
                        MatchCollection matchedParts = rg.Matches(logLine);
                        if (matchedParts.Count < 5)
                        {
                            // Console.WriteLine("No date time found");
                            continue;
                        }
                        // for (int count = 0; count < matchedParts.Count; count++)
                        // {
                        //     Console.WriteLine(matchedParts[count].Value);
                        // }
                        monthStr = matchedParts[0].Value;
                        dateStr = matchedParts[1].Value;
                        hourStr = matchedParts[2].Value;
                        minuteStr = matchedParts[3].Value;
                        secondStr = matchedParts[4].Value;

                        MatchCollection matchedLevels = rgLevel.Matches(logLine);
                        if (matchedLevels.Count < 1)
                        {
                            // Console.WriteLine("No log level found");
                            continue;
                        }
                        // for (int count = 0; count < matchedLevels.Count; count++)
                        // {
                        //     Console.WriteLine(matchedLevels[count].Value);
                        // }
                        logLevel = matchedLevels[0].Value;

                        String lineStart = monthStr + "/" + dateStr + " " + hourStr + ":" + minuteStr + ":" + secondStr + " " + logLevel;
                        if (logLine.StartsWith(lineStart))
                        {
                            string[] lineParts = logLine.Split(lineStart);
                            char[] startColon = { ':' };
                            char[] startDot = { '.' };
                            if (lineParts[1] != null)
                            {
                                linePart = lineParts[1].Trim();
                                linePart = linePart.TrimStart(startColon);
                                linePart = linePart.TrimStart(startDot);

                                DateTime dt1 = new DateTime(2020, Int32.Parse(monthStr), Int32.Parse(dateStr),
                                Int32.Parse(hourStr), Int32.Parse(minuteStr), Int32.Parse(secondStr));
                                String formedLine = lineNum + "," + logLevel + "," + dt1.ToString("dd MMM yyyy")
                                + "," + dt1.ToString("hh:mm tt") + ",\"" + linePart + "\"";
                                csvFile.WriteLine(formedLine);
                                lineNum++;
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }
            catch (System.IO.FileNotFoundException e)
            {
                return false;
            }
            return true;
        }
    }
}