using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLog.Models
{
    internal static class EnumExtensions
    {
        private static readonly
    ConcurrentDictionary<string, string> DisplayNameCache = new ConcurrentDictionary<string, string>();

        public static string DisplayName(this Enum value)
        {
            var key = $"{value.GetType().FullName}.{value}";

            var displayName = DisplayNameCache.GetOrAdd(key, x =>
            {
                var name = (DescriptionAttribute[])value
                    .GetType()
                    .GetTypeInfo()
                    .GetField(value.ToString())
                    .GetCustomAttributes(typeof(DescriptionAttribute), false);

                return name.Length > 0 ? name[0].Description : value.ToString();
            });

            return displayName;
        }
    }

    internal enum Global_Database_Type
    {
        [Description("MSSql")]
        MSSql,
        [Description("MySql")]
        MySql,
        [Description("MariaDb")]
        MariaDb,
        [Description("Postgre")]
        PostgreSql,
        [Description("Oracle")]
        Oracle,
        [Description("MongoDb")]
        MongoDb
    }

    public enum FileSaveType
    {
        /// <summary>
        /// Dont save into a file
        /// </summary>
        [Description("DontSave")]
        DontSave,
        /// <summary>
        /// Saves at the moment
        /// </summary>
        [Description("Standart")]
        Standart,
        /// <summary>
        /// Stores in buffer memory and when there is enough content - save it.
        /// </summary>
        [Description("BufferMemory")]
        BufferMemory
    }

    internal enum LogType
    {
        /// <summary>
        /// TRACE level: This should be used during development to track bugs, but never committed to your VCS.
        /// </summary>
        [Description("Trace")]
        Trace,
        /// <summary>
        /// DEBUG level: log at this level about anything that happens in the program. This is mostly used during 
        /// debugging, and I’d advocate trimming down the number of debug statement before entering the production 
        /// stage, so that only the most meaningful entries are left, and can be activated during troubleshooting.
        /// </summary>
        [Description("Debug")]
        Debug,
        /// <summary>
        /// INFO level: log at this level all actions that are user-driven, or system 
        /// specific (ie regularly scheduled operations…)
        /// </summary>
        [Description("Info")]
        Info,
        /// <summary>
        /// NOTICE level: this will certainly be the level at which the program will run when 
        /// in production. Log at this level all the notable events that are not considered an error.
        /// </summary>
        [Description("Notice")]
        Notice,
        /// <summary>
        /// WARN level: log at this level all events that could potentially 
        /// become an error. For instance if one database call took more than a 
        /// predefined time, or if an in-memory cache is near capacity. This will 
        /// allow proper automated alerting, and during troubleshooting will 
        /// allow to better understand how the system was behaving before the failure.
        /// </summary>
        [Description("Warn")]
        Warn,
        /// <summary>
        /// ERROR level: log every error condition at this level. 
        /// That can be API calls that return errors or internal error conditions.
        /// </summary>
        [Description("Error")]
        Error,
        /// <summary>
        /// FATAL level: too bad, it’s doomsday. Use this very 
        /// scarcely, this shouldn’t happen a lot in a real program. Usually 
        /// logging at this level signifies the end of the program. 
        /// For instance, if a network daemon can’t bind a network socket, log at 
        /// this level and exit is the only sensible thing to do.
        /// </summary>
        [Description("Fatal")]
        Fatal
    }
}
