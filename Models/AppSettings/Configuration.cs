using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLog.Models.AppSettings
{
    internal class Configuration
    {
        public MainConfiguration? Main_Configuration { get; set; }

        public FileConfiguration? File_Configuration { get; set; }

        public EmailConfiguration? Email_Configuration { get; set;}

        public DatabaseConfiguration? Database_Configuration { get; set; }

        public Log? LogType { get; set; }
    }
}
