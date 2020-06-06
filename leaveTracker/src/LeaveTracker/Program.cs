using System;
using System.Collections.Generic;
using System.IO;
using CommandLine;

namespace LeaveTracker
{
    public class Program
    {
        
        class Options
        {
            [Option('c', "employee-id", Required = true, HelpText = "Employee Id.")]
            public String EmployeeId { get; set; }

            [Option('h', "help", Required = false, HelpText = "Usage.")]
            public String Help { get; set; }
        }

        static void Main(string[] args)
        {
        }
        static void RunOptions(Options o)
        {
        }
        static void HandleParseError(IEnumerable<Error> errs)
        {
        }
    }
}