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
using RivalAI.Behavior.Subsystems;
using RivalAI.Helpers;


namespace RivalAI.Behavior.Subsystems{
	
	public class OwnerSystem{
		
		public IMyRemoteControl RemoteControl;
		public string RequiredFactionTag;
		public bool NpcOwned;
        public bool AllowHumansInFaction;
		
		public bool UseGridReclamation;
		public double SecondsBetweenAttempts;
		public int ReclamationTimer;
		public int ReclamationTimerTrigger;
		
		public Random Rnd;
		
		public OwnerSystem(IMyRemoteControl remoteControl = null) {
			
			RemoteControl = null;
			RequiredFactionTag = "";
			NpcOwned = false;
            AllowHumansInFaction = false;

            UseGridReclamation = false;
			SecondsBetweenAttempts = 60;
			ReclamationTimer = 0;
			
			Rnd = new Random();

            Setup(remoteControl);


        }
		
		private void Setup(IMyRemoteControl remoteControl){
			
			if(remoteControl == null){
				
				Logger.AddMsg("OwnerSystem Could Not Init. RemoteControl null", true);
				return;
				
			}
			
			remoteControl.OwnershipChanged += CheckIfNpcOwned;
			this.RemoteControl = remoteControl;
			CheckIfNpcOwned(remoteControl);
			
		}
		
		public void ChangeRequiredFaction(string newFaction){
			
			this.RequiredFactionTag = newFaction;
			CheckIfNpcOwned(this.RemoteControl);
			
		}
		
		public void CheckIfNpcOwned(IMyTerminalBlock block){

			var faction = MyAPIGateway.Session.Factions.TryGetPlayerFaction(block.OwnerId);
			
			if(faction != null){
				
				if(faction.Tag != this.RequiredFactionTag && string.IsNullOrEmpty(this.RequiredFactionTag) == false){
					
					this.NpcOwned = false;
                    Logger.AddMsg("Owner Check: Incorrect Faction Tag", true);
					return;
					
				}

                if(this.AllowHumansInFaction == false) {

                    if(faction.IsEveryoneNpc() == true) {

                        this.NpcOwned = true;
                        Logger.AddMsg("Owner Check: Valid NPC Faction", true);
                        var npcSteam = MyAPIGateway.Players.TryGetSteamId(block.OwnerId);

                        if(npcSteam != 0) {

                            Logger.AddMsg("Warning. NPC Identity: " + block.OwnerId.ToString() + " has a SteamId of: " + npcSteam.ToString() + " - Please Alert Mod Author");

                        }

                        return;

                    }

                } else {

                    var npcSteam = MyAPIGateway.Players.TryGetSteamId(block.OwnerId);

                    if(npcSteam == 0) {

                        Logger.AddMsg("Owner Check: Valid NPC Faction", true);
                        return;

                    }

                }

			}

            //TODO: Maybe Update This To Include Factionless NPCs?
            Logger.AddMsg("Owner Check: Not NPC Faction", true);
            this.NpcOwned = false;
			
		}
		
		public void GridReclamation(){
			
			if(this.UseGridReclamation == false){
				
				return;
				
			}
				
			this.ReclamationTimer++;
			
			if(this.ReclamationTimer < this.SecondsBetweenAttempts){
				
				return;
				
			}
			
			this.ReclamationTimer = 0;
			
			if(this.RemoteControl?.SlimBlock?.CubeGrid == null){
				
				return;
				
			}
			
			var blockList = new List<IMySlimBlock>();
			RemoteControl.SlimBlock.CubeGrid.GetBlocks(blockList);
			var unownedBlocks = new List<IMyCubeBlock>();
			
			foreach(var block in blockList){
				
				if(block.FatBlock == null){
					
					continue;
					
				}
				
				if((block.BlockDefinition as MyCubeBlockDefinition).OwnershipIntegrityRatio == 0){
					
					continue;
					
				}
				
				if(block.FatBlock.OwnerId != this.RemoteControl.OwnerId){
					
					unownedBlocks.Add(block.FatBlock);
					
				}
				
			}
			
			if(unownedBlocks.Count > 0){
				
				var randBlock = unownedBlocks[Rnd.Next(0, unownedBlocks.Count)];
				var blockEntity = randBlock as IMyEntity;
				var myCubeBlock = blockEntity as MyCubeBlock;
				myCubeBlock.ChangeBlockOwnerRequest(this.RemoteControl.OwnerId, MyOwnershipShareModeEnum.Faction);
				
			}

		}
		
	}
	
}