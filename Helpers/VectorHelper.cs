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

namespace RivalAI.Helpers{

    public static class VectorHelper {

        public static Random Rnd = new Random();

        //ClosestDirection
        public static Vector3D ClosestDirection(MatrixD matrix, Vector3D checkCoords, Vector3D ignoreDirectionA, Vector3D ignoreDirectionB) {

            Vector3D closestVector = Vector3D.Zero;
            double closestDistance = 0;

            //Forward
            if(matrix.Forward != ignoreDirectionA && matrix.Forward != ignoreDirectionB) {

                var vectorPos = matrix.Translation + matrix.Forward;
                var distance = Vector3D.Distance(vectorPos, checkCoords);
                bool gotCloser = false;

                if(closestVector == Vector3D.Zero) {

                    closestDistance = distance;
                    closestVector = vectorPos;
                    gotCloser = true;

                }

                if(gotCloser == false && distance < closestDistance) {

                    closestDistance = distance;
                    closestVector = vectorPos;

                }

            }

            //Backward
            if(matrix.Backward != ignoreDirectionA && matrix.Backward != ignoreDirectionB) {

                var vectorPos = matrix.Translation + matrix.Backward;
                var distance = Vector3D.Distance(vectorPos, checkCoords);
                bool gotCloser = false;

                if(closestVector == Vector3D.Zero) {

                    closestDistance = distance;
                    closestVector = vectorPos;
                    gotCloser = true;

                }

                if(gotCloser == false && distance < closestDistance) {

                    closestDistance = distance;
                    closestVector = vectorPos;

                }

            }

            //Up
            if(matrix.Up != ignoreDirectionA && matrix.Up != ignoreDirectionB) {

                var vectorPos = matrix.Translation + matrix.Up;
                var distance = Vector3D.Distance(vectorPos, checkCoords);
                bool gotCloser = false;

                if(closestVector == Vector3D.Zero) {

                    closestDistance = distance;
                    closestVector = vectorPos;
                    gotCloser = true;

                }

                if(gotCloser == false && distance < closestDistance) {

                    closestDistance = distance;
                    closestVector = vectorPos;

                }

            }

            //Down
            if(matrix.Down != ignoreDirectionA && matrix.Down != ignoreDirectionB) {

                var vectorPos = matrix.Translation + matrix.Down;
                var distance = Vector3D.Distance(vectorPos, checkCoords);
                bool gotCloser = false;

                if(closestVector == Vector3D.Zero) {

                    closestDistance = distance;
                    closestVector = vectorPos;
                    gotCloser = true;

                }

                if(gotCloser == false && distance < closestDistance) {

                    closestDistance = distance;
                    closestVector = vectorPos;

                }

            }

            //Left
            if(matrix.Left != ignoreDirectionA && matrix.Left != ignoreDirectionB) {

                var vectorPos = matrix.Translation + matrix.Left;
                var distance = Vector3D.Distance(vectorPos, checkCoords);
                bool gotCloser = false;

                if(closestVector == Vector3D.Zero) {

                    closestDistance = distance;
                    closestVector = vectorPos;
                    gotCloser = true;

                }

                if(gotCloser == false && distance < closestDistance) {

                    closestDistance = distance;
                    closestVector = vectorPos;

                }

            }

            //Right
            if(matrix.Right != ignoreDirectionA && matrix.Right != ignoreDirectionB) {

                var vectorPos = matrix.Translation + matrix.Right;
                var distance = Vector3D.Distance(vectorPos, checkCoords);
                bool gotCloser = false;

                if(closestVector == Vector3D.Zero) {

                    closestDistance = distance;
                    closestVector = vectorPos;
                    gotCloser = true;

                }

                if(gotCloser == false && distance < closestDistance) {

                    closestDistance = distance;
                    closestVector = vectorPos;

                }

            }

            return closestVector;

        }

        public static double CompareDistanceAtSealevel(MyPlanet planet, Vector3D pointA, Vector3D pointB) {

            if(planet == null) {

                return -1;

            }

            var seaLevelA = GetPlanetSealevelAtPosition(pointA, planet);
            var seaLevelB = GetPlanetSealevelAtPosition(pointB, planet);
            return Vector3D.Distance(seaLevelA, seaLevelB);

        }

        //CreateDirectionAndTarget
        public static Vector3D CreateDirectionAndTarget(Vector3D startDir, Vector3D endDir, Vector3D startCoords, double distance) {

            var direction = Vector3D.Normalize(endDir - startDir);
            return direction * distance + startCoords;

        }

        //GetAngleBetweenDirections
        public static double GetAngleBetweenDirections(Vector3D dirA, Vector3D dirB) {

            var radians = MyUtils.GetAngleBetweenVectors(dirA, dirB);
            return (180 / Math.PI) * radians;

        }
		
		//GetDirectionAwayFromTarget
		public static Vector3D GetDirectionAwayFromTarget(Vector3D myPosition, Vector3D targetPosition){
			
			var planet = MyGamePruningStructure.GetClosestPlanet(myPosition);

            if(planet == null) {

                return Vector3D.Normalize(myPosition - targetPosition);

            }else{
				
				var mySealevel = GetPlanetSealevelAtPosition(myPosition, planet);
				var targetSealevel = GetPlanetSealevelAtPosition(targetPosition, planet);
				return Vector3D.Normalize(mySealevel - targetSealevel);
				
			}
			
		}

        //GetThrustDirectionsAwayFromPosition
        public static Vector3I GetThrustDirectionsAwayFromPosition(MatrixD myMatrix, Vector3D targetPosition) {

            var dist = Vector3D.Distance(myMatrix.Translation, targetPosition) / 2;
            Vector3I directions = new Vector3I(1, 1, 1);

            //X
            var leftDist = Vector3D.Distance(targetPosition, myMatrix.Translation + myMatrix.Left);
            var rightDist = Vector3D.Distance(targetPosition, myMatrix.Translation + myMatrix.Right);

            if(rightDist < leftDist) {

                directions.X = -1;

            }

            //Y
            var upDist = Vector3D.Distance(targetPosition, myMatrix.Translation + myMatrix.Up);
            var downDist = Vector3D.Distance(targetPosition, myMatrix.Translation + myMatrix.Down);

            if(upDist < downDist) {

                directions.Y = -1;

            }

            //Z
            var forwardDist = Vector3D.Distance(targetPosition, myMatrix.Translation + myMatrix.Up);
            var backDist = Vector3D.Distance(targetPosition, myMatrix.Translation + myMatrix.Down);

            if(forwardDist < backDist) {

                directions.Z = -1;

            }

            return directions;


        }

        public static Vector3I GetThrustDirectionsAwayFromSurface(MatrixD myMatrix, Vector3D upDirection, Vector3I oldDirections) {

            Vector3I directions = new Vector3I(0, 0, 0);
            Vector3I newDirections = oldDirections;
            double closestAngle = 180;

            if(GetAngleBetweenDirections(-upDirection, myMatrix.Forward) < closestAngle) {

                closestAngle = GetAngleBetweenDirections(-upDirection, myMatrix.Forward);
                directions = new Vector3I(0, 0, -1);

            }

            if(GetAngleBetweenDirections(-upDirection, myMatrix.Backward) < closestAngle) {

                closestAngle = GetAngleBetweenDirections(-upDirection, myMatrix.Backward);
                directions = new Vector3I(0, 0, 1);

            }

            if(GetAngleBetweenDirections(-upDirection, myMatrix.Up) < closestAngle) {

                closestAngle = GetAngleBetweenDirections(-upDirection, myMatrix.Up);
                directions = new Vector3I(0, -1, 0);

            }

            if(GetAngleBetweenDirections(-upDirection, myMatrix.Down) < closestAngle) {

                closestAngle = GetAngleBetweenDirections(-upDirection, myMatrix.Down);
                directions = new Vector3I(0, 1, 0);

            }

            if(GetAngleBetweenDirections(-upDirection, myMatrix.Right) < closestAngle) {

                closestAngle = GetAngleBetweenDirections(-upDirection, myMatrix.Right);
                directions = new Vector3I(-1, 0, 0);

            }

            if(GetAngleBetweenDirections(-upDirection, myMatrix.Left) < closestAngle) {

                closestAngle = GetAngleBetweenDirections(-upDirection, myMatrix.Left);
                directions = new Vector3I(1, 0, 0);

            }

            if(directions.X != 0) {

                newDirections.X = directions.X;

            }

            if(directions.Y != 0) {

                newDirections.Y = directions.Y;

            }

            if(directions.Z != 0) {

                newDirections.Z = directions.Z;

            }

            return newDirections;


        }

        //GetPlanetSealevelAtPosition
        public static Vector3D GetPlanetSealevelAtPosition(Vector3D coords, MyPlanet planet = null) {

            if(planet == null) {

                return Vector3D.Zero;

            }

            var planetEntity = planet as IMyEntity;
            var direction = Vector3D.Normalize(coords - planetEntity.GetPosition());
            return direction * (double)planet.MinimumRadius + planetEntity.GetPosition();

        }

        public static Vector3D GetPositionCenter(IMyEntity entity) {

            if(MyAPIGateway.Entities.Exist(entity) == false) {

                return Vector3D.Zero;

            }

            if(entity?.PositionComp == null) {

                return Vector3D.Zero;

            }

            return entity.PositionComp.WorldAABB.Center;

        }

        //GetPlanetSurfaceCoordsAtPosition
        public static Vector3D GetPlanetSurfaceCoordsAtPosition(Vector3D coords, MyPlanet planet = null) {

            if(planet == null) {

                return Vector3D.Zero;

            }

            var checkCoords = coords;

            return planet.GetClosestSurfacePointGlobal(ref checkCoords);

        }

        public static Vector3D GetPlanetSurfaceCoordsAtPosition(Vector3D coords) {

            var planet = MyGamePruningStructure.GetClosestPlanet(coords);

            if(planet == null)
                return Vector3D.Zero;

            return GetPlanetSurfaceCoordsAtPosition(coords, planet);

        }

        //GetPlanetSurfaceDifference
        public static double GetPlanetSurfaceDifference(Vector3D myCoords, Vector3D testCoords, MyPlanet planet = null) {

            if(planet == null) {

                return 0;

            }

            var testSurfaceCoords = GetPlanetSurfaceCoordsAtPosition(testCoords, planet);
            var mySealevelCoords = GetPlanetSealevelAtPosition(myCoords, planet);
            var testSealevelCoords = GetPlanetSealevelAtPosition(testSurfaceCoords, planet);
            var myDistance = Vector3D.Distance(mySealevelCoords, myCoords);
            var testDistance = Vector3D.Distance(testSealevelCoords, testSurfaceCoords);
            return myDistance - testDistance;

        }

        public static Vector3D GetPlanetUpDirection(Vector3D position) {

            var planet = MyGamePruningStructure.GetClosestPlanet(position);

            if(planet == null) {

                return Vector3D.Zero;

            }

            var planetEntity = planet as IMyEntity;
            var gravityProvider = planetEntity.Components.Get<MyGravityProviderComponent>();

            if(gravityProvider.IsPositionInRange(position) == true) {

                return Vector3D.Normalize(position - planetEntity.GetPosition());

            }

            return Vector3D.Zero;

        }
		
		/*
		The intent of this method is to calculate whether or not the approach to a target
		on a planet is safe, and to return coordinates that properly avoid terrain obstacles.
		
		The calculation starts by checking a path of waypoints in 50m steps toward the 
		target, upto maxDistanceToCheck value or the target if it is closer than that
		value.
		
		At each 50m step on the calculated path, the distance from the surface at that step
		to the planet core is measured. If at any point a path core distance is a higher
		value than the NPC distance to the core, it means there is a terrain obstacle detected.
		
		If a terrain obstacle is detected, a waypoint is created between the NPC and the first-order
		set of path coordinates that was considered higher terrain than the NPC. From that waypoint,
		a new waypoint is returned in the sky at the altitude of the highest terrain + minAltitude value
		
		
		*/

        public static Vector3D GetPlanetWaypointPathing(Vector3D myCoords, Vector3D targetCoords, double minAltitude = 200, double maxDistanceToCheck = 1000, bool returnOriginalTarget = false) {
			
			double minAltitudeRelaxation = 0.75;
            var planet = MyGamePruningStructure.GetClosestPlanet(targetCoords);

            if(planet == null) {

                return targetCoords;

            }

            var planetCoords = planet.PositionComp.WorldAABB.Center; 
            var targetUp = Vector3D.Normalize(targetCoords - planetCoords);
            
            var dirToTarget = Vector3D.Normalize(targetCoords - myCoords);
            var distToTarget = Vector3D.Distance(targetCoords, myCoords);
            double distanceToUse = distToTarget;

            if(distToTarget > maxDistanceToCheck) {

                distanceToUse = maxDistanceToCheck;

            }

            List<Vector3D> pathSteps = new List<Vector3D>();
            double currentPathDistance = 0;

            while(currentPathDistance < distanceToUse) {

                if((distanceToUse - currentPathDistance) < 50) {

                    currentPathDistance = distanceToUse;

                } else {

                    currentPathDistance += 50;

                }

                pathSteps.Add(dirToTarget * currentPathDistance + myCoords);

            }

            var myDistToCore = Vector3D.Distance(myCoords, planetCoords);
			var targetDistToCore = Vector3D.Distance(targetCoords, planetCoords);
			
            double currentHighestDistance = myDistToCore;
            double currentHighestTerrain = 0;
			Vector3D highestTerrainPoint = Vector3D.Zero;
            Vector3D closestHigherNPCTerrain = Vector3D.Zero;

            foreach(var pathPoint in pathSteps) {

                Vector3D pathPointRef = pathPoint;
                Vector3D surfacePoint = planet.GetClosestSurfacePointGlobal(ref pathPointRef);
                double surfacePointToCore = Vector3D.Distance(surfacePoint, planetCoords);

                if(currentHighestTerrain < surfacePointToCore) {

                    currentHighestTerrain = surfacePointToCore;
					highestTerrainPoint = surfacePoint;
					
                }

                if(currentHighestDistance < surfacePointToCore) {

                    currentHighestDistance = surfacePointToCore;
					
					if(closestHigherNPCTerrain == Vector3D.Zero){
						
						closestHigherNPCTerrain = surfacePoint;
						
					}
 
                }

            }
			
			//If Highest Terrain + minAltitude Is Higher Than NPC
            if(currentHighestTerrain + (minAltitude * minAltitudeRelaxation) > myDistToCore) {
				
				if(closestHigherNPCTerrain == Vector3D.Zero){
					
					closestHigherNPCTerrain = highestTerrainPoint;
					
				}

                Logger.AddMsg(string.Format("Higher Terrain Near NPC"), true);
                var forwardStep = dirToTarget * 50 + myCoords;
                return Vector3D.Normalize(forwardStep - planetCoords) * (currentHighestDistance + minAltitude) + planetCoords;

            }
			
			//If NPC is Higher Than Highest Detected Terrain, but Target is Not
			if((currentHighestTerrain - targetDistToCore) > minAltitude) {

                Logger.AddMsg(string.Format("Higher Terrain Near Target"), true);
                return targetUp * (currentHighestTerrain + minAltitude) + planetCoords;

			}
			
			//No Terrain Obstacle Between Target and NPC.
			if(returnOriginalTarget == false) {

				return targetUp * minAltitude + targetCoords;

			} else {

				return targetCoords;

			}

        }

        public static Dictionary<string, Vector3D> GetTransformedGyroRotations(MatrixD refBlock, MatrixD gyro){

            //Get Reference Rotation Directions
            var refPitchDirections = new Dictionary<Vector3D, Vector3D>();
            var refYawDirections = new Dictionary<Vector3D, Vector3D>();
            var refRollDirections = new Dictionary<Vector3D, Vector3D>();

            refPitchDirections.Add(refBlock.Forward, refBlock.Up);
            refPitchDirections.Add(refBlock.Up, refBlock.Backward);
            refPitchDirections.Add(refBlock.Backward, refBlock.Down);
            refPitchDirections.Add(refBlock.Down, refBlock.Forward);
            var refPitchDirectionsList = refPitchDirections.Keys.ToList();

            refYawDirections.Add(refBlock.Forward, refBlock.Right);
            refYawDirections.Add(refBlock.Right, refBlock.Backward);
            refYawDirections.Add(refBlock.Backward, refBlock.Left);
            refYawDirections.Add(refBlock.Left, refBlock.Forward);
            var refYawDirectionsList = refYawDirections.Keys.ToList();

            refRollDirections.Add(refBlock.Up, refBlock.Right);
            refRollDirections.Add(refBlock.Right, refBlock.Down);
            refRollDirections.Add(refBlock.Down, refBlock.Left);
            refRollDirections.Add(refBlock.Left, refBlock.Up);
            var refRollDirectionsList = refRollDirections.Keys.ToList();

            //Gyro Rotation Directions
            var gyroPitchDirections = new Dictionary<Vector3D, Vector3D>();
            var gyroYawDirections = new Dictionary<Vector3D, Vector3D>();
            var gyroRollDirections = new Dictionary<Vector3D, Vector3D>();

            gyroPitchDirections.Add(gyro.Forward, gyro.Up);
            gyroPitchDirections.Add(gyro.Up, gyro.Backward);
            gyroPitchDirections.Add(gyro.Backward, gyro.Down);
            gyroPitchDirections.Add(gyro.Down, gyro.Forward);

            gyroYawDirections.Add(gyro.Forward, gyro.Right);
            gyroYawDirections.Add(gyro.Right, gyro.Backward);
            gyroYawDirections.Add(gyro.Backward, gyro.Left);
            gyroYawDirections.Add(gyro.Left, gyro.Forward);

            gyroRollDirections.Add(gyro.Up, gyro.Right);
            gyroRollDirections.Add(gyro.Right, gyro.Down);
            gyroRollDirections.Add(gyro.Down, gyro.Left);
            gyroRollDirections.Add(gyro.Left, gyro.Up);

            var localPitchDirections = new Dictionary<Vector3D, Vector3D>();
            var localYawDirections = new Dictionary<Vector3D, Vector3D>();
            var localRollDirections = new Dictionary<Vector3D, Vector3D>();
            var result = new Dictionary<string, Vector3D>();

            //Calculate Pitch Axis
            while(true) {

                var checkPitchPitch = refPitchDirectionsList.Except(gyroPitchDirections.Keys.ToList()).ToList();
                if(checkPitchPitch.Count == 0) {

                    //Logger.AddMsg("PitchPitch", true);
                    localPitchDirections = gyroPitchDirections;
                    var sign = GetSignForRotationDirection(refPitchDirections, gyroPitchDirections, refBlock.Forward);
                    result.Add("Pitch", new Vector3D(-sign, 0, 0));
                    break;

                }

                var checkPitchYaw = refPitchDirectionsList.Except(gyroYawDirections.Keys.ToList()).ToList();
                if(checkPitchYaw.Count == 0) {

                    //Logger.AddMsg("PitchYaw", true);
                    localPitchDirections = gyroYawDirections;
                    var sign = GetSignForRotationDirection(refPitchDirections, gyroYawDirections, refBlock.Forward);
                    result.Add("Pitch", new Vector3D(0, sign, 0));
                    break;

                }

                var checkPitchRoll = refPitchDirectionsList.Except(gyroRollDirections.Keys.ToList()).ToList();
                if(checkPitchRoll.Count == 0) {

                    //Logger.AddMsg("PitchRoll", true);
                    localPitchDirections = gyroRollDirections;
                    var sign = GetSignForRotationDirection(refPitchDirections, gyroRollDirections, refBlock.Forward);
                    result.Add("Pitch", new Vector3D(0, 0, sign));
                    break;

                }

                break;

            }


            //Calculate Yaw Axis
            while(true) {

                var checkYawPitch = refYawDirectionsList.Except(gyroPitchDirections.Keys.ToList()).ToList();
                if(checkYawPitch.Count == 0) {

                    //Logger.AddMsg("YawPitch", true);
                    localYawDirections = gyroPitchDirections;
                    var sign = GetSignForRotationDirection(refYawDirections, gyroPitchDirections, refBlock.Forward);
                    result.Add("Yaw", new Vector3D(-sign, 0, 0));
                    break;

                }

                var checkYawYaw = refYawDirectionsList.Except(gyroYawDirections.Keys.ToList()).ToList();
                if(checkYawYaw.Count == 0) {

                    //Logger.AddMsg("YawYaw", true);
                    localYawDirections = gyroYawDirections;
                    var sign = GetSignForRotationDirection(refYawDirections, gyroYawDirections, refBlock.Forward);
                    result.Add("Yaw", new Vector3D(0, sign, 0));
                    break;

                }

                var checkYawRoll = refYawDirectionsList.Except(gyroRollDirections.Keys.ToList()).ToList();
                if(checkYawRoll.Count == 0) {

                    //Logger.AddMsg("YawRoll", true);
                    localYawDirections = gyroRollDirections;
                    var sign = GetSignForRotationDirection(refYawDirections, gyroRollDirections, refBlock.Forward); 
                    result.Add("Yaw", new Vector3D(0, 0, sign));
                    break;

                }

                break;

            }

            //Calculate Roll Axis
            while(true) {

                var checkRollPitch = refRollDirectionsList.Except(gyroPitchDirections.Keys.ToList()).ToList();
                if(checkRollPitch.Count == 0) {

                    //Logger.AddMsg("RollPitch", true);
                    localRollDirections = gyroPitchDirections;
                    var sign = GetSignForRotationDirection(refRollDirections, gyroPitchDirections, refBlock.Up);
                    result.Add("Roll", new Vector3D(-sign, 0, 0));
                    break;

                }

                var checkRollYaw = refRollDirectionsList.Except(gyroYawDirections.Keys.ToList()).ToList();
                if(checkRollYaw.Count == 0) {

                    //Logger.AddMsg("RollYaw", true);
                    localRollDirections = gyroYawDirections;
                    var sign = GetSignForRotationDirection(refRollDirections, gyroYawDirections, refBlock.Up);
                    result.Add("Roll", new Vector3D(0, sign, 0));
                    break;

                }

                var checkRollRoll = refRollDirectionsList.Except(gyroRollDirections.Keys.ToList()).ToList();
                if(checkRollRoll.Count == 0) {

                    //Logger.AddMsg("RollRoll", true);
                    localRollDirections = gyroRollDirections;
                    var sign = GetSignForRotationDirection(refRollDirections, gyroRollDirections, refBlock.Up);
                    result.Add("Roll", new Vector3D(0, 0, sign));
                    break;

                }

                break;

            }

            Logger.AddMsg("End Getting Rotations");

            return result;

        }

        public static double GetSignForRotationDirection(Dictionary<Vector3D, Vector3D> refDict, Dictionary<Vector3D, Vector3D> gyroDict, Vector3D dir) {

            try {

                if(refDict[dir] == gyroDict[dir]) {

                    return 1; //

                }

                return -1;

            } catch(Exception e) {

                Logger.AddMsg("Caught Exception: ", true);
                Logger.AddMsg(e.ToString(), true);

            }

            return 0;
            

        }

        //IsPositionUnderground
        public static bool IsPositionUnderground(Vector3D coords, MyPlanet planet){
			
			if(planet == null){
				
				return false;
				
			}
			
			var closestSurfacePoint = planet.GetClosestSurfacePointGlobal(coords);
			var planetEntity = planet as IMyEntity;
			
			if(Vector3D.Distance(planetEntity.GetPosition(), coords) < Vector3D.Distance(planetEntity.GetPosition(), closestSurfacePoint)){
				
				return true;
				
			}
			
			return false;
			
		}

        public static double RandomDistance(double a, double b) {

            return Rnd.Next((int)a, (int)b);

        }
		
		//RandomDirection
		public static Vector3D RandomDirection(){
			
			return Vector3D.Normalize(MyUtils.GetRandomVector3D());
			
		}
		
		public static Vector3D RandomBaseDirection(MatrixD matrix, bool ignoreForward = false, bool ignoreBackward = false, bool ignoreUp = false, bool ignoreDown = false, bool ignoreLeft = false, bool ignoreRight = false){
			
			var directionList = new List<Vector3D>();
			
			if(ignoreForward == false){
				
				directionList.Add(matrix.Forward);
				
			}
			
			if(ignoreBackward == false){
				
				directionList.Add(matrix.Backward);
				
			}
			
			if(ignoreUp == false){
				
				directionList.Add(matrix.Up);
				
			}
			
			if(ignoreDown == false){
				
				directionList.Add(matrix.Down);
				
			}
			
			if(ignoreLeft == false){
				
				directionList.Add(matrix.Left);
				
			}
			
			if(ignoreRight == false){
				
				directionList.Add(matrix.Right);
				
			}
			
			return directionList[Rnd.Next(0, directionList.Count)];
			
		}
		
		//RandomPerpendicular
		public static Vector3D RandomPerpendicular(Vector3D referenceDir){
			
			var refDir = Vector3D.Normalize(referenceDir);
			return Vector3D.Normalize(MyUtils.GetRandomPerpendicularVector(ref refDir));
			
		}


        //License Details For FirstOrderIntercept and FirstOrderInterceptTime

        /*The MIT License (MIT)

        Copyright (c) 2008 Daniel Brauer

        Permission is hereby granted, free of charge, to any person obtaining a copy 
        of this software and associated documentation files (the "Software"), to deal 
        in the Software without restriction, including without limitation the rights 
        to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
        copies of the Software, and to permit persons to whom the Software is furnished 
        to do so, subject to the following conditions:

        The above copyright notice and this permission notice shall be included in all 
        copies or substantial portions of the Software.

        THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
        IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
        FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
        AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
        WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN 
        CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. */

        public static Vector3 FirstOrderIntercept(Vector3 shooterPosition, Vector3 shooterVelocity, float shotSpeed, Vector3 targetPosition, Vector3 targetVelocity) {

            Vector3 targetRelativePosition = targetPosition - shooterPosition;
            Vector3 targetRelativeVelocity = targetVelocity - shooterVelocity;
            float t = FirstOrderInterceptTime(shotSpeed, targetRelativePosition, targetRelativeVelocity);
            return targetPosition + t * (targetRelativeVelocity);

        }

        //first-order intercept using relative target position
        public static float FirstOrderInterceptTime(float shotSpeed, Vector3 targetRelativePosition, Vector3 targetRelativeVelocity) {

            float velocitySquared = (float)Math.Pow(targetRelativePosition.Length(), 2);

            if(velocitySquared < 0.001f) {

                return 0f;

            }

            float a = velocitySquared - shotSpeed * shotSpeed;

            //handle similar velocities
            if((float)Math.Abs(a) < 0.001f) {

                float t = -(float)Math.Pow(targetRelativePosition.Length(), 2) / (2f * Vector3.Dot(targetRelativeVelocity, targetRelativePosition));
                return (float)Math.Max(t, 0f); //don't shoot back in time

            }

            float b = 2f * Vector3.Dot(targetRelativeVelocity, targetRelativePosition);
            float c = (float)Math.Pow(targetRelativePosition.Length(), 2);
            float determinant = b * b - 4f * a * c;

            if(determinant > 0f) { //determinant > 0; two intercept paths (most common)

                float t1 = (-b + (float)Math.Sqrt(determinant)) / (2f * a);
                float t2 = (-b - (float)Math.Sqrt(determinant)) / (2f * a);

                if(t1 > 0f) {

                    if(t2 > 0f) {

                        return (float)Math.Min(t1, t2); //both are positive

                    } else {

                        return t1; //only t1 is positive

                    }

                } else {

                    return (float)Math.Max(t2, 0f); //don't shoot back in time

                }

            } else if(determinant < 0f) {

                return 0f; //determinant < 0; no intercept path

            } else { //determinant = 0; one intercept path, pretty much never happens

                return (float)Math.Max(-b / (2f * a), 0f); //don't shoot back in time

            }

        }

    }
	
}
