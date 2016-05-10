﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Records.Metadata;
using Snowflake.Utility;

namespace Snowflake.Records.Game
{
    public class SqliteGameLibrary : IGameLibrary
    {
        public IMetadataStore MetadataStore { get; }

        private readonly SqliteDatabase backingDatabase;
        public SqliteGameLibrary(SqliteDatabase database)
        {
            this.backingDatabase = database;
            this.MetadataStore = new SqliteMetadataStore(database);
            this.CreateDatabase();
        }
        private void CreateDatabase()
        {
            this.backingDatabase.CreateTable("games",
                "uuid TEXT PRIMARY KEY",
                "record TEXT",
                "key TEXT",
                "value TEXT");
        }
        public void Add(IGameRecord record)
        {
            throw new NotImplementedException();
        }

        public void Remove(IGameRecord record)
        {
            throw new NotImplementedException();
        }

        public IGameRecord Get(Guid guid)
        {
            throw new NotImplementedException();
        }

        public void Remove(Guid guid)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IGameRecord> SearchByMetadata(string key, string likeValue)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IGameRecord> GetByMetadata(string key, string exactValue)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IGameRecord> GetRecords()
        {
            throw new NotImplementedException();
        }

       
        public IEnumerable<IGameRecord> GetGameRecords()
        {
            throw new NotImplementedException();
        }

        public IGameRecord GetGameByUuid(Guid uuid)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IGameRecord> GetGamesByTitle(string nameSearch)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IGameRecord> GetGamesByPlatform(string platformId)
        {
            throw new NotImplementedException();
        }
    }
}
