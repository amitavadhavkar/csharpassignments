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
                    if (fields.Length >= 2)
                    {
                        Int32 employeeId = Int32.Parse(fields[0]);
                        string employeeName = fields[1];
                        Int32 managerId = -1;
                        if (fields.Length == 3)
                        {
                            managerId = Int32.Parse(fields[2]);
                        }
                        employees.Add(employeeId, employeeName);
                        if (managerId != -1)
                        {
                            employeeManagers.Add(employeeId, managerId);
                            List<Int32> employees;
                            if (managerEmployees.TryGetValue(managerId, out employees))
                            {
                                employees.Add(employeeId);
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
                Console.WriteLine("Invalid employees.csv");
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