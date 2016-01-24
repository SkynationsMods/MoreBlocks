using SharedGameData;
using SNScript;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SNScriptUtils;
using PreciseMaths;

namespace MoreBlocksScripts
{
    class TeleportActivator : ISpecialBlock
    {
        public IChunk Chunk;
        public Point3D Position;
        private Boolean debug = false;

        public TeleportActivator(IChunk chunk, Point3D position) //Constructor
        {
            this.Chunk = chunk;
            this.Position = position;
        }

        public void Use(IActor actor)
        {
            //set up needed Variables
            IGameServer Server = actor.State as IGameServer;
            IBiomeManager biomeManager = Server.Biomes;
            Dictionary<uint, IBiomeSystem> SystemsCollection = biomeManager.GetSystems();
            uint currentSystemID = actor.InstanceID;
            IBiomeSystem currentSystem;
            SystemsCollection.TryGetValue(currentSystemID, out currentSystem);

            //Permission Check, is the Player allowed to use the Teleporter?
            if (currentSystem.Nation != string.Empty && currentSystem.Nation != actor.Nation)
            {
                Server.ChatManager.SendActorMessage("You must be in the Nation that claimed this System to use the Teleporter.", actor);
                return;
            }

            //Set up ChunkDictionary so we can access the Chunks in System (We need the Chunk to Teleport the player to later)
            Dictionary<Point3D, IChunk> ChunkDictionary = SNScriptUtils._Utils.CreateChunkDictionary(currentSystem);

            //create offset List (the 4 Positions around our Activator)
            List<Point3D> offsetList = new List<Point3D>();
            offsetList.Add(new Point3D(0, -1, -1)); offsetList.Add(new Point3D(0, -1, 1));
            offsetList.Add(new Point3D(1, -1, 0)); offsetList.Add(new Point3D(-1, -1, 0));

            //if (debug) { Console.WriteLine("Past OffsetList!"); };
            List<Object[,]> SpecialBlockList = null;
            Teleporter nearbyTeleporter = new Object() as Teleporter;
            //Get List of Teleporters in vicinity to the Activator based on offsetList (expected Teleporter locations)
            if (SNScriptUtils._Utils.FindCustomSpecialBlocksAround(this.Position, this.Chunk, offsetList, (uint)7100, ChunkDictionary, out SpecialBlockList))
            {
                //if (debug) { Console.WriteLine("Found some Teleporters around, now assigning the first one found"); };
                //just take the first one found for now
                IChunk tmpChunk = SpecialBlockList[0][0, 1] as IChunk;
                Point3D TargetTeleporterLocalPos = (Point3D)SpecialBlockList[0][0, 0];
                ISpecialBlock targetSpecialBlock = tmpChunk.GetSpecialBlocks().FirstOrDefault(item => (item is Teleporter) && ((item as Teleporter).Position == TargetTeleporterLocalPos));
                nearbyTeleporter = targetSpecialBlock as Teleporter;
            }
            else
            {
                Server.ChatManager.SendActorMessage("There is no Teleporter around the Activator!.", actor);
                return;
            }
            //if (debug) { Console.WriteLine("Past Finding the Nearby Teleporter! @ " + nearbyTeleporter.Position.ToString()); };
            //Check if Player is standing on the Teleporter right now
            //WIP

            //Get TargetTeleporterPos
            Point3D TargetTeleporterFakeGlobalPos = nearbyTeleporter.GetTargetTeleporter();

            //Check if TargetTeleporter exists
            //Get Chunk where the TargetTeleporter is in
            IChunk targetChunk = new Object() as IChunk;
            if (SNScriptUtils._Utils.getChunkObjFromFakeGlobalPos(TargetTeleporterFakeGlobalPos, ChunkDictionary, out targetChunk))
            {
                Point3D TargetTeleporterLocalPos = new Point3D(
                TargetTeleporterFakeGlobalPos.X - (int)targetChunk.Position.X,
                TargetTeleporterFakeGlobalPos.Y - (int)targetChunk.Position.Y,
                TargetTeleporterFakeGlobalPos.Z - (int)targetChunk.Position.Z
                );
                ushort targetBlockID = new ushort();
                targetBlockID = targetChunk.Blocks[targetChunk.GetBlockIndex(TargetTeleporterLocalPos.X, TargetTeleporterLocalPos.Y, TargetTeleporterLocalPos.Z)];
                if ((targetBlockID) == (ushort)7100) //TargetTeleporter still exists!
                {
                    ISpecialBlock targetSpecialBlock = targetChunk.GetSpecialBlocks().FirstOrDefault(item => (item is Teleporter) && ((item as Teleporter).Position == TargetTeleporterLocalPos));
                    Teleporter targetTeleporter = targetSpecialBlock as Teleporter;
                    //Teleport Player to TargetTeleporterPos + Offset (2.1 Blocks Up (Y)) or he'd get stuck in the Teleporter
                    double playerTeleportLocalPosY = (double)targetTeleporter.Position.Y + 2.1;
                    DoubleVector3 playerTeleportLocalPos = new DoubleVector3((double)targetTeleporter.Position.X, playerTeleportLocalPosY, (double)targetTeleporter.Position.Z);
                    actor.SetLocalPosition(targetChunk.ID, targetChunk.World, playerTeleportLocalPos);
                }
                else
                {
                    if (debug) { Console.WriteLine("TargetTeleporter does not exist anymore, other block ID found: " + ((int)targetBlockID).ToString()); };
                    if (debug) { Console.WriteLine("I've been looking at Position: " + TargetTeleporterLocalPos.ToString()); };
                    //Vector3 TargetTeleporterLocalPosV3 = new Vector3((float)TargetTeleporterLocalPos.X, (float)TargetTeleporterLocalPos.Y, (float)TargetTeleporterLocalPos.Z) 
                    //Vector3 recGlobPos = Vector3.Zero;
                    //Vector3.Transform(  TargetTeleporterLocalPosV3, targetChunk.World, recGlobPos );
                    Server.ChatManager.SendActorMessage("The Teleporter you wanted to teleport to, does not exist anymore!", actor);
                    nearbyTeleporter.DeleteTargetTeleporter();
                    return;
                }
            }
            else
            {
                //if (debug) { Console.WriteLine("Chunk does not exist! Pos: " + TargetTeleporterFakeGlobalPos.ToString()); };
                Server.ChatManager.SendActorMessage("The Teleporter you wanted to teleport to, does not exist anymore!", actor);
                nearbyTeleporter.DeleteTargetTeleporter();
                return;
            }
        }

        public void Deserialize(BinaryReader reader)
        {
            // Read data about the block you wrote when serializing. Must read all data exactly how it was written.
        }

        public void Serialize(BinaryWriter writer)
        {
            // Write save data about the block when saving it.
        }

        public void Update()
        {
            // Update called every server frame.

            //TODO: rate limiting!
        }

        public void Broken()
        {
            // Block broken by mining, explosion etc.
        }

        public void Destroy()
        {
            // Block destroyed(this includes being unloaded, which is not the same as Broken()).
        }

    }
}