﻿using System.Data.SQLite;
using System.IO;

namespace Snowflake.Utility
{
    public abstract class BaseDatabase
    {
        public string FileName { get; }
        private readonly string dbConnectionString;

        public BaseDatabase(string fileName)
        {
            this.FileName = fileName;

            if (!File.Exists(this.FileName))
            {
                SQLiteConnection.CreateFile(this.FileName);
            }
            this.dbConnectionString = $"Data Source={this.FileName};Version=3;PRAGMA journal_mode=WAL;";
        }

        public SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(this.dbConnectionString);
        }
    }
}
