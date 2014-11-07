using XSockets.Core.Common.Socket.Event.Interface;
using XSockets.Core.XSocket;
using XSockets.Core.XSocket.Helpers;
using XSockets.Plugin.Framework.Attributes;

namespace NDCIOT.Controllers.Demo1
{
    /// <summary>
    /// Implement/Override your custom actionmethods, events etc in this real-time MVC controller
    /// </summary>
    [XSocketMetadata("ChatSimple")]
    public class ChatSimpleController : XSocketController
    {
        /// <summary>
        /// This will broadcast any message to all clients
        /// connected to this controller.
        /// To use Pub/Sub replace InvokeToAll with PublishToAll
        /// </summary>
        /// <param name="message"></param>
        public override void OnMessage(IMessage message)
        {
            this.InvokeToAll(message);
        }
    }
}
