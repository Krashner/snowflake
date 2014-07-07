﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.API.Interface;

namespace Snowflake.API.Information.Game
{
    public class Info: IInfo
    {
        public string PlatformId { get; private set; }
        public string Name { get; private set; }
        public virtual Dictionary<string, string> Images { get; set; }
        public Dictionary<string, dynamic> Metadata { get; set; }

        public Info(string platformId, string name, Dictionary<string,string> images, Dictionary<string, dynamic> metadata)
        {
            this.PlatformId = platformId;
            this.Name = name;
            this.Images = images;
            this.Metadata = metadata;
        }


    }
}
