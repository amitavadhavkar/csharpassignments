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
            // EmployeeCatalog.PrintEmployeeManager();
            // EmployeeCatalog.PrintManagerEmployees();
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
            string leaveRecordId;
            string assignTo;
            string title;
            string description;
            string startDate;
            string endDate;
            string status;

            while (!choice.Equals("Quit"))
            {
                switch (choice)
                {
                    case "CreateLeave":
                        Console.WriteLine("assignTo:");
                        assignTo = Console.ReadLine();
                        Console.WriteLine("title:");
                        title = Console.ReadLine();
                        Console.WriteLine("description:");
                        description = Console.ReadLine();
                        Console.WriteLine("startDate:");
                        startDate = Console.ReadLine();
                        Console.WriteLine("endDate:");
                        endDate = Console.ReadLine();
                        LeaveService.CreateLeave
                        (employeeId, assignTo, title, description, startDate, endDate);
                        Console.WriteLine("Leave created successfully");
                        break;
                    case "ListMyLeaves":
                        LeaveService.ListMyLeaves(employeeId);
                        Console.WriteLine("Leaves listed successfully");
                        break;
                    case "UpdateLeave":
                        Console.WriteLine("leaveRecordId:");
                        leaveRecordId = Console.ReadLine();
                        Console.WriteLine("status:");
                        status = Console.ReadLine();
                        LeaveService.UpdateLeave(employeeId, leaveRecordId, status);
                        Console.WriteLine("Leave status updated successfully");
                        break;
                    case "SearchLeavesByTitle":
                        Console.WriteLine("title:");
                        title = Console.ReadLine();
                        LeaveService.SearchLeavesByTitle(title);
                        Console.WriteLine("Leaves searched by title successfully");
                        break;
                    case "SearchLeavesByStatus":
                        Console.WriteLine("status:");
                        status = Console.ReadLine();
                        LeaveService.SearchLeavesByStatus(status);
                        Console.WriteLine("Leaves searched by status successfully");
                        break;
                    default:
                        Console.WriteLine("Invalid choice");
                        break;
                }
                Console.WriteLine("Enter next choice:");
                choice = Console.ReadLine();
            }
            LeaveService.SaveLeaves();
            Console.WriteLine("Exiting Leave Tracker");
            System.Environment.Exit(0);
        }

        static void OnProcessExit(object sender, EventArgs e)
        {
            Console.WriteLine("Saving leaves.csv before exit");
            LeaveService.SaveLeaves();
        }
        static void RunOptions(Options o)
        {
        }
        static void HandleParseError(IEnumerable<Error> errs)
        {
        }
    }
}