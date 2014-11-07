using XSockets.Core.Common.Socket.Event.Interface;
using XSockets.Core.XSocket;
using XSockets.Core.XSocket.Helpers;

namespace NDCIOT.Controllers.Demo4
{
    public class Missile : XSocketController
    {     
        /// <summary>
        /// Use IMessage to let anything data pass through
        /// </summary>
        /// <param name="message"></param>
        public void Command(IMessage message)
        {            
            //Send message to subscribers (only the missile launcher in this case)
            this.PublishToAll(message);
        }        
    }
}
