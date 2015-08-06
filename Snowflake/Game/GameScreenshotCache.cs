﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Snowflake.Extensions;

namespace Snowflake.Game
{
    public class GameScreenshotCache : IGameScreenshotCache
    {
        public GameScreenshotCache(string rootPath, string cacheKey)
        {
            this.RootPath = rootPath;
            this.CacheKey = cacheKey;
            this.fullPath = Path.Combine(this.RootPath, this.CacheKey);
            if (!Directory.Exists(this.fullPath)) Directory.CreateDirectory(this.fullPath);
            this.registerFile = Path.Combine(this.fullPath, "screenshots.json");
            this.LoadScreenshotCollection();
        }
        public GameScreenshotCache(string cacheKey) : this(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Snowflake", "screenshots"), cacheKey) { }
        public string RootPath { get; }
        public IReadOnlyList<string> ScreenshotCollection => this.screenshotCollection.AsReadOnly();
        private IList<string> screenshotCollection;
        private readonly string registerFile;
        private void LoadScreenshotCollection()
        {
            if (!File.Exists(this.registerFile))
            {
                this.screenshotCollection = new List<string>();
            }
            else
            {
                try
                {
                    this.screenshotCollection = JsonConvert.DeserializeObject<IList<string>>(File.ReadAllText(this.registerFile));
                }
                catch (JsonException)
                {
                    this.screenshotCollection = new List<string>(); //try to rebuild screenshot cache
                    foreach (string screenshotFile in Directory.EnumerateFiles(this.fullPath).Where(screenshotFile => Path.GetExtension(screenshotFile) == ".png"))
                    {
                        this.screenshotCollection.Add(screenshotFile);
                    }
                }
            }
            File.WriteAllText(this.registerFile, JsonConvert.SerializeObject(this.screenshotCollection));
        }
        public string CacheKey { get; }
        readonly string fullPath;
        public void AddScreenshot(Uri screenshotUri)
        {
            using (var webClient = new WebClient())
            {
                try
                {
                    byte[] imageData = screenshotUri.Scheme == "file" ? File.ReadAllBytes(screenshotUri.LocalPath) : webClient.DownloadData(screenshotUri);
                    using (Stream imageStream = new MemoryStream(imageData))
                    using (Image image = Image.FromStream(imageStream, true, true))
                    {
                        this.AddScreenshot(image);
                    }
                }
                catch
                {
                    Console.WriteLine($"[WARN] Swallowed UnknownException: Unable to download {screenshotUri.AbsoluteUri} to game cache");
                }
            }
        }

        public void AddScreenshot(Image screenshotData)
        {
            string fileName = $"{DateTime.Now.ToString("yyyy-MM-dd-HH-mm")}_{Guid.NewGuid()}.png";
            try
            {
                if (File.Exists(Path.Combine(this.fullPath, fileName))) File.Delete(Path.Combine(this.fullPath, fileName));
                screenshotData.Save(Path.Combine(this.fullPath, fileName), ImageFormat.Png);
                this.screenshotCollection.Add(fileName);
                File.WriteAllText(this.registerFile, JsonConvert.SerializeObject(this.screenshotCollection));
            }
            catch (ArgumentException)
            {
                Console.WriteLine("[WARN] Swallowed ArgumentException: Attemped to add invalid screenshot to game cache");
            }
            catch
            {
                Console.WriteLine("[WARN] Swallowed UnknownException: Unable to save image to screenshot cache");
            }
        }
        public void RemoveScreenshot(int screenshotIndex)
        {
            string fileName = this.screenshotCollection[screenshotIndex];
            File.Delete(Path.Combine(this.fullPath, fileName));
            this.screenshotCollection.RemoveAt(screenshotIndex);
            File.WriteAllText(this.registerFile, JsonConvert.SerializeObject(this.screenshotCollection));
        }

    }
}
