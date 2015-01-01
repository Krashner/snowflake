﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Emulator.Configuration;
using Snowflake.Controller;
using Snowflake.Service;
using Snowflake.Game;
using System.Diagnostics;
using System.Collections;
using System.IO;
using Snowflake.Emulator.Input;
using Snowflake.Plugin;
/*
namespace Snowflake.Emulator
{
    public class EmulatorBridge : BasePlugin, IEmulatorBridge
    {
        public IReadOnlyDictionary<string, IControllerTemplate> ControllerTemplates { get; private set; }
        public IReadOnlyDictionary<string, IInputTemplate> InputTemplates { get; private set; }
        public IReadOnlyDictionary<string, IConfigurationTemplate> ConfigurationTemplates { get; private set; }
        public IReadOnlyList<string> SupportedPlatforms { get; private set; }

        public EmulatorBridge(IDictionary<string, ControllerTemplate> controllerTemplates, IDictionary<string, InputTemplate> inputTemplates, IDictionary<string, ConfigurationTemplate> configurationTemplates, IList<string> supportedPlatforms)
        {
            this.ControllerTemplates = controllerTemplates.AsReadOnly();
            this.InputTemplates = inputTemplates.AsReadOnly();
            this.ConfigurationTemplates = configurationTemplates.AsReadOnly();
            this.SupportedPlatforms = supportedPlatforms.AsReadOnly();
        }
           

        public void StartRom(GameInfo gameInfo, ControllerProfile profile)
        {
            var retroArch = CoreService.LoadedCore.EmulatorManager.EmulatorAssemblies["retroarch"];
            string path = CoreService.LoadedCore.EmulatorManager.GetAssemblyDirectory(retroArch);
            var startInfo = new ProcessStartInfo(path);
            startInfo.WorkingDirectory = Path.Combine(CoreService.LoadedCore.EmulatorManager.AssembliesLocation, "retroarch");
            startInfo.Arguments = String.Format(@"{0} --libretro ""cores/bsnes_balanced_libretro.dll"" --config retroarch.cfg.clean --appendconfig controller.cfg", gameInfo.FileName);
            Console.WriteLine(startInfo.Arguments);
            var platform = CoreService.LoadedCore.LoadedPlatforms[gameInfo.PlatformId];
            File.WriteAllText("controller.cfg", CompileController(1, platform.Controllers[CoreService.LoadedCore.ControllerPortsDatabase.GetPort(platform, 1)], this.ControllerTemplates["NES_CONTROLLER"], profile, this.InputTemplates["retroarch"]));
            Process.Start(startInfo).WaitForExit();
           //todo needs a place to output configurations
            //configurationflags please
        }
        
        public string CompileConfiguration(ConfigurationTemplate configTemplate, ConfigurationProfile configProfile)
        {
            var template = new StringBuilder(configTemplate.StringTemplate);
            foreach (var configurationValue in configProfile.ConfigurationValues)
            {
                Type configurationvalueType = configurationValue.Value.GetType();
                string stringValue;
                if (configurationvalueType == typeof(bool))
                {
                    stringValue = configTemplate.BooleanMapping.FromBool(configurationValue.Value);
                }
                else
                {
                    stringValue = configurationValue.Value.ToString();
                }
                template.Replace("{" + configurationValue.Key + "}", stringValue);
            }
            return template.ToString();
        }
        public string CompileController(int playerIndex, ControllerDefinition controllerDefinition, ControllerTemplate controllerTemplate, ControllerProfile controllerProfile, InputTemplate inputTemplate)
        {
            var template = new StringBuilder(inputTemplate.StringTemplate);
            var controllerMappings = controllerProfile.ProfileType == ControllerProfileType.KEYBOARD_PROFILE ? 
                controllerTemplate.KeyboardControllerMappings : controllerTemplate.GamepadControllerMappings;

            foreach (ControllerInput input in controllerDefinition.ControllerInputs.Values)
            {
                string templateKey = controllerMappings["default"].InputMappings[input.InputName];
                string inputSetting = controllerProfile.InputConfiguration[input.InputName];
                string emulatorValue = controllerProfile.ProfileType == ControllerProfileType.KEYBOARD_PROFILE ? 
                    inputTemplate.KeyboardMappings.First().Value[inputSetting] : inputTemplate.GamepadMappings.First().Value[inputSetting]; 
                template.Replace("{" + templateKey + "}", emulatorValue);
            }

            foreach (var key in inputTemplate.TemplateKeys)
            {
                template.Replace("{N}", playerIndex.ToString()); //Player Index
                if (controllerMappings["default"].KeyMappings.ContainsKey(key))
                {
                    template.Replace("{" + key + "}", controllerMappings["default"].KeyMappings[key]); //Non-input keys
                }
                else
                {
                    template.Replace("{" + key + "}", inputTemplate.NoBind); //Non-input keys
                }
            }
            return template.ToString();
        }
    }
}
*/