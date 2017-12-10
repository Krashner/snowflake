﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Scraping
{
    public interface IScraperDirective
    {
        AttachTarget Target { get; }
        Directive Directive { get; }
        string Type { get; }
    }
}
