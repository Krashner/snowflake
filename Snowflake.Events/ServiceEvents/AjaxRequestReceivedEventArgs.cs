﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Ajax;
using Snowflake.Service;
namespace Snowflake.Events.ServiceEvents
{
    public class AjaxResponseSendingEventArgs : SnowflakeEventArgs
    {
        public IJSResponse SendingResponse { get; set; }
        public AjaxResponseSendingEventArgs(ICoreService eventCoreInstance, IJSResponse sendingResponse)
            : base(eventCoreInstance)
        {
            this.SendingResponse = sendingResponse;
        }
    }
}
