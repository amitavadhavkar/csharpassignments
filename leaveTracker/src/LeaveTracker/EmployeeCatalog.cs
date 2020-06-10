using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic.FileIO;

namespace LeaveTracker
{
    public class EmployeeCatalog
    {
        private static Dictionary<Int32, string> employees = new Dictionary<Int32, string>();
        private static Dictionary<Int32, Int32> employeeManagers = new Dictionary<Int32, Int32>();

        private static Dictionary<Int32, List<Int32>> managerEmployees
        = new Dictionary<Int32, List<Int32>>();
        public static void LoadEmployees()
        {
            if (File.Exists(InputArguments.IN_FILE) &&
            (new FileInfo(InputArguments.IN_FILE)).Length > 0)
            {
                TextFieldParser parser = new TextFieldParser(InputArguments.IN_FILE);
                parser.SetDelimiters(new string[] { "," });
                parser.HasFieldsEnclosedInQuotes = false;

                try
                {
                    // Skip over header line.
                    parser.ReadLine();

                    while (!parser.EndOfData)
                    {
                        string[] fields = parser.ReadFields();
                        if (fields.Length >= 2
                        && fields[0] != null && !fields[0].Trim().Equals("")
                        && fields[1] != null && !fields[1].Trim().Equals(""))
                        {
                            // Console.WriteLine(fields[0]);
                            // Console.WriteLine(fields[1]);

                            Int32 employeeId = Int32.Parse(fields[0]);
                            string employeeName = fields[1];
                            Int32 managerId = -1;
                            if (fields.Length == 3)
                            {
                                if (fields[2] != null && !fields[2].Trim().Equals(""))
                                {
                                    // Console.WriteLine(":" + fields[2] + ":");
                                    managerId = Int32.Parse(fields[2]);
                                }
                            }
                            employees.Add(employeeId, employeeName);
                            if (managerId != -1)
                            {
                                employeeManagers.Add(employeeId, managerId);
                                List<Int32> employees;
                                if (managerEmployees.TryGetValue(managerId, out employees))
                                {
                                    employees.Add(employeeId);
                                    managerEmployees.Remove(managerId);
                                    managerEmployees.Add(managerId, employees);
                                }
                                else
                                {
                                    List<Int32> employeesToAdd = new List<Int32>();
                                    employeesToAdd.Add(employeeId);
                                    managerEmployees.Add(managerId, employeesToAdd);
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid employees.csv");
                            break;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                    Console.WriteLine("Invalid employees.csv. Reason:{}", e.Message);
                }
            }
            else
            {
                Console.WriteLine(InputArguments.IN_FILE + " not found");
            }
        }

        public static void PrintEmployeeManager()
        {
            foreach (KeyValuePair<Int32, Int32> kvp in employeeManagers)
            {
                Console.WriteLine("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
            }
        }

        public static void PrintManagerEmployees()
        {
            foreach (KeyValuePair<Int32, List<Int32>> kvp in managerEmployees)
            {
                Console.WriteLine("Key = {0}", kvp.Key);
                foreach(Int32 emp in kvp.Value)
                {
                    Console.Write(emp + ",");
                }
                Console.WriteLine();
            }
        }

        public static Dictionary<Int32, string> GetEmployees()
        {
            return null;
        }

        public static string GetEmployeeName(Int32 employeeId)
        {
            string name;
            if (!employees.TryGetValue(employeeId, out name))
            {
                return null;
            }
            else
            {
                return name;
            }
        }

        public static Int32 GetManagerId(Int32 employeeId)
        {
            Int32 managerId;
            if (!employeeManagers.TryGetValue(employeeId, out managerId))
            {
                return -1;
            }
            else
            {
                return managerId;
            }
        }

        public static List<Int32> GetEmployeesForManagerId(Int32 managerId)
        {
            List<Int32> employeeIds;
            if (!managerEmployees.TryGetValue(managerId, out employeeIds))
            {
                return null;
            }
            else
            {
                return employeeIds;
            }
        }
    }
}