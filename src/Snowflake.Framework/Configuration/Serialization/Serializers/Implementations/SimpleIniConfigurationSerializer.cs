﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Configuration.Serialization.Serializers.Implementations
{
    public class SimpleIniConfigurationSerializer
        : AbstractStringConfigurationSerializer
    {
        public SimpleIniConfigurationSerializer()
        {
        }

        public override void SerializeBlockBegin(IConfigurationSerializationContext<string> context)
        {
            context.AppendLine($"[{String.Join('.', context.GetFullScope())}]");
        }
        public override void SerializeBlockEnd(IConfigurationSerializationContext<string> context)
        {
            return;
        }

        public override void SerializeNodeValue(bool value, string key, IConfigurationSerializationContext<string> context)
        {
            context.AppendLine($"{key}={value}");
        }

        public override void SerializeNodeValue(double value, string key, IConfigurationSerializationContext<string> context)
        {
            context.AppendLine($"{key}={value}");
        }

        public override void SerializeNodeValue(Enum value, string enumValue, string key, IConfigurationSerializationContext<string> context)
        {
            context.AppendLine($"{key}={enumValue}");
        }

        public override void SerializeNodeValue(long value, string key, IConfigurationSerializationContext<string> context)
        {
            context.AppendLine($"{key}={value}");
        }

        public override void SerializeNodeValue(string value, string key, IConfigurationSerializationContext<string> context)
        {
            context.AppendLine($"{key}={value}");
        }

        public override void SerializeNodeValue(string key, IConfigurationSerializationContext<string> context)
        {
            context.AppendLine($"{key}=");
        }
    }
}