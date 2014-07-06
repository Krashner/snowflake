﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.IO;
using Snowflake.API;
using Snowflake.UI.Theme;
using Snowflake.API.Constants;
using Snowflake.API.Constants.Plugin;
using System.Diagnostics;

namespace Snowflake.UI.EntryPoint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ThemeServer server = new ThemeServer(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "snowflake"));
            server.StartServer();

            //var scraper = new Scraper.TheGamesDB.ScraperTheGamesDB();
            //var results = scraper.GetSearchResults("Super Mario World", "NINTENDO_SNES");   
            // Console.WriteLine(scraper.GetGameDetails(results[0].ID).Item2.Boxarts[ImagesInfoFields.snowflake_img_boxart_back][0]);
            //Console.WriteLine(scraper.PluginInfo["authors"][0]);
            var retroarch = new Emulator.RetroArch.EmulatorRetroArch();
            var proc = retroarch.GetProcessStartInfo("","");
            Console.WriteLine(proc.FileName);
            Console.WriteLine(proc.Arguments);
            Process.Start(proc);
        }
    }
}
