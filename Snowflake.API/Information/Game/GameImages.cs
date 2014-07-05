﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;

namespace Snowflake.API.Information.Game
{
    public class GameImages
    {
        public List<string[]> Fanarts { get; set; }
        public List<string[]> Screenshots { get; set; }
        public Dictionary<string, string[]> Boxarts { get; set; }
        public string CachePath { get; private set; }
        public string ImagesID { get; private set; }
        public GameImages(string cachePath = "imagecache")
        {
            this.CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Snowflake", "data", cachePath);
            this.ImagesID = new ShortGuid().ToString();
            this.Fanarts = new List<string[]>();
            this.Screenshots = new List<string[]>();
            this.Boxarts = new Dictionary<string, string[]>();
        }

        public void AddFromUrl(GameImageType imageType, Uri imageUrl)
        {
            string imageFileName="";
            switch (imageType){
                case GameImageType.Fanart:
                    imageFileName="fanart_"+this.Fanarts.Count+1;
                    break;
                case GameImageType.Screenshot:
                    imageFileName="screenshot_"+this.Screenshots.Count+1;
                    break;
                case GameImageType.Boxart_back:
                    imageFileName="boxart_back";
                    break;
                case GameImageType.Boxart_front:
                    imageFileName="boxart_back";
                    break;
                case GameImageType.Boxart_full:
                    imageFileName="boxart_back";
                    break;
            }
            string downloadPath = Path.Combine(this.CachePath, this.ImagesID, imageFileName);
            using (WebClient webclient = new WebClient())
            {
                webclient.DownloadFile(imageUrl, downloadPath);
            }
            switch (imageType)
            {
                case GameImageType.Fanart:
                    this.Fanarts.Add(new string[] { downloadPath, imageUrl.AbsoluteUri });
                    break;
                case GameImageType.Screenshot:
                    this.Screenshots.Add(new string[] { downloadPath, imageUrl.AbsoluteUri });
                    break;
                case GameImageType.Boxart_back:
                    this.Boxarts.Add("snowflake_boxart_back", new string[] { downloadPath, imageUrl.AbsoluteUri });
                    break;
                case GameImageType.Boxart_front:
                    this.Boxarts.Add("snowflake_boxart_front", new string[] { downloadPath, imageUrl.AbsoluteUri });
                    break;
                case GameImageType.Boxart_full:
                    this.Boxarts.Add("snowflake_boxart_full", new string[] { downloadPath, imageUrl.AbsoluteUri });
                    break;
            }
        }
    }

    public enum GameImageType
    {
        Fanart,
        Screenshot,
        Boxart_front,
        Boxart_back,
        Boxart_full
    }
}
