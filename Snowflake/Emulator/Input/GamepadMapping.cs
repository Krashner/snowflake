﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Emulator.Input
{
    public class GamepadMapping : IGamepadMapping
    {
        public string GAMEPAD_A { get; private set; }
        public string GAMEPAD_B { get; private set; }
        public string GAMEPAD_X { get; private set; }
        public string GAMEPAD_Y { get; private set; }
        public string GAMEPAD_START { get; private set; }
        public string GAMEPAD_SELECT { get; private set; }
        public string GAMEPAD_L1 { get; private set; }
        public string GAMEPAD_L2 { get; private set; }
        public string GAMEPAD_L3 { get; private set; }
        public string GAMEPAD_R1 { get; private set; }
        public string GAMEPAD_R2 { get; private set; }
        public string GAMEPAD_R3 { get; private set; }
        public string GAMEPAD_L_X_UP { get; private set; }
        public string GAMEPAD_L_X_DOWN { get; private set; }
        public string GAMEPAD_L_Y_RIGHT { get; private set; }
        public string GAMEPAD_L_Y_LEFT { get; private set; }
        public string GAMEPAD_R_X_UP { get; private set; }
        public string GAMEPAD_R_X_DOWN { get; private set; }
        public string GAMEPAD_R_Y_RIGHT { get; private set; }
        public string GAMEPAD_R_Y_LEFT { get; private set; }
        public string GAMEPAD_GUIDE { get; private set; }
        public string GAMEPAD_DPAD_UP { get; private set; }
        public string GAMEPAD_DPAD_DOWN { get; private set; }
        public string GAMEPAD_DPAD_LEFT { get; private set; }
        public string GAMEPAD_DPAD_RIGHT { get; private set; }
        private IDictionary<string, string> mappingData;
        public GamepadMapping(IDictionary<string, string> mappingData)
        {
            this.mappingData = mappingData;
            this.GAMEPAD_A = mappingData["GAMEPAD_A"];
            this.GAMEPAD_B = mappingData["GAMEPAD_B"];
            this.GAMEPAD_X = mappingData["GAMEPAD_X"];
            this.GAMEPAD_Y = mappingData["GAMEPAD_Y"];
            this.GAMEPAD_START = mappingData["GAMEPAD_START"];
            this.GAMEPAD_SELECT = mappingData["GAMEPAD_SELECT"];
            this.GAMEPAD_L1 = mappingData["GAMEPAD_L1"];
            this.GAMEPAD_L2 = mappingData["GAMEPAD_L2"];
            this.GAMEPAD_L3 = mappingData["GAMEPAD_L3"];
            this.GAMEPAD_R1 = mappingData["GAMEPAD_R1"];
            this.GAMEPAD_R2 = mappingData["GAMEPAD_R2"];
            this.GAMEPAD_R3 = mappingData["GAMEPAD_R3"];
            this.GAMEPAD_L_X_UP = mappingData["GAMEPAD_L_X_UP"];
            this.GAMEPAD_L_X_DOWN = mappingData["GAMEPAD_L_X_DOWN"];
            this.GAMEPAD_L_Y_RIGHT = mappingData["GAMEPAD_L_Y_RIGHT"];
            this.GAMEPAD_L_Y_LEFT = mappingData["GAMEPAD_L_Y_LEFT"];
            this.GAMEPAD_R_X_UP = mappingData["GAMEPAD_R_X_UP"];
            this.GAMEPAD_R_X_DOWN = mappingData["GAMEPAD_R_X_DOWN"];
            this.GAMEPAD_R_Y_RIGHT = mappingData["GAMEPAD_R_Y_RIGHT"];
            this.GAMEPAD_R_Y_LEFT = mappingData["GAMEPAD_R_Y_LEFT"];
            this.GAMEPAD_GUIDE = mappingData["GAMEPAD_GUIDE"];
            this.GAMEPAD_DPAD_UP = mappingData["GAMEPAD_DPAD_UP"];
            this.GAMEPAD_DPAD_DOWN = mappingData["GAMEPAD_DPAD_DOWN"];
            this.GAMEPAD_DPAD_LEFT = mappingData["GAMEPAD_DPAD_LEFT"];
            this.GAMEPAD_DPAD_RIGHT = mappingData["GAMEPAD_DPAD_RIGHT"];
        }
        public string this[string key] {
            get { return this.mappingData[key]; }
        }
    }
}
