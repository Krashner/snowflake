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
    }
}
