﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Snowflake.Collections;
using Snowflake.Information.Game;
using Snowflake.Plugin;

namespace Snowflake.Scraper
{
    [InheritedExport(typeof(IScraper))]
    public interface IScraper : IPlugin
    {
        BiDictionary<string, string> ScraperMap { get; }
        IList<GameScrapeResult> GetSearchResults(string searchQuery);
        IList<GameScrapeResult> GetSearchResults(string searchQuery, string platformId);
        Tuple<Dictionary<string, string>, GameImages> GetGameDetails(string id);
      
    }
}
