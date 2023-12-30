using DnsClient.Protocol;
using Microsoft.VisualBasic;
using MongoDB.Bson;
using MongoDB.Driver;
using MySqlConnector;
using Npgsql;
using NpgsqlTypes;
using Oracle.ManagedDataAccess.Client;
using SimpleLog.Entities;
using SimpleLog.Models;
using SimpleLog.Models.AppSettings;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static System.Net.WebRequestMethods;

namespace SimpleLog.Services.DatabaseServices
{
    internal class DatabaseServices
    {
        public static Configuration conf = ConfigurationServices.ConfigService.BindConfigObject();

        /// <summary>
        /// Depending on the name of the DB, goes to the function for that stuff.
        /// </summary>
        /// <param name="DbName"></param>
        /// <param name="storeLog"></param>
        /// <param name="isEmailSend"></param>
        /// <param name="saveInDatabase"></param>
        public static void SaveIntoDatabase(string DbName, StoreLog storeLog, bool? isEmailSend, bool? saveInDatabase)
        {
            if(DbName.Equals(Global_Database_Type.MSSql.DisplayName()))
                InsertIntoMSSql(storeLog, isEmailSend);
            else if(DbName.Equals(Global_Database_Type.MySql.DisplayName()))
                InsertIntoMySql(storeLog, isEmailSend);
            //else if (DbName.Equals(Global_Database_Type.MariaDb.DisplayName()))
            //    InsertIntoMariaDb(storeLog, isEmailSend);
            else if(DbName.Equals(Global_Database_Type.PostgreSql.DisplayName()))
                InsertIntoPostgreSql(storeLog, isEmailSend);
            //else if(DbName.Equals(Global_Database_Type.Oracle.DisplayName()))
            //    InsertIntoOracle(storeLog, isEmailSend);
            else if(DbName.Equals(Global_Database_Type.MongoDb.DisplayName()))
                InsertIntoMongoDb(storeLog, isEmailSend);
            else
                return;
        }

        /// <summary>
        /// Insert log into MSSql database.
        /// </summary>
        /// <param name="storeLog"></param>
        /// <param name="isEmailSend"></param>
        public static void InsertIntoMSSql(StoreLog storeLog, bool? isEmailSend)
        {
            SqlConnection connection = new SqlConnection(conf.Database_Configuration.Connection_String);
            SqlCommand cmd = new SqlCommand(null, connection);

            DatabaseMigrations.CreateMSSqlIfNotExists(connection, cmd);

            connection.Open();

            int EmailID = 0;

            string query = string.Empty;

            if (isEmailSend is true)
            {
                query = "INSERT INTO EmailLog(From_Email, To_Email, Bcc, Email_Subject, Email_Body, Time_Sent) " +
                    "VALUES(@From_Email, @To_Email, @Bcc, @Email_Subject, @Email_Body, @Time_Sent);";

                cmd.Parameters.AddWithValue("@From_Email", conf.Email_Configuration.Email_From);
                cmd.Parameters.AddWithValue("@To_Email", conf.Email_Configuration.Email_To);
                cmd.Parameters.AddWithValue("@Bcc", conf.Email_Configuration.Email_Bcc);
                cmd.Parameters.AddWithValue("@Email_Subject", storeLog.Log_Type + " " + storeLog.Log_Created);
                cmd.Parameters.AddWithValue("@Email_Body", storeLog.Log_Error);
                cmd.Parameters.AddWithValue("@Time_Sent", DateTime.UtcNow.ToString());

                cmd.CommandText = query + "SELECT  SCOPE_IDENTITY()";

                EmailID = Int32.Parse(cmd.ExecuteScalar().ToString());
            }

            query = "INSERT INTO StoreLog(Log_Type, Log_Error, Log_Created, Log_FileName, Log_Path, Log_SendEmail, Email_ID, Saved_In_Database) " +
                "VALUES(@Log_Type, @Log_Error, @Log_Created, @Log_FileName, @Log_Path, @Log_SendEmail, @Email_ID, @Saved_In_Database)";

            cmd.Parameters.AddWithValue("@Log_Type", storeLog.Log_Type);
            cmd.Parameters.AddWithValue("@Log_Error", storeLog.Log_Error);
            cmd.Parameters.AddWithValue("@Log_Created", storeLog.Log_Created);
            cmd.Parameters.AddWithValue("@Log_FileName", storeLog.Log_FileName);
            cmd.Parameters.AddWithValue("@Log_Path", storeLog.Log_Path);
            cmd.Parameters.AddWithValue("@Log_SendEmail", isEmailSend);
            cmd.Parameters.AddWithValue("@Email_ID", EmailID);
            cmd.Parameters.AddWithValue("@Saved_In_Database", DateTime.UtcNow.ToString());

            cmd.CommandText = query;

            cmd.ExecuteNonQuery();

            connection.Close();
        }

        /// <summary>
        /// Insert log into MySql database.
        /// </summary>
        /// <param name="storeLog"></param>
        /// <param name="isEmailSend"></param>
        public static void InsertIntoMySql(StoreLog storeLog, bool? isEmailSend)
        {
            MySqlConnection connection = new MySqlConnection(conf.Database_Configuration.Connection_String);
            MySqlCommand cmd = new MySqlCommand(null, connection);

            DatabaseMigrations.CreateMySqlIfNotExists(connection, cmd);
            
            connection.Open();
            
            int EmailID = 0;

            string query = string.Empty;

            if(isEmailSend is true)
            {
                query = "INSERT INTO EmailLog(From_Email, To_Email, Bcc, Email_Subject, Email_Body, Time_Sent) " +
                    "VALUES(@From_Email, @To_Email, @Bcc, @Email_Subject, @Email_Body, @Time_Sent);";

                cmd.Parameters.AddWithValue("@From_Email", conf.Email_Configuration.Email_From);
                cmd.Parameters.AddWithValue("@To_Email", conf.Email_Configuration.Email_To);
                cmd.Parameters.AddWithValue("@Bcc", conf.Email_Configuration.Email_Bcc);
                cmd.Parameters.AddWithValue("@Email_Subject", storeLog.Log_Type + " " + storeLog.Log_Created);
                cmd.Parameters.AddWithValue("@Email_Body", storeLog.Log_Error);
                cmd.Parameters.AddWithValue("@Time_Sent", DateTime.UtcNow.ToString());

                cmd.CommandText = query;
                cmd.ExecuteNonQuery();

                EmailID = (int)cmd.LastInsertedId;
            }

            query = "INSERT INTO StoreLog(Log_Type, Log_Error, Log_Created, Log_FileName, Log_Path, Log_SendEmail, Email_ID, Saved_In_Database) " +
                "VALUES(@Log_Type, @Log_Error, @Log_Created, @Log_FileName, @Log_Path, @Log_SendEmail, @Email_ID, @Saved_In_Database)";

            cmd.Parameters.AddWithValue("@Log_Type", storeLog.Log_Type);
            cmd.Parameters.AddWithValue("@Log_Error", storeLog.Log_Error);
            cmd.Parameters.AddWithValue("@Log_Created", storeLog.Log_Created);
            cmd.Parameters.AddWithValue("@Log_FileName", storeLog.Log_FileName);
            cmd.Parameters.AddWithValue("@Log_Path", storeLog.Log_Path);
            cmd.Parameters.AddWithValue("@Log_SendEmail", isEmailSend);
            cmd.Parameters.AddWithValue("@Email_ID", EmailID);
            cmd.Parameters.AddWithValue("@Saved_In_Database", DateTime.UtcNow.ToString());

            connection.Open();
            cmd.CommandText = query;

            cmd.ExecuteNonQuery();

            connection.Close();

        }

        public static void InsertIntoMariaDb(StoreLog storeLog, bool? isEmailSend)
        {
            //  To be Implemented
        }

        /// <summary>
        /// Insert log into PostgreSql database.
        /// </summary>
        /// <param name="storeLog"></param>
        /// <param name="isEmailSend"></param>
        public static void InsertIntoPostgreSql(StoreLog storeLog, bool? isEmailSend)
        {
            NpgsqlConnection connection = new NpgsqlConnection(conf.Database_Configuration.Connection_String);
            NpgsqlCommand cmd = new NpgsqlCommand(null, connection);

            DatabaseMigrations.CreatePostgreSqlIfNotExists(connection, cmd);

            connection.Open();

            int EmailID = 0;

            string query = string.Empty;

            if (isEmailSend is true)
            {
                query = "INSERT INTO EmailLog(\"From_Email\", \"To_Email\", \"Bcc\", \"Email_Subject\", \"Email_Body\", \"Time_Sent\") " +
                    "VALUES(@From_Email, @To_Email, @Bcc, @Email_Subject, @Email_Body, @Time_Sent) Returning \"ID\"";

                cmd.Parameters.AddWithValue("@From_Email", conf.Email_Configuration.Email_From);
                cmd.Parameters.AddWithValue("@To_Email", conf.Email_Configuration.Email_To);
                cmd.Parameters.AddWithValue("@Bcc", conf.Email_Configuration.Email_Bcc);
                cmd.Parameters.AddWithValue("@Email_Subject", storeLog.Log_Type + " " + storeLog.Log_Created);
                cmd.Parameters.AddWithValue("@Email_Body", storeLog.Log_Error);
                cmd.Parameters.AddWithValue("@Time_Sent", DateTime.UtcNow.ToString());

                cmd.CommandText = query;

                EmailID = (int)cmd.ExecuteScalar();
            }

            query = "INSERT INTO StoreLog(\"Log_Type\", \"Log_Error\", \"Log_Created\", \"Log_FileName\", \"Log_Path\", \"Log_SendEmail\", \"Email_ID\", \"Saved_In_Database\") " +
                "VALUES(@Log_Type, @Log_Error, @Log_Created, @Log_FileName, @Log_Path, @Log_SendEmail, @Email_ID, @Saved_In_Database)";

            cmd.Parameters.AddWithValue("@Log_Type", storeLog.Log_Type);
            cmd.Parameters.AddWithValue("@Log_Error", storeLog.Log_Error);
            cmd.Parameters.AddWithValue("@Log_Created", storeLog.Log_Created);
            cmd.Parameters.AddWithValue("@Log_FileName", storeLog.Log_FileName);
            cmd.Parameters.AddWithValue("@Log_Path", storeLog.Log_Path);
            cmd.Parameters.AddWithValue("@Log_SendEmail", isEmailSend);
            cmd.Parameters.AddWithValue("@Email_ID", EmailID);
            cmd.Parameters.AddWithValue("@Saved_In_Database", DateTime.UtcNow.ToString());

            cmd.CommandText = query;

            cmd.ExecuteNonQuery();

            connection.Close();
        }

        public static void InsertIntoOracle(StoreLog storeLog, bool? isEmailSend)
        {
            //  To be implemented
        }

        /// <summary>
        /// Insert Log into the MongoDb database
        /// </summary>
        /// <param name="storeLog"></param>
        /// <param name="isEmailSend"></param>
        public static void InsertIntoMongoDb(StoreLog storeLog, bool? isEmailSend)
        {
            //  Make MongoDb Connection
            MongoClient dbClient = new MongoClient(conf.Database_Configuration.Connection_String);

            //  Get the database
            var database = new MongoClient().GetDatabase(MongoUrl.Create(conf.Database_Configuration.Connection_String).DatabaseName);

            DatabaseMigrations.CreateMongoDbIfNotExists(database);

            int EmailID = 0;

            //  Insert Document into collection EmailLog
            if (isEmailSend is true)
            {
                var emailLogCollection = database.GetCollection<EmailLog>("EmailLog");

                EmailLog emailLog = new EmailLog()
                {
                    ID = (int)emailLogCollection.CountDocuments(Builders<EmailLog>.Filter.Empty, new CountOptions() { Hint = "_id_" }) + 1,
                    From_Email = conf.Email_Configuration.Email_From,
                    To_Email = conf.Email_Configuration.Email_To,
                    Bcc = conf.Email_Configuration.Email_Bcc,
                    Email_Subject = storeLog.Log_Type + " " + storeLog.Log_Created,
                    Email_Body = storeLog.Log_Error,
                    Time_Sent = DateTime.UtcNow.ToString()
                };

                emailLogCollection.InsertOne(emailLog);

                EmailID = emailLog.ID;
            }

            //  Insert Document into collection StoreLog
            var storeLogCollection = database.GetCollection<StoreLog>("StoreLog");

            storeLog.ID = (int)storeLogCollection.CountDocuments(Builders<StoreLog>.Filter.Empty, new CountOptions() { Hint = "_id_"}) + 1;
            storeLog.Email_ID = EmailID;

            storeLogCollection.InsertOne(storeLog);
        }
    }
}
