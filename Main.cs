﻿using Exiled.API.Features;
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

namespace NGMainPlugin
{
    public class Main : Plugin<Config>
    {
        public override string Author { get; } = "Skorp 1.0";
        public override string Name { get; } = "NGMainPlugin";
        public override string Prefix { get; } = "NGM";
        public override Version Version { get; } = new Version(0, 1, 0);

        public override Version RequiredExiledVersion { get; } = new Version(8, 5, 0);

        public EventHandlers EventHandlers;
        public LobbySystem.Handler LobbySystemHandler;

        public static Main Instance { get; private set; }
        private Harmony Harmony { get; set; } = new Harmony("LobbySystem");

        public override void OnEnabled()
        {
            

            this.Harmony.PatchAll();
            Config.LoadItems();

            Log.Debug("Registering items..");
            CustomItem.RegisterItems(overrideClass: Config.ItemConfigs);

            Instance = this;
            EventHandlers = new EventHandlers(this);
            LobbySystemHandler = new LobbySystem.Handler();

            Player.UsingItemCompleted += EventHandlers.OnTakingPainkiller;
            Player.TriggeringTesla += EventHandlers.OnTriggeringTesla;
            SCP079.GainingLevel += EventHandlers.OnSCP079GainingLvl;
            Server.RoundStarted += EventHandlers.OnRoundStarted;
            Server.WaitingForPlayers += LobbySystemHandler.OnWaitingForPlayers;
            Player.Verified += LobbySystemHandler.OnVerified;

            SCPSwap.Plugin = this;
            Durchsage.Plugin = this;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            CustomItem.UnregisterItems();
            this.Harmony.UnpatchAll();

            Player.TriggeringTesla -= EventHandlers.OnTriggeringTesla;
            SCP079.GainingLevel -= EventHandlers.OnSCP079GainingLvl;
            Server.RoundStarted -= EventHandlers.OnRoundStarted;
            Player.UsingItemCompleted -= EventHandlers.OnTakingPainkiller;
            Server.WaitingForPlayers -= LobbySystemHandler.OnWaitingForPlayers;
            Player.Verified -= LobbySystemHandler.OnVerified;

            EventHandlers = null;
            Instance = null;
            LobbySystemHandler = null;
            SCPSwap.Plugin = null;
            Durchsage.Plugin = null;

            base.OnDisabled();
        }
    }
}
