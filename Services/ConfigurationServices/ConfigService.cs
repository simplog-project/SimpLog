using SimpleLog.Models.AppSettings;
using System;
using System.IO;
using System.Text.Json;

namespace SimpleLog.Services.ConfigurationServices
{
    internal static class ConfigService
    {
        #region Main Configuration Variable

        public static readonly string? _WhyLogIsNotWorkingPath = null;
        public static readonly string? _WhyLogIsNotWorkingFileName = null;
        public static readonly bool?   _Disable_Log = null;

        #endregion Main Configuration Variable

        #region Log File Configuration Variable

        static readonly string? _PathToSaveLogs = null;
        static readonly string? _LogFileName = null;
        static readonly bool? _Enable_File_Log = null;

        #endregion Log File Configuration Variable

        #region Email Configuration Variable

        static readonly bool? _SendEmail_Globally = null;
        static readonly string? _Email_From = null;
        static readonly string? _Email_To = null;
        static readonly string? _Email_BCC = null;
        static readonly string? _Host = null;
        static readonly string? _Port = null;
        static readonly string? _Key = null;
        static readonly string? _Value = null;

        #endregion Email Configuration Variable

        #region Database Configuration Variable

        static readonly string? _Connection_String = null;
        static readonly string? _Global_Database_Type = null;
        static readonly bool? _Use_OleDB = null;
        static readonly bool? _Global_Enabled_Save = null;

        #endregion Database Configuration Variable

        #region Log Type Configuration Variable
        
        static readonly bool? _TraceLog;
        static readonly bool? _TraceSendEmail        = true;
        static readonly bool? _TraceSaveInDatabase   = false;

        static readonly bool? _DebugLog              = false;
        static readonly bool? _DebugSendEmail        = false;
        static readonly bool? _DebugSaveInDatabase   = false;
        
        static readonly bool? _InfoLog               = false;
        static readonly bool? _InfoSendEmail         = false;
        static readonly bool? _InfoSaveInDatabase    = false;
        
        static readonly bool? _NoticeLog             = false;
        static readonly bool? _NoticeSendEmail       = false;
        static readonly bool? _NoticeSaveInDatabase  = false;
        
        static readonly bool? _WarnLog               = false;
        static readonly bool? _WarnSendEmail         = false;
        static readonly bool? _WarnSaveInDatabase    = false;
        
        static readonly bool? _ErrorLog              = false;
        static readonly bool? _ErrorSendEmail        = false;
        static readonly bool? _ErrorSaveInDatabase   = false;
        
        static readonly bool? _FatalLog              = false;
        static readonly bool? _FatalSendEmail        = false;
        static readonly bool? _FatalSaveInDatabase   = false;

        #endregion Log Type Configuration Variable

        static ConfigService()
        {
            Configuration? simpLogConfig;

            //  If there is not found a configuration file
            if (!File.Exists(Environment.CurrentDirectory + "\\simplog.json"))
            {
                simpLogConfig = new Configuration()
                {
                    Main_Configuration = new MainConfiguration()
                    {
                        WhyLogIsNotWorkingFileName = null,
                        WhyLogIsNotWorkingPath = null,
                        Disable_Log = null
                    },
                    File_Configuration = new FileConfiguration()
                    {
                        Enable_File_Log = null,
                        LogFileName = null,
                        PathToSaveLogs = null,
                    },
                    Email_Configuration = new EmailConfiguration()
                    {
                        Email_From = null,
                        Email_Bcc = null,
                        Email_Connection = new EmailConnection()
                        {
                            API_Key = null,
                            API_Value = null,
                            Host = null,
                            Port = null,
                        },
                        Email_To = null,
                        SendEmail_Globally = null,
                    },
                    Database_Configuration = new DatabaseConfiguration()
                    {
                        Connection_String = null,
                        Global_Database_Type = null,
                        Global_Enabled_Save = null,
                        Use_OleDB = null
                    },
                    LogType = new Log()
                    {
                        Debug = new LogTypeObject()
                        {
                            Log = null,
                            SaveInDatabase = null,
                            SendEmail = null
                        },
                        Error = new LogTypeObject()
                        {
                            Log = null,
                            SaveInDatabase = null,
                            SendEmail = null
                        },
                        Fatal = new LogTypeObject()
                        {
                            Log = null,
                            SaveInDatabase = null,
                            SendEmail = null
                        },
                        Info = new LogTypeObject()
                        {
                            Log = null,
                            SaveInDatabase = null,
                            SendEmail = null
                        },
                        Notice = new LogTypeObject()
                        {
                            Log = null,
                            SaveInDatabase = null,
                            SendEmail = null
                        },
                        Trace = new LogTypeObject()
                        {
                            Log = null,
                            SaveInDatabase = null,
                            SendEmail = null
                        },
                        Warn = new LogTypeObject()
                        {
                            Log = null,
                            SaveInDatabase = null,
                            SendEmail = null
                        }
                    }
                };
            }
            else
                simpLogConfig = JsonSerializer.Deserialize<Configuration>(File.ReadAllText(Environment.CurrentDirectory + "\\simplog.json"));

            #region Main Configuration Get From Json

            _WhyLogIsNotWorkingPath     = simpLogConfig.Main_Configuration.WhyLogIsNotWorkingPath;
            _WhyLogIsNotWorkingFileName = simpLogConfig.Main_Configuration.WhyLogIsNotWorkingFileName;
            _Disable_Log                = simpLogConfig.Main_Configuration.Disable_Log;
            //            _WhyLogIsNotWorkingPath = (simpLogConfig.Main_Configuration.WhyLogIsNotWorkingPath == null) ? string.Empty : simpLogConfig.Main_Configuration.WhyLogIsNotWorkingPath.ToString();

            #endregion Main Configuration Get From Json

            #region Log File Configuration Get From Json

            _PathToSaveLogs = simpLogConfig.File_Configuration.PathToSaveLogs;
            _LogFileName = simpLogConfig.File_Configuration.LogFileName;
            _Enable_File_Log = simpLogConfig.File_Configuration.Enable_File_Log;

            #endregion Log File Configuration Get From Json

            #region Email Configuration Get From Json
            _SendEmail_Globally = simpLogConfig.Email_Configuration.SendEmail_Globally;
            _Email_From = simpLogConfig.Email_Configuration.Email_From;
            _Email_To = simpLogConfig.Email_Configuration.Email_To;
            _Email_BCC = simpLogConfig.Email_Configuration.Email_Bcc;
            _Host = null;
            _Port = simpLogConfig.Email_Configuration.Email_Connection.Port;
            _Key = simpLogConfig.Email_Configuration.Email_Connection.API_Key;
            _Value = simpLogConfig.Email_Configuration.Email_Connection.API_Value;

            #endregion Email Configuration Get From Json

            #region Database Configuration Get From Json

            _Connection_String = simpLogConfig.Database_Configuration.Connection_String;
            _Global_Database_Type = simpLogConfig.Database_Configuration.Global_Database_Type;
            _Use_OleDB = simpLogConfig.Database_Configuration.Use_OleDB;
            _Global_Enabled_Save = simpLogConfig.Database_Configuration.Global_Enabled_Save;

            #endregion Database Configuration Get From Json

            #region Log Type Configuration Get From Json

            //  Checks if the configuration exists at all
            _TraceLog = simpLogConfig.LogType.Trace.Log;
            _TraceSendEmail = simpLogConfig.LogType.Trace.SendEmail;
            _TraceSaveInDatabase = simpLogConfig.LogType.Trace.SaveInDatabase;

            _DebugLog = simpLogConfig.LogType.Debug.Log;
            _DebugSendEmail = simpLogConfig.LogType.Debug.SendEmail;
            _DebugSaveInDatabase = simpLogConfig.LogType.Debug.SaveInDatabase;

            _InfoLog = simpLogConfig.LogType.Info.Log;
            _InfoSendEmail = simpLogConfig.LogType.Info.SendEmail;
            _InfoSaveInDatabase = simpLogConfig.LogType.Info.SaveInDatabase;

            _NoticeLog = simpLogConfig.LogType.Notice.Log;
            _NoticeSendEmail = simpLogConfig.LogType.Notice.SendEmail;
            _NoticeSaveInDatabase = simpLogConfig.LogType.Notice.SaveInDatabase;

            _WarnLog = simpLogConfig.LogType.Warn.Log;
            _WarnSendEmail = simpLogConfig.LogType.Warn.SendEmail;
            _WarnSaveInDatabase = simpLogConfig.LogType.Warn.SaveInDatabase;

            _ErrorLog = simpLogConfig.LogType.Error.Log;
            _ErrorSendEmail = simpLogConfig.LogType.Error.SendEmail;
            _ErrorSaveInDatabase = simpLogConfig.LogType.Error.SaveInDatabase;

            _FatalLog = simpLogConfig.LogType.Fatal.Log;
            _FatalSendEmail = simpLogConfig.LogType.Fatal.SendEmail;
            _FatalSaveInDatabase = simpLogConfig.LogType.Fatal.SaveInDatabase;

            #endregion Log Type Configuration Get From Json
        }

        /// <summary>
        /// Check if the path exists
        /// </summary>
        /// <param name="path_to_save_log"></param>
        /// <returns></returns>
        public static bool PathCheck(string? path_to_save_log)
        {
            if (!string.IsNullOrEmpty(path_to_save_log) && Directory.Exists(path_to_save_log))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Put into an object the configuration from appsettings.json file
        /// </summary>
        /// <returns></returns>
        public static Configuration BindConfigObject()
        {
            return new Configuration()
            {
                Main_Configuration      = new MainConfiguration() 
                {
                    WhyLogIsNotWorkingPath = _WhyLogIsNotWorkingPath,
                    WhyLogIsNotWorkingFileName = _WhyLogIsNotWorkingFileName,
                    Disable_Log = _Disable_Log
                },
                File_Configuration       = new FileConfiguration()
                {
                    PathToSaveLogs  = _PathToSaveLogs,
                    LogFileName     = _LogFileName,
                    Enable_File_Log = _Enable_File_Log
                },
                Email_Configuration     = new EmailConfiguration()
                {
                    Email_From          = _Email_From,
                    Email_To            = _Email_To,
                    Email_Bcc           = _Email_BCC,
                    SendEmail_Globally  = _SendEmail_Globally,
                    Email_Connection    = new EmailConnection()
                    {
                        Host        = _Host,
                        Port        = _Port,
                        API_Key     = _Key,
                        API_Value   = _Value,
                    }
                },
                Database_Configuration  = new DatabaseConfiguration()
                {
                    Connection_String       = _Connection_String,
                    Global_Database_Type    = _Global_Database_Type,
                    Use_OleDB               = _Use_OleDB,
                    Global_Enabled_Save     = _Global_Enabled_Save
                },
                LogType                 = new Log()
                {
                    Trace   = new LogTypeObject()
                    {
                        Log             = _TraceLog,
                        SendEmail       = _TraceSendEmail,
                        SaveInDatabase  = _TraceSaveInDatabase
                    },
                    Debug   = new LogTypeObject()
                    {
                        Log             = _DebugLog,
                        SendEmail       = _DebugSendEmail,
                        SaveInDatabase  = _DebugSaveInDatabase
                    },
                    Info    = new LogTypeObject()
                    {
                        Log             = _InfoLog,
                        SendEmail       = _InfoSendEmail,
                        SaveInDatabase  = _InfoSaveInDatabase
                    },
                    Notice  = new LogTypeObject()
                    {
                        Log             = _NoticeLog,
                        SendEmail       = _NoticeSendEmail,
                        SaveInDatabase  = _NoticeSaveInDatabase
                    },
                    Warn    = new LogTypeObject()
                    {
                        Log             = _WarnLog,
                        SendEmail       = _WarnSendEmail,
                        SaveInDatabase  = _WarnSaveInDatabase
                    },
                    Error   = new LogTypeObject()
                    {
                        Log             = _ErrorLog,
                        SendEmail       = _ErrorSendEmail,
                        SaveInDatabase  = _ErrorSaveInDatabase
                    },
                    Fatal   = new LogTypeObject()
                    {
                        Log             = _FatalLog,
                        SendEmail       = _FatalSendEmail,
                        SaveInDatabase  = _FatalSaveInDatabase
                    },
                }
            };
        }
    }
}
