﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.API.Information
{
    public class Platform : Info
    {
        public Platform(string platformId, string name, IDictionary<string, string> images, IDictionary<string, string> metadata, IList<string> fileExtensions, string romIdentifierPlugin): base(platformId, name, images, metadata)
        {
            this.FileExtensions = fileExtensions;
            this.RomIdentifierPlugin = romIdentifierPlugin;
        }
        public IList<string> FileExtensions { get; private set; }
        public string RomIdentifierPlugin { get; private set; }

    }
}
