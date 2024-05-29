using Exiled.API.Interfaces;
using Exiled.API.Features;
using Exiled.Events.Commands.Reload;
using Exiled.Loader;
using System;
using System.ComponentModel;
using System.IO;
using YamlDotNet.Serialization;
using AudioSystem.Models.SoundConfigs;
using System.Collections.Generic;

namespace NGMainPlugin
{
    public class Config : IConfig
    {
        /// <summary>
        /// Gets item Config settings.
        /// </summary>
        [YamlIgnore]
        public Configs.Items ItemConfigs { get; private set; } = null!;

        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; }

        [Description("Should Tutorials be ignored by teslas? Default: false")]
        public bool NoTesTuts { get; set; } = false;

        [Description("Should you be able to swap SCPs only one time a round? Default: true")]
        public bool SingleSwap { get; set; } = true;

        [Description("After how many seconds should you not be able to swap scps anymore? Default: 60")]
        public float ScpSwapTimeout { get; set; } = 60f;

        [Description("Should SCP079 be able to use CASSI only once per round? Default: true")]
        public bool Single079Cassi { get; set; } = true;

        public List<LocalSoundConfig> ListOfPossibeMusics { get; set; } = new List<LocalSoundConfig>()
        {
            new LocalSoundConfig()
            {
                SoundName = "test",
                Volume = 25,
                DummyName = "test",
                Radius = 2f
            },
            new LocalSoundConfig()
            {
                SoundName = "test1",
                Volume = 25,
                DummyName = "test1",
                Radius = 4f
            },
        };

        /// <summary>
        /// Gets or sets a value indicating what folder item configs will be stored in.
        /// </summary>
        public string ItemConfigFolder { get; set; } = Path.Combine(Paths.Configs, "CustomItems");

        /// <summary>
        /// Gets or sets a value indicating what file will be used for item configs.
        /// </summary>
        public string ItemConfigFile { get; set; } = "global.yml";

        /// <summary>
        /// Loads the item configs.
        /// </summary>
        public void LoadItems()
        {
            if (!Directory.Exists(ItemConfigFolder))
                Directory.CreateDirectory(ItemConfigFolder);

            string filePath = Path.Combine(ItemConfigFolder, ItemConfigFile);
            Log.Info($"{filePath}");
            if (!File.Exists(filePath))
            {
                ItemConfigs = new Configs.Items();
                File.WriteAllText(filePath, Loader.Serializer.Serialize(ItemConfigs));
            }
            else
            {
                ItemConfigs = Loader.Deserializer.Deserialize<Configs.Items>(File.ReadAllText(filePath));
                File.WriteAllText(filePath, Loader.Serializer.Serialize(ItemConfigs));
            }
        }
    }
}
