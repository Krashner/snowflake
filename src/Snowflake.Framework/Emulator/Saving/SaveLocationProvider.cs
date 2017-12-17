﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Snowflake.Records.Game;
using Snowflake.Services;

namespace Snowflake.Emulator.Saving
{
    public class SaveLocationProvider : ISaveLocationProvider
    {
        private DirectoryInfo SaveLocationRoot { get; }
        public SaveLocationProvider(IContentDirectoryProvider contentDirectoryProvider)
        {
            this.SaveLocationRoot = contentDirectoryProvider.ApplicationData.CreateSubdirectory("saves");
        }

        public async Task<ISaveLocation> CreateSaveLocationAsync(IGameRecord gameRecord, string saveType)
        {

            Guid saveGuid = Guid.NewGuid();

            DirectoryInfo locationRoot = this.SaveLocationRoot
                .CreateSubdirectory(saveGuid.ToString());
            var saveLocation = new SaveLocation(gameRecord.Guid, saveType, locationRoot, saveGuid, DateTimeOffset.UtcNow);
            var manifest = saveLocation.ToManifest();
            var json = JsonConvert.SerializeObject(manifest, Formatting.Indented);
            await File.WriteAllTextAsync(Path.Combine(locationRoot.FullName, "manifest.json"), json);
            return saveLocation;
        }

        public void DeleteSaveLocation(Guid saveLocationGuid)
        {
            this.SaveLocationRoot.CreateSubdirectory(saveLocationGuid.ToString()).Delete(true);
        }

        public async Task<IEnumerable<ISaveLocation>> GetAllSaveLocationsAsync()
        {
            ISaveLocation[] results = await Task.WhenAll(
                   from directory in this.SaveLocationRoot.EnumerateDirectories()
                   where Guid.TryParse(directory.Name, out _)
                   let guid = Guid.Parse(directory.Name)
                   select this.GetSaveLocationAsync(guid));
            return results;
        }

        public async Task<ISaveLocation> GetSaveLocationAsync(Guid saveLocationGuid)
        {
            string manifestPath = Path.Combine(this.SaveLocationRoot.FullName, saveLocationGuid.ToString(),
                "manifest.json");
            if (!File.Exists(manifestPath))
            {
                throw new FileNotFoundException();
            }

            SaveLocationManifest manifest = JsonConvert.DeserializeObject<SaveLocationManifest>(await File.ReadAllTextAsync(manifestPath));
            return manifest.ToSaveLocation();
        }

        public async Task<IEnumerable<ISaveLocation>> GetSaveLocationsAsync(IGameRecord gameRecord)
        {
            return (await this.GetAllSaveLocationsAsync())
                .Where(s => s.RecordGuid == gameRecord.Guid);
        }
    }
}
