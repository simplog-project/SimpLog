using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLog.Entities
{
    internal class EmailLog
    {
        [Key]
        public int ID { get; set; }

        public string From_Email { get; set; }

        public string To_Email { get; set; }

        public string? Bcc { get; set; }

        public string Email_Subject { get; set; }

        public string Email_Body { get; set; }

        public string Time_Sent { get; set; }
    }
}
