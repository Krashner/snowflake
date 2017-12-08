﻿using Snowflake.Scraping.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Snowflake.Extensibility;

namespace Snowflake.Scraping.Tests
{
    [RequiresRoot("Test")]
    [Plugin("DependentScraper")]
    public class DependentScraper : Scraper
    {
        public DependentScraper()
            : base(typeof(DependentScraper), AttachTarget.Root, SeedContent.RootSeedType)
        {
        }

        public override IEnumerable<SeedContent> Scrape(ISeed parent, ILookup<string, SeedContent> rootSeeds, ILookup<string, SeedContent> childSeeds)
        {
            yield return ("TestDependent", $"{rootSeeds["Test"].First()} from Dependent Scraper");
        }
    }
}
