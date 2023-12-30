using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLog.Models.AppSettings
{
    internal class EmailConfiguration
    {
        public bool? SendEmail_Globally { get; set; }

        public string? Email_From { get; set; }

        public string? Email_To { get; set; }

        public string? Email_Bcc { get; set; }

        public EmailConnection? Email_Connection { get; set; }
    }
}
