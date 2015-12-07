using SharedGameData;
using SNScript;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreBlocksDoorScript
{
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

            //get baseDoorID (thats why Doors wanting to use this script have to start with a multiples of 10)
            int baseDoorID = ((int)currentBlock / 10) * 10;
            int offset = (int)currentBlock - baseDoorID;

            //call ToggleDoors function with variables
            ToggleDoor(chunk, baseDoorID, offset, x, y, z);
        }

        private void ToggleDoor(IChunk chunk, int baseDoorID, int offset, int x, int y, int z)
        {
            switch (offset) //switches - we love 'em, don't we? Too lazy for a proper alg
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