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
using RivalAI.Entities;

namespace RivalAI.Behavior.Subsystems.Profiles {

	[ProtoContract]
	public class TargetProfile {

		[ProtoMember(1)]
		public bool UseCustomTargeting;

		[ProtoMember(2)]
		public int TimeUntilTargetAcquisition;

		[ProtoMember(3)]
		public bool UseTargetRefresh;

		[ProtoMember(4)]
		public int TimeUntilNextRefresh;

		[ProtoMember(5)]
		public bool UseTargetLastKnownPosition;

		[ProtoMember(6)]
		public int TimeUntilNextEvaluation;

		[ProtoMember(7)]
		public TargetTypeEnum Target;

		[ProtoMember(8)]
		public List<BlockTypeEnum> BlockTargets;

		[ProtoMember(9)]
		public TargetSortEnum GetTargetBy;

		[ProtoMember(10)]
		public double MaxDistance;

		[ProtoMember(11)]
		public List<TargetFilterEnum> MatchAllFilters;

		[ProtoMember(12)]
		public List<TargetFilterEnum> MatchAnyFilters;

		[ProtoMember(13)]
		public List<TargetFilterEnum> MatchNoneFilters;

		[ProtoMember(14)]
		public OwnerTypeEnum Owners;

		[ProtoMember(15)]
		public RelationTypeEnum Relations;

		[ProtoMember(16)]
		public List<string> FactionTargets;

		[ProtoMember(17)]
		public bool OnlyGetFromEntityOwner;

		[ProtoMember(18)]
		public bool GetFromMinorityGridOwners;

		[ProtoMember(19)]
		public bool PrioritizeSpecifiedFactions;

		[ProtoMember(20)]
		public CheckEnum IsStatic;

		[ProtoMember(21)]
		public double MinAltitude;

		[ProtoMember(22)]
		public double MaxAltitude;

		[ProtoMember(23)]
		public double NonBroadcastVisualRange;

		[ProtoMember(24)]
		public double MinGravity;

		[ProtoMember(25)]
		public double MaxGravity;

		[ProtoMember(26)]
		public double MinSpeed;

		[ProtoMember(27)]
		public double MaxSpeed;

		[ProtoMember(28)]
		public float MinTargetValue;

		[ProtoMember(29)]
		public float MaxTargetValue;

		[ProtoMember(30)]
		public string ProfileSubtypeId;

		[ProtoIgnore]
		public bool BuiltUniqueFilterList;

		[ProtoIgnore]
		public List<TargetFilterEnum> AllUniqueFilters;
		public TargetProfile() {

			UseCustomTargeting = false;

			Target = TargetTypeEnum.None;
			BlockTargets = new List<BlockTypeEnum>();

			TimeUntilTargetAcquisition = 1;
			UseTargetRefresh = false;
			TimeUntilNextRefresh = 60;
			TimeUntilNextEvaluation = 1;

			MaxDistance = 12000;

			MatchAllFilters = new List<TargetFilterEnum>();
			MatchAnyFilters = new List<TargetFilterEnum>();
			MatchNoneFilters = new List<TargetFilterEnum>();
			GetTargetBy = TargetSortEnum.ClosestDistance;

			Owners = OwnerTypeEnum.None;
			Relations = RelationTypeEnum.None;
			FactionTargets = new List<string>();

			OnlyGetFromEntityOwner = false;
			GetFromMinorityGridOwners = false;
			PrioritizeSpecifiedFactions = false;

			IsStatic = CheckEnum.Ignore;

			MinAltitude = -100000;
			MaxAltitude = 100000;

			NonBroadcastVisualRange = 1500;

			MinGravity = 0;
			MaxGravity = 1.1;

			MinSpeed = 0;
			MaxSpeed = 110;

			MinTargetValue = 0;
			MaxTargetValue = 1;

			ProfileSubtypeId = "";
			BuiltUniqueFilterList = false;
			AllUniqueFilters = new List<TargetFilterEnum>();

		}
		
		public void InitTags(string customData) {

			if(string.IsNullOrWhiteSpace(customData) == false) {

				var descSplit = customData.Split('\n');

				foreach(var tag in descSplit) {

					//UseCustomTargeting
					if(tag.Contains("[UseCustomTargeting:") == true) {

						this.UseCustomTargeting = TagHelper.TagBoolCheck(tag);

					}

					//TimeUntilTargetAcquisition
					if (tag.Contains("[TimeUntilTargetAcquisition:") == true) {

						this.TimeUntilTargetAcquisition = TagHelper.TagIntCheck(tag, this.TimeUntilTargetAcquisition);

					}

					//UseTargetRefresh
					if (tag.Contains("[UseTargetRefresh:") == true) {

						this.UseTargetRefresh = TagHelper.TagBoolCheck(tag);

					}

					//TimeUntilNextRefresh
					if (tag.Contains("[TimeUntilNextRefresh:") == true) {

						this.TimeUntilNextRefresh = TagHelper.TagIntCheck(tag, this.TimeUntilNextRefresh);

					}

					//UseTargetLastKnownPosition
					if (tag.Contains("[UseTargetLastKnownPosition:") == true) {

						this.UseTargetLastKnownPosition = TagHelper.TagBoolCheck(tag);

					}

					//TimeUntilNextEvaluation
					if (tag.Contains("[TimeUntilNextEvaluation:") == true) {

						this.TimeUntilNextEvaluation = TagHelper.TagIntCheck(tag, this.TimeUntilNextEvaluation);

					}

					//Target
					if (tag.Contains("[Target:") == true) {

						this.Target = TagHelper.TagTargetTypeEnumCheck(tag);

					}

					//BlockTargets
					if (tag.Contains("[BlockTargets:") == true) {

						var tempValue = TagHelper.TagBlockTargetTypesCheck(tag);

						if(tempValue != BlockTypeEnum.None && this.BlockTargets.Contains(tempValue) == false) {

							this.BlockTargets.Add(tempValue);

						}

					}

					//GetTargetBy
					if (tag.Contains("[GetTargetBy:") == true) {

						this.GetTargetBy = TagHelper.TagTargetSortEnumCheck(tag);

					}

					//MaxDistance
					if (tag.Contains("[MaxDistance:") == true) {

						this.MaxDistance = TagHelper.TagDoubleCheck(tag, this.MaxDistance);

					}

					//MatchAllFilters
					if (tag.Contains("[MatchAllFilters:") == true) {

						var tempValue = TagHelper.TagTargetFilterEnumCheck(tag);

						if (tempValue != TargetFilterEnum.None && !this.MatchAllFilters.Contains(tempValue)) {

							this.MatchAllFilters.Add(tempValue);

						}

					}

					//MatchAnyFilters
					if (tag.Contains("[MatchAnyFilters:") == true) {

						var tempValue = TagHelper.TagTargetFilterEnumCheck(tag);

						if (tempValue != TargetFilterEnum.None && !this.MatchAnyFilters.Contains(tempValue)) {

							this.MatchAnyFilters.Add(tempValue);

						}

					}

					//MatchNoneFilters
					if (tag.Contains("[MatchNoneFilters:") == true) {

						var tempValue = TagHelper.TagTargetFilterEnumCheck(tag);

						if (tempValue != TargetFilterEnum.None && !this.MatchNoneFilters.Contains(tempValue)) {

							this.MatchNoneFilters.Add(tempValue);

						}

					}

					//Owners
					if (tag.Contains("[Owners:") == true) {

						var tempValue = TagHelper.TagTargetOwnerEnumCheck(tag);

						if(!this.Owners.HasFlag(tempValue)) {

							this.Owners |= tempValue;

						}

					}
					
					//Relations
					if(tag.Contains("[Relations:") == true) {

						var tempValue = TagHelper.TagTargetRelationEnumCheck(tag);

						if(this.Relations.HasFlag(tempValue) == false) {

							this.Relations |= tempValue;

						}

					}

					//FactionTargets
					if (tag.Contains("[FactionTargets:") == true) {

						var tempValue = TagHelper.TagStringCheck(tag);

						if (!string.IsNullOrWhiteSpace(tempValue) && !this.FactionTargets.Contains(tempValue)) {

							this.FactionTargets.Add(tempValue);

						}

					}

					//OnlyGetFromEntityOwner
					if (tag.Contains("[OnlyGetFromEntityOwner:") == true) {

						this.OnlyGetFromEntityOwner = TagHelper.TagBoolCheck(tag);

					}

					//GetFromMinorityGridOwners
					if (tag.Contains("[GetFromMinorityGridOwners:") == true) {

						this.GetFromMinorityGridOwners = TagHelper.TagBoolCheck(tag);

					}

					//PrioritizeSpecifiedFactions
					if (tag.Contains("[PrioritizeSpecifiedFactions:") == true) {

						this.PrioritizeSpecifiedFactions = TagHelper.TagBoolCheck(tag);

					}

					//IsStatic
					if (tag.Contains("[IsStatic:") == true) {

						this.IsStatic = TagHelper.TagCheckEnumCheck(tag);

					}

					//MinAltitude
					if (tag.Contains("[MinAltitude:") == true) {

						this.MinAltitude = TagHelper.TagDoubleCheck(tag, this.MinAltitude);

					}

					//MaxAltitude
					if (tag.Contains("[MaxAltitude:") == true) {

						this.MaxAltitude = TagHelper.TagDoubleCheck(tag, this.MaxAltitude);

					}

					//NonBroadcastVisualRange
					if (tag.Contains("[NonBroadcastVisualRange:") == true) {

						this.NonBroadcastVisualRange = TagHelper.TagDoubleCheck(tag, this.NonBroadcastVisualRange);

					}

					//MinGravity
					if (tag.Contains("[MinGravity:") == true) {

						this.MinGravity = TagHelper.TagDoubleCheck(tag, this.MinGravity);

					}

					//MaxGravity
					if (tag.Contains("[MaxGravity:") == true) {

						this.MaxGravity = TagHelper.TagDoubleCheck(tag, this.MaxGravity);

					}

					//MinSpeed
					if (tag.Contains("[MinSpeed:") == true) {

						this.MinSpeed = TagHelper.TagDoubleCheck(tag, this.MinSpeed);

					}

					//MaxSpeed
					if (tag.Contains("[MaxSpeed:") == true) {

						this.MaxSpeed = TagHelper.TagDoubleCheck(tag, this.MaxSpeed);

					}

					//MinTargetValue
					if (tag.Contains("[MinTargetValue:") == true) {

						this.MinTargetValue = TagHelper.TagFloatCheck(tag, this.MinTargetValue);

					}

					//MaxTargetValue
					if (tag.Contains("[MaxTargetValue:") == true) {

						this.MaxTargetValue = TagHelper.TagFloatCheck(tag, this.MaxTargetValue);

					}


				}

			}

		}

	}
}
