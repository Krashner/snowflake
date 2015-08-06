﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace Snowflake.Tests
{
    internal static class TestUtilities
    {
        /// <summary>
        /// Gets a TestResource from the TestResources folder
        /// </summary>
        /// <param name="resourceName">The filename of the resource</param>
        /// <returns>The TestResource<</returns>
        internal static Stream GetResource(string resourceName)
        {
            return Assembly.GetCallingAssembly().GetManifestResourceStream($"{Assembly.GetCallingAssembly().GetName().Name}.TestResources.{resourceName}");
        }

        /// <summary>
        /// Gets a TestResource from the TestResources folder as a string
        /// </summary>
        /// <param name="resourceName">The filename of the resource</param>
        /// <returns>The TestResource</returns>
        internal static string GetStringResource(string resourceName)
        {
            using (Stream stream = TestUtilities.GetResource(resourceName))
            using (var reader = new StreamReader(stream))
            {
                string file = reader.ReadToEnd();
                return file;
            }
        }
    }
}
