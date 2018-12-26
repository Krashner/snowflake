﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Zio;

namespace Snowflake.Model.FileSystem
{
    public class File : IFile
    {
        internal File(IDirectory parentDirectory, FileEntry file)
        {
            this.RawInfo = file;
            this.ParentDirectory = parentDirectory;
        }

        public string Name => this.RawInfo.Name;

        private FileEntry RawInfo { get; }

        public long Length => this.RawInfo.Length;

        public IDirectory ParentDirectory { get; }

        public bool Created => this.RawInfo.Exists;

        public Stream OpenStream()
        {
            return this.OpenStream(FileAccess.ReadWrite);
        }

        public Stream OpenStream(FileAccess rw)
        {
            return this.RawInfo.Open(FileMode.OpenOrCreate, rw);
        }

        public void Rename(string newName)
        {
            this.RawInfo.MoveTo((UPath)"/" / Path.GetFileName(newName));
        }

        public void Delete()
        {
            this.RawInfo.Delete();
        }
    }
}
