using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace LeaveTracker
{
    public class LeaveService
    {
        public static List<Leave> leaves = new List<Leave>();

        public static void SaveLeaves()
        {
            using (System.IO.StreamWriter leavesFile =
            new StreamWriter(InputArguments.OUT_FILE, false))
            {
                foreach (Leave leave in leaves)
                {
                    leavesFile.WriteLine(leave.ToString());
                }
            }
        }
        public static void LoadLeaves()
        {
            if (File.Exists(InputArguments.OUT_FILE) &&
            (new FileInfo(InputArguments.OUT_FILE)).Length > 0)
            {
                TextFieldParser parser = new TextFieldParser(InputArguments.OUT_FILE);
                parser.SetDelimiters(new string[] { "," });
                parser.HasFieldsEnclosedInQuotes = false;
                Int32 id;
                DateTime startDate;
                DateTime endDate;
                LeaveStatus leaveStatus;


                try
                {
                    // Skip over header line.
                    parser.ReadLine();

                    while (!parser.EndOfData)
                    {
                        string[] fields = parser.ReadFields();
                        if (fields.Length == 8)
                        {
                            try
                            {
                                id = Int32.Parse(fields[0]);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Invalid Id in record, skipping line. Reason: {}", e.Message);
                                continue;
                            }
                            try
                            {
                                startDate = DateTime.ParseExact(fields[5], InputArguments.DATE_FORMAT, InputArguments.DATE_CULTURE_PROVIDER);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Invalid start date in record, skipping line. Reason: {}", e.Message);
                                continue;
                            }
                            try
                            {
                                endDate = DateTime.ParseExact(fields[6], InputArguments.DATE_FORMAT, InputArguments.DATE_CULTURE_PROVIDER);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Invalid end date in record, skipping line. Reason: {}", e.Message);
                                continue;
                            }
                            try
                            {
                                leaveStatus = (LeaveStatus)Enum.Parse(typeof(LeaveStatus), fields[7], true);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Invalid leave status in record, skipping line. Reason: {}", e.Message);
                                continue;
                            }
                            Leave leave = new Leave(
                                id,
                                fields[1],
                                fields[2],
                                fields[3],
                                fields[4],
                                startDate,
                                endDate,
                                leaveStatus
                            );
                        }
                        else
                        {
                            Console.WriteLine("Invalid line, skipping");
                            continue;
                        }
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Error parsing leaves.csv.", e);
                }
            }
        }


        public static void CreateLeave(
            string employeeId,
            string managerName,
            string title,
            string description,
            string startDate,
            string endDate
        )
        {
            Int32 id;
            DateTime startDt;
            DateTime endDt;
            try
            {
                id = Int32.Parse(employeeId);
            }
            catch (Exception e)
            {
                throw new Exception("Invalid employee Id:" + employeeId, e);

            }
            if (null == managerName || managerName.Length == 0)
            {
                throw new Exception("Invalid manager name:" + managerName);
            }
            if (null == description || description.Length == 0)
            {
                throw new Exception("Invalid description:" + description);
            }
            if (null == title || title.Length == 0)
            {
                throw new Exception("Invalid title:" + title);
            }
            if (null == startDate || startDate.Length == 0)
            {
                throw new Exception("Invalid start date:" + startDate);
            }
            try
            {
                startDt = DateTime.ParseExact(startDate, InputArguments.DATE_FORMAT, InputArguments.DATE_CULTURE_PROVIDER);
            }
            catch (Exception e)
            {
                throw new Exception("Invalid start date:" + startDate, e);
            }
            if (null == endDate || endDate.Length == 0)
            {
                throw new Exception("Invalid end date:" + endDate);
            }
            try
            {
                endDt = DateTime.ParseExact(endDate, InputArguments.DATE_FORMAT, InputArguments.DATE_CULTURE_PROVIDER);
            }
            catch (Exception e)
            {
                throw new Exception("Invalid end date:" + endDate, e);
            }
            string employeeName = EmployeeCatalog.GetEmployeeName(id);
            if (employeeName == null)
            {
                throw new Exception("Invalid employee Id:" + employeeId);
            }

            Int32 managerId = EmployeeCatalog.GetManagerId(id);
            if (managerId == -1)
            {
                throw new Exception("No manager for employee Id:" + employeeId);
            }
            string managerNm = EmployeeCatalog.GetEmployeeName(managerId);
            if (!managerName.Equals(managerNm))
            {
                throw new Exception("Wrong manager name input:" + managerName + ", expected: " + managerNm);
            }

            Int32 leaveRecordId = leaves.Count + 1;
            Leave newLeave = new Leave(
                leaveRecordId,
                employeeName,
                managerName,
                title,
                description,
                startDt,
                endDt,
                LeaveStatus.Pending
            );

            leaves.Add(newLeave);
        }

        public static void ListMyLeaves(string employeeId)
        {
            Int32 id;
            try
            {
                id = Int32.Parse(employeeId);
            }
            catch (Exception e)
            {
                throw new Exception("Invalid employee Id:" + employeeId, e);

            }
            string employeeName = EmployeeCatalog.GetEmployeeName(id);
            if (employeeName == null)
            {
                throw new Exception("Invalid employee Id:" + employeeId);
            }
            foreach (Leave leave in leaves)
            {
                if (leave.creator.Equals(employeeName))
                {
                    Console.WriteLine(leave);
                }
            }
        }

        public static void SearchLeavesByTitle(String title)
        {
            foreach (Leave leave in leaves)
            {
                if (leave.title.Equals(title))
                {
                    Console.WriteLine(leave);
                }
            }
        }
        public static void SearchLeavesByStatus(String leaveStatus)
        {
            try
            {
                LeaveStatus leaveSts = (LeaveStatus)Enum.Parse(typeof(LeaveStatus), leaveStatus, true);
                foreach (Leave leave in leaves)
                {
                    if (leave.leaveStatus == leaveSts)
                    {
                        Console.WriteLine(leave);
                    }
                }

            }
            catch (Exception e)
            {
                throw new Exception("Invalid leave status in search, aborting", e);
            }
        }

        public static void UpdateLeave(string employeeId, string leaveRecordId, string status)
        {
            Int32 id;
            LeaveStatus leaveStatus;
            try
            {
                id = Int32.Parse(employeeId);
            }
            catch (Exception e)
            {
                throw new Exception("Invalid employee Id:" + employeeId, e);
            }
            Int32 leaveRecId;
            try
            {
                leaveRecId = Int32.Parse(leaveRecordId);
            }
            catch (Exception e)
            {
                throw new Exception("Invalid leave record Id:" + leaveRecordId, e);
            }
            try
            {
                leaveStatus = (LeaveStatus)Enum.Parse(typeof(LeaveStatus), status, true);
            }
            catch (Exception e)
            {
                throw new Exception("Invalid leave status.", e);
            }
            string managerName = EmployeeCatalog.GetEmployeeName(id);
            foreach (Leave leave in leaves)
            {
                if (leave.manager.Equals(managerName)
                && leave.id == leaveRecId)
                {
                    leave.leaveStatus = leaveStatus;
                }
            }
        }
    }
}