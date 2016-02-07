using SharedGameData;
using SNScript;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreciseMaths;

namespace MoreBlocksScripts
{
    class Teleporter : ISpecialBlock
    {
        public IChunk Chunk;
        public Point3D Position;
        private Boolean debug = false;

        private int BlockVersion = 1;
        private Point3D TargetTeleporter = Point3D.Zero;
        private int outConnCount = 0;

        private Dictionary<Point3D, IChunk> ChunkDictionary;

        public Teleporter(IChunk chunk, Point3D position) //Constructor
        {
            this.Chunk = chunk;
            this.Position = position;
        }

        public void Use(IActor targetActor)
        {
            //get variables
            IChunk chunk = this.Chunk;
            int x = this.Position.X;
            int y = this.Position.Y;
            int z = this.Position.Z;
            IActor actor = targetActor as IActor;
            IGameServer Server = actor.State as IGameServer;
            IBiomeManager biomeManager = Server.Biomes;
            var SystemsCollection = biomeManager.GetSystems();
            uint currentSystemID = actor.InstanceID;
            IBiomeSystem currentSystem;
            SystemsCollection.TryGetValue(currentSystemID, out currentSystem);

            this.ChunkDictionary = SNScriptUtils._Utils.CreateChunkDictionary(currentSystem);

            if (!chunk.IsStaticChunk)
                Server.ChatManager.SendActorMessage("A Teleporter cannot be used on a ship.", actor);
            else if (currentSystem.Nation != string.Empty && currentSystem.Nation != actor.Nation)
                Server.ChatManager.SendActorMessage("You must be in the Nation that claimed this System to use the Teleporter.", actor);
            else
            {
                //save global position of Block which has just been clicked on
                Point3D posA = new Point3D((int)chunk.Position.X + x, (int)chunk.Position.Y + y, (int)chunk.Position.Z + z);
                //if there already is a stored Variable ..
                if (actor.SessionVariables.ContainsKey("Teleporter"))
                {
                    // .. get the saved point (a previously clicked teleporter)
                    Point3D posB = (Point3D)actor.SessionVariables["Teleporter"];
                    // and delete it from the sessionVariables, we have it in posA and posB now
                    actor.SessionVariables.Remove("Teleporter");
                    // obvious check
                    if (posA == posB)
                        Server.ChatManager.SendActorMessage("A Teleporter can not link with itself.", actor);
                    //if they differ we have two points: The two Teleporters, the player wants to link!
                    else
                    {
                        ushort blockID;
                        //check if previously clicked Teleporter still exists
                        if (!SNScriptUtils._Utils.GetBlockIdAtFakeGlobalPos(this.ChunkDictionary, posB, out blockID) || (int)blockID != 7100)
                        {
                            if (debug) { Console.WriteLine("blockID=" + ((int)blockID).ToString()); }
                            Server.ChatManager.SendActorMessage("The previous Teleporter you clicked has been destroyed", actor);
                        }
                        else
                        {
                            //now we can link the two teleporters
                            //get the ChunkKey of the Chunk where the SourceTeleporter is in
                            Point3D staticChunkKey = SNScriptUtils._Utils.GetChunkKeyFromFakeGlobalPos(posB.ToDoubleVector3);
                            //Get SourceChunk Object
                            IChunk sourceChunk = this.ChunkDictionary[staticChunkKey];
                            //Get all SpecialBlocks in the SourceChunk (our Teleporter is in there)
                            List<ISpecialBlock> specialblockslist = sourceChunk.GetSpecialBlocks();
                            //calculate position of SourceTeleporter within its Chunk
                            Point3D posSourceTelePorter = new Point3D(posB.X - staticChunkKey.X, posB.Y - staticChunkKey.Y, posB.Z - staticChunkKey.Z);
                            //get the SourceTeleporter by the Position we just calculated
                            ISpecialBlock SpecialBlock = specialblockslist.First(item => (item is Teleporter) && ((item as Teleporter).Position == posSourceTelePorter));
                            //Now we have the previously clicked Teleporter as accesible Object
                            Teleporter sourceTeleporter = SpecialBlock as Teleporter;
                            //Set the Teleporter, the SourceTeleporter is linked to
                            sourceTeleporter.SetTargetTeleporter(posA);
                            //Inform the Player what happened: The two Teleporters are linked
                            Server.ChatManager.SendActorMessage("You can now Teleport from the first Teleporter to the one you clicked last.", actor);
                        }
                    }
                }
                else
                {
                    actor.SessionVariables.Add("Teleporter", (object)posA);
                    Server.ChatManager.SendActorMessage("Right click a second Teleporter to link it with this one.", actor);
                }
            }

            //get the index for the Block array from the given x, y and z
            int index = chunk.GetBlockIndex(x, y, z);
            //get the specific Block data by its index
            ushort currentBlock = chunk.Blocks[index];

        }

        public void SetTargetTeleporter(Point3D target)
        {
            this.outConnCount = 1;
            this.TargetTeleporter = target;
        }

        public Point3D GetTargetTeleporter()
        {
            return this.TargetTeleporter;
        }

        public void DeleteTargetTeleporter()
        {
            this.outConnCount = 0;
            this.TargetTeleporter = new Point3D(0, 0, 0);
        }

        public void Deserialize(BinaryReader reader)
        {   // Read data about the block you wrote when serializing. Must read all data exactly how it was written.
            int version = reader.ReadInt32();
            this.outConnCount = reader.ReadInt32();

            if (outConnCount > 0)
            {
                this.TargetTeleporter = new Point3D(
                    reader.ReadInt32(),
                    reader.ReadInt32(),
                    reader.ReadInt32()
                );
            }

            int incConnCount = reader.ReadInt32();
            //Console.WriteLine("blockver: " + version.ToString() + ", outConnCount: " + outConnCount.ToString() + ", targetTP: " + this.TargetTeleporter.ToString() + ", incConnCount: " + incConnCount.ToString());
        }

        public void Serialize(BinaryWriter writer)
        {   // Write save data about the block when saving it.
            writer.Write(this.BlockVersion);
            writer.Write(this.outConnCount);
            writer.Write(this.TargetTeleporter.X);
            writer.Write(this.TargetTeleporter.Y);
            writer.Write(this.TargetTeleporter.Z);
            writer.Write((int)0);

            //Console.WriteLine("blockver: " + this.BlockVersion.ToString() + ", outConnCount " + this.outConnCount.ToString() + ", targetTP: " + this.TargetTeleporter.ToString() + ", incConnCount: 0");

        }

        public void Update()
        {
            // Update called every server frame.

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