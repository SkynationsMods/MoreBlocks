using SharedGameData;
using SNScript;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreBlocksScripts
{   //IMPORTANT: THE TILES OF ANY DOOR THAT WANTS TO MAKE USE OF THIS SCRIPT HAVE TO START WITH AN ID BEING A MULTIPLE OF 10
    //ALSO THE ORDER OF THE 8 DIFFERENT DOOR STATES, HAS TO BE EXACTLY THE SAME AS IN THE DEFAULT TILES.XML
    //WITH THESE CONDITIONS MET, THIS SCRIPT CAN BE UNIVERSALLY USED FOR ALL DOORS
    class DoorToggle : ISpecialBlock
    {
        private IChunk  Chunk;
        private Point3D Position;

        public DoorToggle(IChunk chunk, Point3D position) //Constructor
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

            //permission check, is the Player allowed to open the door?
            //if (GamePermissions.OnlyNationAccess(actor, chunk.NationOwner))
            //    return;

            //get the index for the Block array from the given x, y and z
            int index = chunk.GetBlockIndex(x, y, z);
            //get the specific Block data by its index
            ushort currentBlock = chunk.Blocks[index];

            //get baseDoorID (reason why the doorID has to be a multiple of 10, for this script to properly work)
            int baseDoorID = ((int)currentBlock / 10) * 10;
            int offset = (int)currentBlock - baseDoorID;

            //call ToggleDoors function with variables
            ToggleDoor(chunk, baseDoorID, offset, x, y, z);
        }

        private void ToggleDoor(IChunk chunk, int baseDoorID, int offset, int x, int y, int z)
        {
            switch (offset)
            {
                case 0:
                    chunk.ChangeBlock((ushort)(baseDoorID + 1), x, y, z);
                    chunk.ChangeBlock((ushort)(baseDoorID + 5), x, y + 1, z);
                    break;
                case 1:
                    chunk.ChangeBlock((ushort)(baseDoorID + 0), x, y, z);
                    chunk.ChangeBlock((ushort)(baseDoorID + 4), x, y + 1, z);
                    break;
                case 2:
                    chunk.ChangeBlock((ushort)(baseDoorID + 3), x, y, z);
                    chunk.ChangeBlock((ushort)(baseDoorID + 7), x, y + 1, z);
                    break;
                case 3:
                    chunk.ChangeBlock((ushort)(baseDoorID + 2), x, y, z);
                    chunk.ChangeBlock((ushort)(baseDoorID + 6), x, y + 1, z);
                    break;
                case 4:
                    chunk.ChangeBlock((ushort)(baseDoorID + 5), x, y, z);
                    chunk.ChangeBlock((ushort)(baseDoorID + 1), x, y - 1, z);
                    break;
                case 5:
                    chunk.ChangeBlock((ushort)(baseDoorID + 4), x, y, z);
                    chunk.ChangeBlock((ushort)(baseDoorID + 0), x, y - 1, z);
                    break;
                case 6:
                    chunk.ChangeBlock((ushort)(baseDoorID + 7), x, y, z);
                    chunk.ChangeBlock((ushort)(baseDoorID + 3), x, y - 1, z);
                    break;
                case 7:
                    chunk.ChangeBlock((ushort)(baseDoorID + 6), x, y, z);
                    chunk.ChangeBlock((ushort)(baseDoorID + 2), x, y - 1, z);
                    break;
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