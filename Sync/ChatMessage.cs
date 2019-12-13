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
using RivalAI;
using RivalAI.Behavior;
using RivalAI.Behavior.Settings;
using RivalAI.Behavior.Subsystems;
using RivalAI.Helpers;

namespace RivalAI.Sync {

    public enum ChatMsgMode {

        None,
        ServerProcessing,
        ReturnMessage

    }

    [ProtoContract]
    public class ChatMessage {

        [ProtoMember(1)]
        public ChatMsgMode Mode;

        [ProtoMember(2)]
        public string Message;

        [ProtoMember(3)]
        public long PlayerId;

        [ProtoMember(4)]
        public ulong SteamId;

        [ProtoMember(5)]
        public string ReturnMessage;

        [ProtoMember(6)]
        public string ClipboardPayload;

        public ChatMessage() {

            Mode = ChatMsgMode.None;
            Message = "";
            PlayerId = 0;
            SteamId = 0;
            ReturnMessage = "";
            ClipboardPayload = "";

        }

        public bool ProcessDebugMode() {

            // /RAI.DebugMode.true

            var msg = this.Message.Split('.');

            if(msg.Length < 3) {

                this.ReturnMessage = "Command Received Could Not Be Read Properly.";
                return false;

            }

            bool result = false;

            if(bool.TryParse(msg[2], out result) == false) {

                this.ReturnMessage = "Debug Mode Value Not Recognized. Accepts true or false.";
                return true;

            }

            Logger.LoggerDebugMode = result;
            this.ReturnMessage = "Debug Mode Enabled: " + result.ToString();
            return true;

        }

    }

}
