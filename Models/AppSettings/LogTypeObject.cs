using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLog.Models.AppSettings
{
    internal class LogTypeObject
    {
        public bool? Log { get; set; }

        public bool? SendEmail { get; set; }

        public bool? SaveInDatabase { get; set; }
    }
}
