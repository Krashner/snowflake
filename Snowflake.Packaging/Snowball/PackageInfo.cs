﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NuGet;

namespace Snowflake.Packaging.Snowball
{
    public class PackageInfo
    {
        [JsonProperty("name")]
        public string Name { get; }

        [JsonProperty("description")]
        public string Description { get; }

        [JsonProperty("authors")]
        public IList<string> Authors { get; }

        [JsonProperty("version")]
        public SemanticVersion Version { get; }

        [JsonProperty("dependencies")]
        public IList<Dependency> Dependencies { get; }

        [JsonProperty("packagetype")]
        [JsonConverter(typeof (StringEnumConverter))]
        public PackageType PackageType { get; }

        public PackageInfo(string name, string description, IList<string> authors, string version,
            IList<string> dependencies, PackageType packageType)
        {
            this.Version = new SemanticVersion(version);
            this.Name = name;
            this.Description = description;
            this.Authors = authors;
            this.Dependencies = dependencies.Select(dependency => new Dependency(dependency)).ToList();
            this.PackageType = packageType;
        }

        [JsonConstructor]
        public PackageInfo(string name, string description, IList<string> authors, string version,
            IList<string> dependencies, string packageType)
            : this(
                name, description, authors, version, dependencies,
                (PackageType) Enum.Parse(typeof (PackageType), packageType, true))
        {
        }
    }


    public enum PackageType
    {
        Plugin,
        Theme,
        EmulatorAssembly
    }
}