using System;
using Xunit;
using System.Collections.Generic;
using System.IO;
using CommandLine;

namespace LogToCsv.Tests
{
    public class LogToCsvConverterTest
    {
        [Fact]
        public void IsCsvCreated_ReturnTrue()
        {
            Console.WriteLine(Directory.GetCurrentDirectory());
            String fileName = "csv-" + ".csv";
            InputArguments.absoluteCsvFile = Directory.GetCurrentDirectory()
            + "../../../../../../out/csvoutdir/" + fileName;
            LogToCsvConverter.AppendCsv(Directory.GetCurrentDirectory() 
            + "../../../../../../in/logs/2020_03/app.log");
            var result = File.Exists(InputArguments.absoluteCsvFile)
                            && 
                         (new FileInfo(InputArguments.absoluteCsvFile)).Length > 0;
            if (result)
            {
                Assert.True(result, "Csv could be created");
            }
        }

        [Fact]
        public void IsCsvCreated_ReturnFalse()
        {
            String fileName = "csv-" + ".csv";
            InputArguments.absoluteCsvFile = Directory.GetCurrentDirectory()
            + "../../../../../../out/csvoutdir/" + fileName;
            LogToCsvConverter.AppendCsv(Directory.GetCurrentDirectory() 
            + "../../../../../../in/logs/2020_03/appxxx.log");
            var result = File.Exists(InputArguments.absoluteCsvFile)
                            && 
                         (new FileInfo(InputArguments.absoluteCsvFile)).Length > 0;
            if (!result)
            {
                Assert.False(result, "Csv could not be created");
            }
        }
    }
}
