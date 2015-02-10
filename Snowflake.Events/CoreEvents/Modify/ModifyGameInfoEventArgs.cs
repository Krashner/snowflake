﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Game;
using Snowflake.Service;

namespace Snowflake.Events.CoreEvents.Modify
{
    public class ModifyGameInfoEventArgs : SnowflakeEventArgs
    {
        public IGameInfo PreviousGameInfo { get; private set; }
        public IGameInfo ModifiedGameInfo { get; set; }
        public ModifyGameInfoEventArgs(ICoreService eventCoreInstance, IGameInfo previousGameInfo, IGameInfo modifiedGameInfo)
            : base(eventCoreInstance)
        {
            this.PreviousGameInfo = previousGameInfo;
            this.ModifiedGameInfo = modifiedGameInfo;
        }
    }
}
