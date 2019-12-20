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
    public class TriggerProfile {

        [ProtoMember(1)]
        public string Type;

        [ProtoMember(2)]
        public bool UseTrigger;

        [ProtoMember(3)]
        public double TargetDistance;

        [ProtoMember(4)]
        public bool InsideAntenna;

        [ProtoMember(5)]
        public float MinCooldownMs;

        [ProtoMember(6)]
        public float MaxCooldownMs;

        [ProtoMember(7)]
        public bool StartsReady;

        [ProtoMember(8)]
        public int MaxActions;

        [ProtoMember(9)]
        public ActionProfile Actions;

        [ProtoMember(10)]
        public List<string> DamageTypes;

        [ProtoMember(11)]
        public List<string> TimerNames;

        [ProtoMember(12)]
        public ChatProfile ChatMessage;

        [ProtoMember(13)]
        public SpawnProfile Spawner;

        [ProtoMember(14)]
        public bool Triggered;

        [ProtoMember(15)]
        public int CooldownTime;

        [ProtoMember(16)]
        public int TriggerCount;

        [ProtoMember(17)]
        public DateTime LastTriggerTime;

        [ProtoMember(18)]
        public int MinPlayerReputation;

        [ProtoMember(19)]
        public int MaxPlayerReputation;

        [ProtoMember(20)]
        public ConditionProfile Conditions;

        [ProtoMember(21)]
        public long DetectedEntityId;

        [ProtoIgnore]
        public Random Rnd;

        public TriggerProfile() {

            Type = "";

            UseTrigger = false;
            TargetDistance = 3000;
            InsideAntenna = false;
            MinCooldownMs = 0;
            MaxCooldownMs = 1;
            StartsReady = false;
            MaxActions = -1;
            Actions = new ActionProfile();
            DamageTypes = new List<string>();
            TimerNames = new List<string>();
            ChatMessage = new ChatProfile();
            Spawner = new SpawnProfile();
            Conditions = new ConditionProfile();

            Triggered = false;
            CooldownTime = 0;
            TriggerCount = 0;
            LastTriggerTime = MyAPIGateway.Session.GameDateTime;
            DetectedEntityId = 0;

            MinPlayerReputation = -1501;
            MaxPlayerReputation = 1501;


            Rnd = new Random();

        }

        public void ActivateTrigger() {

            if(MaxActions >= 0 && TriggerCount >= MaxActions) {

                UseTrigger = false;
                return;

            }

            if(CooldownTime > 0) {

                TimeSpan duration = MyAPIGateway.Session.GameDateTime - LastTriggerTime;

                if(duration.TotalMilliseconds >= CooldownTime) {

                    Triggered = true;

                }

            } else {

                Triggered = true;

            }

        }

        public void InitTags(string customData) {

            if(string.IsNullOrWhiteSpace(customData) == false) {

                var descSplit = customData.Split('\n');

                foreach(var tag in descSplit) {

                    //Type
                    if(tag.Contains("[Type:") == true) {

                        Type = TagHelper.TagStringCheck(tag);

                    }

                    //UseTrigger
                    if(tag.Contains("[UseTrigger:") == true) {

                        UseTrigger = TagHelper.TagBoolCheck(tag);

                    }

                    //InsideAntenna
                    if(tag.Contains("[InsideAntenna:") == true) {

                        InsideAntenna = TagHelper.TagBoolCheck(tag);

                    }

                    //TargetDistance
                    if(tag.Contains("[TargetDistance:") == true) {

                        TargetDistance = TagHelper.TagDoubleCheck(tag, TargetDistance);

                    }

                    //MinCooldown
                    if(tag.Contains("[MinCooldownMs:") == true) {

                        MinCooldownMs = TagHelper.TagFloatCheck(tag, MinCooldownMs);

                    }

                    //MaxCooldown
                    if(tag.Contains("[MaxCooldownMs:") == true) {

                        MaxCooldownMs = TagHelper.TagFloatCheck(tag, MaxCooldownMs);

                    }

                    //StartsReady
                    if(tag.Contains("[StartsReady:") == true) {

                        StartsReady = TagHelper.TagBoolCheck(tag);

                    }

                    //Actions
                    if(tag.Contains("[Actions:") == true) {

                        var tempValue = TagHelper.TagStringCheck(tag);

                        if(string.IsNullOrWhiteSpace(tempValue) == false) {

                            byte[] byteData = { };

                            if(TagHelper.ActionObjectTemplates.TryGetValue(tempValue, out byteData) == true) {

                                try {

                                    var profile = MyAPIGateway.Utilities.SerializeFromBinary<ActionProfile>(byteData);

                                    if(profile != null) {

                                        Actions = profile;

                                    }

                                } catch(Exception) {



                                }

                            }

                        }

                    }

                    //DamageTypes
                    if(tag.Contains("[DamageTypes:") == true) {

                        var tempValue = TagHelper.TagStringCheck(tag);

                        if(DamageTypes.Contains(tempValue) == false) {

                            DamageTypes.Add(tempValue);

                        }

                    }

                    //TimerNames
                    if(tag.Contains("[TimerNames:") == true) {

                        var tempValue = TagHelper.TagStringCheck(tag);

                        if(TimerNames.Contains(tempValue) == false) {

                            TimerNames.Add(tempValue);

                        }

                    }

                    //ChatMessage
                    if(tag.Contains("[ChatMessage:") == true) {

                        var tempValue = TagHelper.TagStringCheck(tag);

                        if(string.IsNullOrWhiteSpace(tempValue) == false) {

                            byte[] byteData = { };

                            if(TagHelper.ChatObjectTemplates.TryGetValue(tempValue, out byteData) == true) {

                                try {

                                    var profile = MyAPIGateway.Utilities.SerializeFromBinary<ChatProfile>(byteData);

                                    if(profile != null) {

                                        ChatMessage = profile;

                                    }

                                } catch(Exception) {



                                }

                            }

                        }

                    }

                    //Spawner
                    if(tag.Contains("[Spawner:") == true) {

                        var tempValue = TagHelper.TagStringCheck(tag);

                        if(string.IsNullOrWhiteSpace(tempValue) == false) {

                            byte[] byteData = { };

                            if(TagHelper.SpawnerObjectTemplates.TryGetValue(tempValue, out byteData) == true) {

                                try {

                                    var profile = MyAPIGateway.Utilities.SerializeFromBinary<SpawnProfile>(byteData);

                                    if(profile != null) {

                                        Spawner = profile;

                                    }

                                } catch(Exception) {



                                }

                            }

                        }

                    }

                    //Conditions
                    if(tag.Contains("[Conditions:") == true) {

                        var tempValue = TagHelper.TagStringCheck(tag);

                        if(string.IsNullOrWhiteSpace(tempValue) == false) {

                            byte[] byteData = { };

                            if(TagHelper.ConditionObjectTemplates.TryGetValue(tempValue, out byteData) == true) {

                                try {

                                    var profile = MyAPIGateway.Utilities.SerializeFromBinary<ConditionProfile>(byteData);

                                    if(profile != null) {

                                        this.Conditions = profile;

                                    }

                                } catch(Exception) {



                                }

                            }

                        }

                    }

                }

            }

            if(MinCooldownMs > MaxCooldownMs) {

                MinCooldownMs = MaxCooldownMs;

            }

            if(StartsReady == true) {

                CooldownTime = 0;

            } else {

                CooldownTime = Rnd.Next((int)MinCooldownMs, (int)MaxCooldownMs);

            }


        }

    }
}
