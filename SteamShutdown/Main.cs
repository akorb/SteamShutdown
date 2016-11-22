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

        public Main()
        {
            InitializeComponent();

            lbUnwatched.GotFocus += listBox_GotFocus;
            lbWatching.GotFocus += listBox_GotFocus;

            lbUnwatched.Items.AddRange(Steam.Apps.Where(x => x.IsDownloading).ToArray());

            Steam.AppInfoChanged += Steam_AppInfoChanged;
            Steam.AppInfoDeleted += Steam_AppInfoDeleted;
        }

        private void UpdateFocused(ListBox listbox)
        {
            focused = listbox;

            if (listbox == lbUnwatched)
                unfocused = lbWatching;
            else if (listbox == lbWatching)
                unfocused = lbUnwatched;
        }

        private void listBox_GotFocus(object sender, EventArgs e)
        {
            UpdateFocused((ListBox)sender);

            unfocused.ClearSelected();

            if (focused == lbUnwatched)
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

            if (lbUnwatched.Items.Contains(e.AppInfo))
                lbUnwatched.Items.Remove(e.AppInfo);
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
                bool doShutdown = lbWatching.Items.Cast<AppInfo>().All(x => !x.IsDownloading);

                if (doShutdown)
                    Shutdown();
            }

            if (e.AppInfo.IsDownloading && !lbUnwatched.Items.Contains(e.AppInfo) && !lbWatching.Items.Contains(e.AppInfo))
            {
                lbUnwatched.Items.Add(e.AppInfo);
            }
            else if (AppInfo.CheckDownloading(e.PreviousState) && !e.AppInfo.IsDownloading)
            {
                if (lbWatching.Items.Contains(e.AppInfo))
                    lbWatching.Items.Remove(e.AppInfo);
                else if (lbUnwatched.Items.Contains(e.AppInfo))
                    lbUnwatched.Items.Remove(e.AppInfo);
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
