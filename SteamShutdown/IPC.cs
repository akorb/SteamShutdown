using System;
using System.ServiceModel;
using System.Windows.Forms;

namespace SteamShutdown
{
    [ServiceContract()]
    interface ITestContract
    {
        [OperationContract]
        void ShowAnInstanceIsRunning();
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class IPCTestServer : ITestContract
    {
        public void ShowAnInstanceIsRunning()
        {
            string text = "I'm already running." + Environment.NewLine + "Click the taskbar icon to open it.";
            CustomApplicationContext.NotifyIcon?.ShowBalloonTip(2000, "Oops", text, ToolTipIcon.Info);
        }
    }
}
