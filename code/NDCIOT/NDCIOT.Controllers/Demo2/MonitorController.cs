using NDCIOT.Controllers.Model;
using XSockets.Core.XSocket;
using XSockets.Core.XSocket.Helpers;
using XSockets.Plugin.Framework.Attributes;

namespace NDCIOT.Controllers.Demo2
{
    /// <summary>
    /// The controller that people monitoring connects to
    /// </summary>
    [XSocketMetadata("monitor")]
    public class MonitorController : XSocketController
    {
        /// <summary>
        /// Individual sensor level
        /// </summary>
        public int Threshold { get; set; }

        /// <summary>
        /// Change the threshold for a specific hardware
        /// </summary>
        public void SetThreshold(HardwareSettings hws)
        {
            //Set new hardware limit
            this.InvokeTo<SensorController>(p => p.Hardware == hws.Hardware, hws.Threshold, "threshold");

            //Tell all clients about the change
            this.InvokeToAll<MonitorController>(hws.Threshold, "threshold" + hws.Hardware);
        }
    }
}