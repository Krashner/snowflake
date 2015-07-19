﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using Snowflake.Service;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
namespace Snowflake.Shell.Windows
{
    internal class SnowflakeShell
    {
        string ShellRoot
        {
            get
            {
                return Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "electron");
            }
        }
        Process currentShellInstance;
        internal SnowflakeShell()
        {
            this.StartCore();
        }

        public void StartCore()
        {
#if DEBUG
            CoreService.InitCore(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));
#else
            CoreService.InitCore();
#endif
            CoreService.InitPluginManager();
            CoreService.LoadedCore.ServerManager.RegisterServer("ThemeServer", new ThemeServer(Path.Combine(CoreService.LoadedCore.AppDataDirectory, "theme")));
            CoreService.LoadedCore.ServerManager.StartServer("ThemeServer");
        }
        public void RestartCore()
        {
            this.ShutdownCore();
            this.StartCore();
        }

        public void ShutdownCore()
        {
            CoreService.DisposeLoadedCore();
            GC.WaitForPendingFinalizers();
        }

       
        public Process StartShell(params string[] args)
        {
            if (this.ShellAvailable())
            {
                IList<string> arguments = new List<string>(args);
                arguments.Insert(0, this.ShellRoot);
                var electronShell = this.GetShell();
                electronShell.Arguments = String.Join(" ", arguments);
                if (this.currentShellInstance != null) this.currentShellInstance.Close();
                this.currentShellInstance = Process.Start(electronShell);
                return this.currentShellInstance;
            }
            return null;
        }

        public bool ShellAvailable()
        {
            return File.Exists(Path.Combine(this.ShellRoot, "node_modules", "electron-prebuilt", "dist", "electron.exe"));
        }

        public ProcessStartInfo GetShell()
        {
            return new ProcessStartInfo(Path.Combine(this.ShellRoot, "node_modules", "electron-prebuilt", "dist", "electron.exe"));
        }
  
    }
}
