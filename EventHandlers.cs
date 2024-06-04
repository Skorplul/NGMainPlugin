﻿using System;
using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Scp079;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using Exiled.API.Enums;
using MEC;

namespace NGMainPlugin
{
    public class EventHandlers
    {
        private readonly Main plugin;

        public static int PcCurentLvl;
        readonly Random random = new Random();
        public List<EffectType> PainEffects = new List<EffectType>()
        {
            EffectType.Invisible,
            EffectType.Bleeding,
            EffectType.Poisoned,
            EffectType.MovementBoost,
            EffectType.Flashed,
            EffectType.Ensnared
        };
        public List<ItemType> PainItems = new List<ItemType>()
        {
            ItemType.Painkillers,
            ItemType.AntiSCP207,
            ItemType.KeycardFacilityManager,
            ItemType.KeycardScientist,
            ItemType.Medkit,
            ItemType.SCP018,
            ItemType.SCP207
        };

        public EventHandlers (Main plugin)
        {
            this.plugin = plugin;
        }

        public void OnRoundStarted()
        {
            PcCurentLvl = 1;
            Commands.SCPSwap.swaped.Clear();
            Commands.Durchsage.spoke.Clear();

            foreach (Lift lift in (IEnumerable<Lift>)Lift.List)
                Timing.RunCoroutine(Methods.CheckingPlayerLift(lift));
        }

        public void OnTriggeringTesla(TriggeringTeslaEventArgs ev)
        {
            if (ev.Player.Role == RoleTypeId.Tutorial)
            {
                if (plugin.Config.NoTesTuts)
                {
                    ev.IsAllowed = false;
                }
            }
        }

        public void OnSCP079GainingLvl(GainingLevelEventArgs ev)
        {
            PcCurentLvl = ev.NewLevel;
        }

        public void OnTakingPainkiller(UsingItemCompletedEventArgs ev)
        {
            EffectType GetEff(int EffectListVal)
            {
                return PainEffects[EffectListVal];
            }
            ItemType GetItem(int ItemListVal)
            {
                return PainItems[ItemListVal];
            }

            string GetEffString(EffectType SelEffect)
            {
                switch (SelEffect)
                {
                    case EffectType.Invisible:
                        return "Invisible";
                    case EffectType.Bleeding:
                        return "Bleeding";
                    case EffectType.Poisoned:
                        return "Poisoned";
                    case EffectType.MovementBoost:
                        return "Movement Boost";
                    case EffectType.Flashed:
                        return "Flashed";
                    case EffectType.Ensnared:
                        return "Ensnared";
                    default:
                        return "Error";
                }
            }
            string GetItemString(ItemType SelItem)
            {
                switch (SelItem)
                {
                    case ItemType.Painkillers:
                        return "Painkillers";
                    case ItemType.AntiSCP207:
                        return "Anti-SCP-207";
                    case ItemType.KeycardFacilityManager:
                        return "Facility Manager Keycard";
                    case ItemType.KeycardScientist:
                        return "Scientist Keycard";
                    case ItemType.Medkit:
                        return "Medkit";
                    case ItemType.SCP018:
                        return "SCP-018";
                    case ItemType.SCP207:
                        return "SCP-207";
                    default:
                        return "Error";
                }
            }

            if (ev.Item.Type == ItemType.Painkillers)
            {
                int Durr = random.Next(5, 15);
                int RandEff = random.Next(0, 13);

                if (RandEff <= 5)
                {
                    EffectType DoEffect = GetEff(RandEff);
                    string EffectString = GetEffString(DoEffect);

                    if (EffectString == "Error")
                    {
                        Log.Warn("Error in the selection for Painkiller-Effect selection!");
                        return;
                    }

                    ev.Player.ShowHint($"[<color=#FB045B>P</color><color=#F81353>a</color><color=#F5224B>i</color><color=#F23143>n</color><color=#EF403B>k</color><color=#EC4F33>i</color><color=#E95E2B>l</color><color=#E66D23>l</color><color=#E37C1B>e</color><color=#E08B13>r</color>]: You recieved <color=#f90000ff>{EffectString}</color> for <color=#f90000ff>{Durr}</color> seconds!");
                    ev.Player.EnableEffect(DoEffect, 10, Durr, false);
                }
                else if (RandEff >= 6)
                {
                    RandEff = RandEff - 6;

                    ItemType DoItem = GetItem(RandEff);
                    string ItemString = GetItemString(DoItem);

                    if (ItemString == "Error")
                    {
                        Log.Warn("Error in the selection for Painkiller-item selection!");
                        return;
                    }

                    ev.Player.ShowHint($"[<color=#FB045B>P</color><color=#F81353>a</color><color=#F5224B>i</color><color=#F23143>n</color><color=#EF403B>k</color><color=#EC4F33>i</color><color=#E95E2B>l</color><color=#E66D23>l</color><color=#E37C1B>e</color><color=#E08B13>r</color>]: You recieved <color=#f90000ff>{ItemString}</color>!");
                    ev.Player.AddItem(DoItem);
                }
                
            }
        }

        public void OnBan(BannedEventArgs ev)
        {
            Map.Broadcast(6, $"[< color =#f67979>N</color><color=#e86e6c>e</color><color=#d96260>x</color><color=#cb5754>u</color><color=#bd4c48>s</color><color=#af413c>G</color><color=#a13631>a</color><color=#932a26>m</color><color=#851f1b>i</color><color=#771211>n</color><color=#6a0303>g</color>]: {ev.Player.Nickname} has been banned from the server!");
        }

    }
}
