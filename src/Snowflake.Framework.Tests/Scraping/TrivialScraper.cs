﻿using Snowflake.Scraping.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Snowflake.Scraping.Tests
{
    [Target(SeedContent.RootSeedType)]
    [Attach(AttachTarget.Root)]
    [Group("MyGroup", "Test")]
    public class GroupScraper : Scraper
    {
        public override IEnumerable<SeedContent> Scrape(ISeed parent, ILookup<string, SeedContent> rootSeeds, ILookup<string, SeedContent> childSeeds)
        {
            yield return ("Test", "Hello World");
            yield return ("TestTwo", "Foo Bar");
        }
    }
}
