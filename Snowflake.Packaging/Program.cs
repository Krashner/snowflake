﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Snowflake.Packaging.Snowball;
using Newtonsoft.Json;
using CommandLine;
namespace Snowflake.Packaging
{
    static class Program
    {
        
        public static void Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<PackOptions, 
                MakePluginOptions, MakeThemeOptions, MakeEmulatorAssemblyOptions, 
                InstallOptions, UninstallOptions, PublishOptions, SetupOptions>(args)
                .WithParsed<PackOptions>(options =>
                {
                    var package = Package.LoadDirectory(options.PluginRoot);
                    package.Pack(options.OutputDirectory ?? Environment.CurrentDirectory, options.PluginRoot);
                })
                .WithParsed<MakePluginOptions>(options =>
                {
                    options.OutputDirectory = options.OutputDirectory ?? Environment.CurrentDirectory;
                    Package.MakeFromPlugin(options.PluginFile, options.PackageInfoFile, options.OutputDirectory);
                })
                .WithParsed<MakeEmulatorAssemblyOptions>(options =>
                {
                    options.OutputDirectory = options.OutputDirectory ?? Environment.CurrentDirectory;
                    Package.MakeFromEmulatorDefinition(options.EmulatorDefinitionFile, options.PackageInfoFile, options.OutputDirectory);
                })
                .WithParsed<MakeThemeOptions>(options =>
                {
                     options.OutputDirectory = options.OutputDirectory ?? Environment.CurrentDirectory;
                     Package.MakeFromTheme(options.ThemeDirectory, options.PackageInfoFile, options.OutputDirectory);
               });
            

            var packageInfo = new PackageInfo("name-Test", "desc-Test", new List<string>() {"test-Auth"}, "1.0.0", new List<string>() { "testdep@1.0.0" }, PackageType.Plugin);
            string serialized = JsonConvert.SerializeObject(packageInfo);
            var newPackage = JsonConvert.DeserializeObject<PackageInfo>(serialized);

        }
    }
}
