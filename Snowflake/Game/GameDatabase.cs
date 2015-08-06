﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using Newtonsoft.Json;
using Snowflake.Utility;

namespace Snowflake.Game
{
    public class GameDatabase : BaseDatabase, IGameDatabase
    {
        public GameDatabase(string fileName)
            : base(fileName)
        {
            this.CreateDatabase();
        }

        private void CreateDatabase()
        {
            SQLiteConnection dbConnection = this.GetConnection();
            dbConnection.Open();
            var sqlCommand = new SQLiteCommand(@"CREATE TABLE IF NOT EXISTS games(
                                                                platform_id TEXT,
                                                                uuid TEXT PRIMARY KEY,
                                                                filename TEXT,
                                                                name TEXT,
                                                                metadata TEXT,
                                                                crc32 TEXT
                                                                )", dbConnection);
            sqlCommand.ExecuteNonQuery();
            dbConnection.Close();
        }

        public void AddGame(IGameInfo game)
        {
            SQLiteConnection dbConnection = this.GetConnection();
            dbConnection.Open();
            using (var sqlCommand = new SQLiteCommand(@"INSERT OR REPLACE INTO games VALUES(
                                          @platform_id,
                                          @uuid,
                                          @filename,
                                          @name,
                                          @metadata,
                                          @crc32)", dbConnection))
            {
                sqlCommand.Parameters.AddWithValue("@platform_id", game.PlatformID);
                sqlCommand.Parameters.AddWithValue("@uuid", game.UUID);
                sqlCommand.Parameters.AddWithValue("@filename", game.FileName);
                sqlCommand.Parameters.AddWithValue("@name", game.Name);
                sqlCommand.Parameters.AddWithValue("@metadata", JsonConvert.SerializeObject(game.Metadata));
                sqlCommand.Parameters.AddWithValue("@crc32", game.CRC32);
                sqlCommand.ExecuteNonQuery();
            }
            dbConnection.Close();
        }

        public void RemoveGame(IGameInfo game)
        {
            SQLiteConnection dbConnection = this.GetConnection();
            dbConnection.Open();
            using (var sqlCommand = new SQLiteCommand("DELETE FROM `games` WHERE `uuid` == @uuid", dbConnection))
            {
                sqlCommand.Parameters.AddWithValue("@uuid", game.UUID);
                sqlCommand.ExecuteNonQuery();
            }
            dbConnection.Close();
        }

        public IGameInfo GetGameByUUID(string uuid)
        {
            try
            {
                return this.GetGamesByColumn("uuid", uuid)[0];
            }
            catch
            {
                return null;
            }
        }

        public IList<IGameInfo> GetGamesByPlatform(string platformId)
        {
            return this.GetGamesByColumn("platform_id", platformId);
        }
        public IList<IGameInfo> GetGamesByName(string nameSearch)
        {
            return this.GetGamesByColumn("name", nameSearch);
        }
        private IList<IGameInfo> GetGamesByColumn(string colName, string searchQuery)
        {
            SQLiteConnection dbConnection = this.GetConnection();
            dbConnection.Open();
            using (var sqlCommand = new SQLiteCommand($@"SELECT * FROM `games` WHERE `{colName}` == @searchQuery"
                , dbConnection))
            {
                sqlCommand.Parameters.AddWithValue("@searchQuery", searchQuery);
                using (var reader = sqlCommand.ExecuteReader())
                {
                    var result = new DataTable();
                    lock (reader) {
                        try
                        {
                            result.Load(reader);
                        }
                        catch (AccessViolationException)
                        {
                            System.Threading.Thread.Sleep(500);
                            result.Load(reader);
                        }
                        var gamesResults = (from DataRow row in result.Rows select this.GetGameFromDataRow(row)).ToList();
                        dbConnection.Close();
                        return gamesResults;
                    }
                }
            }
        }
        public IList<IGameInfo> GetAllGames()
        {
            SQLiteConnection dbConnection = this.GetConnection();
            dbConnection.Open();
            using (var sqlCommand = new SQLiteCommand(@"SELECT * FROM `games`"
                , dbConnection))
            {
                using (var reader = sqlCommand.ExecuteReader())
                {
                    var result = new DataTable();
                    result.Load(reader);
                    var gamesResults = (from DataRow row in result.Rows select this.GetGameFromDataRow(row)).ToList();
                    dbConnection.Close();
                    return gamesResults;
                }
            }
        }
        private IGameInfo GetGameFromDataRow(DataRow row)
        {
            var platformId = row.Field<string>("platform_id");
            var uuid = row.Field<string>("uuid");
            var fileName = row.Field<string>("filename");
            var name = row.Field<string>("name");
            var metadata = JsonConvert.DeserializeObject<IDictionary<string, string>>(row.Field<string>("metadata"));
            var crc32 = row.Field<string>("crc32");

            return new GameInfo(platformId, name, metadata, uuid, fileName, crc32);
        }

    }
}
