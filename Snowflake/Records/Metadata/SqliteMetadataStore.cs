﻿using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Snowflake.Utility;

namespace Snowflake.Records.Metadata
{
    public class SqliteMetadataStore : IMetadataStore
    {
        private readonly SqliteDatabase backingDatabase;
        public SqliteMetadataStore(SqliteDatabase database)
        {
            this.backingDatabase = database;
            this.CreateDatabase();
        }

        private void CreateDatabase()
        {
            this.backingDatabase.Execute(@"CREATE TABLE IF NOT EXISTS metadata(
                                                                uuid TEXT PRIMARY KEY,
                                                                record TEXT,
                                                                key TEXT,
                                                                value TEXT
                                                                )");
        }

        public void Set(IRecordMetadata metadata)
        {

            this.backingDatabase.Execute(@"INSERT OR REPLACE INTO metadata(uuid, record, key, value) 
                                        VALUES (
                                          @Guid,
                                          @Record,
                                          @Key,
                                          @Value", metadata);

        }

        public void Remove(IRecordMetadata metadata)
        {
            this.backingDatabase.Execute(@"INSERT OR REPLACE INTO metadata(uuid, record, key, value) 
                                        VALUES (
                                          @Guid,
                                          @Record,
                                          @Key,
                                          @Value", metadata);
        }

        public void Remove(Guid metadataId)
        {
            this.backingDatabase.Execute("DELETE FROM metadata WHERE uuid = @metadataId", new { metadataId });
        }

        public IDictionary<string, IRecordMetadata> GetAllForElement(Guid target)
        {
            return this.backingDatabase.Query<RecordMetadata>
                (@"SELECT * FROM metadata WHERE record = @target", new { target } )
                .ToDictionary(m => m.Key, m => m as IRecordMetadata);
        }


        public IEnumerable<IRecordMetadata> Search(string key, string likeValue)
        {

            return this.backingDatabase.Query<RecordMetadata>
                (@"SELECT * FROM metadata WHERE key = @key AND value LIKE %@likeValue%",
                    new { key, likeValue });
        }

        public IEnumerable<IRecordMetadata> GetAll(string key, string exactValue)
        {

            return this.backingDatabase.Query<RecordMetadata>
                (@"SELECT * FROM metadata WHERE key = @key AND value = @exactValue",
                    new { key, exactValue });
        }


        public IEnumerable<IRecordMetadata> GetAll()
        {
            return this.backingDatabase.Query<RecordMetadata>(@"SELECT * FROM metadata");
        }

        public IRecordMetadata Get(Guid metadataId)
        {
            return this.backingDatabase.QueryFirstOrDefault<RecordMetadata>
                (@"SELECT * FROM metadata WHERE uuid = @metadataId LIMIT 1", new { metadataId });
        }

        public IRecordMetadata Get(string key, Guid recordId)
        {
            return this.backingDatabase.QueryFirstOrDefault<RecordMetadata>
                ($@"SELECT * FROM metadata WHERE record = @recordId AND key = @key LIMIT 1",
                    new { key, recordId });
        }
    }
}
