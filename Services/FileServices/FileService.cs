using SimpleLog.Entities;
using SimpleLog.Models;
using SimpleLog.Models.AppSettings;
using SimpleLog.Services.SimpLogServices;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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

        public SimpLog.FileLog.Services.FileServices.FileService _obj = new SimpLog.FileLog.Services.FileServices.FileService();



        /// <summary>
        /// Pass message and check if it is for execution
        /// </summary>
        /// <param name="bufferMessage"></param>
        /// <returns></returns>
        internal async Task BufferMessage(string message, LogType bufferMessageType, string path_to_save_log, string log_file_name)
        {
            await AppendMessage(message, bufferMessageType, path_to_save_log, log_file_name);

            //await SaveMessageIntoLogFile();
        }

        /// <summary>
        /// Add message to StringBuilder Message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="messageType"></param>
        /// <param name="path_to_save_log"></param>
        /// <param name="log_file_name"></param>
        /// <returns></returns>
        internal async Task AppendMessage(string message, LogType messageType, string path_to_save_log, string log_file_name)
        {
            //  String builder where the message will be saved
            StringBuilder Message = new StringBuilder();

            //  If there was previous information for this file to be saved, take it!
            if (Logs.ContainsKey(path_to_save_log + "\\" + Path.GetFileNameWithoutExtension(log_file_name) + FileFormat))
                Message = Logs[path_to_save_log + "\\" + Path.GetFileNameWithoutExtension(log_file_name) + FileFormat];

            Message.Append(
                DateTime.UtcNow.ToString(DateFormat) +
                Separator +
                MessageType(messageType) +
                Separator +
                message +
                Environment.NewLine);

            Logs[path_to_save_log + "\\" + Path.GetFileNameWithoutExtension(log_file_name) + FileFormat] = Message;

            //  Clear 
            //Logs.Remove(path_to_save_log + log_file_name + FileFormat);

            //Logs.Add(
            //    path_to_save_log + log_file_name + FileFormat, 
            //    Message.Append(
            //        DateTime.UtcNow.ToString(DateFormat) +
            //        Separator +
            //        MessageType(messageType) +
            //        Separator +
            //        message +
            //        Environment.NewLine)
            //    );
        }

        /// <summary>
        /// Log rotation implemented
        /// </summary>
        /// <returns></returns>
        internal async Task FileRenameIfNeeded(string filePath_final, string fileName_final)
        {
            string  fullFilePathName    = filePath_final + PathSeparator + fileName_final + FileFormat;
            int     i                   = 1;
            bool    ready               = false;

            if (!File.Exists(fullFilePathName) || new FileInfo(fullFilePathName).Length < 7485760)
                return;

            do
            {
                if (File.Exists(filePath_final + PathSeparator + fileName_final + i + FileFormat))
                    continue;

                File.Move(fullFilePathName, filePath_final + PathSeparator + fileName_final + i + FileFormat);

                ready = true;
            } while(!ready);
        }

        /// <summary>
        /// Save log message from buffer memory.
        /// </summary>
        /// <returns></returns>
        internal async Task SaveMessageIntoLogFile()
        {
            if (Logs.Count <= 0) return;

            foreach (var log in Logs)
            {
                //if (log.Value.Length < 95)
                //    continue;

                await FileRenameIfNeeded(
                    Path.GetDirectoryName(log.Key),
                    Path.GetFileNameWithoutExtension(log.Key));

                if (File.Exists(log.Key))
                {
                    // Edit the file with a larger buffer. 65k.
                    using (StreamWriter sw = new StreamWriter(new FileStream(Path.GetDirectoryName(log.Key) + PathSeparator + Path.GetFileNameWithoutExtension(log.Key) + FileFormat, FileMode.Append)))
                    {
                        sw.Write(log.Value);
                    }
                }
                else
                    await File.WriteAllTextAsync(log.Key, log.Value.ToString());

                Logs.Remove(log.Key);
            }
        }

        /// <summary>
        /// Save the log without putting into a buffer. If a user wants every log to be saved on a different place.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="messageType"></param>
        /// <param name="path_to_save_log"></param>
        /// <param name="log_file_name"></param>
        /// <returns></returns>
        internal async Task ImediateSaveMessageIntoLogFile(string message, LogType messageType, string? path_to_save_log, string? log_file_name)
        {
            await FileRenameIfNeeded(path_to_save_log, log_file_name);

            message = DateTime.UtcNow.ToString(DateFormat) + Separator + MessageType(messageType) + Separator + message + Environment.NewLine;

            if (File.Exists(path_to_save_log + PathSeparator + log_file_name + FileFormat))
            {
                // Edit the file with a larger buffer. 65k.
                using (StreamWriter sw = new StreamWriter(new FileStream(path_to_save_log + PathSeparator + log_file_name + FileFormat, FileMode.Append)))
                {
                    sw.Write(message);
                }
            }
            else
                await File.WriteAllTextAsync(path_to_save_log + PathSeparator + log_file_name + FileFormat, message);
        }

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
                await _obj.Save(
                    message, 
                    (SimpLog.FileLog.Models.LogType)logType, 
                    (SimpLog.FileLog.Models.FileSaveType?)saveType, 
                    path_to_save_log, 
                    log_file_name);

                bool? isEmailSent = false;

                //  Send Email
                if (ShouldSendEmail(sendEmail, logType))
                    isEmailSent = await SendEmail(sendEmail, logType, message);

                //  Send into a database
                if (ShouldSaveInDb(saveInDatabase, logType))
                    DatabaseServices.DatabaseServices.SaveIntoDatabase(
                        configuration.Database_Configuration.Global_Database_Type,
                        storeLog(message, isEmailSent, logType, saveInDatabase, saveType, path_to_save_log, log_file_name),
                        isEmailSent, saveInDatabase);
            }
            catch(Exception ex)
            {
                await SaveSimpLogError(ex.Message);
                //Dispose();
            }
        }

        /// <summary>
        /// SimpLog error if the library is not working
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        internal async Task SaveSimpLogError(string message)
        {
            //  Check if it has been disabled from appsettings.json file.
            if(configuration.Main_Configuration.Disable_Log is not null && 
                configuration.Main_Configuration.Disable_Log is true)
                return;

            string path     = string.Empty;
            string fileName = string.Empty;

            //  Checks the path in case of an error
            if (configuration.Main_Configuration.WhyLogIsNotWorkingPath is not null && Directory.Exists(configuration.Main_Configuration.WhyLogIsNotWorkingPath))
                path = configuration.Main_Configuration.WhyLogIsNotWorkingPath.ToString();
            else
                path = Path.GetTempPath();

            //  Create directory if not exists
            if(!Directory.Exists(path))
                Directory.CreateDirectory(path);

            //  Checks file name
            if (configuration.Main_Configuration.WhyLogIsNotWorkingFileName is not null)
                fileName = configuration.Main_Configuration.WhyLogIsNotWorkingFileName.ToString();
            else
                fileName = "SimpLogError";

            await ImediateSaveMessageIntoLogFile(message, LogType.Error, path, fileName);
        }

        /// <summary>
        /// Checks all configurations should be saved log into a file at all
        /// </summary>
        /// <param name="saveType"></param>
        /// <param name="logType"></param>
        /// <returns></returns>
        internal bool ShouldSaveInFile(FileSaveType? saveType, LogType logType)
        {
            //  disabled from custom controller that should not be saved into a log file
            if(saveType.Equals(FileSaveType.DontSave) || (_Enable_File_Log is not null && _Enable_File_Log is false)) 
                return false;

            switch(logType)
            {
                case LogType.Trace:
                    {
                        if (_Trace_File is not null && _Trace_File is false)
                            return false;
                        break;
                    }
                case LogType.Debug:
                    {
                        if (_Debug_File is not null && _Debug_File is false)
                            return false;
                        break;
                    }
                case LogType.Info:
                    {
                        if (_Info_File is not null && _Info_File is false)
                            return false;
                        break;
                    }
                case LogType.Notice:
                    {
                        if (_Notice_File is not null && _Notice_File is false)
                            return false;
                        break;
                    }
                case LogType.Warn:
                    {
                        if (_Warn_File is not null && _Warn_File is false)
                            return false;
                        break;
                    }
                case LogType.Error:
                    {
                        if (_Error_File is not null && _Error_File is false)
                            return false;
                        break;
                    }
                case LogType.Fatal:
                    {
                        if (_Fatal_File is not null && _Fatal_File is false)
                            return false;
                        break;
                    }
            }

            return true;
        }

        /// <summary>
        /// Checks the configurations for sending Email.
        /// </summary>
        /// <param name="sendEmail"></param>
        /// <param name="logType"></param>
        /// <returns></returns>
        internal bool ShouldSendEmail(bool? sendEmail, LogType logType)
        {
            //  Check for global disable or for a specific log disable sending email. Also if Email_From or Email_To are empty
            //  there is no point of continue
            if( string.IsNullOrEmpty(configuration.Email_Configuration.Email_From) || 
                string.IsNullOrEmpty(configuration.Email_Configuration.Email_To) ||
                sendEmail is false || 
                (configuration.Email_Configuration.SendEmail_Globally is not null && configuration.Email_Configuration.SendEmail_Globally is false))
                return false;

            switch (logType)
            {
                case LogType.Trace:
                    {
                        if (_Trace_Email is not null && _Trace_Email is false)
                            return false;
                        break;
                    }
                case LogType.Debug:
                    {
                        if (_Debug_Email is not null && _Debug_Email is false)
                            return false;
                        break;
                    }
                case LogType.Info:
                    {
                        if (_Info_Email is not null && _Info_Email is false)
                            return false;
                        break;
                    }
                case LogType.Notice:
                    {
                        if (_Notice_Email is not null && _Notice_Email is false)
                            return false;
                        break;
                    }
                case LogType.Warn:
                    {
                        if (_Warn_Email is not null && _Warn_Email is false)
                            return false;
                        break;
                    }
                case LogType.Error:
                    {
                        if (_Error_Email is not null && _Error_Email is false)
                            return false;
                        break;
                    }
                case LogType.Fatal:
                    {
                        if (_Fatal_Email is not null && _Fatal_Email is false)
                            return false;
                        break;
                    }
            }

            return true;
        }

        /// <summary>
        /// Checks the configurations for saving in Db at all.
        /// </summary>
        /// <param name="saveInDatabase"></param>
        /// <param name="logType"></param>
        /// <returns></returns>
        internal bool ShouldSaveInDb(bool? saveInDatabase, LogType logType)
        {
            //  Check if the db log is active at global level.
            if(saveInDatabase is false ||
                (configuration.Database_Configuration.Global_Enabled_Save is not null && configuration.Database_Configuration.Global_Enabled_Save is false) ||
                configuration.Database_Configuration.Connection_String is null ||
                CheckDbTypeFormat() is false)
                return false;

            switch (logType)
            {
                case LogType.Trace:
                    {
                        if (_Trace_Db is not null && _Trace_Db is false)
                            return false;
                        break;
                    }
                case LogType.Debug:
                    {
                        if (_Debug_Db is not null && _Debug_Db is false)
                            return false;
                        break;
                    }
                case LogType.Info:
                    {
                        if (_Info_Db is not null && _Info_Db is false)
                            return false;
                        break;
                    }
                case LogType.Notice:
                    {
                        if (_Notice_Db is not null && _Notice_Db is false)
                            return false;
                        break;
                    }
                case LogType.Warn:
                    {
                        if (_Warn_Db is not null && _Warn_Db is false)
                            return false;
                        break;
                    }
                case LogType.Error:
                    {
                        if (_Error_Db is not null && _Error_Db is false)
                            return false;
                        break;
                    }
                case LogType.Fatal:
                    {
                        if (_Fatal_Db is not null && _Fatal_Db is false)
                            return false;
                        break;
                    }
            }

            return true;
        }

        /// <summary>
        /// Checks if the string typed as db format is in the options.
        /// </summary>
        /// <returns></returns>
        internal bool CheckDbTypeFormat()
        {
            switch (configuration.Database_Configuration.Global_Database_Type)
            {
                case "MSSql":
                    return true;
                case "MySql":
                    return true;
                case "Postgre":
                    return true;
                case "Oracle":
                    return true;
                case "MongoDb":
                    return true;
                default :
                    return false;
            }
        }

        /// <summary>
        /// Main logic for saving into a Log File
        /// </summary>
        /// <param name="path_to_save_log"></param>
        /// <param name="log_file_name"></param>
        /// <param name="saveType"></param>
        /// <param name="message"></param>
        /// <param name="logType"></param>
        /// <returns></returns>
        internal async Task SaveIntoFile(string? path_to_save_log, string? log_file_name, FileSaveType? saveType, string message, LogType logType)
        {
            //  Checks if there should be saved a log into a file.
            if (saveType == FileSaveType.DontSave)
                return;

            //  Gets the right path from three different options. First is custom path in the function. If there is no, it is
            //  searching into the appsettings.json file for a path. If again there is no, then it is using by default in 
            //  local user Temp directory.
            if(string.IsNullOrEmpty(path_to_save_log) && !string.IsNullOrEmpty(configuration.File_Configuration.PathToSaveLogs))
                path_to_save_log = configuration.File_Configuration.PathToSaveLogs;
            else if(string.IsNullOrEmpty(path_to_save_log) && string.IsNullOrEmpty(configuration.File_Configuration.PathToSaveLogs))
                path_to_save_log = Path.GetTempPath();

            //  Checks the path of the folder. If the path is false but it is given somewhere then use default one
            if(!ConfigurationServices.ConfigService.PathCheck(path_to_save_log))
                path_to_save_log = Path.GetTempPath();

            //  Gets file name. If there is setup one in the function name it is with the advantage. If not then takes
            //  from appsettings.json file. If the both are empty it creates default with format DayOfYear_SimpLog of the current date name.
            if (string.IsNullOrEmpty(log_file_name) && !string.IsNullOrEmpty(configuration.File_Configuration.LogFileName))
                log_file_name = configuration.File_Configuration.LogFileName;
            else if(string.IsNullOrEmpty(log_file_name) && string.IsNullOrEmpty(configuration.File_Configuration.LogFileName))
                log_file_name = "SimpLog_" + DateTime.Now.DayOfYear.ToString();
            //  else log_file_name is the value

            //  Checks the type of saving into a file
            if (saveType.Equals(FileSaveType.BufferMemory))
                await BufferMessage(message, logType, path_to_save_log, log_file_name);
            else
                await ImediateSaveMessageIntoLogFile(message, logType, path_to_save_log, log_file_name);
        }

        /// <summary>
        /// Main logic for sending Email
        /// </summary>
        /// <param name="sendEmail"></param>
        /// <param name="logType"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        internal async Task<bool> SendEmail(bool? sendEmail, LogType logType, string message)
        {
            //  Check if needs to send email notification
            if ((sendEmail is true || ToSendMail(logType)) && configuration.Email_Configuration.SendEmail_Globally is true)
                EmailServices.EmailService.SendMail(DateTime.UtcNow.ToString(DateFormat) + Separator + MessageType(logType) + Separator + message);
            else
                return false;

            return true;
        }

        /// <summary>
        /// Populates the object for StoreLog in database table
        /// </summary>
        /// <param name="message"></param>
        /// <param name="isEmailSent"></param>
        /// <param name="logType"></param>
        /// <param name="saveInDatabase"></param>
        /// <param name="saveType"></param>
        /// <param name="path_to_save_log"></param>
        /// <param name="log_file_name"></param>
        /// <returns></returns>
        internal StoreLog storeLog(
            string message, 
            bool? isEmailSent, 
            LogType? logType, 
            bool? saveInDatabase, 
            FileSaveType? saveType, 
            string? path_to_save_log,
            string? log_file_name)
        {
            StoreLog storeLog = new StoreLog()
            {
                Log_Created = DateTime.UtcNow.ToString(),
                Log_Error = message,
                Log_SendEmail = isEmailSent,
                Log_Type = logType.ToString(),
                Saved_In_Database = saveInDatabase,
                Log_File_Save_Type = saveType.Value.DisplayName()
            };

            if (!string.IsNullOrEmpty(path_to_save_log))
                storeLog.Log_Path = path_to_save_log;
            else if (!string.IsNullOrEmpty(configuration.File_Configuration.PathToSaveLogs))
                storeLog.Log_Path = configuration.File_Configuration.PathToSaveLogs;
            else
                storeLog.Log_Path = "";

            if (!string.IsNullOrEmpty(log_file_name))
                storeLog.Log_FileName = log_file_name;
            else if (!string.IsNullOrEmpty(configuration.File_Configuration.LogFileName))
                storeLog.Log_FileName = configuration.File_Configuration.LogFileName;
            else
                storeLog.Log_FileName = "";

            return storeLog;
        }

        /// <summary>
        /// Gets from configuration for every log type configuration for sending email. Is it enabled or disabled
        /// </summary>
        /// <param name="logType"></param>
        /// <returns></returns>
        internal bool ToSendMail(LogType logType)
        {
            switch(logType)
            { 
                case LogType.Trace:
                    return configuration.LogType.Trace.SendEmail.Value;
                case LogType.Debug:
                    return configuration.LogType.Debug.SendEmail.Value;
                case LogType.Info:
                    return configuration.LogType.Info.SendEmail.Value;
                case LogType.Notice:
                    return configuration.LogType.Notice.SendEmail.Value;
                case LogType.Warn:
                    return configuration.LogType.Warn.SendEmail.Value;
                case LogType.Error:
                    return configuration.LogType.Error.SendEmail.Value;
                case LogType.Fatal:
                    return configuration.LogType.Fatal.SendEmail.Value;
                default:
                    return false;
            }
        }
    }
}
