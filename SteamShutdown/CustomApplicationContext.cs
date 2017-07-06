using SteamShutdown.Modes;
using System;
using System.Linq;
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
        const string DefaultTooltip = "Execute action after chosen game downloads finished.";

        /// <summary>
		/// This class should be created and passed into Application.Run( ... )
		/// </summary>
		public CustomApplicationContext()
        {
            InitializeContext();

            Steam.AppInfoChanged += Steam_AppInfoChanged;
            Steam.AppInfoDeleted += Steam_AppInfoDeleted;
        }

        private void Steam_AppInfoDeleted(AppInfoEventArgs e)
        {
            StateMachine.WatchedGames.Remove(e.AppInfo);
        }

        private void Steam_AppInfoChanged(AppInfoChangedEventArgs e)
        {
            if (StateMachine.WatchedGames.Count > 0)
            {
                bool doShutdown = StateMachine.WatchedGames.All(x => !x.IsDownloading);

                if (doShutdown)
                    Shutdown();
            }

            if (e.AppInfo.IsDownloading && !StateMachine.WatchedGames.Contains(e.AppInfo))
            {
                if (StateMachine.WaitForAll)
                    StateMachine.WatchedGames.Add(e.AppInfo);
            }
            else if (AppInfo.CheckDownloading(e.PreviousState) && !e.AppInfo.IsDownloading)
            {
                StateMachine.WatchedGames.Remove(e.AppInfo);
            }
        }

        private void ContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = false;
            var root = notifyIcon.ContextMenuStrip.Items;
            root.Clear();

            var sortedApps = Steam.SortedApps.Where(x => x.IsDownloading);
            foreach (AppInfo game in sortedApps)
            {
                AddToolStripItem(root, game.Name, Item_Click, !StateMachine.WaitForAll && StateMachine.WatchedGames.Contains(game), game, !StateMachine.WaitForAll);
            }

            root.Add(new ToolStripSeparator());
            var modeNode = (ToolStripMenuItem)AddToolStripItem(root, "Modes");

            foreach (var mode in Mode.GetAllModes)
            {
                AddToolStripItem(modeNode.DropDownItems, mode.Name, Mode_Click, mode.GetType() == StateMachine.ActiveMode.GetType(), mode);
            }

            root.Add(new ToolStripSeparator());
            AddToolStripItem(root, "Complete all downloads", AllItem_Click, StateMachine.WaitForAll);

            root.Add(new ToolStripSeparator());
            AddToolStripItem(root, "Close", CloseItem_Click);
        }

        private void CloseItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Mode_Click(object sender, EventArgs e)
        {
            StateMachine.ActiveMode = (Mode)((ToolStripItem)sender).Tag;
        }

        private void AllItem_Click(object sender, EventArgs e)
        {
            var allItem = (ToolStripMenuItem)sender;
            StateMachine.WaitForAll = !allItem.Checked;

            if (StateMachine.WaitForAll)
            {
                StateMachine.WatchedGames.Clear();
                StateMachine.WatchedGames.AddRange(Steam.Apps.Where(x => x.IsDownloading));
            }
        }

        private void Item_Click(object sender, EventArgs e)
        {
            var item = (ToolStripMenuItem)sender;

            if (!item.Checked)
            {
                StateMachine.WatchedGames.Add((AppInfo)item.Tag);
            }
            else
            {
                StateMachine.WatchedGames.Remove((AppInfo)item.Tag);
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
            MessageBox.Show(StateMachine.ActiveMode.Name);
#else
            StateMachine.ActiveMode.Execute();
#endif
        }

        # region generic code framework

        System.ComponentModel.IContainer components;	// a list of components to dispose when the context is disposed
        NotifyIcon notifyIcon;				            // the icon that sits in the system tray

        private void InitializeContext()
        {
            components = new System.ComponentModel.Container();
            notifyIcon = new NotifyIcon(components)
            {
                ContextMenuStrip = new ContextMenuStrip(),
                Icon = Properties.Resources.icon,
                Text = DefaultTooltip,
                Visible = true
            };
            notifyIcon.ContextMenuStrip.Opening += ContextMenuStrip_Opening;
        }

        /// <summary>
		/// When the application context is disposed, dispose things like the notify icon.
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) { components.Dispose(); }
        }

        /// <summary>
        /// If we are presently showing a form, clean it up.
        /// </summary>
        protected override void ExitThreadCore()
        {
            notifyIcon.Visible = false; // should remove lingering tray icon
            base.ExitThreadCore();
        }

        # endregion generic code framework
    }
}
