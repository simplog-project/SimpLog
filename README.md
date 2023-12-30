# What is SimpLog
Simple and very flexible tool for development with .NET Core. Saves logs to a file, or a database where you want. Also gives opportunity to 
receive logs via email with the proper configuration.

# Log Types in SimpLog
| Type | Description |
| ----- | ----- |
| Trace | This should be used during development to track bugs, but never committed to your VCS. |
| Debug | Log at this level about anything that happens in the program. This is mostly used during debugging, and I’d advocate trimming down the number of debug statement before entering the production stage, so that only the most meaningful entries are left, and can be activated during troubleshooting. |
| Info | Log at this level all actions that are user-driven, or system specific (ie regularly scheduled operations…) |
| Notice | This will certainly be the level at which the program will run when in production. Log at this level all the notable events that are not considered an error. |
| Warn | Log at this level all events that could potentially become an error. For instance if one database call took more than a predefined time, or if an in-memory cache is near capacity. This will allow proper automated alerting, and during troubleshooting will allow to better understand how the system was behaving before the failure. |
| Error | Log every error condition at this level. That can be API calls that return errors or internal error conditions. |
| Fatal | Too bad, it’s doomsday. Use this very scarcely, this shouldn’t happen a lot in a real program. Usually logging at this level signifies the end of the program. For instance, if a network daemon can’t bind a network socket, log at this level and exit is the only sensible thing to do. |

# Features of SimpLog

| Features | Description |
| ----- | ----- |
| &#128195; Log into a file | With SimpLog you can save logs into a file where you want on your local machine. |
| &#128231; Email notifications | With SimpLog you can also send emails with the logs. |
| &#128218; Log into database | With SimpLog you can save logs into a database. It is possible to save it in MSSql, MySql, PostgreSql and MongoDb. Have in mind, that if the tables are not created, they will be created automatically in the database from the connection string in SimpLog:Database_Configuration:Connection_String |
| &#128375; Support Debug | If you have any issues with the library, it has its log into a file. Just send us the content from there and we will take care of the rest. The configuration is in SimpLog:Main fields in appsettings.json.  |

# &#128218; Database structure for logging into a database

You need two tables, no matter the database. The databases are as follows:

| Database | Description |
| ----- | ----- |
| EmailLog | Saves what emails has been sent if there has been sent at all |
| StoreLog | Saves what is the error and has connection with EmailLog if the row has been sent via email. |

Scripts for creating tables:

| 🟢 MSSql |
| ----- |
```
if object_id('StoreLog', 'U') is null     
create table [StoreLog](        
    [ID] int IDENTITY(1,1) PRIMARY KEY,
    [Log_Type] varchar(50),
    [Log_Error] varchar(50),
    [Log_Created] varchar(50),
    [Log_FileName] varchar(50),
    [Log_Path] varchar(50),
    [Log_SendEmail] bit,
    [Email_ID] int,
    [Saved_In_Database] varchar(50)     
)  
if object_id('EmailLog', 'U') is null     
create table [EmailLog](        
    [ID] int IDENTITY(1,1) PRIMARY KEY,
    [From_Email] varchar(50),
    [To_Email] varchar(50),
    [Bcc] varchar(50),
    [Email_Subject] varchar(50),
    [Email_Body] varchar(50),
    [Time_Sent] varchar(50)     
) 

```

| 🐬 MySql |
| ----- |
```
create table if not exists StoreLog(        
    ID int AUTO_INCREMENT PRIMARY KEY,
    Log_Type varchar(50),
    Log_Error varchar(50),
    Log_Created varchar(50),
    Log_FileName varchar(50),
    Log_Path varchar(50),
    Log_SendEmail bit,
    Email_ID int,
    Saved_In_Database varchar(50)     
); 

create table if not exists EmailLog(        
    ID int AUTO_INCREMENT PRIMARY KEY,
    From_Email varchar(50),
    To_Email varchar(50),
    Bcc varchar(50),
    Email_Subject varchar(50),
    Email_Body varchar(50),
    Time_Sent varchar(50)     
) 
```
| 🐘 PostgreSql |
| ----- |
```
create table if not exists StoreLog(     
    "ID" serial,
    "Log_Type" varchar(50),
    "Log_Error" varchar(50),
    "Log_Created" varchar(50),
    "Log_FileName" varchar(50),
    "Log_Path" varchar(50),
    "Log_SendEmail" boolean,
    "Email_ID" int,
    "Saved_In_Database" varchar(50)     
);  
create table if not exists EmailLog(     
    "ID" serial,
    "From_Email" varchar(50),
    "To_Email" varchar(50),
    "Bcc" varchar(50),
    "Email_Subject" varchar(50),
    "Email_Body" varchar(50),
    "Time_Sent" varchar(50)     
); 
```

| MongoDb |
| ----- |
```
There is no script, just creates two collections called EmailLog and StoreLog.
```

# Configuration

**In Program.cs**
In Program.cs only if you use buffer type for saving into a log file. In other cases it is not needed at all.
```
await ServicesExtensions.SimpLog();
```

**In Controller**

```
private SimpLog _simpLog = new SimpLog();
```

and call the log like
```
_simpLog.Trace("place your message here");
```

options are as follows
```
_simpLog.Info({1}, {2}, {3}, {4}, {5}); 
```
and only {1} is required

| Option | Short Description | Full Description |
| ----- | ----- | ----- |
| {1} | Message | The message you want to log. |
| {2} | Save file type | There are three types of save type. See the table below |
| {3} | Send Email | If it is set to false the email notifications will be disabled only for this instance. If null or true, depending on the appsettings.json file EmailConfiguration section. |
| {4} | Path to save log | If you need this instance to be saved in a different path, here place the new path of the log file. If it is null, file path will be taken from appsettings.json. The default path, if not set anywhere is the temp folder of the current user on the local machine. |
| {5} | Log file name | If you need for this instance log file to be saved in a separate file with specific filename, set it here. If it is null, filename will be taken from appsettings.json. If no value is set here or in the appsettings.json the file will be with name SimpLog_ and todays number of the day of the year. If the log has been created on the first of February, then the filename if not set nowhere, by default will be SimpLog_32.txt |

File Save Types
| Name | Description |
| ----- | ----- |
| DontSave | This option disables saving into a file only when called for the specific case! |
| Standart | Opens file, saves it and closes file. On every request it opens and closes again the file |
| BufferMemory | Creates buffer memory and a background process is also working in this case. When the information in the buffer become more than 95 characters, opens log file, saves the content and closes it. It is not on every request but on 95 symbols or every minute. Possible data loss in this case! |

**In simplog.json**

Create simplog.json file in the root folder of your startup project. On the same level where is appsettings.json. Please have in mind that every configuration in simplog.json is optional ☺️

```
  "SimpLog": {                              -> all of the below fields are optional
    "Main": {
      "WhyLogIsNotWorkingPath": string,     ->  If you want it to be saved somewhere else, just place here the path. Default value is
      "WhyLogIsNotWorkingFileName": string, ->  The file name of the custom log for the SimpLog library. If you need our support, needs to be send the content of this file.
      "Disable_Log": bool                   ->  If you want to disable SimpLog library log for library issues, then turn it to true. Default value is
    },
    "File_Configuration": {                 -> The configuration for log into a file.
      "PathToSaveLogs": string,             -> Path to save the log. Default is Temp folder for the user in local machine.
      "LogFileName": string,                -> Log name of the log. Default value is "SimpLog_" + DateTime.Now.DayOfYear.ToString()
      "Enable_File_Log": bool               -> Default value is true. If you want to disable the file log, should be set to false.
    },
    "Email_Configuration": {                -> Email configuration.
      "SendEmail_Globally": bool,           -> Field to disable sending emails globally. Default value is true.
      "Email_From": string,                 -> Who will be the sender of your emails.
      "Email_To": string,                   -> Who will be the recipient of the emails.
      "Email_BCC": string,                  -> If you want, you can add blind copy.
      "Email_Connection": {                 -> Email configuration to the email service provider.
        "Host": string,
        "Port": string,
        "API_Key": string,
        "API_Value": string
      }
    },
    "Database_Configuration": {             -> Database configurations
      "Connection_String": string,          -> Depending on database type, use the correct connection string.
      "Global_Database_Type": string,       -> The type of a database. Please spell it correctly "MSSql" or "MySql" or "Postgre" or "MongoDb". N.B! Be carefull with spelling!
      "Use_OleDB": bool,                    -> Still not implementet, not any functionality here.
      "Global_Enabled_Save": bool           -> You can globally disable or enable saving into database. Default value is
    },
    "LogType": {
      "Trace": {                            -> TYPE OF LOG == Trace.
        "Log": true,                        -> For the type Trace, should be enabled or disabled logging into a file. Default value is true.
        "SendEmail": true,                  -> For the TYPE OF LOG, should be enabled or disabled sending emails. Default value is true.
        "SaveInDatabase": true              -> For the TYPE OF LOG, should be enabled saving into database. Default value is true.
      },
      "Debug": {                            -> Analogically TYPE OF LOG here is Debug 
        "Log": true,
        "SendEmail": true,
        "SaveInDatabase": true
      },
      "Info": {                             -> Analogically TYPE OF LOG here is Info
        "Log": true,
        "SendEmail": true,
        "SaveInDatabase": true
      },
      "Notice": {                           -> Analogically TYPE OF LOG here is Notice
        "Log": true,
        "SendEmail": true,
        "SaveInDatabase": true
      },
      "Warn": {                             -> Analogically TYPE OF LOG here is Warn
        "Log": true,
        "SendEmail": true,
        "SaveInDatabase": true
      },
      "Error": {                            -> Analogically TYPE OF LOG here is Error
        "Log": true,
        "SendEmail": true,
        "SaveInDatabase": true
      },
      "Fatal": {                            -> Analogically TYPE OF LOG here is Fatal
        "Log": true,
        "SendEmail": true,
        "SaveInDatabase": true
      }
    }
  }
  ```



# Default values

- If path was not set by you nowhere, check in Temp folder for your user in the local machine
- If name was not set nowhere, the default one is "SimpLog_" + DateTime.Now.DayOfYear.ToString()

Hope you enjoy the NuGet Package! 😉
