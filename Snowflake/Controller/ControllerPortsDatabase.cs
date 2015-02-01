﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using Snowflake.Platform;
using System.Data;
using Snowflake.Utility;

namespace Snowflake.Controller
{
    public class ControllerPortsDatabase : BaseDatabase, IControllerPortsDatabase
    {
        public ControllerPortsDatabase(string fileName)
            : base(fileName)
        {
            this.CreateDatabase();
        }

        private void CreateDatabase()
        {
            this.DBConnection.Open();
            var sqlCommand = new SQLiteCommand(@"CREATE TABLE IF NOT EXISTS ports(
                                                                platform_id TEXT PRIMARY KEY,
                                                                port1 TEXT,
                                                                port2 TEXT,
                                                                port3 TEXT,
                                                                port4 TEXT,
                                                                port5 TEXT,
                                                                port6 TEXT,
                                                                port7 TEXT,
                                                                port8 TEXT
                                                                )", this.DBConnection);
            sqlCommand.ExecuteNonQuery();
            this.DBConnection.Close();
        }

        public void AddPlatform(IPlatformInfo platformInfo)
        {
            this.DBConnection.Open();
            using (var sqlCommand = new SQLiteCommand(@"INSERT OR IGNORE INTO ports VALUES(
                                                                @platform_id,
                                                                null,
                                                                null,
                                                                null,
                                                                null,
                                                                null,
                                                                null,
                                                                null,
                                                                null
                                                                )", this.DBConnection))
            {
                sqlCommand.Parameters.AddWithValue("@platform_id", platformInfo.PlatformId);
                sqlCommand.ExecuteNonQuery();
                this.DBConnection.Close();
            }
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                this.SetDefaults_Win32(platformInfo); //Set windows defaults if runs on windows. 
            }
            else
            {
                this.SetDefaults_KeyboardOnly(platformInfo); //Only set keyboard defaults
            }
        }
        public string GetDeviceInPort(IPlatformInfo platformInfo, int portNumber)
        {
            if (portNumber > 8 || portNumber < 1){
                return String.Empty;
            }
           
            this.DBConnection.Open();
            using (var sqlCommand = new SQLiteCommand("SELECT `%portNumber` FROM `ports` WHERE `platform_id` == @platformId", this.DBConnection))
            {
                sqlCommand.CommandText = sqlCommand.CommandText.Replace("%portNumber", "port"+portNumber);
                sqlCommand.Parameters.AddWithValue("@platformId", platformInfo.PlatformId);
                using (var reader = sqlCommand.ExecuteReader())
                {
                    var result = new DataTable();
                    result.Load(reader);
                    var row = result.Rows[0];
                    this.DBConnection.Close();
                    return row.Field<string>("port"+portNumber);
                }
            }
        }

        public void SetDeviceInPort(IPlatformInfo platformInfo, int portNumber, string deviceName)
        {
            if (portNumber > 8 || portNumber < 1)
            {
                throw new IndexOutOfRangeException("Snowflake only supports consoles up to 8 controller ports");
            }
            this.DBConnection.Open();
            using (var sqlCommand = new SQLiteCommand("UPDATE `ports` SET `%portNumber` = @controllerId WHERE `platform_id` == @platformId", this.DBConnection))
            {
                sqlCommand.CommandText = sqlCommand.CommandText.Replace("%portNumber", "port" + portNumber);
                sqlCommand.Parameters.AddWithValue("@controllerId", deviceName);
                sqlCommand.Parameters.AddWithValue("@platformId", platformInfo.PlatformId);
                sqlCommand.ExecuteNonQuery();
            }
            this.DBConnection.Close();
        }

        private void SetDefaults_Win32(IPlatformInfo platformInfo)
        {
            this.SetDeviceInPort(platformInfo, 1, InputDeviceNames.KeyboardDevice);
            this.SetDeviceInPort(platformInfo, 2, InputDeviceNames.XInputDevice1);
            this.SetDeviceInPort(platformInfo, 3, InputDeviceNames.XInputDevice2);
            this.SetDeviceInPort(platformInfo, 4, InputDeviceNames.XInputDevice3);
            this.SetDeviceInPort(platformInfo, 5, InputDeviceNames.XInputDevice4);
        }
        private void SetDefaults_KeyboardOnly(IPlatformInfo platformInfo)
        {
            this.SetDeviceInPort(platformInfo, 1, InputDeviceNames.KeyboardDevice);
        }
    }
}
