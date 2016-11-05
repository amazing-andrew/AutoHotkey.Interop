using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoHotkey.Interop.Pipes
{
    internal class MessageHandlerPipeServer : NamedPipeServer
    {
        Func<string, string> messageHandler = null;

        public MessageHandlerPipeServer(string pipeName, Func<string,string> messageHandler) : base(pipeName) {
            if (pipeName == null) throw new ArgumentNullException("pipeName");
            if (messageHandler == null) throw new ArgumentNullException("messageHandler");

            this.messageHandler = messageHandler;
        }

        protected override string HandleClientMessage(string clientMessage) {
            return messageHandler(clientMessage);
        }
    }
}
