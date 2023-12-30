using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLog.Entities
{
    internal class StoreLog
    {
        [Key]
        public int ID { get; set; }

        public string Log_Type { get; set; }

        public string Log_Error { get; set; }

        public string? Log_Created { get; set; }

        public string Log_FileName { get; set; }

        public string Log_File_Save_Type { get; set; }

        public string Log_Path { get; set; }

        public bool? Log_SendEmail { get; set; }

        /// <summary>
        /// Private key to Email table
        /// </summary>
        public int Email_ID { get; set; }

        public bool? Saved_In_Database { get; set; }
    }
}
