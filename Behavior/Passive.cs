using System;
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

namespace RivalAI.Behavior{
	
	public class Passive : CoreBehavior, IBehavior{

		//Configurable
		public double FighterEngageDistanceSpace;
		public double FighterEngageDistancePlanet;
		
		public bool ReceivedEvadeSignal;
		public bool ReceivedRetreatSignal;
		public bool ReceivedExternalTarget;
		
		public byte Counter;

		public Passive() {

			FighterEngageDistanceSpace = 300;
			FighterEngageDistancePlanet = 600;
			
			ReceivedEvadeSignal = false;
			ReceivedRetreatSignal = false;
			ReceivedExternalTarget = false;
			
			Counter = 0;

		}

		public override void BehaviorInit(IMyRemoteControl remoteControl) {

			//Core Setup
			CoreSetup(remoteControl);

			//Behavior Specific Defaults
			NewAutoPilot.Collision.UseCollisionDetection = false;
			Despawn.UsePlayerDistanceTimer = false;
			NewAutoPilot.Targeting.Data.UseCustomTargeting = false;

			//Get Settings From Custom Data
			InitCoreTags();

			SetupCompleted = true;

		}

	}

}
	
