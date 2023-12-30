using MongoDB.Driver;
using MySqlConnector;
using Npgsql;
using SimpleLog.Entities;
using SimpleLog.Models.AppSettings;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLog.Services.DatabaseServices
{
    internal class DatabaseMigrations
    {
        public static Configuration conf = ConfigurationServices.ConfigService.BindConfigObject();

        /// <summary>
        /// Create MSSql tables if not exists.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="cmd"></param>
        public static void CreateMSSqlIfNotExists(SqlConnection connection, SqlCommand cmd)
        {
            StringBuilder query = new StringBuilder();

            query.Append($" if object_id({"'StoreLog'"}, {"'U'"}) is null ");
            query.Append($"    create table [StoreLog] ");
            query.Append($"    ( ");
            query.Append($"       [{"ID"}] int IDENTITY(1,1) PRIMARY KEY ");
            query.Append($"      ,[{"Log_Type"}] varchar(50) ");
            query.Append($"      ,[{"Log_Error"}] varchar(50) ");
            query.Append($"      ,[{"Log_Created"}] varchar(50) ");
            query.Append($"      ,[{"Log_FileName"}] varchar(50) ");
            query.Append($"      ,[{"Log_Path"}] varchar(50) ");
            query.Append($"      ,[{"Log_SendEmail"}] bit ");
            query.Append($"      ,[{"Email_ID"}] int ");
            query.Append($"      ,[{"Saved_In_Database"}] varchar(50) ");
            query.Append($"    ) ");

            query.Append($" if object_id({"'EmailLog'"}, {"'U'"}) is null ");
            query.Append($"    create table [EmailLog] ");
            query.Append($"    ( ");
            query.Append($"       [{"ID"}] int IDENTITY(1,1) PRIMARY KEY ");
            query.Append($"      ,[{"From_Email"}] varchar(50) ");
            query.Append($"      ,[{"To_Email"}] varchar(50) ");
            query.Append($"      ,[{"Bcc"}] varchar(50) ");
            query.Append($"      ,[{"Email_Subject"}] varchar(50) ");
            query.Append($"      ,[{"Email_Body"}] varchar(50) ");
            query.Append($"      ,[{"Time_Sent"}] varchar(50) ");
            query.Append($"    ) ");

            connection.Open();

            cmd.CommandText = query.ToString();
            cmd.ExecuteNonQuery();
            
            connection.Close();
        }

        /// <summary>
        /// Create MySql table if not exists.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="cmd"></param>
        public static void CreateMySqlIfNotExists(MySqlConnection connection, MySqlCommand cmd)
        {
            StringBuilder query = new StringBuilder();

            query.Append($"create table if not exists StoreLog ");
            query.Append($"    ( ");
            query.Append($"       ID int AUTO_INCREMENT PRIMARY KEY ");
            query.Append($"      ,Log_Type varchar(50) ");
            query.Append($"      ,Log_Error varchar(50) ");
            query.Append($"      ,Log_Created varchar(50) ");
            query.Append($"      ,Log_FileName varchar(50) ");
            query.Append($"      ,Log_Path varchar(50) ");
            query.Append($"      ,Log_SendEmail bit ");
            query.Append($"      ,Email_ID int ");
            query.Append($"      ,Saved_In_Database varchar(50) ");
            query.Append($"    ); ");

            query.Append($"create table if not exists EmailLog ");
            query.Append($"    ( ");
            query.Append($"       ID int AUTO_INCREMENT PRIMARY KEY ");
            query.Append($"      ,From_Email varchar(50) ");
            query.Append($"      ,To_Email varchar(50) ");
            query.Append($"      ,Bcc varchar(50) ");
            query.Append($"      ,Email_Subject varchar(50) ");
            query.Append($"      ,Email_Body varchar(50) ");
            query.Append($"      ,Time_Sent varchar(50) ");
            query.Append($"    ) ");

            connection.Open();

            cmd.CommandText = query.ToString();
            cmd.ExecuteNonQuery();

            connection.Close();
        }

        public static void CreateMariaDbIfNotExists()
        {

        }

        /// <summary>
        /// Create PostgreSql tables if not exists
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="cmd"></param>
        public static void CreatePostgreSqlIfNotExists(NpgsqlConnection connection, NpgsqlCommand cmd)
        {
            StringBuilder query = new StringBuilder();
            query.Append($" create table if not exists StoreLog ");
            query.Append($"    ( ");
            query.Append($"    \"{"ID"}\" serial ");
            query.Append($"   ,\"{"Log_Type"}\" varchar(50) ");
            query.Append($"   ,\"{"Log_Error"}\" varchar(50) ");
            query.Append($"   ,\"{"Log_Created"}\" varchar(50) ");
            query.Append($"   ,\"{"Log_FileName"}\" varchar(50) ");
            query.Append($"   ,\"{"Log_Path"}\" varchar(50) ");
            query.Append($"   ,\"{"Log_SendEmail"}\" boolean ");
            query.Append($"   ,\"{"Email_ID"}\" int ");
            query.Append($"   ,\"{"Saved_In_Database"}\" varchar(50) ");
            query.Append($"    ); ");

            query.Append($" create table if not exists EmailLog ");
            query.Append($"    ( ");
            query.Append($"    \"{"ID"}\" serial ");
            query.Append($"   ,\"{"From_Email"}\" varchar(50) ");
            query.Append($"   ,\"{"To_Email"}\" varchar(50) ");
            query.Append($"   ,\"{"Bcc"}\" varchar(50) ");
            query.Append($"   ,\"{"Email_Subject"}\" varchar(50) ");
            query.Append($"   ,\"{"Email_Body"}\" varchar(50) ");
            query.Append($"   ,\"{"Time_Sent"}\" varchar(50) ");
            query.Append($"    ); ");

            cmd.CommandText = query.ToString();

            connection.Open();

            cmd.ExecuteNonQuery();

            connection.Close();
        }

        public static void CreateOracleIfNotExists()
        {
            StringBuilder query = new StringBuilder();
            query.Append($" create table if not exists StoreLog ");
            query.Append($"    ( ");
            query.Append($"    \"{"ID"}\" number GENERATED BY DEFAULT AS IDENTITY ");
            query.Append($"   ,\"{"Log_Type"}\" varchar(50) ");
            query.Append($"   ,\"{"Log_Error"}\" varchar(50) ");
            query.Append($"   ,\"{"Log_Created"}\" varchar(50) ");
            query.Append($"   ,\"{"Log_FileName"}\" varchar(50) ");
            query.Append($"   ,\"{"Log_Path"}\" varchar(50) ");
            query.Append($"   ,\"{"Log_SendEmail"}\" char(1) ");
            query.Append($"   ,\"{"Email_ID"}\" number ");
            query.Append($"   ,\"{"Saved_In_Database"}\" varchar(50) ");
            query.Append($"    ); ");

            query.Append($" create table if not exists EmailLog ");
            query.Append($"    ( ");
            query.Append($"    \"{"ID"}\" number GENERATED BY DEFAULT AS IDENTITY ");
            query.Append($"   ,\"{"From_Email"}\" varchar(50) ");
            query.Append($"   ,\"{"To_Email"}\" varchar(50) ");
            query.Append($"   ,\"{"Bcc"}\" varchar(50) ");
            query.Append($"   ,\"{"Email_Subject"}\" varchar(50) ");
            query.Append($"   ,\"{"Email_Body"}\" varchar(50) ");
            query.Append($"   ,\"{"Time_Sent"}\" varchar(50) ");
            query.Append($"    ); ");
        }

        /// <summary>
        /// Create MongoDb tables if not exists
        /// </summary>
        /// <param name="database"></param>
        public static void CreateMongoDbIfNotExists(IMongoDatabase database)
        {
            //  Create collection EmailLog if not exists
            if (!database.ListCollectionNames().ToList().Contains("EmailLog"))
                database.CreateCollectionAsync("EmailLog", new CreateCollectionOptions());

            //  Create collection StoreLog if not exists
            if (!database.ListCollectionNames().ToList().Contains("StoreLog"))
                database.CreateCollectionAsync("StoreLog", new CreateCollectionOptions());
        }
    }
}
