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
using RivalAI.Behavior.Subsystems;
using RivalAI.Helpers;

namespace RivalAI.Helpers {

    public static class DamageHelper {

        public static HashSet<IMyCubeGrid> MonitoredGrids = new HashSet<IMyCubeGrid>();
        public static Dictionary<IMyCubeGrid, Action<object, MyDamageInformation>> RegisteredDamageHandlers = new Dictionary<IMyCubeGrid, Action<object, MyDamageInformation>>();

        public static void DamageHandler(object target, MyDamageInformation info) {

            var block = target as IMySlimBlock;

            if(block == null) {

                return;

            }

            var grid = block.CubeGrid;

            if(MonitoredGrids.Contains(grid)) {

                Action<object, MyDamageInformation> action = null;

                if(RegisteredDamageHandlers.TryGetValue(grid, out action)) {

                    action?.Invoke(target, info);
                    return;

                }

            }

        }

        

    }

}
