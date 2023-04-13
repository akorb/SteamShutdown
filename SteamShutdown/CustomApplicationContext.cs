using SteamShutdown.Actions;
using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace SteamShutdown
{
    /// <summary>
    /// Framework for running application as a tray app.
    /// </summary>
    /// <remarks>
    /// Tray app code adapted from "Creating Applications with NotifyIcon in Windows Forms", Jessica Fosler,
    /// http://windowsclient.net/articles/notifyiconapplications.aspx
    /// </remarks>
    public class CustomApplicationContext : ApplicationContext
    {
        const string TOOL_TIP = "Execute action after chosen game downloads finished.";

        /// <summary>
		/// This class should be created and passed into Application.Run( ... )
		/// </summary>
		public CustomApplicationContext()
        {
            InitializeContext();

            Steam.AppInfoChanged += Steam_AppInfoChanged;
            Steam.AppInfoDeleted += Steam_AppInfoDeleted;
        }

        private void Steam_AppInfoDeleted(object sender, AppInfoEventArgs e)
        {
            SteamShutdown.WatchedGames.Remove(e.AppInfo);
        }

        private void Steam_AppInfoChanged(object sender, AppInfoChangedEventArgs e)
        {
            if (SteamShutdown.WatchedGames.Count > 0)
            {
                bool doShutdown = SteamShutdown.WatchedGames.All(x => !x.IsDownloading);

                if (doShutdown)
                    Shutdown();
            }

            if (e.AppInfo.IsDownloading && !SteamShutdown.WatchedGames.Contains(e.AppInfo))
            {
                if (SteamShutdown.WaitForAll)
                    SteamShutdown.WatchedGames.Add(e.AppInfo);
            }
            else if (App.CheckDownloading(e.PreviousState) && !e.AppInfo.IsDownloading)
            {
                SteamShutdown.WatchedGames.Remove(e.AppInfo);
            }
        }

        private void ContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = false;
            var root = NotifyIcon.ContextMenuStrip.Items;
            root.Clear();

            var downloadingApps = Steam.Apps.Where(x => x.IsDownloading).ToList();
            if (downloadingApps.Count > 0)
            {
                foreach (App game in downloadingApps)
                {
                    AddToolStripItem(root, game.Name, Item_Click, !SteamShutdown.WaitForAll && SteamShutdown.WatchedGames.Contains(game), game, !SteamShutdown.WaitForAll);
                }
            }
            else
            {
                AddToolStripItem(root, "No games downloading", enabled: false);
            }

            root.Add(new ToolStripSeparator());
            var modeNode = (ToolStripMenuItem)AddToolStripItem(root, "Actions");

            foreach (var mode in Actions.Action.GetAllActions)
            {
                AddToolStripItem(modeNode.DropDownItems, mode.Name, this.Mode_Click, mode.GetType() == SteamShutdown.ActiveMode.GetType(), mode);
            }

            root.Add(new ToolStripSeparator());
            AddToolStripItem(root, "Complete all downloads", AllItem_Click, SteamShutdown.WaitForAll);

            root.Add(new ToolStripSeparator());
            AddToolStripItem(root, "Close", CloseItem_Click);
        }

        private void CloseItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Mode_Click(object sender, EventArgs e)
        {
            SteamShutdown.ActiveMode = (Actions.Action)((ToolStripItem)sender).Tag;
        }

        private void AllItem_Click(object sender, EventArgs e)
        {
            var allItem = (ToolStripMenuItem)sender;
            SteamShutdown.WaitForAll = !allItem.Checked;

            SteamShutdown.WatchedGames.Clear();
            if (SteamShutdown.WaitForAll)
            {
                SteamShutdown.WatchedGames.AddRange(Steam.Apps.Where(x => x.IsDownloading));
            }
        }

        private void Item_Click(object sender, EventArgs e)
        {
            var item = (ToolStripMenuItem)sender;

            if (!item.Checked)
            {
                SteamShutdown.WatchedGames.Add((App)item.Tag);
            }
            else
            {
                SteamShutdown.WatchedGames.Remove((App)item.Tag);
            }
        }

        private ToolStripItem AddToolStripItem(ToolStripItemCollection root, string name, Action<object, EventArgs> clickAction = null, bool isChecked = false, object tag = null, bool enabled = true)
        {
            var item = root.Add(name);
            item.Tag = tag;
            item.Enabled = enabled;

            if (clickAction != null)
                item.Click += (o, e) => clickAction(o, e);

            if (isChecked)
                ((ToolStripMenuItem)item).Checked = true;

            return item;
        }

        private void Shutdown()
        {
#if DEBUG
            MessageBox.Show(SteamShutdown.ActiveMode.Name);
#else
            var timer = new System.Timers.Timer(30000.0);
            timer.AutoReset = false;

            NotifyIcon.ShowBalloonTip(5000, "", $"The action \"{SteamShutdown.ActiveMode.Name}\" will be executed in 30 seconds.{Environment.NewLine}Quit to abort.", ToolTipIcon.Info);

            var modeToExecute = SteamShutdown.ActiveMode;
            timer.Elapsed += (o, e) => modeToExecute.Execute();
            timer.Start();
            SteamShutdown.Log("Started timer for action.");
#endif
        }

        System.ComponentModel.IContainer components;	// a list of components to dispose when the context is disposed
        public static NotifyIcon NotifyIcon { get; private set; }				            // the icon that sits in the system tray

        private void InitializeContext()
        {
            components = new System.ComponentModel.Container();
            NotifyIcon = new NotifyIcon(components)
            {
                ContextMenuStrip = new ContextMenuStrip(),
                Icon = Properties.Resources.icon,
                Text = TOOL_TIP,
                Visible = true
            };
            NotifyIcon.MouseUp += NotifyIcon_MouseUp;
            NotifyIcon.ContextMenuStrip.Opening += ContextMenuStrip_Opening;
            NotifyIcon.ContextMenuStrip.RenderMode = ToolStripRenderMode.Professional;
            if (!ColorUtils.SystemUsesLightTheme())
                NotifyIcon.ContextMenuStrip.Renderer = new DarkRenderer(ColorUtils.GetAccentColor());

            NotifyIcon.ShowBalloonTip(2000, "Hello", "You find me in the taskbar.", ToolTipIcon.Info);
        }

        private void NotifyIcon_MouseUp(object sender, MouseEventArgs e)
        {
            // Show ContextMenuStrip with left click too
            if (e.Button == MouseButtons.Left)
            {
                MethodInfo mi = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);
                mi.Invoke(NotifyIcon, null);
            }
        }


        /// <summary>
        /// When the application context is disposed, dispose things like the notify icon.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) { components.Dispose(); }
        }

        /// <summary>
        /// If we are presently showing a form, clean it up.
        /// </summary>
        protected override void ExitThreadCore()
        {
            NotifyIcon.Visible = false; // should remove lingering tray icon
            base.ExitThreadCore();
        }
    }
}
