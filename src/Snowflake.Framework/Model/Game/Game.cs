﻿using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Model.Database;
using Snowflake.Model.FileSystem;
using Snowflake.Model.Records;
using Snowflake.Model.Records.File;
using Snowflake.Model.Records.Game;
using Zio;

namespace Snowflake.Model.Game
{
    public class Game : IGame
    {
        internal Game(IGameRecord record, IFileSystem gameFsRoot, FileRecordLibrary files)
        {
            this.Root = new Directory(gameFsRoot);
            this.FileRecordLibrary = files;
            this.Record = record;
            this.SavesRoot = this.Root.OpenDirectory("saves");
            this.ProgramRoot = this.Root.OpenDirectory("program");
            this.MediaRoot = this.Root.OpenDirectory("media");
            this.ResourceRoot = this.Root.OpenDirectory("resource");
            this.RuntimeRoot = this.Root.OpenDirectory("runtime");
            this.MiscRoot = this.Root.OpenDirectory("misc");
        }

        private Directory Root { get; }
        internal FileRecordLibrary FileRecordLibrary { get; }
        public IDirectory SavesRoot { get; }

        public IDirectory ProgramRoot { get; }

        public IDirectory MediaRoot { get; }

        public IDirectory MiscRoot { get; }

        public IDirectory ResourceRoot { get; }

        public IDirectory RuntimeRoot { get; }

        public IGameRecord Record { get; }

        public IEnumerable<IFileRecord> Files => this.FileRecordLibrary.GetFileRecords(this.Root);

        public IDirectory GetRuntimeLocation()
        {
            return this.RuntimeRoot.OpenDirectory(Guid.NewGuid().ToString());
        }

        public IDirectory GetSavesLocation(string saveType)
        {
            throw new NotImplementedException();
        }

        public IFileRecord? GetFileInfo(IFile file) => this.FileRecordLibrary.GetRecord(file);

        public IFileRecord RegisterFile(IFile file, string mimetype)
        {
            this.FileRecordLibrary.RegisterFile(file, mimetype);
            return this.GetFileInfo(file)!;
        }
    }
}
