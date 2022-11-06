﻿using BepInEx;
using Moonstorm.Starstorm2.API;
using R2API.Utils;
using R2API;
using R2API.Networking;
using System;
using UnityEngine;

namespace Moonstorm.Starstorm2
{
    [BepInDependency("com.TeamMoonstorm.MoonstormSharedUtils", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.DestroyedClone.AncientScepter", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.RiskyLives.RiskyMod", BepInDependency.DependencyFlags.SoftDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [R2APISubmoduleDependency(
        nameof(DotAPI),
        nameof(PrefabAPI),
        nameof(NetworkingAPI))]
    [BepInPlugin(guid, modName, version)]
    public class Starstorm : BaseUnityPlugin
    {
        internal const string guid = "com.TeamMoonstorm.Starstorm2-Nightly";
        internal const string modName = "Starstorm 2 Nightly";
        internal const string version = "0.3.45";

        public static Starstorm instance;
        public static PluginInfo pluginInfo;
        public static bool DEBUG = true;

        public static bool ScepterInstalled = false;
        public static bool RiskyModInstalled = false;

        public void Awake()
        {
            instance = this;
            pluginInfo = Info;
            SS2Log.logger = Logger;
            if (DEBUG)
            {
                base.gameObject.AddComponent<SS2DebugUtil>();
            }
            new SS2Assets().Init();
            if(!SS2Assets.LoadAsset<Texture2D>("spike"))
            {
                SS2Log.Fatal("Spike not found :c");
                Destroy(this);
                return;
            }
            new SS2Config().Init();
            new SS2Content().Init();
            new SS2Language().Init();
            ConfigurableFieldManager.AddMod(this);
        }

        private void Start()
        {
            SoundBankManager.Init();
            SetupModCompat();
            //On.RoR2.EquipmentCatalog.Init += RemoveUnfitEquipmentsFromChaos;
        }

        //private void RemoveUnfitEquipmentsFromChaos(EquipmentCatalog.orig_Init orig)
        //{
        //    orig();
        //    SS2Content.Equipments.BackThruster.canBeRandomlyTriggered = false;
        //    SS2Content.Equipments.PressurizedCanister.canBeRandomlyTriggered = false;
        //    SS2Content.Equipments.MIDAS.canBeRandomlyTriggered = false;
        //    //EquipmentCatalog.randomTriggerEquipmentList.Remove(SS2Content.Equipments.BackThruster.equipmentIndex);
        //    //EquipmentCatalog.randomTriggerEquipmentList.Remove(SS2Content.Equipments.PressurizedCanister.equipmentIndex);
        //}

        private void SetupModCompat()
        {
            ScepterInstalled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.DestroyedClone.AncientScepter");
            RiskyModInstalled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.RiskyLives.RiskyMod");
        }
    }
}