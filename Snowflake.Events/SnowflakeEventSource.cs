﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Events
{
    public partial class SnowflakeEventSource
    {
        public SnowflakeEventSource()
        {

        }
        public void RegisterEvent(Func<object, SnowflakeEventArgs> eventHandler, string eventName)
        {
           
        }
    }
}
