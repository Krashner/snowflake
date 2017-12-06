﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Scraping.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AttachAttribute : Attribute
    {
        public AttachTarget Target { get; }
        public AttachAttribute(AttachTarget target)
        {
            this.Target = target;
        }
    }
}
