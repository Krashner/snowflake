﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Configuration
{
    public abstract class IterableConfigurationSection : ConfigurationSection, IIterableConfigurationSection
    {
        public int InterationNumber { get; set; }  
    }
}
