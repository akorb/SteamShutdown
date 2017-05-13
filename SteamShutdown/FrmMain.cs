using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace SteamShutdown
{
    public partial class FrmMain : Form
    {
        private BindingList<AppInfo> _srcApps;

        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            grdData.AutoGenerateColumns = false;
            cbModes.Items.AddRange(new Modes.Mode[] { new Modes.ShutdownMode(), new Modes.SleepMode() });
            cbModes.SelectedIndex = 0;

            _srcApps = new BindingList<AppInfo>(Steam.Apps.Where(x => x.IsDownloading).ToArray());
            grdData.DataSource = _srcApps;

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

            if (_srcApps.Contains(e.AppInfo))
                _srcApps.Remove(e.AppInfo);
        }

        private void Steam_AppInfoChanged(AppInfoChangedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => { Steam_AppInfoChanged(e); }));
                return;
            }
            //Gets all the checked rows from grdData [DataGridView].
            var lstCheckedRows = grdData.SelectedRows.Cast<DataGridViewRow>()
                .Where(w => w.Cells[0].Value != null && (bool)w.Cells[0].Value)
                .Select(s => s.DataBoundItem as AppInfo)
                .ToList();

            if (_srcApps.Count > 0)
            {
                bool doShutdown = lstCheckedRows.All(x => !x.IsDownloading);
                if (doShutdown)
                    Shutdown();
            }

            //
            if (e.AppInfo.IsDownloading && !_srcApps.Contains(e.AppInfo))
            {
                _srcApps.Add(e.AppInfo);
                if (cbAll.Checked)
                    grdData.Rows.Cast<DataGridViewRow>().FirstOrDefault(f => f.DataBoundItem == e.AppInfo).Cells[0].Value = true;
                else
                    grdData.Rows.Cast<DataGridViewRow>().FirstOrDefault(f => f.DataBoundItem == e.AppInfo).Cells[0].Value = false;
            }
            //
            else if (AppInfo.CheckDownloading(e.PreviousState) && !e.AppInfo.IsDownloading)
            {
                if (_srcApps.Contains(e.AppInfo))
                    _srcApps.Remove(e.AppInfo);
            }
        }

        private void Shutdown()
        {
            var mode = (Modes.Mode)cbModes.SelectedItem;

#if DEBUG
            MessageBox.Show(mode.Name);
#else
            mode.Execute();
#endif
        }
    }
}