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
using UnityEngine;
using System;

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
            ev.Player.ShowHint("You can't reload! This item reloads it's self over time!", 7);
            base.OnReloading(ev);
        }

        protected override void OnShot(ShotEventArgs ev)
        {
            try
            {
                // Perform a raycast to determine what the shot hit
                if (Physics.Raycast(ev.Player.CameraTransform.position, ev.Player.CameraTransform.forward, out RaycastHit hit, Mathf.Infinity))
                {
                    // Debug: Print the name of the hit object
                    Log.Info($"Hit object: {hit.transform.name}");

                    // Check if the hit object or its parents have a Door component
                    Door door = hit.transform.GetComponent<Door>();
                    if (door == null)
                    {
                        door = hit.transform.GetComponentInParent<Door>();
                    }

                    if (door == null)
                    {
                        // Extended search: check nearby colliders within a small radius
                        Collider[] colliders = Physics.OverlapSphere(hit.point, 1.0f);
                        foreach (var collider in colliders)
                        {
                            door = collider.GetComponent<Door>();
                            if (door != null)
                            {
                                Log.Info($"Door component found on nearby collider: {collider.transform.name}");
                                break;
                            }
                        }
                    }

                    if (door == null)
                    {
                        Log.Warn($"No Door component found on the hit object or its parents: {hit.transform.name}");
                        LogHierarchy(hit.transform);
                    }
                    else
                    {
                        Log.Info($"Door component identified: {door.Base.name}");

                        // Check if the door is already in the list
                        if (!RemLockedDorrs.Contains(door))
                        {
                            // Add the door to the list
                            RemLockedDorrs.Add(door);
                            ev.Player.ShowHint($"Door {door.Base.name} has been added to the remote control list.", 5);
                        }
                        else
                        {
                            // Remove the door from the list
                            RemLockedDorrs.Remove(door);
                            ev.Player.ShowHint($"Door {door.Base.name} has been removed from the remote control list.", 5);
                        }
                    }
                }
                else
                {
                    Log.Warn("Raycast did not hit any object.");
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Exception in OnShot: {ex}");
            }


            base.OnShot(ev);
        }

        private void LogHierarchy(Transform transform)
        {
            int safetyCounter = 0; // Safeguard counter to prevent infinite loops
            while (transform != null && safetyCounter < 100)
            {
                Log.Info($"Object: {transform.name} - Tag: {transform.tag} - Layer: {LayerMask.LayerToName(transform.gameObject.layer)}");
                transform = transform.parent;
                safetyCounter++; // Increment the safeguard counter
            }
        }
        //door.Lock(180f, DoorLockType.AdminCommand);
        //door.Unlock();
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
