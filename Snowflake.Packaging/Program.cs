﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Snowflake.Packaging.Snowball;
using Snowflake.Packaging.Publishing;
using Newtonsoft.Json;
using CommandLine;
using Snowflake.Packaging.Installing;

namespace Snowflake.Packaging
{
    static class Program
    {
        
        public static void Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<PackOptions,
                MakePluginOptions, MakeThemeOptions, MakeEmulatorAssemblyOptions,
                InstallOptions, UninstallOptions, PublishOptions, AuthOptions, SignOptions, VerifyOptions>(args)
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
                })
                .WithParsed<SignOptions>(options =>
                {
                    Signing.SignSnowball(options.FileName);
                })
                .WithParsed<VerifyOptions>(options =>
                {
                    bool signed = Signing.VerifySnowball(options.FileName, options.FileName + ".sig", options.FileName + ".key");
                    Console.WriteLine(signed);
                })
                .WithParsed<PublishOptions>(options =>
                {
                    try
                    {
                        if (!String.IsNullOrWhiteSpace(Properties.Settings.Default.githubToken) &&
                            !String.IsNullOrWhiteSpace(Properties.Settings.Default.nugetToken))
                        {
                            Task.Run(async () =>
                            {
                                PublishActions.PackNuget(options.PackageFile);
                                await PublishActions.MakeGithubIndex(Package.FromZip(options.PackageFile).PackageInfo);
                            }).Wait();
                        }
                        else
                        {
                            Console.WriteLine(
                                "Unable to find GitHub or NuGet details. Please run snowball auth to enter your GitHub details and NuGet API key.");
                        }
                    }
                    catch (AggregateException ex)
                    {
                        Console.WriteLine(
                            $"Unable to publish package {options.PackageFile}: {String.Join(", ", ex.InnerExceptions.Select(_ex => _ex.Message))}");
                    }
                })
                .WithParsed<AuthOptions>(options =>
                {
                    Console.Clear();
                    Console.WriteLine("Console cleared for security purposes.");
                    Task.Run(async () => {
                        Account.SaveDetails(await Account.CreateGithubToken(options.GithubUser, options.GithubPassword), options.NuGetAPIKey);
                        await Account.MakeRepoFork(Account.GetGithubToken());
                        }).Wait();
                })
                .WithParsed<InstallOptions>(options =>
                {
                    options.SnowflakeRoot = options.SnowflakeRoot ??
                                            Path.Combine(
                                                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                                "Snowflake");
                    var manager = new InstallManager(options.SnowflakeRoot);
                    if (options.LocalInstall)
                    {
                        foreach (string file in options.PackageFiles)
                        {
                            try
                            {
                                manager.InstallSinglePackage(file);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Could not install package {file}: {ex.Message}");
                                continue;
                            }
                        }
                    }
                    else
                    {
                        Task.Run(async () =>
                        {
                            try {
                                var deps = await GithubPackages.ResolveDependencies(options.PackageFiles[0]);
                                foreach (var dep in deps)
                                {
                                    Console.WriteLine(dep.Name);
                                }
                            }
                            catch (Octokit.ApiException)
                            {
                                Console.WriteLine("Rate limit exceeded, log in with GitHub credentials using snowball auth");
                            }
                        }).Wait();
                    }
                    //todo wrap in try/catch

                })
                .WithParsed<UninstallOptions>(options =>
                {
                        options.SnowflakeRoot = options.SnowflakeRoot ??
                                                Path.Combine(
                                                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                                    "Snowflake");
                        var manager = new InstallManager(options.SnowflakeRoot);
                        manager.UninstallPackage(options.PackageIds[0]);
                        //todo wrap in try/catch
                }); 
            

            var packageInfo = new PackageInfo("name-Test", "desc-Test", new List<string>() {"test-Auth"}, "1.0.0", new List<string>() { "testdep@1.0.0" }, PackageType.Plugin);
            string serialized = JsonConvert.SerializeObject(packageInfo);
            var newPackage = JsonConvert.DeserializeObject<PackageInfo>(serialized);

        }
    }
}
