﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Emulator.Configuration.Mapping;
using Snowflake.Emulator.Configuration.Template;
using Snowflake.Extensions;

using SharpYaml.Serialization;

namespace Snowflake.Emulator.Configuration
{
    public class ConfigurationProfile 
    {
        //todo write a proper 
        public IReadOnlyDictionary<string, dynamic> ConfigurationValues { get; private set; }
        public string TemplateID { get; private set; }
     
        public ConfigurationProfile (string templateId, IDictionary<string, dynamic> value)
        {
            this.ConfigurationValues = value.AsReadOnly();
            this.TemplateID = templateId;
        }

        public static ConfigurationProfile FromDictionary (IDictionary<string, dynamic> protoTemplate)
        {
            return new ConfigurationProfile(protoTemplate["TemplateID"], ((IDictionary<object, dynamic>)protoTemplate["ConfigurationValues"])
                .ToDictionary(value => (string)value.Key, value => value.Value));
        }

        public static IList<ConfigurationProfile> FromManyDictionaries(IList<IDictionary<string, dynamic>> protoTemplates)
        {
            return protoTemplates.Select(protoTemplate => ConfigurationProfile.FromDictionary(protoTemplate)).ToList();
        }

    }
}
