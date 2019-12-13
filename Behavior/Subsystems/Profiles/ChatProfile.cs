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
    public class ChatProfile {

        [ProtoMember(1)]
        public bool UseChat;

        [ProtoMember(2)]
        public int MinTime;

        [ProtoMember(3)]
        public int MaxTime;

        [ProtoMember(4)]
        public bool StartsReady;

        [ProtoMember(5)]
        public int Chance;

        [ProtoMember(6)]
        public int MaxChats;

        [ProtoMember(7)]
        public bool BroadcastRandomly;

        [ProtoMember(8)]
        public List<string> ChatMessages;

        [ProtoMember(9)]
        public List<string> ChatAudio;

        [ProtoMember(10)]
        public List<BroadcastType> BroadcastChatType;

        [ProtoMember(11)]
        public int SecondsUntilChat;

        [ProtoMember(12)]
        public int ChatSentCount;

        [ProtoMember(13)]
        public DateTime LastChatTime;

        [ProtoMember(14)]
        public int MessageIndex;

        [ProtoMember(15)]
        public string AuthorOverride;

        [ProtoMember(16)]
        public string ColorOverride;

        [ProtoIgnore]
        public Random Rnd;

        public ChatProfile() {

            UseChat = false;
            MinTime = 0;
            MaxTime = 1;
            StartsReady = false;
            Chance = 100;
            MaxChats = -1;
            BroadcastRandomly = false;
            ChatMessages = new List<string>();
            ChatAudio = new List<string>();
            BroadcastChatType = new List<BroadcastType>();
            AuthorOverride = "";
            ColorOverride = "";

            SecondsUntilChat = 0;
            ChatSentCount = 0;
            LastChatTime = MyAPIGateway.Session.GameDateTime;
            MessageIndex = 0;

            Rnd = new Random();

        }

        public bool ProcessChat(ref string msg, ref string audio, ref BroadcastType type) {

            if(UseChat == false) {

                return false;

            }

            if(MaxChats >= 0 && ChatSentCount >= MaxChats) {

                UseChat = false;
                return false;

            }

            TimeSpan duration = MyAPIGateway.Session.GameDateTime - LastChatTime;

            if(duration.TotalSeconds < SecondsUntilChat) {

                return false;

            }

            string thisMsg = "";
            string thisSound = "";
            BroadcastType thisType = BroadcastType.None;

            GetChatAndSoundFromLists(ref thisMsg, ref thisSound, ref thisType);

            if(string.IsNullOrWhiteSpace(thisMsg) == true || thisType == BroadcastType.None) {

                return false;

            }

            LastChatTime = MyAPIGateway.Session.GameDateTime;
            SecondsUntilChat = Rnd.Next(MinTime, MaxTime);
            ChatSentCount++;

            msg = thisMsg;
            audio = thisSound;
            type = thisType;
            return true;

        }

        public void InitTags(string customData) {

            if(string.IsNullOrWhiteSpace(customData) == false) {

                var descSplit = customData.Split('\n');

                foreach(var tag in descSplit) {

                    //UseChat
                    if(tag.Contains("[UseChat:") == true) {

                        UseChat = TagHelper.TagBoolCheck(tag);

                    }

                    //ChatMinTime
                    if(tag.Contains("[MinTime:") == true) {

                        MinTime = TagHelper.TagIntCheck(tag, MinTime);

                    }

                    //ChatMaxTime
                    if(tag.Contains("[MaxTime:") == true) {

                        MaxTime = TagHelper.TagIntCheck(tag, MaxTime);

                    }

                    //ChatStartsReady
                    if(tag.Contains("[StartsReady:") == true) {

                        StartsReady = TagHelper.TagBoolCheck(tag);

                    }

                    //ChatChance
                    if(tag.Contains("[Chance:") == true) {

                        Chance = TagHelper.TagIntCheck(tag, Chance);

                    }

                    //MaxChats
                    if(tag.Contains("[MaxChats:") == true) {

                        MaxChats = TagHelper.TagIntCheck(tag, MaxChats);

                    }

                    //BroadcastRandomly
                    if(tag.Contains("[BroadcastRandomly:") == true) {

                        BroadcastRandomly = TagHelper.TagBoolCheck(tag);

                    }

                    //ChatMessages
                    if(tag.Contains("[ChatMessages:") == true) {

                        var tempValue = TagHelper.TagStringCheck(tag);
                        if(string.IsNullOrWhiteSpace(tempValue) == false) {

                            ChatMessages.Add(tempValue);

                        }

                    }

                    //ChatAudio
                    if(tag.Contains("[ChatAudio:") == true) {

                        var tempValue = TagHelper.TagStringCheck(tag);
                        if(string.IsNullOrWhiteSpace(tempValue) == false) {

                            ChatAudio.Add(tempValue);

                        }

                    }

                    //BroadcastChatType
                    if(tag.Contains("[BroadcastChatType:") == true) {

                        BroadcastChatType.Add(TagHelper.TagBroadcastTypeEnumCheck(tag));

                    }

                    //AuthorOverride
                    if(tag.Contains("[AuthorOverride:") == true) {

                        AuthorOverride = TagHelper.TagStringCheck(tag);

                    }

                    //ColorOverride
                    if(tag.Contains("[ColorOverride:") == true) {

                        ColorOverride = TagHelper.TagStringCheck(tag);

                    }

                }

            }

            if(MinTime > MaxTime) {

                MinTime = MaxTime;

            }

            if(StartsReady == true) {

                SecondsUntilChat = 0;

            } else {

                SecondsUntilChat = Rnd.Next(MinTime, MaxTime);

            }


        }

        private void GetChatAndSoundFromLists(ref string message, ref string sound, ref BroadcastType type) {

            if(ChatMessages.Count == 0) {

                return;

            }

            if(BroadcastRandomly == true) {

                var index = Rnd.Next(0, ChatMessages.Count);
                message = ChatMessages[index];

                if(ChatAudio.Count >= ChatMessages.Count) {

                    sound = ChatAudio[index];

                }

                if(BroadcastChatType.Count >= ChatMessages.Count) {

                    type = BroadcastChatType[index];

                }

            } else {

                if(MessageIndex >= ChatMessages.Count) {

                    MessageIndex = 0;

                }

                message = ChatMessages[MessageIndex];

                if(ChatAudio.Count >= ChatMessages.Count) {

                    sound = ChatAudio[MessageIndex];

                }

                if(BroadcastChatType.Count >= ChatMessages.Count) {

                    type = BroadcastChatType[MessageIndex];

                }

                MessageIndex++;

            }


        }

    }
}
