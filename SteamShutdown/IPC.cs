using System;
using System.ServiceModel;
using System.Windows.Forms;

namespace SteamShutdown
{
    static class RPC
    {
        const string address = "net.pipe://localhost/SteamShutdown/ShowBalloonTip";

        public static void ShowAnInstanceIsRunning()
        {
            NetNamedPipeBinding binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
            EndpointAddress ep = new EndpointAddress(address);
            IBubbleContract channel = ChannelFactory<IBubbleContract>.CreateChannel(binding, ep);
            channel.ShowAnInstanceIsRunning();
        }

        public static void StartServer()
        {
            NetNamedPipeBinding binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
            ServiceHost serviceHost = new ServiceHost(typeof(IPCBubbleServer));
            serviceHost.AddServiceEndpoint(typeof(IBubbleContract), binding, address);
            serviceHost.Open();
        }
    }

    [ServiceContract()]
    interface IBubbleContract
    {
        [OperationContract]
        void ShowAnInstanceIsRunning();
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class IPCBubbleServer : IBubbleContract
    {
        public void ShowAnInstanceIsRunning()
        {
            string text = "I'm already running." + Environment.NewLine + "Click the taskbar icon to open it.";
            CustomApplicationContext.NotifyIcon?.ShowBalloonTip(2000, "Oops", text, ToolTipIcon.Info);
        }
    }
}
