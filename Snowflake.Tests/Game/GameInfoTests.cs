﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Game;
using Snowflake.Tests.Fakes;
using Xunit;
namespace Snowflake.Game.Tests
{
    public class GameInfoTests
    {
        [Fact]
        public void GameInfoCreation_Test()
        {
            Assert.NotNull(new GameInfo("TEST", "TEST", new Dictionary<string, string>(){{"snowflake_mediastore", "TEST"}}, "TEST", "TEST.TEST", "TEST"));
        }

    }
}
