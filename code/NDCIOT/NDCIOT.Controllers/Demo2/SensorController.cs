using NDCIOT.Controllers.Model;
using XSockets.Core.Common.Socket.Event.Interface;
using XSockets.Core.XSocket;
using XSockets.Core.XSocket.Helpers;
using XSockets.Plugin.Framework.Attributes;

namespace NDCIOT.Controllers.Demo2
{
    /// <summary>
    /// The controller that hardware connects to
    /// </summary>
    [XSocketMetadata("sensor")]
    public class SensorController : XSocketController
    {
        public Hardware Hardware { get; set; }

        /// <summary>
        /// The hardware sent a level change
        /// </summary>        
        public void Change(IMessage message)
        {
            try
            {
                var v = message.Extract<int>();
                this.InvokeTo<MonitorController>(p => v <= p.Threshold, v, "Change" + Hardware);
            }
            catch{}
        }
    }
}
