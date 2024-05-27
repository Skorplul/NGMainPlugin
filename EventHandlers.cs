using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Scp079;
using Exiled.Events.EventArgs.Player;
using PluginAPI.Events;
using PlayerRoles;
using Exiled.API.Enums;

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

        public EventHandlers (Main plugin)
        {
            this.plugin = plugin;
        }

        public void OnRoundStarted()
        {
            PcCurentLvl = 1;
            Commands.SCPSwap.swaped.Clear();
            Commands.Durchsage.spoke.Clear();
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

            if (ev.Item.Type == ItemType.Painkillers)
            {
                int Durr = random.Next(5, 30);
                int RandEff = random.Next(1, 7);
                EffectType DoEffect = GetEff(RandEff);
                string EffectString = GetEffString(DoEffect);
                
                ev.Player.ShowHint($"[<color=#FB045B>P</color><color=#F81353>a</color><color=#F5224B>i</color><color=#F23143>n</color><color=#EF403B>k</color><color=#EC4F33>i</color><color=#E95E2B>l</color><color=#E66D23>l</color><color=#E37C1B>e</color><color=#E08B13>r</color>]: You recieved <color=#f90000ff>{EffectString}</color> for <color=#f90000ff>{Durr}</color> seconds!"); 
                ev.Player.EnableEffect(DoEffect, Durr, false);
                

            }
        }
    }
}
