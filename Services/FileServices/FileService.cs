using SimpleLog.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static SimpleLog.Models.Constants;

namespace SimpleLog.Services.FileServices
{
    internal class FileService
    {
        /// <summary>
        /// FullPath + FileName is the key and value is what should be saved into the log
        /// </summary>
        public static Dictionary<string, StringBuilder> Logs = new Dictionary<string, StringBuilder>();

        public static Models.AppSettings.Configuration configuration = ConfigurationServices.ConfigService.BindConfigObject();

        internal readonly bool? _Enable_File_Log = (configuration.File_Configuration.Enable_File_Log == null) ? true : Convert.ToBoolean(configuration.File_Configuration.Enable_File_Log);

        internal readonly bool? _Trace_File   = (configuration.LogType.Trace.Log == null) ? true : Convert.ToBoolean(configuration.LogType.Trace.Log);
        internal readonly bool? _Debug_File   = (configuration.LogType.Debug.Log == null) ? true : Convert.ToBoolean(configuration.LogType.Debug.Log);
        internal readonly bool? _Info_File    = (configuration.LogType.Info.Log == null) ? true : Convert.ToBoolean(configuration.LogType.Info.Log);
        internal readonly bool? _Notice_File  = (configuration.LogType.Notice.Log == null) ? true : Convert.ToBoolean(configuration.LogType.Notice.Log);
        internal readonly bool? _Warn_File    = (configuration.LogType.Warn.Log == null) ? true : Convert.ToBoolean(configuration.LogType.Warn.Log);
        internal readonly bool? _Error_File   = (configuration.LogType.Error.Log == null) ? true : Convert.ToBoolean(configuration.LogType.Error.Log);
        internal readonly bool? _Fatal_File   = (configuration.LogType.Fatal.Log == null) ? true : Convert.ToBoolean(configuration.LogType.Fatal.Log);

        internal readonly bool? _Trace_Email  = (configuration.LogType.Trace.SendEmail == null) ? true : Convert.ToBoolean(configuration.LogType.Trace.SendEmail);
        internal readonly bool? _Debug_Email  = (configuration.LogType.Debug.SendEmail == null) ? true : Convert.ToBoolean(configuration.LogType.Debug.SendEmail);
        internal readonly bool? _Info_Email   = (configuration.LogType.Info.SendEmail == null) ? true : Convert.ToBoolean(configuration.LogType.Info.SendEmail);
        internal readonly bool? _Notice_Email = (configuration.LogType.Notice.SendEmail == null) ? true : Convert.ToBoolean(configuration.LogType.Notice.SendEmail);
        internal readonly bool? _Warn_Email   = (configuration.LogType.Warn.SendEmail == null) ? true : Convert.ToBoolean(configuration.LogType.Warn.SendEmail);
        internal readonly bool? _Error_Email  = (configuration.LogType.Error.SendEmail == null) ? true : Convert.ToBoolean(configuration.LogType.Error.SendEmail);
        internal readonly bool? _Fatal_Email  = (configuration.LogType.Fatal.SendEmail == null) ? true : Convert.ToBoolean(configuration.LogType.Fatal.SendEmail);

        internal readonly bool? _Trace_Db = (configuration.LogType.Trace.SaveInDatabase == null) ? true : Convert.ToBoolean(configuration.LogType.Trace.SaveInDatabase);
        internal readonly bool? _Debug_Db = (configuration.LogType.Debug.SaveInDatabase == null) ? true : Convert.ToBoolean(configuration.LogType.Debug.SaveInDatabase);
        internal readonly bool? _Info_Db = (configuration.LogType.Info.SaveInDatabase == null) ? true : Convert.ToBoolean(configuration.LogType.Info.SaveInDatabase);
        internal readonly bool? _Notice_Db = (configuration.LogType.Notice.SaveInDatabase == null) ? true : Convert.ToBoolean(configuration.LogType.Notice.SaveInDatabase);
        internal readonly bool? _Warn_Db = (configuration.LogType.Warn.SaveInDatabase == null) ? true : Convert.ToBoolean(configuration.LogType.Warn.SaveInDatabase);
        internal readonly bool? _Error_Db = (configuration.LogType.Error.SaveInDatabase == null) ? true : Convert.ToBoolean(configuration.LogType.Error.SaveInDatabase);
        internal readonly bool? _Fatal_Db = (configuration.LogType.Fatal.SaveInDatabase == null) ? true : Convert.ToBoolean(configuration.LogType.Fatal.SaveInDatabase);

        public SimpLog.FileLog.Services.SimpLogServices.SimpLog _obj = new SimpLog.FileLog.Services.SimpLogServices.SimpLog();

        /// <summary>
        /// Converts message type from enum to string.
        /// </summary>
        /// <param name="logType"></param>
        /// <returns></returns>
        internal string MessageType(LogType logType)
        {
            switch (logType)
            {
                case LogType.Trace:
                    return LogType_Trace;
                case LogType.Debug:
                    return LogType_Debug;
                case LogType.Info:
                    return LogType_Info;
                case LogType.Notice:
                    return LogType_Notice;
                case LogType.Warn:
                    return LogType_Warn;
                case LogType.Error:
                    return LogType_Error;
                case LogType.Fatal:
                    return LogType_Fatal;
                default:
                    return LogType_NoType;
            }
        }

        /// <summary>
        /// Distributes what type of save is it configured. File, Email of Database.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logType"></param>
        /// <param name="saveType"></param>
        /// <param name="sendEmail"></param>
        /// <param name="saveInDatabase"></param>
        /// <param name="path_to_save_log"></param>
        /// <param name="log_file_name"></param>
        /// <returns></returns>
        internal async Task Save(
            string message, 
            LogType logType, 
            FileSaveType? saveType      = FileSaveType.Standart, 
            bool? sendEmail             = true,
            bool? saveInDatabase        = false,
            string? path_to_save_log    = null,
            string? log_file_name       = null)
        {
            try
            {
                //  Save into a file
                await _obj.SaveLogFile(message, (SimpLog.FileLog.Models.FileSaveType?)saveType, path_to_save_log, log_file_name, (SimpLog.FileLog.Models.LogType)logType);

                bool? isEmailSent = false;

                //  Send email
                if(sendEmail is not null && sendEmail is true)
                {
                    await new SimpLog.Email.Services.SimpLogServices.SimpLog().SendEmail("", (SimpLog.Email.Models.LogType)logType, sendEmail);

                    isEmailSent = true;
                }

                //  Save into a database
                SaveIntoDatabase(
                    configuration.Database_Configuration.Global_Database_Type, 
                    message, 
                    isEmailSent, 
                    logType, 
                    (FileSaveType)saveType, 
                    path_to_save_log, 
                    log_file_name);
            }
            catch(Exception ex)
            {
                //await SaveSimpLogError(ex.Message);
                //Dispose();
            }
        }

        /// <summary>
        /// Depending on the name of the DB, goes to the function for that stuff.
        /// </summary>
        /// <param name="DbName"></param>
        /// <param name="isEmailSend"></param>
        public static void SaveIntoDatabase(string DbName, string message, bool? isEmailSend, LogType logType, FileSaveType saveType, string? path_to_save_log, string? log_file_name)
        {
            if (DbName.Equals(Global_Database_Type.MSSql.DisplayName()))
                new SimpLog.Databases.MSSQL.Services.SimpLogServices.SimpLog().SaveIntoMSSQL(
                    message,
                    true,
                    (SimpLog.Databases.MSSQL.Models.LogType)logType,
                    saveType.ToString(),
                    isEmailSend,
                    true,
                    path_to_save_log,
                    log_file_name);
            else if (DbName.Equals(Global_Database_Type.MySql.DisplayName()))
                new SimpLog.Databases.MySQL.Services.SimpLogServices.SimpLog().SaveIntoMySQL(
                    message,
                    true,
                    (SimpLog.Databases.MySQL.Models.LogType)logType,
                    saveType.ToString(),
                    isEmailSend,
                    true,
                    path_to_save_log,
                    log_file_name);
            else if (DbName.Equals(Global_Database_Type.PostgreSql.DisplayName()))
                new SimpLog.Databases.PostgreSQL.Services.SimpLogServices.SimpLog().SaveIntoPostgreSQL(
                    message,
                    true,
                    (SimpLog.Databases.PostgreSQL.Models.LogType)logType,
                    saveType.ToString(),
                    isEmailSend,
                    true,
                    path_to_save_log,
                    log_file_name);
            else if (DbName.Equals(Global_Database_Type.MongoDb.DisplayName()))
                new SimpLog.Databases.MongoDb.Services.SimpLogServices.SimpLog().SaveIntoMongoDb(
                    message, 
                    true, 
                    (SimpLog.Databases.MongoDb.Models.LogType)logType, 
                    saveType.ToString(),
                    isEmailSend,
                    true,
                    path_to_save_log,
                    log_file_name);
            else
                return;
        }
    }
}
