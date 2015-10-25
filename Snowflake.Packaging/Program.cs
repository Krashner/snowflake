﻿using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CommandLine;
using Snowflake.Packaging.Installing;
using Snowflake.Packaging.Publishing;
using Snowflake.Packaging.Snowball;

namespace Snowflake.Packaging
{
    internal static class Program
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
                    Package.MakeFromEmulatorDefinition(options.EmulatorDefinitionFile, options.PackageInfoFile,
                        options.OutputDirectory);
                })
                .WithParsed<MakeThemeOptions>(options =>
                {
                    options.OutputDirectory = options.OutputDirectory ?? Environment.CurrentDirectory;
                    Package.MakeFromTheme(options.ThemeDirectory, options.PackageInfoFile, options.OutputDirectory);
                })
                .WithParsed<SignOptions>(options => { Signing.SignSnowball(options.FileName); })
                .WithParsed<VerifyOptions>(options =>
                {
                    bool signed = Signing.VerifySnowball(options.FileName, options.FileName + ".sig",
                        options.FileName + ".key");
                    Console.WriteLine(signed);
                })
                .WithParsed<PublishOptions>(options =>
                {
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.githubToken) &&
                            !string.IsNullOrWhiteSpace(Properties.Settings.Default.nugetToken))
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
                            $"Unable to publish package {options.PackageFile}: {string.Join(", ", ex.InnerExceptions.Select(_ex => _ex.Message))}");
                    }
                })
                .WithParsed<AuthOptions>(options =>
                {
                    Console.Clear();
                    Console.WriteLine("Console cleared for security purposes.");
                    Task.Run(async () =>
                    {
                        Account.SaveDetails(
                            await Account.CreateGithubToken(options.GithubUser, options.GithubPassword),
                            options.NuGetAPIKey);
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
                                manager.InstallPackage(file);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Could not install package {file}: {ex.Message}");
                            }
                        }
                    }
                    else
                    {
                        var dependencies = manager.PackageRepository.ResolveDependencies(options.PackageFiles);
                        string tempPath = Program.GetTemporaryDirectory();
                        string downloadPath = Program.GetTemporaryDirectory();

                        using (var webClient = new WebClient())
                        {
                            foreach (var dependency in dependencies)
                            {
                                string nugetDownloadPath = downloadPath + Path.GetRandomFileName();
                                string version = dependency.Item2?.ToString() ??
                                                 dependency.Item1.ReleaseVersions.OrderByDescending(
                                                     _version => _version.Key).First().Key.ToString();
                                Console.WriteLine($"Downloading {dependency.Item1.Name} {version}");
                                webClient.DownloadFile(LocalRepository.GetNugetDownload(dependency.Item1, version),
                                    nugetDownloadPath);
                                using (var nugetPkg = new ZipArchive(File.OpenRead(nugetDownloadPath)))
                                {
                                    var snowballEntry = nugetPkg.Entries.First(entry => entry.Name.EndsWith(".snowball"));
                                    var _snowballSig = nugetPkg.GetEntry(snowballEntry.Name + ".sig");
                                    var snowballKey = nugetPkg.GetEntry(snowballEntry.Name + ".key");
                                    var snowballSig = new MemoryStream();
                                    _snowballSig.Open().CopyTo(snowballSig);
                                    bool signed = Signing.VerifySnowball(snowballEntry.Open(), snowballSig.ToArray(),
                                        new StreamReader(snowballKey.Open()).ReadToEnd());
                                    if (signed)
                                        snowballEntry.ExtractToFile(Path.Combine(tempPath, Path.GetRandomFileName()));
                                    else
                                    {
                                        Console.WriteLine(
                                            $"Unable to install {dependency.Item1.Name}, it is improperly signed. File an issue at https://github.com/SnowflakePowered-Packages/snowball-packages/issues/new");
                                    }
                                }
                            }
                        }

                        foreach (string fileName in Directory.EnumerateFiles(tempPath))
                            manager.InstallPackage(fileName);
                    }
                })
                .WithParsed<UninstallOptions>(options =>
                {
                    options.SnowflakeRoot = options.SnowflakeRoot ??
                                            Path.Combine(
                                                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                                "Snowflake");
                    var manager = new InstallManager(options.SnowflakeRoot);
                    foreach (string packageId in options.PackageIds)
                    {
                        try
                        {
                            manager.UninstallPackage(packageId);
                            Console.WriteLine($"Uninstalled package {packageId}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Unable to uninstall {packageId}: {ex.Message}");
                        }
                    }
                });
        }

        private static string GetTemporaryDirectory()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }
    }
}