﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Snowflake.Model.Database;
using Snowflake.Model.Database.Models;
using Snowflake.Model.Game;
using Xunit;
using Zio;
using Zio.FileSystems;

namespace Snowflake.Records.Tests
{
    public class GameLibraryIntegrationTests
    {
        [Fact]
        public void GameLibraryIntegrationCreate_Test()
        {
            var path = new DirectoryInfo(Path.GetTempPath())
                .CreateSubdirectory(Path.GetFileNameWithoutExtension(Path.GetTempFileName()));
            var fs = new PhysicalFileSystem();
            var gfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(path.FullName));

            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            var glib = new GameRecordLibrary(optionsBuilder);
            var flib = new FileRecordLibrary(optionsBuilder);

            var gl = new GameLibrary(glib, flib, gfs);
            var game = gl.CreateGame("NINTENDO_NES");
        }

        [Fact]
        public void GameLibraryIntegrationUpdate_Test()
        {
            var path = new DirectoryInfo(Path.GetTempPath())
                .CreateSubdirectory(Path.GetFileNameWithoutExtension(Path.GetTempFileName()));
            var fs = new PhysicalFileSystem();
            var gfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(path.FullName));

            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            var glib = new GameRecordLibrary(optionsBuilder);
            var flib = new FileRecordLibrary(optionsBuilder);

            var gl = new GameLibrary(glib, flib, gfs);
            var game = gl.CreateGame("NINTENDO_NES");
            game.Record.Title = "My Awesome Game";
            gl.UpdateGame(game.Record);

            var file = game.MiscRoot.OpenFile("Test.txt");
            file.OpenStream().Close();
            game.RegisterFile(file, "application/text");
            var record = game.GetFileInfo(file);
            record.Metadata.Add("file_metadata", "test");
            gl.UpdateFile(record);

            var newGame = gl.GetGame(game.Record.RecordId);
            Assert.NotEmpty(newGame.Files);
            
        }
    }
}
