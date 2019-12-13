﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Common;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces;
using Sandbox.ModAPI.Weapons;
using SpaceEngineers.Game.ModAPI;
using ProtoBuf;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Utils;
using VRageMath;
using RivalAI.Behavior.Settings;
using RivalAI.Helpers;

namespace RivalAI.Behavior.Subsystems.Profiles {

    [ProtoContract]
    public class SpawnProfile {

        [ProtoMember(1)]
        public bool UseSpawn;

        [ProtoMember(2)]
        public bool StartsReady;

        [ProtoMember(3)]
        public float FirstSpawnTimeMs;

        [ProtoMember(4)]
        public float SpawnMinCooldownMs;

        [ProtoMember(5)]
        public float SpawnMaxCooldownMs;

        [ProtoMember(6)]
        public int MaxSpawns;

        [ProtoMember(7)]
        public List<string> SpawnGroups;

        [ProtoMember(8)]
        public int CooldownTime;

        [ProtoMember(9)]
        public int SpawnCount;

        [ProtoMember(10)]
        public DateTime LastSpawnTime;

        [ProtoIgnore]
        public Random Rnd;

        public SpawnProfile() {

            UseSpawn = false;
            StartsReady = false;
            FirstSpawnTimeMs = 0;
            SpawnMinCooldownMs = 0;
            SpawnMaxCooldownMs = 1;
            MaxSpawns = -1;
            SpawnGroups = new List<string>();

            CooldownTime = 0;
            SpawnCount = 0;
            LastSpawnTime = MyAPIGateway.Session.GameDateTime;


            Rnd = new Random();

        }

        public void ProcessSpawn() {

            if(MaxSpawns >= 0 && SpawnCount >= MaxSpawns) {

                UseSpawn = false;
                return;

            }

            TimeSpan duration = MyAPIGateway.Session.GameDateTime - LastSpawnTime;

            if(duration.TotalMilliseconds < CooldownTime) {

                return;

            }

            //Do Spawn Shit

        }

        public void InitTags(string customData) {

            if(string.IsNullOrWhiteSpace(customData) == false) {

                var descSplit = customData.Split('\n');

                foreach(var tag in descSplit) {

                    //UseSpawn
                    if(tag.Contains("[UseSpawn:") == true) {

                        UseSpawn = TagHelper.TagBoolCheck(tag);

                    }

                    //FirstSpawnTimeMs
                    if(tag.Contains("[FirstSpawnTimeMs:") == true) {

                        FirstSpawnTimeMs = TagHelper.TagFloatCheck(tag, FirstSpawnTimeMs);

                    }

                    //SpawnMinCooldownMs
                    if(tag.Contains("[MinCooldownMs:") == true) {

                        SpawnMinCooldownMs = TagHelper.TagFloatCheck(tag, SpawnMinCooldownMs);

                    }

                    //SpawnMaxCooldownMs
                    if(tag.Contains("[MaxCooldownMs:") == true) {

                        SpawnMaxCooldownMs = TagHelper.TagFloatCheck(tag, SpawnMaxCooldownMs);

                    }

                    //MaxSpawns
                    if(tag.Contains("MaxSpawns:") == true) {

                        MaxSpawns = TagHelper.TagIntCheck(tag, MaxSpawns);

                    }

                    //SpawnGroups
                    if(tag.Contains("SpawnGroups:") == true) {

                        var tempvalue = TagHelper.TagStringCheck(tag);

                        if(string.IsNullOrWhiteSpace(tempvalue) == false) {

                            SpawnGroups.Add(tempvalue);

                        }

                    }

                }

            }

            if(SpawnMinCooldownMs > SpawnMaxCooldownMs) {

                SpawnMinCooldownMs = SpawnMaxCooldownMs;

            }


        }

    }
}