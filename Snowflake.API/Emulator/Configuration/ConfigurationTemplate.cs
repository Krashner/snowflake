﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Snowflake.Extensions;
using Snowflake.Emulator.Configuration.Mapping;
using Snowflake.Emulator.Configuration.Type;
using SharpYaml.Serialization;

namespace Snowflake.Emulator.Configuration
{
    public class ConfigurationTemplate
    {
        private string stringTemplate;
        private IDictionary<string, ConfigurationEntry> configurationEntries;
        private IDictionary<string, CustomType> customTypes;
        public string ConfiguratioName { get; private set; }
        public IReadOnlyDictionary<string, ConfigurationEntry> ConfigurationEntries { get { return this.configurationEntries.AsReadOnly(); } }
        public string FileName { get; private set; }
        public BooleanMapping BooleanMapping { get; private set; }
        public IReadOnlyDictionary<string, CustomType> CustomTypes { get { return this.customTypes.AsReadOnly();  } }
        public ConfigurationTemplate(string stringTemplate, IDictionary<string, ConfigurationEntry> configurationEntries, IDictionary<string, CustomType> customTypes, BooleanMapping booleanMapping, string fileName, string configurationName)
        {
            this.stringTemplate = stringTemplate;
            this.configurationEntries = configurationEntries;
            this.customTypes = customTypes;
            this.BooleanMapping = booleanMapping;
            this.FileName = fileName;
            this.ConfiguratioName = configurationName;
        }

        public static ConfigurationTemplate FromYaml(string yaml)
        {
            var serializer = new Serializer();
            dynamic protoTemplate = serializer.Deserialize(yaml);
            string stringTemplate = protoTemplate["template"];
            var booleanMapping = new BooleanMapping(protoTemplate["boolean"][true], protoTemplate["boolean"][false]);
            var types = new Dictionary<string, CustomType>();
            var entries = new Dictionary<string, ConfigurationEntry>();
            var fileName = protoTemplate["filename"];
            var configName = protoTemplate["configuration_name"];

            foreach (var value in protoTemplate["types"])
            {
                var typeValues = new List<CustomTypeValue>();
                foreach (var type in value.Value)
                {
                    typeValues.Add(new CustomTypeValue(type[0], type[1]));
                }
                types.Add(value.Key,new CustomType(value.Key, typeValues));
            }
            foreach (var value in protoTemplate["keys"])
            {
                entries.Add(value.Key, new ConfigurationEntry(value.Value["description"], value.Value["protection"], value.Value["type"], value.Key));
            }
            
            return new ConfigurationTemplate(stringTemplate, entries,types,booleanMapping, fileName, configName);
        }
    }
}
