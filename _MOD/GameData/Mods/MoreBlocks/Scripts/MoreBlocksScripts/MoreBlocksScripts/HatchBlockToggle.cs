﻿using SharedGameData;
using SNScript;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameServer.World.Chunks;

namespace MoreBlocksScripts
{   //IMPORTANT: This Script switches a Block between two different states
    //for the Script to properly work, the first state (Tile) has to have an even ID
    //and the following odd ID is the second state of the Block
    //With this conditions met, you can use this Script for any block that is 
    //supposed to have two states, which are toggled by right click (Use)
    class HatchBlockToggle : ISpecialBlock
    {
        private IChunk Chunk;
        private Point3D Position;

        public HatchBlockToggle(IChunk chunk, Point3D position) //Constructor
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
            IActor actor = targetActor;

            //permission check, is the Player allowed to open the door?
            Chunk castedChunk = chunk as Chunk;
            string nationName = castedChunk.NationOwner;
            if ((!string.IsNullOrEmpty(nationName) && (actor.Nation != nationName)))
                return;
            
            //get the index for the Block array from the given x, y and z
            int index = chunk.GetBlockIndex(x, y, z);
            //get the specific Block data by its index
            ushort currentBlock = chunk.Blocks[index];

			
            //get baseHatchID (reason why the hatchID has to be a multiple of 10, for this script to properly work)
            int baseHatchID = ((int)currentBlock / 10) * 10;
            int offset = (int)currentBlock - baseHatchID;

			if(0 <= offset && offset <= 3)
			{
				chunk.ChangeBlock((ushort)(currentBlock + 4), x, y, z);
			}
            if (4 <= offset && offset <= 7)
			{
				chunk.ChangeBlock((ushort)(currentBlock - 4), x, y, z);
			}
        }

        public static bool IsOdd(int value)
        {
            return value % 2 != 0;
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