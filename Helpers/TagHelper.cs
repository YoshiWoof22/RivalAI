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
using RivalAI.Behavior.Subsystems.Profiles;
using RivalAI.Entities;

namespace RivalAI.Helpers {

	public static class TagHelper {

		public static Dictionary<string, string> BehaviorTemplates = new Dictionary<string, string>();

		public static Dictionary<string, byte[]> ActionObjectTemplates = new Dictionary<string, byte[]>();
		public static Dictionary<string, byte[]> ChatObjectTemplates = new Dictionary<string, byte[]>();
		public static Dictionary<string, byte[]> ConditionObjectTemplates = new Dictionary<string, byte[]>();
		public static Dictionary<string, byte[]> SpawnerObjectTemplates = new Dictionary<string, byte[]>();
		public static Dictionary<string, byte[]> TargetObjectTemplates = new Dictionary<string, byte[]>();
		public static Dictionary<string, byte[]> TriggerObjectTemplates = new Dictionary<string, byte[]>();
		public static Dictionary<string, byte[]> TriggerGroupObjectTemplates = new Dictionary<string, byte[]>();

		public static void Setup() {

			var definitionList = MyDefinitionManager.Static.GetEntityComponentDefinitions();

			//Get All Chat, Spawner
			foreach (var def in definitionList) {

				try {

					if (string.IsNullOrWhiteSpace(def.DescriptionText) == true) {

						continue;

					}

					if (def.DescriptionText.Contains("[RivalAI Chat]") == true && ChatObjectTemplates.ContainsKey(def.Id.SubtypeName) == false) {

						var chatObject = new ChatProfile();
						chatObject.InitTags(def.DescriptionText);
						chatObject.ProfileSubtypeId = def.Id.SubtypeName;
						var chatBytes = MyAPIGateway.Utilities.SerializeToBinary<ChatProfile>(chatObject);
						//Logger.WriteLog("Chat Profile Added: " + def.Id.SubtypeName);
						ChatObjectTemplates.Add(def.Id.SubtypeName, chatBytes);
						continue;

					}

					if (def.DescriptionText.Contains("[RivalAI Spawn]") == true && SpawnerObjectTemplates.ContainsKey(def.Id.SubtypeName) == false) {

						var spawnerObject = new SpawnProfile();
						spawnerObject.InitTags(def.DescriptionText);
						spawnerObject.ProfileSubtypeId = def.Id.SubtypeName;
						var spawnerBytes = MyAPIGateway.Utilities.SerializeToBinary<SpawnProfile>(spawnerObject);
						//Logger.WriteLog("Spawner Profile Added: " + def.Id.SubtypeName);
						SpawnerObjectTemplates.Add(def.Id.SubtypeName, spawnerBytes);
						continue;

					}

				} catch (Exception) {

					Logger.MsgDebug(string.Format("Caught Error While Processing Definition {0}", def.Id));

				}

			}

			foreach (var def in definitionList) {

				try {

					if(string.IsNullOrWhiteSpace(def.DescriptionText) == true) {

						continue;

					}

					if(def.DescriptionText.Contains("[RivalAI Action]") == true && ActionObjectTemplates.ContainsKey(def.Id.SubtypeName) == false) {

						var actionObject = new ActionProfile();
						actionObject.InitTags(def.DescriptionText);
						actionObject.ProfileSubtypeId = def.Id.SubtypeName;
						var targetBytes = MyAPIGateway.Utilities.SerializeToBinary<ActionProfile>(actionObject);
						//Logger.WriteLog("Action Profile Added: " + def.Id.SubtypeName);
						ActionObjectTemplates.Add(def.Id.SubtypeName, targetBytes);
						continue;

					}
			
					if(def.DescriptionText.Contains("[RivalAI Condition]") == true && ChatObjectTemplates.ContainsKey(def.Id.SubtypeName) == false) {

						var conditionObject = new ConditionProfile();
						conditionObject.InitTags(def.DescriptionText);
						conditionObject.ProfileSubtypeId = def.Id.SubtypeName;
						var conditionBytes = MyAPIGateway.Utilities.SerializeToBinary<ConditionProfile>(conditionObject);
						//Logger.WriteLog("Condition Profile Added: " + def.Id.SubtypeName);
						ConditionObjectTemplates.Add(def.Id.SubtypeName, conditionBytes);
						continue;

					}

					if(def.DescriptionText.Contains("[RivalAI Target]") == true && TargetObjectTemplates.ContainsKey(def.Id.SubtypeName) == false) {

						var targetObject = new TargetProfile();
						targetObject.InitTags(def.DescriptionText);
						targetObject.ProfileSubtypeId = def.Id.SubtypeName;
						var targetBytes = MyAPIGateway.Utilities.SerializeToBinary<TargetProfile>(targetObject);
						//Logger.WriteLog("Target Profile Added: " + def.Id.SubtypeName);
						TargetObjectTemplates.Add(def.Id.SubtypeName, targetBytes);
						continue;

					}

				} catch(Exception) {

					Logger.MsgDebug(string.Format("Caught Error While Processing Definition {0}", def.Id));

				}

			}

			//Get All Triggers - Build With Action, Chat and Spawner
			foreach(var def in definitionList) {

				if(string.IsNullOrWhiteSpace(def.DescriptionText) == true) {

					continue;

				}

				if(def.DescriptionText.Contains("[RivalAI Trigger]") == true && TriggerObjectTemplates.ContainsKey(def.Id.SubtypeName) == false) {

					var triggerObject = new TriggerProfile();
					triggerObject.InitTags(def.DescriptionText);
					triggerObject.ProfileSubtypeId = def.Id.SubtypeName;
					var triggerBytes = MyAPIGateway.Utilities.SerializeToBinary<TriggerProfile>(triggerObject);
					//Logger.WriteLog("Trigger Profile Added: " + def.Id.SubtypeName);
					TriggerObjectTemplates.Add(def.Id.SubtypeName, triggerBytes);
					continue;

				}

			}

			//Get All TriggerGroups
			foreach (var def in definitionList) {

				if (string.IsNullOrWhiteSpace(def.DescriptionText) == true) {

					continue;

				}

				if (def.DescriptionText.Contains("[RivalAI TriggerGroup]") == true && TriggerObjectTemplates.ContainsKey(def.Id.SubtypeName) == false) {

					var triggerObject = new TriggerGroupProfile();
					triggerObject.InitTags(def.DescriptionText);
					triggerObject.ProfileSubtypeId = def.Id.SubtypeName;
					var triggerBytes = MyAPIGateway.Utilities.SerializeToBinary<TriggerGroupProfile>(triggerObject);
					//Logger.WriteLog("Trigger Profile Added: " + def.Id.SubtypeName);
					TriggerGroupObjectTemplates.Add(def.Id.SubtypeName, triggerBytes);
					continue;

				}


			}

			//Get All Behavior
			foreach (var def in definitionList) {

				if (string.IsNullOrWhiteSpace(def.DescriptionText)) {

					continue;

				}

				if ((def.DescriptionText.Contains("[RivalAI Behavior]") || def.DescriptionText.Contains("[Rival AI Behavior]")) && BehaviorTemplates.ContainsKey(def.Id.SubtypeName) == false) {

					BehaviorTemplates.Add(def.Id.SubtypeName, def.DescriptionText);
					continue;

				}

			}

			//Print Profile Names To Log:
			BuildKeyListAndWriteToLog("Behavior", BehaviorTemplates.Keys);
			BuildKeyListAndWriteToLog("Trigger", TriggerObjectTemplates.Keys);
			BuildKeyListAndWriteToLog("Condition", ConditionObjectTemplates.Keys);
			BuildKeyListAndWriteToLog("Action", ActionObjectTemplates.Keys);
			BuildKeyListAndWriteToLog("Chat", ChatObjectTemplates.Keys);
			BuildKeyListAndWriteToLog("Spawn", SpawnerObjectTemplates.Keys);
			BuildKeyListAndWriteToLog("Target", TargetObjectTemplates.Keys);

		}

		private static void BuildKeyListAndWriteToLog(string profileType, IEnumerable<string> stringList) {

			var sb = new StringBuilder();
			sb.Append("Detected Profiles: " + profileType).AppendLine();

			foreach (var subtypeName in stringList.OrderBy(x => x)) {

				sb.Append(" - ").Append(subtypeName).AppendLine();
			
			}

			sb.AppendLine();
			Logger.WriteLog(sb.ToString());

		}

		private static string [] ProcessTag(string tag){
			
			var thisTag = tag.Trim();

			if (thisTag.Length > 0 && thisTag[0] == '[')
				thisTag = thisTag.Remove(0,1);

			if (thisTag.Length > 0 && thisTag[thisTag.Length - 1] == ']')
				thisTag = thisTag.Remove(thisTag.Length - 1,1);

			var tagSplit = thisTag.Split(':');
			string a = "";
			string b = "";

			if(tagSplit.Length > 2) {

				a = tagSplit[0];

				for(int i = 1;i < tagSplit.Length;i++) {

					b += tagSplit[i];

					if(i != tagSplit.Length - 1) {

						b += ":";

					}

				}

				tagSplit = new string[] {a,b};

			}

			return tagSplit;
			
		}

		public static BlockTypeEnum TagBlockTargetTypesCheck(string tag) {

			BlockTypeEnum result = BlockTypeEnum.None;
			var tagSplit = ProcessTag(tag);

			if(tagSplit.Length == 2) {

				if(BlockTypeEnum.TryParse(tagSplit[1], out result) == false) {

					return BlockTypeEnum.None;

				}

			}

			return result;

		}

		public static Base6Directions.Direction TagBase6DirectionCheck(string tag) {

			Base6Directions.Direction result = Base6Directions.Direction.Forward;
			var tagSplit = ProcessTag(tag);

			if(tagSplit.Length == 2) {

				bool parseResult = Base6Directions.Direction.TryParse(tagSplit[1], out result) == false;

			}

			return result;

		}

		public static bool TagBoolCheck(string tag){
			
			bool result = false;
			var tagSplit = ProcessTag(tag);

			if(tagSplit.Length == 2){

				bool parseResult = bool.TryParse(tagSplit[1], out result);
				
			}
			
			return result;
			
		}

		public static BroadcastType TagBroadcastTypeEnumCheck(string tag) {

			BroadcastType result = BroadcastType.None;
			var tagSplit = ProcessTag(tag);

			if(tagSplit.Length == 2) {

				if(SpawnPositioningEnum.TryParse(tagSplit[1], out result) == false) {

					return BroadcastType.None;

				}

			}

			return result;

		}

		public static CheckEnum TagCheckEnumCheck(string tag) {

			CheckEnum result = CheckEnum.Ignore;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (CheckEnum.TryParse(tagSplit[1], out result) == false) {

					return CheckEnum.Ignore;

				}

			}

			return result;

		}

		public static Direction TagDirectionEnumCheck(string tag) {

			Direction result = Direction.None;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (Direction.TryParse(tagSplit[1], out result) == false) {

					return Direction.None;

				}

			}

			return result;

		}

		public static double TagDoubleCheck(string tag, double defaultValue){
			
			double result = defaultValue;
			var tagSplit = ProcessTag(tag);
					
			if(tagSplit.Length == 2){
				
				if(double.TryParse(tagSplit[1], out result) == false){
					
					return defaultValue;
					
				}
				
			}
			
			return result;
			
		}
		
		public static float TagFloatCheck(string tag, float defaultValue){
			
			float result = defaultValue;
			var tagSplit = ProcessTag(tag);
					
			if(tagSplit.Length == 2){
				
				if(float.TryParse(tagSplit[1], out result) == false){
					
					return defaultValue;
					
				}
				
			}
			
			return result;
			
		}
		
		public static int TagIntCheck(string tag, int defaultValue){
			
			int result = defaultValue;
			var tagSplit = ProcessTag(tag);
					
			if(tagSplit.Length == 2){
				
				if(int.TryParse(tagSplit[1], out result) == false){
					
					return defaultValue;
					
				}
				
			}
			
			return result;
			
		}

		public static long TagLongCheck(string tag, long defaultValue){

			long result = defaultValue;
			var tagSplit = ProcessTag(tag);
					
			if(tagSplit.Length == 2){
				
				if(long.TryParse(tagSplit[1], out result) == false){
					
					return defaultValue;
					
				}
				
			}
			
			return result;
			
		}

		public static string TagStringCheck(string tag) {

			string result = "";
			var tagSplit = ProcessTag(tag);

			if(tagSplit.Length == 2) {

				result = tagSplit[1];

			}

			return result;

		}

		public static SpawnPositioningEnum TagSpawnPositioningEnumCheck(string tag) {

			SpawnPositioningEnum result = SpawnPositioningEnum.RandomDirection;
			var tagSplit = ProcessTag(tag);

			if(tagSplit.Length == 2) {

				if(SpawnPositioningEnum.TryParse(tagSplit[1], out result) == false) {

					return SpawnPositioningEnum.RandomDirection;

				}

			}

			return result;

		}

		public static TargetSortEnum TagTargetDistanceEnumCheck(string tag) {

			TargetSortEnum result = TargetSortEnum.Random;
			var tagSplit = ProcessTag(tag);

			if(tagSplit.Length == 2) {

				if(TargetSortEnum.TryParse(tagSplit[1], out result) == false) {

					return TargetSortEnum.Random;

				}

			}

			return result;

		}

		public static TargetFilterEnum TagTargetFilterEnumCheck(string tag) {

			TargetFilterEnum result = TargetFilterEnum.None;
			var tagSplit = ProcessTag(tag);

			if(tagSplit.Length == 2) {

				if(TargetFilterEnum.TryParse(tagSplit[1], out result) == false) {

					return TargetFilterEnum.None;

				}

			}

			return result;

		}

		public static TargetObstructionEnum TagTargetObstructionEnumCheck(string tag) {

			TargetObstructionEnum result = TargetObstructionEnum.None;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (TargetObstructionEnum.TryParse(tagSplit[1], out result) == false) {

					return TargetObstructionEnum.None;

				}

			}

			return result;

		}

		public static OwnerTypeEnum TagTargetOwnerEnumCheck(string tag) {

			OwnerTypeEnum result = OwnerTypeEnum.None;
			var tagSplit = ProcessTag(tag);

			if(tagSplit.Length == 2) {

				if(OwnerTypeEnum.TryParse(tagSplit[1], out result) == false) {

					return OwnerTypeEnum.None;

				}

			}

			return result;

		}

		public static RelationTypeEnum TagTargetRelationEnumCheck(string tag) {

			RelationTypeEnum result = RelationTypeEnum.None;
			var tagSplit = ProcessTag(tag);

			if(tagSplit.Length == 2) {

				if(RelationTypeEnum.TryParse(tagSplit[1], out result) == false) {

					return RelationTypeEnum.None;

				}

			}

			return result;

		}

		public static TargetSortEnum TagTargetSortEnumCheck(string tag) {

			TargetSortEnum result = TargetSortEnum.Random;
			var tagSplit = ProcessTag(tag);

			if (tagSplit.Length == 2) {

				if (TargetSortEnum.TryParse(tagSplit[1], out result) == false) {

					return TargetSortEnum.Random;

				}

			}

			return result;

		}

		public static TargetTypeEnum TagTargetTypeEnumCheck(string tag) {

			TargetTypeEnum result = TargetTypeEnum.None;
			var tagSplit = ProcessTag(tag);

			if(tagSplit.Length == 2) {

				if(TargetTypeEnum.TryParse(tagSplit[1], out result) == false) {

					return TargetTypeEnum.None;

				}

			}

			return result;

		}

		public static TriggerAction TagTriggerActionCheck(string tag) {

			TriggerAction result = TriggerAction.None;
			var tagSplit = ProcessTag(tag);

			if(tagSplit.Length == 2) {

				if(TriggerAction.TryParse(tagSplit[1], out result) == false) {

					return TriggerAction.None;

				}

			}

			return result;

		}

		public static Vector3D TagVector3DCheck(string tag) {

			Vector3D result = Vector3D.Zero;
			var tagSplit = ProcessTag(tag);

			if(tagSplit.Length == 2) {

				if(Vector3D.TryParse(FixVectorString(tagSplit[1]), out result) == false) {

					return Vector3D.Zero;

				}

			}

			return result;

		}

		public static string FixVectorString(string source) {

			string newString = source;

			if (newString.Length == 0)
				return source;

			if (newString[0] == '{')
				newString = newString.Remove(0, 1);

			if (newString.Length == 0)
				return source;

			if (newString[newString.Length - 1] == '}')
				newString = newString.Remove(newString.Length - 1, 1);

			return newString;

		}

	}
	
}
