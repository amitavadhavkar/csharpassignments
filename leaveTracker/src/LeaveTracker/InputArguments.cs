using System.Collections.Generic;
using System.Globalization;

namespace LeaveTracker
{
    public static class InputArguments
    {
        public const string IN_FILE = "../../in/employees.csv";
        public const string OUT_FILE = "../../out/leaves/leaves.csv";

        public static CultureInfo DATE_CULTURE_PROVIDER = CultureInfo.InvariantCulture;
        public const string DATE_FORMAT = "dd-MM-yyyy";

    }
}