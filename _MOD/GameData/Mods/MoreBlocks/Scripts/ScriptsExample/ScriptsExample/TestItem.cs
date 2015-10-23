using SharedGameData.Items;
using SNScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptsExample
{
    public class TestItem : IItemScript
    {
        public void OnEquip(object actor, object item)
        {

        }

        public void OnUnequip(object actor, object item)
        {

        }

        public void OnUse(object actor, Microsoft.Xna.Framework.Ray ray, object item)
        {
            IActor myActor = actor as IActor;
            IGameServer server = myActor.State as IGameServer;

            server.ChatManager.SendActorMessage("Hey, this is a test!", myActor);
        }
    }
}
