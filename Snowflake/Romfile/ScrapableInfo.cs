﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Utility;
using Snowflake.Utility.Hash;
namespace Snowflake.Romfile
{
    public class ScrapableInfo : IScrapableInfo
    {
        public string QueryableTitle { get; }
        public string RomId { get; }
        public string RomInternalName { get; }

        public string OriginalFilename { get; }
        public string StonePlatformId { get; }
        public IStructuredFilename StructuredFilename { get; }

        /// <summary>
        /// Initialize a ScrapbleInfo with all details
        /// </summary>
        /// <param name="queryableTitle">Queryable Title</param>
        /// <param name="originalFilename">Original Filename</param>
        /// <param name="romId">Game ID</param>
        /// <param name="stonePlatformId">Stone Platform ID</param>
        public ScrapableInfo(string queryableTitle, string originalFilename, string romId, string romInternalName, string stonePlatformId)
        {
            this.QueryableTitle = queryableTitle;
            this.OriginalFilename = originalFilename;
            this.RomId = romId;
            this.RomInternalName = romInternalName;
            this.StructuredFilename = new StructuredFilename(originalFilename);
            this.StonePlatformId = stonePlatformId;
        }
        /// <summary>
        /// Initialize a ScrapableInfo with parsable queryable title filename
        /// </summary>
        /// <param name="originalFilename">Original Filename</param>
        /// <param name="romId">Game ID</param>
        /// <param name="stonePlatformId">Stone Platform ID</param>
        public ScrapableInfo(string originalFilename, string romId, string romInternalName, string stonePlatformId)
        {
            this.StructuredFilename = new StructuredFilename(originalFilename);
            this.QueryableTitle = this.StructuredFilename.Title;
            this.OriginalFilename = originalFilename;
            this.RomId = romId;
            this.RomInternalName = romInternalName;
            this.StonePlatformId = stonePlatformId;
        }
        /// <summary>
        /// Initialize a ScrapableInfo with a vetted filesignature
        /// </summary>
        /// <param name="originalFilename">Original Filename</param>
        /// <param name="fileSignature">A confirmed filesignature</param>
        public ScrapableInfo(string originalFilename, IFileSignature fileSignature)
        {
            this.StructuredFilename = new StructuredFilename(originalFilename);
            this.QueryableTitle = this.StructuredFilename.Title;
            this.OriginalFilename = originalFilename;
            this.RomId = fileSignature?.GetGameId(originalFilename);
            this.RomInternalName = fileSignature?.GetInternalName(originalFilename);
            this.StonePlatformId = fileSignature?.SupportedPlatform;
        }
        /// <summary>
        /// Initialize a ScrapableInfo with a vetted filesignature
        /// </summary>
        /// <param name="originalFilename">Original Filename</param>
        /// <param name="fileSignature">A confirmed filesignature</param>
        /// <param name="stonePlatformId">The platform id. Should match the fileSignature</param>
        public ScrapableInfo(string originalFilename, IFileSignature fileSignature, string stonePlatformId)
        {
            this.StructuredFilename = new StructuredFilename(originalFilename);
            this.QueryableTitle = this.StructuredFilename.Title;
            this.OriginalFilename = originalFilename;
            this.RomId = fileSignature?.GetGameId(originalFilename);
            this.RomInternalName = fileSignature?.GetInternalName(originalFilename);
            this.StonePlatformId = stonePlatformId;
        }
        /// <summary>
        /// Initialize a ScrapableInfo with parsable queryable title filename without a game id or internal name
        /// </summary>
        /// <param name="originalFilename">Original Filename</param>
        /// <param name="stonePlatformId">Stone Platform ID</param>
        public ScrapableInfo(string originalFilename, string stonePlatformId) : this(originalFilename, null, null, stonePlatformId)
        {
        }

        public string HashCrc32()
        {
            return FileHash.GetCRC32(this.OriginalFilename);
        }

        public string HashMd5()
        {
            return FileHash.GetMD5(this.OriginalFilename);
        }

        public string GetUUID()
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            //The UUID format is the game title, plus the game's rom ID if available (if it is not it is "null"), and the platform id. 
            //Preferably a rom's internal name is used to be deterministic, but if its not available, the CRC32 hash of the filename is used as the rom's internal name
            //A deterministic UUID is indicated by the i prefix.
            string determinismPrefix = (this.RomInternalName != String.Empty) ? "i_" : "_";
            string hashName = (this.RomInternalName != String.Empty)
                ? this.RomInternalName
                : this.HashCrc32();
            return determinismPrefix + BitConverter.ToString(md5.ComputeHash(new MemoryStream(Encoding.UTF8.GetBytes($"{this.RomInternalName}|{this.RomId}|{this.StonePlatformId}|")))).Replace("-", string.Empty).ToLowerInvariant();
        }
    }
}
