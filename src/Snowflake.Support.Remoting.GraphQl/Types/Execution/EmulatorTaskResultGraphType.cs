﻿using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;
using Snowflake.Execution.Extensibility;

namespace Snowflake.Support.Remoting.GraphQL.Types.Execution
{
    public class EmulatorTaskResultGraphType : ObjectGraphType<IEmulatorTaskResult>
    {
        public EmulatorTaskResultGraphType()
        {
            Name = "EmulatorTaskResult";
            Description = "The result of a running task.";
            Field(t => t.EmulatorName).Description("The name of the emulator executing this task.");
            Field(t => t.IsRunning).Description("Whether or not this task is currently running.");
        }
    }
}
