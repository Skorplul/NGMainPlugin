using Exiled.API.Interfaces;
using Exiled.API.Features;
using Exiled.Loader;
using System;
using System.ComponentModel;
using System.IO;
using YamlDotNet.Serialization;
using AudioSystem.Models.SoundConfigs;
using System.Collections.Generic;
using UnityEngine;
using Exiled.API.Enums;
using PlayerRoles;


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

        /// <summary>
        /// Gets or sets if tutorials should trigger teslas.
        /// </summary>
        [Description("Should Tutorials be ignored by teslas? Default: false")]
        public bool NoTesTuts { get; set; } = false;

        /// <summary>
        /// Gets or sets if should be able to only swap one time in one round.
        /// </summary>
        [Description("Should you be able to swap SCPs only one time a round? Default: true")]
        public bool SingleSwap { get; set; } = true;

        /// <summary>
        /// Gets or sets the time SCPs have to use .scpswap.
        /// </summary>
        [Description("After how many seconds should you not be able to swap scps anymore? Default: 60")]
        public float ScpSwapTimeout { get; set; } = 60f;

        /// <summary>
        /// Gets or sets the valid SCPs for the .scpswap command.
        /// </summary>
        [Description("Ihr könnt diese config ändern, aber für den Fall ihr wollt eine Rolle hinzufügen geht das nocht nicht, nur entfernen und wieder hinzufügen der bereits existierenden Rollen!")]
        public List<RoleTypeId> SwapableScps { get; set; } = new List<RoleTypeId>()
        {
            RoleTypeId.Scp049,
            RoleTypeId.Scp079,
            RoleTypeId.Scp3114,
            RoleTypeId.Scp096,
            RoleTypeId.Scp173,
            RoleTypeId.Scp106,
            RoleTypeId.Scp939,
        };

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
        [Description("LobbySystem")]

        public double LobbyTime { get; set; } = 30.0;

        public int MinimumPlayers { get; set; } = 2;

        [Description("Edit the text shown")]
        public string TextShown { get; set; } = "<size=27>│ %status% │ <b>SERVER NAME</b>  │ <color=red>%playercount%/%maxplayers%</color> Inmates Waiting │</size>";

        public string PausedStatus { get; set; } = "<color=red>\uD83D\uDFE5</color> Lobby Paused";

        public string WaitingStatus { get; set; } = "<color=yellow>\uD83D\uDFE8</color> Waiting for Players";

        public string StartingStatus { get; set; } = "<color=green>\uD83D\uDFE9</color> Starting in %countdown% Seconds";

        public RoomType SpawnRoom { get; set; }

        public Vector3 SpawnPosition { get; set; } = new Vector3(0.0f, 995.6f, -8f);

    }
}
