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
            EmployeeCatalog.LoadEmployees();
            LeaveService.LoadLeaves();
            Console.WriteLine("Enter employee Id");
            string employeeId = Console.ReadLine();
            Console.WriteLine("Choices:");
            Console.WriteLine("CreateLeave:AssignTo,Title,Description,StartDate,EndDate");
            Console.WriteLine("ListMyLeaves:");
            Console.WriteLine("UpdateLeave:");
            Console.WriteLine("SearchLeavesByTitle:");
            Console.WriteLine("SearchLeavesByStatus:");
            Console.WriteLine("Quit:");
            string choice = Console.ReadLine();
            while (!choice.Equals("Quit"))
            {
                switch (choice)
                {
                    case "CreateLeave":
                        string assignTo = Console.ReadLine();
                        string title = Console.ReadLine();
                        string description = Console.ReadLine();
                        string startDate = Console.ReadLine();
                        string endDate = Console.ReadLine();
                        LeaveService.CreateLeave
                        (employeeId, assignTo, title, description, startDate, endDate);
                        break;
                    case "ListMyLeave":
                        break;
                    case "UpdateLeave":
                        break;
                    case "SearchLeavesByTitle":
                        break;
                    case "SearchLeavesByStatus":
                        break;
                    default:
                        Console.WriteLine("Invalid choice");
                        break;
                }
                choice = Console.ReadLine();
            }
        }

        static void OnProcessExit(object sender, EventArgs e)
        {
            Console.WriteLine("Saving leaves.csv before exit");
        }
        static void RunOptions(Options o)
        {
        }
        static void HandleParseError(IEnumerable<Error> errs)
        {
        }
    }
}