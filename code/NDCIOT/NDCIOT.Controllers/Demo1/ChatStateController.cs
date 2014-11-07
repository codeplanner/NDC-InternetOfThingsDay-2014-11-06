using NDCIOT.Controllers.Model;
using XSockets.Core.Common.Socket.Event.Interface;
using XSockets.Core.XSocket;
using XSockets.Core.XSocket.Helpers;
using XSockets.Plugin.Framework.Attributes;

namespace NDCIOT.Controllers.Demo1
{
    [XSocketMetadata("ChatState")]
    public class ChatStateController : XSocketController
    {
        /// <summary>
        /// Color - Connection specific (per client) 
        /// </summary>       
        public Color Color { get; set; }

        /// <summary>
        /// Send only to client with the same color as the caller
        /// </summary>        
        public void SomeTopic(IMessage message)
        {
            this.InvokeTo<ChatStateController>(p => p.Color == this.Color, message);
        }
    }
}