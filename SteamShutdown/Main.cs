using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace SteamShutdown
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();

            clbDownloadingGames.Items.AddRange(Steam.Apps.Where(x => x.State == 1026).ToArray());

            Steam.AppInfoChanged += Steam_AppInfoChanged;
            Steam.AppInfoDeleted += Steam_AppInfoDeleted;
        }

        private void Steam_AppInfoDeleted(AppInfoEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => { Steam_AppInfoDeleted(e); }));
                return;
            }

            clbDownloadingGames.Items.Remove(e.AppInfo);
        }

        private void Steam_AppInfoChanged(AppInfoChangedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => { Steam_AppInfoChanged(e); }));
                return;
            }

            if (clbDownloadingGames.CheckedItems.Count > 0)
            {
                bool doShutdown = clbDownloadingGames.CheckedItems.Cast<AppInfo>().All(x => x.State != 1026);

                if (doShutdown)
                    Shutdown();
            }

            if (e.AppInfo.State == 1026 && !clbDownloadingGames.Items.Contains(e.AppInfo))
            {
                clbDownloadingGames.Items.Add(e.AppInfo);
            }
            else if (e.PreviousState == 1026 && e.AppInfo.State != 1026 && clbDownloadingGames.Items.Contains(e.AppInfo))
            {
                clbDownloadingGames.Items.Remove(e.AppInfo);
            }
        }

        private void Shutdown()
        {
#if DEBUG
            MessageBox.Show("Shutdown");
#else
            Process.Start("shutdown", "/s /t 0");
#endif
        }
    }
}
