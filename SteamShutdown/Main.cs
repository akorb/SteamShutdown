using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace SteamShutdown
{
    public partial class Main : Form
    {
        ListBox focused;
        ListBox unfocused;

        // 1042 = queued
        public Main()
        {
            InitializeComponent();

            lbDownloading.GotFocus += listBox_GotFocus;
            lbWatching.GotFocus += listBox_GotFocus;

            lbDownloading.Items.AddRange(Steam.Apps.Where(x => x.State == 1026).ToArray());

            Steam.AppInfoChanged += Steam_AppInfoChanged;
            Steam.AppInfoDeleted += Steam_AppInfoDeleted;
        }

        private void UpdateFocused(ListBox listbox)
        {
            focused = listbox;

            if (listbox == lbDownloading)
                unfocused = lbWatching;
            else if (listbox == lbWatching)
                unfocused = lbDownloading;
        }

        private void listBox_GotFocus(object sender, EventArgs e)
        {
            UpdateFocused((ListBox)sender);

            unfocused.ClearSelected();

            if (focused == lbDownloading)
                btnSwitch.Text = ">>";
            else if (focused == lbWatching)
                btnSwitch.Text = "<<";
        }

        private void SwitchItems(object[] items, ListBox from, ListBox to)
        {
            foreach (object item in items)
            {
                from.Items.Remove(item);
            }

            to.Items.AddRange(items);
        }



        private void Steam_AppInfoDeleted(AppInfoEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => { Steam_AppInfoDeleted(e); }));
                return;
            }

            if (lbDownloading.Items.Contains(e.AppInfo))
                lbDownloading.Items.Remove(e.AppInfo);
            else if (lbWatching.Items.Contains(e.AppInfo))
                lbWatching.Items.Remove(e.AppInfo);
        }

        private void Steam_AppInfoChanged(AppInfoChangedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => { Steam_AppInfoChanged(e); }));
                return;
            }

            if (lbWatching.Items.Count > 0)
            {
                bool doShutdown = lbWatching.Items.Cast<AppInfo>().All(x => x.State != 1026);

                if (doShutdown)
                    Shutdown();
            }

            if (e.AppInfo.State == 1026 && !lbDownloading.Items.Contains(e.AppInfo))
            {
                lbDownloading.Items.Add(e.AppInfo);
            }
            else if (e.PreviousState == 1026 && e.AppInfo.State != 1026)
            {
                if (lbWatching.Items.Contains(e.AppInfo))
                    lbWatching.Items.Remove(e.AppInfo);
                else if (lbDownloading.Items.Contains(e.AppInfo))
                    lbDownloading.Items.Remove(e.AppInfo);
            }
        }

        private void btnSwitch_Click(object sender, EventArgs e)
        {
            SwitchItems(focused.SelectedItems.Cast<object>().ToArray(), focused, unfocused);
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
