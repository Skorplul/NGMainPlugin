using System.Collections.Generic;
using MEC;
using Exiled.CustomItems.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Spawn;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using InventorySystem.Items.Firearms;

using Firearm = Exiled.API.Features.Items.Firearm;
using FirearmPickup = InventorySystem.Items.Firearms.FirearmPickup;
using System.ComponentModel;
using Exiled.Events.EventArgs.Player;
using LiteDB;
using UnityEngine;
using System.Linq;

namespace NGMainPlugin.Items
{
    /// <inheritdoc/>
    [CustomItem(ItemType.GunCOM18)]
    internal class DoorRemote : CustomWeapon
    {
        private readonly List<Door> RemLockedDorrs = new List<Door>();

        ///<inheritdoc/>
        public override uint Id { get; set; } = 2;

        ///<inheritdoc/>
        public override string Name { get; set; } = "Door Remote";

        ///<inheritdoc/>
        public override string Description { get; set; } = "This is a remote controle, which can lock and unlock doors!";

        ///<inheritdoc/>
        public override float Weight { get; set; } = 1.3f;

        ///<inheritdoc/>
        public override SpawnProperties? SpawnProperties { get; set; } = new()
        {
            Limit = 2,
            DynamicSpawnPoints = new List<DynamicSpawnPoint>
            {
                new()
                {
                    Chance = 60,
                    Location = SpawnLocationType.InsideHid,
                },
                new()
                {
                    Chance = 30,
                    Location = SpawnLocationType.InsideServersBottom,
                }
            },
        };

        ///<inheritdoc/>
        public override byte ClipSize { get; set; } = 6;

        ///<inheritdoc/>
        public override float Damage { get; set; } = 0f;

        /// <summary>
        /// Gets or sets how often ammo will be regenerated. Regeneration occurs at all times, however this timer is reset when the weapon is picked up or dropped.
        /// </summary>
        [Description("How often ammo will be regenerated. Regeneration occurs at all times, however this timer is reset when the weapon is picked up or dropped.")]
        public float RegenerationDelay { get; set; } = 10f;

        /// <summary>
        /// Gets or sets the amount of ammo that will be regenerated each regeneration cycle.
        /// </summary>
        [Description("The amount of ammo that will be regenerated each regeneration cycle.")]
        public byte RegenerationAmount { get; set; } = 1;

        private List<CoroutineHandle> Coroutines { get; } = new();

        /// <inheritdoc/>
        public override void Init()
        {
            Coroutines.Add(Timing.RunCoroutine(DoAmmoRegeneration()));
            base.Init();
        }

        /// <inheritdoc/>
        public override void Destroy()
        {
            foreach (CoroutineHandle handle in Coroutines)
                Timing.KillCoroutines(handle);

            base.Destroy();
        }

        ///<inheritdoc/>
        protected override void SubscribeEvents()
        {

            base.SubscribeEvents();
        }

        ///<inheritdoc/>
        protected override void UnsubscribeEvents()
        {

            base.UnsubscribeEvents();
        }

        protected override void OnHurting(HurtingEventArgs ev)
        {
            ev.IsAllowed = false;
            base.OnHurting(ev);
        }

        protected override void OnAcquired(Player player, Item item, bool displayMessage)
        {
            if(item is Firearm firearm)
            {
                firearm.Ammo = 4;
            }
            base.OnAcquired(player, item, displayMessage);
        }

        /// <inheritdoc/>
        protected override void ShowPickedUpMessage(Player player)
        {
            Coroutines.Add(Timing.RunCoroutine(DoInventoryRegeneration(player)));

            base.ShowPickedUpMessage(player);
        }

        protected override void OnReloading(ReloadingWeaponEventArgs ev)
        {
            ev.IsAllowed = false;
            ev.Player.ShowHint("You can't reload, this item reloads it's self with time!", 7);
            base.OnReloading(ev);
        }

        protected override void OnShot(ShotEventArgs ev)
        {
            // Perform a raycast to determine what the shot hit
            Physics.Raycast(ev.Player.CameraTransform.position, ev.Player.CameraTransform.forward, out RaycastHit hit, Mathf.Infinity);
            
            // Try to get the Door component from the hit object
            Door door = hit.transform.GetComponent<Door>();

            // If the hit object is a door
            if (door != null)
                {
                 // Check if the door is already in the list
                 if (!RemLockedDorrs.Contains(door))
                 {
                    // Add the door to the list
                    door.Lock(999999f, DoorLockType.AdminCommand);
                    RemLockedDorrs.Add(door);
                 }
                 else
                 {
                    // Remove the door from the list
                    door.Unlock();
                    RemLockedDorrs.Remove(door);
                 }
            }
            

            base.OnShot(ev);
        }

        private IEnumerator<float> DoInventoryRegeneration(Player player)
        {
            while (true)
            {
                yield return Timing.WaitForSeconds(RegenerationDelay);

                bool hasItem = false;

                foreach (Item item in player.Items)
                {
                    if (!Check(item) || !(item is Firearm firearm))
                        continue;
                    if (firearm.Ammo < ClipSize)
                        firearm.Ammo += RegenerationAmount;
                    hasItem = true;
                }

                if (!hasItem)
                    yield break;
            }
        }

        private IEnumerator<float> DoAmmoRegeneration()
        {
            while (true)
            {
                yield return Timing.WaitForSeconds(RegenerationDelay);

                foreach (Pickup pickup in Pickup.List)
                {
                    if (Check(pickup) && pickup.Base is FirearmPickup firearmPickup && firearmPickup.NetworkStatus.Ammo < ClipSize)
                    {
                        firearmPickup.NetworkStatus = new FirearmStatus((byte)(firearmPickup.NetworkStatus.Ammo + RegenerationAmount), firearmPickup.NetworkStatus.Flags, firearmPickup.NetworkStatus.Attachments);

                        yield return Timing.WaitForSeconds(0.5f);
                    }
                }
            }
        }
    }
}
