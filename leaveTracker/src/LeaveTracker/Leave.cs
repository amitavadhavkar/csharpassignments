using System;
using System.Collections.Generic;

namespace LeaveTracker
{
    public class Leave
    {
        public Int32 id;
        public string creator;
        public string manager;
        public string title;
        public string description;
        public DateTime startDate;
        public DateTime endDate;
        public LeaveStatus leaveStatus;

        public Leave(Int32 id, string creator, string manager, string title, string description,
        DateTime startDate, DateTime endDate, LeaveStatus leaveStatus)
        {
            this.id = id;
            this.creator = creator;
            this.manager = manager;
            this.title = title;
            this.description = description;
            this.startDate = startDate;
            this.endDate = endDate;
            this.leaveStatus = leaveStatus;
        }

        public override string ToString()
        {
            return id + "," + creator + "," + manager
            + "," + title + "," + description + ","
            + startDate.ToString(InputArguments.DATE_FORMAT, InputArguments.DATE_CULTURE_PROVIDER) + ","
            + endDate.ToString(InputArguments.DATE_FORMAT, InputArguments.DATE_CULTURE_PROVIDER) + ","
            + Enum.GetName(typeof(LeaveStatus), leaveStatus);
        }
    }
}