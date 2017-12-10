﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Snowflake.Extensibility.Provisioning;
using Snowflake.Extensibility.Provisioning.Standalone;
using Snowflake.Scraping.Attributes;

namespace Snowflake.Scraping
{
    public abstract class Culler : ProvisionedPlugin, ICuller
    {
        public Culler(Type pluginType, string targetType)
            : this(new StandalonePluginProvision(pluginType), targetType)
        {
        }

        public Culler(IPluginProvision pluginProvision, string targetType)
            : base(pluginProvision)
        {
            this.TargetType = targetType;
        }

        public string TargetType { get; }
        public abstract IEnumerable<ISeed> Filter(IEnumerable<ISeed> seedsToTrim, ISeedRootContext context);
    }
}
