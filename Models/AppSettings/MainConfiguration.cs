using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLog.Models.AppSettings
{
    internal class MainConfiguration
    {
        public string? WhyLogIsNotWorkingPath { get; set; }

        public string? WhyLogIsNotWorkingFileName { get; set; }

        public bool? Disable_Log { get; set; }
    }
}
