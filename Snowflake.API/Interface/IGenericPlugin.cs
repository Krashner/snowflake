﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;

namespace Snowflake.API.Interface
{
    [InheritedExport(typeof(IGenericPlugin))]
    public interface IGenericPlugin:IPlugin
    {
    }
}
