﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;
using Zio;
using Zio.FileSystems;
using FS = Snowflake.Model.FileSystem;

namespace Snowflake.FileSystem
{
    public class DirectoryTests
    {
        static string NormalizePath(string path)
        {
            return Path.GetFullPath(new Uri(path).LocalPath)
                       .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                       .ToLowerInvariant();
        }

        [Fact]
        public void DirectoryPath_Impl_Test()
        {
            var fs = new PhysicalFileSystem();
            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            Assert.Equal(NormalizePath(pfs.ConvertPathToInternal("/")),
                NormalizePath(temp));

            var pfs_sub = pfs.GetOrCreateSubFileSystem("/test");
            Assert.Equal(NormalizePath(pfs_sub.ConvertPathToInternal("/")),
                NormalizePath(Path.Combine(temp, "test")));

            var pfs_sub_sub = pfs_sub.GetOrCreateSubFileSystem("/test");
            Assert.Equal(NormalizePath(pfs_sub_sub.ConvertPathToInternal("/")),
                NormalizePath(Path.Combine(temp, "test", "test")));

        }

        [Fact]
        public void DirectoryPath_Test()
        {
            var fs = new PhysicalFileSystem();
            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            var dir = new FS.Directory("test", pfs);

            Assert.Equal(NormalizePath(dir.GetPath().FullName),
                NormalizePath(temp));

            Assert.Equal(NormalizePath(dir.OpenDirectory("test").GetPath().FullName),
              NormalizePath(Path.Combine(temp, "test")));

            Assert.Equal(NormalizePath(dir.OpenDirectory("test").OpenDirectory("test").GetPath().FullName),
                NormalizePath(Path.Combine(temp, "test", "test")));


        }


    }
}
