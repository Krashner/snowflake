﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Plugin.Interface
{
    public interface IInfo
    {
        string PlatformId { get; }
        string Name { get; }
        IDictionary<string, string> Metadata { get; set; }

    }
}
