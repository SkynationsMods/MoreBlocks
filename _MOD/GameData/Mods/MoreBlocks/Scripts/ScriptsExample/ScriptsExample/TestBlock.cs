using SharedGameData;
using SNScript;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptsExample
{
    class TestBlock : ISpecialBlock
    {
        public TestBlock(IChunk chunk, Point3D position)
        {
            // Chunk is the chunk the block belong to. Position is where it is in that particular chunk(NOT WORLD POSITION).

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

        public void Use(IActor actor)
        {
            // Player right clicking the block
            IGameServer server = actor.State as IGameServer;

            int healthDifference = 100 - actor.Health;

            actor.Heal(healthDifference);

            server.ChatManager.SendActorMessage(string.Format("You have been healed {0} points.", healthDifference), actor);
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
