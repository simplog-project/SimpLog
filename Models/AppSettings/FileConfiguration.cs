using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLog.Models.AppSettings
{
    internal class FileConfiguration
    {
        public string? PathToSaveLogs { get; set; }

        public string? LogFileName { get; set; }

        public bool? Enable_File_Log { get; set; }
    }
}
