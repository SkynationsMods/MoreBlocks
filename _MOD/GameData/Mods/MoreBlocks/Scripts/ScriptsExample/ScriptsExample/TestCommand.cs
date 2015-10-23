using SNScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptsExample
{
    class TestCommand : GameCommand
    {
        public override string[] Aliases
        {
            get { return new string[] { "/test" }; }
        }

        public override string CommandDescription
        {
            get
            {
                return "A test command in ScriptsExample.";
            }
        }

        public override Priviledges Priviledge
        {
            get { return Priviledges.Player; }
        }
        
        public TestCommand(IGameServer server)
            : base(server)
        {
        }

        public override bool Use(IActor actor, string message, string[] parameters)
        {
            Server.ChatManager.SendActorMessage("Hello world!", actor);

            return true;
        }
    }
}
