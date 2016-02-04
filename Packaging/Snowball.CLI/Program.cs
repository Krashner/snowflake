﻿using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CommandLine;
using Newtonsoft.Json;
using Snowball.Installation;
using Snowball.Packaging;
using Snowball.Packaging.Packagers;
using Snowball.Publishing;
using Snowball.Secure;


namespace Snowflake.Packaging
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
#if !DEBUG
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                Console.WriteLine($"A fatal error occured: {((Exception) e.ExceptionObject).Message}");
                Environment.Exit(1);
            };
#endif
            string appDataDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Snowball");
            var localRepository = new LocalRepository(appDataDirectory);
            var accountKeyStore = new AccountKeyStore(appDataDirectory);
            var packageKeyStore = new PackageKeyStore(appDataDirectory);

            var result = Parser.Default.ParseArguments<MakePackageOptions>(args)
                .WithParsed<MakePackageOptions>(options =>
                {
                    if (Program.AtLeastTwo(options.MakePlugin, options.MakeTheme, options.MakeEmulator))
                        throw new InvalidOperationException("You can only specify a single type.");
                    Packager packager = null;
                    if (options.MakePlugin) packager = new PluginPackager();
                    if (options.MakeEmulator) packager = new EmulatorAssemblyPackager();
                    if (options.MakeTheme) packager = new ThemePackager();
                    if (packager == null)
                        throw new InvalidOperationException("No package type specified.");
                            //todo probably a more elegant way to this
                    options.OutputDirectory = options.OutputDirectory ?? Environment.CurrentDirectory;
                    string packageRoot =
                        Path.GetDirectoryName(packager.Make(Path.GetFullPath(options.FileName),
                            options.PackageInfoFile));
                    if (!options.WrapNuget)
                    {
                        Console.WriteLine(Package.LoadDirectory(packageRoot).Pack(options.OutputDirectory));
                    }
                    else
                    {
                        var nugetWrapper = new NugetWrapper(Package.LoadDirectory(packageRoot), packageKeyStore);
                        var nugetPackage = nugetWrapper.MakeNugetPackage();
                        File.Copy(nugetPackage.Item1,
                            Path.Combine(options.OutputDirectory, Path.GetFileName(nugetPackage.Item1)), true);
                        File.WriteAllText(Path.Combine(options.OutputDirectory,
                            $"{nugetWrapper.Package.PackageInfo.Name}.{nugetWrapper.Package.PackageInfo.PackageType}.rel.json"
                            .ToLowerInvariant()),
                            JsonConvert.SerializeObject(nugetPackage.Item2));
                    }
                });
        }
        private static bool AtLeastTwo(bool a, bool b, bool c)
        {
            //http://stackoverflow.com/questions/3076078/check-if-at-least-two-out-of-three-booleans-are-true
            return a ? (b || c) : (b && c);
        }
        private static string GetTemporaryDirectory()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }
    }
}