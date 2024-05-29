using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.API.Features.Items;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using System;
using System.Collections.Generic;
using UnityEngine;
using Exiled.API.Features;
using FacilitySoundtrack;

namespace NGMainPlugin.Items
{
    [CustomItem(ItemType.Coin)]
    public class SoundTrap : CustomItem
    {
        public override uint Id { get; set; } = 1;

        public override string Name { get; set; } = "Soundtrap";

        public override string Description { get; set; } = "Uppon placement, this coin will play random sounds.";

        public override float Weight { get; set; } = 0f;

        public override SpawnProperties? SpawnProperties { get; set; } = new()
        {
            Limit = 1,
            DynamicSpawnPoints = new List<DynamicSpawnPoint>
            {
                new()
                {
                    Chance = 30,
                    Location = SpawnLocationType.InsideGr18,
                },
                new()
                {
                    Chance = 50,
                    Location = SpawnLocationType.Inside049Armory,
                }
            }
        };

        protected override void OnDropping(DroppingItemEventArgs ev)
        {
            Vector3 SoundPos = ev.Player.Position;
            Vector3 Rot = Vector3.zero;


            base.OnDropping(ev);
        }



    }
}
