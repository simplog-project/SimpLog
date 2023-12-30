using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLog.Models
{
    internal class Constants
    {
        public const string DateFormat = "yyyy-MM-dd'T'HH:mm:ss.fffffff'Z'";

        public const string FileFormat = ".txt";

        public const string Separator = " | ";

        public const string PathSeparator = "\\";

        public const string LogType_Trace = "TRACE:";

        public const string LogType_Debug = "DEBUG:";

        public const string LogType_Info = "INFO:";

        public const string LogType_Notice = "NOTICE:";

        public const string LogType_Warn = "WARN:";

        public const string LogType_Error = "ERROR:";

        public const string LogType_Fatal = "FATAL:";

        public const string LogType_NoType = "NO TYPE:";
    }
}
