using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLog.Models.AppSettings
{
    internal class EmailConnection
    {
        public string? Host { get; set; }

        public string? Port { get; set; }

        public string? API_Key { get; set; }

        public string? API_Value { get; set; }
    }
}
