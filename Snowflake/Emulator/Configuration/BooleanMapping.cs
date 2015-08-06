﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Snowflake.Emulator.Configuration
{
    public class BooleanMapping : IBooleanMapping
    {
        public string True { get; }
        public string False { get; }

        public BooleanMapping(string trueValue, string falseValue)
        {
            this.True = trueValue;
            this.False = falseValue;
        }

        public string FromBool(bool value)
        {
            return value ? this.True : this.False;
        }

    }
}
