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
            IBiomeSystem currentSystem = ((IGameServer)actor.State).Biomes.GetSystems()[actor.InstanceID];

            //Permission Check, is the Player allowed to use the Teleporter?
            if (currentSystem.Nation != string.Empty && currentSystem.Nation != actor.Nation)
            {
                Server.ChatManager.SendActorMessage("You must be in the Nation that claimed this System to use the Teleporter.", actor);
                return;
            }

            //Set up ChunkDictionary so we can access the Chunks in System (We need the Chunk to Teleport the player to later)
            Dictionary<Point3D, IChunk> ChunkDictionary = SNScriptUtils._Utils.CreateChunkDictionary(currentSystem);

            //create offset List (the 4 Positions around our Activator where we expect at least one Teleporter)
            List<Point3D> offsetList = new List<Point3D>();
            offsetList.Add(new Point3D(0, -1, -1)); offsetList.Add(new Point3D(0, -1, 1));offsetList.Add(new Point3D(1, -1, 0)); offsetList.Add(new Point3D(-1, -1, 0));

            List<Object[,]> SpecialBlockList = null;
            Teleporter nearbyTeleporter = new Object() as Teleporter;

            //Get List of Teleporters in vicinity to the Activator based on offsetList (expected Teleporter locations)
            if (SNScriptUtils._Utils.FindCustomSpecialBlocksAround(this.Position, this.Chunk, offsetList, (uint)7100, ChunkDictionary, out SpecialBlockList))
            {
                //just take the first one found for now
                IChunk tmpChunk = SpecialBlockList[0][0, 1] as IChunk;
                Point3D TeleporterLocalPos = (Point3D)SpecialBlockList[0][0, 0];
                ISpecialBlock targetSpecialBlock = tmpChunk.GetSpecialBlocks().FirstOrDefault(item => (item is Teleporter) && ((item as Teleporter).Position == TeleporterLocalPos));
                nearbyTeleporter = targetSpecialBlock as Teleporter;
                
            }
            else
            {
                Server.ChatManager.SendActorMessage("There is no Teleporter around the Activator!", actor);
                return;
            }

            //Check if Player is standing on the Teleporter right now
            Point3D teleporterPadFakeGlobalPos = new Point3D(
                nearbyTeleporter.Position.X + (int)((IChunk)SpecialBlockList[0][0, 1]).Position.X, 
                nearbyTeleporter.Position.Y + (int)((IChunk)SpecialBlockList[0][0, 1]).Position.Y + 1,
                nearbyTeleporter.Position.Z + (int)((IChunk)SpecialBlockList[0][0, 1]).Position.Z
                );

            Point3D actorPos = _Utils.GetActorFakeGlobalPos(actor, new Point3D(0, -1, 0));

            if (teleporterPadFakeGlobalPos != actorPos)
            {
                Server.ChatManager.SendActorMessage("You have to stand on the Teleporter platform to use the Teleporter!", actor);
                return;
            }
            
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

                    Server.ChatManager.SendActorMessage("The Teleporter you wanted to teleport to, does not exist anymore!", actor);
                    nearbyTeleporter.DeleteTargetTeleporter();
                    return;
                }
            }
            else
            {
                if (debug) { Console.WriteLine("Chunk does not exist! Pos: " + TargetTeleporterFakeGlobalPos.ToString()); };
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