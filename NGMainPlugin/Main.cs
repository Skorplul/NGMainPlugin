using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using Server = Exiled.Events.Handlers.Server;
using Player = Exiled.Events.Handlers.Player;
using Map = Exiled.Events.Handlers.Map;
using SCP079 = Exiled.Events.Handlers.Scp079;
using Exiled.CustomItems.API.Features;
using HarmonyLib;
using NGMainPlugin.Commands;
using NGMainPlugin.Systems.PainkillerHandlers;
using Exiled.API.Interfaces;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.Features;
using System.IO;
using NGMainPlugin.Systems.RespawnTimer.API.Features;
using System.Net;
using NGMainPlugin.Systems.RespawnTimer;

namespace NGMainPlugin
{
    public class Main : Plugin<Config>
    {
        public override string Author { get; } = "Skorp 1.0";
        public override string Name { get; } = "NGMainPlugin (Dev-Build)";
        public override string Prefix { get; } = "NGM (Dev)";
        public override Version Version { get; } = new Version(1, 0, 0);

        public override Version RequiredExiledVersion { get; } = new Version(8, 9, 4);

        public EventHandlers EventHandlers;
        public PainkillerHand PainkillerHand;
        public Systems.LobbySystem.Handler LobbySystemHandler;
        public Systems.DiscordLogs.LogHandler DCLogHandler;
        public Systems.RespawnTimer.RespawnTimerHandler respawnTimerHandler;

        public static Main Instance { get; private set; }
        private Harmony Harmony { get; set; } = new Harmony("LobbySystem");

        public static NGMainPlugin.Systems.RespawnTimer.RespawnTimer Singleton;

        public static string RespawnTimerDirectoryPath { get; } = Path.Combine(Paths.Configs, nameof(RespawnTimer));

        public override void OnEnabled()
        {
            this.Harmony.PatchAll();
            Config.LoadItems(); 

            Log.Debug("Registering items..");
            CustomItem.RegisterItems(overrideClass: Config.ItemConfigs);

            Instance = this;
            EventHandlers = new EventHandlers(this);
            PainkillerHand = new PainkillerHand(this);
            LobbySystemHandler = new Systems.LobbySystem.Handler();
            DCLogHandler = new Systems.DiscordLogs.LogHandler();

            Player.UsingItemCompleted += PainkillerHand.OnTakingPainkiller;
            Player.TriggeringTesla += EventHandlers.OnTriggeringTesla;
            SCP079.GainingLevel += EventHandlers.OnSCP079GainingLvl;
            Server.RoundStarted += EventHandlers.OnRoundStarted;
            Server.WaitingForPlayers += LobbySystemHandler.OnWaitingForPlayers;
            Player.Verified += LobbySystemHandler.OnVerified;
            Player.Banned += EventHandlers.OnBan;
            Player.Kicked += EventHandlers.OnKick;


            SCPSwap.Plugin = this;
            Durchsage.Plugin = this;

            /// only for RespawnTimer
            RespawnTimer.RespawnTimer.Singleton = this;
            if (!Directory.Exists(RespawnTimer.RespawnTimer.RespawnTimerDirectoryPath))
            {
                Log.Warn("RespawnTimer directory does not exist. Creating...");
                Directory.CreateDirectory(RespawnTimer.RespawnTimer.RespawnTimerDirectoryPath);
            }
            string str = Path.Combine(RespawnTimer.RespawnTimer.RespawnTimerDirectoryPath, "ExampleTimer");
            if (!Directory.Exists(str))
                this.DownloadExampleTimer(str);
            Exiled.Events.Handlers.Map.Generated += new CustomEventHandler(EventHandler.OnGenerated);
            Exiled.Events.Handlers.Server.RoundStarted += new CustomEventHandler(EventHandler.OnRoundStart);
            Exiled.Events.Handlers.Server.ReloadedConfigs += new CustomEventHandler(((Plugin<RespawnTimer.Configs.Config>)this).OnReloaded);
            Exiled.Events.Handlers.Player.Dying += new CustomEventHandler<DyingEventArgs>(EventHandler.OnDying);
            foreach (IPlugin<IConfig> plugin in Exiled.Loader.Loader.Plugins)
            {
                switch (plugin.Name)
                {
                    case "Serpents Hand":
                        if (plugin.Config.IsEnabled)
                        {
                            RespawnTimer.API.API.SerpentsHandTeam.Init(plugin.Assembly);
                            Log.Debug("Serpents Hand plugin detected!");
                            break;
                        }
                        break;
                    case "UIURescueSquad":
                        if (plugin.Config.IsEnabled)
                        {   
                            RespawnTimer.API.API.UiuTeam.Init(plugin.Assembly);
                            Log.Debug("UIURescueSquad plugin detected!");
                            break;
                        }
                        break;
                }
            }
            if (!this.Config.ReloadTimerEachRound)
                this.OnReloaded();
            /// end


            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            CustomItem.UnregisterItems();
            this.Harmony.UnpatchAll();

            Player.TriggeringTesla -= EventHandlers.OnTriggeringTesla;
            SCP079.GainingLevel -= EventHandlers.OnSCP079GainingLvl;
            Server.RoundStarted -= EventHandlers.OnRoundStarted;
            Player.UsingItemCompleted -= PainkillerHand.OnTakingPainkiller;
            Server.WaitingForPlayers -= LobbySystemHandler.OnWaitingForPlayers;
            Player.Verified -= LobbySystemHandler.OnVerified;
            Player.Banned -= EventHandlers.OnBan;
            Player.Kicked -= EventHandlers.OnKick;

            PainkillerHand = null;
            EventHandlers = null;
            Instance = null;
            LobbySystemHandler = null;
            DCLogHandler = null;
            SCPSwap.Plugin = null;
            Durchsage.Plugin = null;

            /// only for RespawnTimer
            if (this.Config.Timers.IsEmpty<string, string>())
            {
                Log.Error("Timer list is empty!");
            }
            else
            {
                TimerView.CachedTimers.Clear();
                foreach (string name in this.Config.Timers.Values)
                    TimerView.AddTimer(name);
            }
        }

        private void DownloadExampleTimer(string exampleTimerDirectory)
        {
            string str1 = exampleTimerDirectory + ".zip";
            string str2 = exampleTimerDirectory + "_Temp";
            using (WebClient webClient = new WebClient())
            {
                Log.Warn("Downloading ExampleTimer.zip...");
                webClient.DownloadFile(string.Format("https://github.com/Michal78900/RespawnTimer/releases/download/v{0}/ExampleTimer.zip", (object)this.Version), str1);
                Log.Info("ExampleTimer.zip has been downloaded!");
                Log.Warn("Extracting...");
                ZipFile.ExtractToDirectory(str1, str2);
                Directory.Move(Path.Combine(str2, "ExampleTimer"), exampleTimerDirectory);
                Directory.Delete(str2);
                System.IO.File.Delete(str1);
                Log.Info("Done!");
            }
        }

        public override string Name => nameof(RespawnTimer);

        public override string Author => "Michal78900";

        public override Version Version => new Version(4, 0, 2);

        public override Version RequiredExiledVersion => new Version(7, 0, 5);

        public override PluginPriority Priority => PluginPriority.Last;
        ///
            base.OnDisabled();
        }
    }
}
