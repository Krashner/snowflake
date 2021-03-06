using Snowflake.Configuration;
using Snowflake.Configuration.Attributes;

// autogenerated using generate_retroarch.py
namespace Snowflake.Plugin.Emulators.RetroArch.Configuration.Internal
{
    [ConfigurationSection("location", "Location Options")]
    public interface LocationConfiguration : IConfigurationSection<LocationConfiguration>
    {
        /// <summary>
        ///     Gets or sets a value indicating whether not applicable on Desktop devices
        /// </summary>
        [ConfigurationOption("location_allow", false, DisplayName = "Location Allow", Private = true)]
        bool LocationAllow { get; set; }

        // this can be enum but null is the only possible value.
        [ConfigurationOption("location_driver", "null", DisplayName = "Location Driver", Private = true)]
        string LocationDriver { get; set; }
    }
}
