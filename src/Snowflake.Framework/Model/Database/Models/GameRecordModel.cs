﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Snowflake.Model.Game;

namespace Snowflake.Model.Database.Models
{
    internal class GameRecordModel : RecordModel
    {
        public PlatformId Platform { get; set; }

        public List<GameRecordConfigurationProfileModel> ConfigurationProfiles { get; set; }

        internal static void SetupModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameRecordModel>()
                .Property(r => r.Platform)
                .HasConversion(p => p.ToString(),
                    p => new PlatformId(p))
                .IsRequired();
        }
    }
}
