﻿using RivalAI.Helpers;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivalAI.Behavior {

	public enum BehaviorManagerMode {
	
		None,
		Parallel,
		MainThread
	
	}

	public enum BehaviorManageSubmode {

		None,
		Collision,
		Targeting,
		AutoPilot,
		Weapons,
		Triggers,
		Behavior

	}

	public static class BehaviorManager {

		public static List<IBehavior> Behaviors = new List<IBehavior>();

		public static BehaviorManagerMode Mode = BehaviorManagerMode.None;
		public static BehaviorManageSubmode Submode = BehaviorManageSubmode.None;
		public static int CurrentBehaviorIndex = 0;

		private static bool _debugDraw = false;

		private static byte _barrageCounter = 0;
		private static byte _behaviorCounter = 0;

		public static void ProcessBehaviors() {

			if (_debugDraw) {

				for (int i = Behaviors.Count - 1; i >= 0; i--) {

					if (!Behaviors[i].IsClosed() && Behaviors[i].IsAIReady()) {

						Behaviors[i].DebugDrawWaypoints();

					}

				}

			}

			_barrageCounter++;

			if (Mode != BehaviorManagerMode.None) {

				try {

					ProcessParallelMethods();
					ProcessMainThreadMethods();

				} catch (Exception e) {

					Logger.MsgDebug("Exception in Main Behavior Processing", DebugTypeEnum.General);
					Logger.MsgDebug(e.ToString(), DebugTypeEnum.General);

				}

			} else {

				_behaviorCounter++;

			}

			if ((_barrageCounter % 10) == 0) {

				ProcessWeaponsBarrage();
				_barrageCounter = 0;

			}

			if (_behaviorCounter == 15) {

				for (int i = Behaviors.Count - 1; i >= 0; i--) {

					if (Behaviors[i].IsClosed()) {

						Behaviors.RemoveAt(i);
						continue;
					
					}
				
				}

				//Logger.MsgDebug("Start Parallel For All Behaviors", DebugTypeEnum.General);
				Mode = BehaviorManagerMode.Parallel;
				_behaviorCounter = 0;

			}

		}

		private static void ProcessParallelMethods() {

			if (Mode != BehaviorManagerMode.Parallel)
				return;

			MyAPIGateway.Parallel.Start(() => {

				try {

					ProcessCollisionChecksParallel();
					ProcessTargetingParallel();
					ProcessAutoPilotParallel();
					ProcessWeaponsParallel();
					ProcessTriggersParallel();
					Mode = BehaviorManagerMode.MainThread;

				} catch (Exception e) {

					Logger.MsgDebug("Exception in Parallel Calculations", DebugTypeEnum.General);
					Logger.MsgDebug(e.ToString(), DebugTypeEnum.General);

				}
				

			});
	
		}

		private static void ProcessMainThreadMethods() {

			if (Mode != BehaviorManagerMode.MainThread)
				return;

			ProcessAutoPilotMain();
			ProcessWeaponsMain();
			ProcessTriggersMain();
			ProcessDespawnConditions();
			ProcessMainBehavior();
			Mode = BehaviorManagerMode.None;

		}

		private static void ProcessCollisionChecksParallel() {

			for (int i = Behaviors.Count - 1; i >= 0; i--) {

				if (!Behaviors[i].IsAIReady())
					continue;

				Behaviors[i].ProcessCollisionChecks();

			}
		
		}

		private static void ProcessTargetingParallel() {

			for (int i = Behaviors.Count - 1; i >= 0; i--) {

				if (!Behaviors[i].IsAIReady())
					continue;

				Behaviors[i].ProcessTargetingChecks();

			}

		}

		private static void ProcessAutoPilotParallel() {

			for (int i = Behaviors.Count - 1; i >= 0; i--) {

				if (!Behaviors[i].IsAIReady())
					continue;

				Behaviors[i].ProcessAutoPilotChecks();

			}

		}

		private static void ProcessAutoPilotMain() {

			for (int i = Behaviors.Count - 1; i >= 0; i--) {

				if (!Behaviors[i].IsAIReady())
					continue;

				Behaviors[i].EngageAutoPilot();

			}

		}

		private static void ProcessWeaponsParallel() {

			for (int i = Behaviors.Count - 1; i >= 0; i--) {

				if (!Behaviors[i].IsAIReady())
					continue;

				Behaviors[i].ProcessWeaponChecks();

			}

		}

		private static void ProcessWeaponsMain() {

			for (int i = Behaviors.Count - 1; i >= 0; i--) {

				if (!Behaviors[i].IsAIReady())
					continue;

				Behaviors[i].SetInitialWeaponReadiness();
				Behaviors[i].FireWeapons();

			}

		}

		private static void ProcessWeaponsBarrage() {

			for (int i = Behaviors.Count - 1; i >= 0; i--) {

				if (!Behaviors[i].IsAIReady())
					continue;

				Behaviors[i].FireBarrageWeapons();

			}

		}

		private static void ProcessTriggersParallel() {

			for (int i = Behaviors.Count - 1; i >= 0; i--) {

				if (!Behaviors[i].IsAIReady())
					continue;

				Behaviors[i].ProcessTriggerChecks();

			}

		}

		private static void ProcessTriggersMain() {

			for (int i = Behaviors.Count - 1; i >= 0; i--) {

				if (!Behaviors[i].IsAIReady())
					continue;

				Behaviors[i].ProcessActivatedTriggers();

			}

		}

		private static void ProcessDespawnConditions() {

			for (int i = Behaviors.Count - 1; i >= 0; i--) {

				if (!Behaviors[i].IsAIReady())
					continue;

				Behaviors[i].CheckDespawnConditions();

			}

		}

		private static void ProcessMainBehavior() {

			for (int i = Behaviors.Count - 1; i >= 0; i--) {

				if (!Behaviors[i].IsAIReady())
					continue;

				Behaviors[i].RunMainBehavior();

			}

		}

	}

}