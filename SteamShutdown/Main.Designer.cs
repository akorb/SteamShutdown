namespace SteamShutdown
{
	partial class Main
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.clbDownloadingGames = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // clbDownloadingGames
            // 
            this.clbDownloadingGames.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clbDownloadingGames.FormattingEnabled = true;
            this.clbDownloadingGames.Location = new System.Drawing.Point(0, 0);
            this.clbDownloadingGames.Name = "clbDownloadingGames";
            this.clbDownloadingGames.Size = new System.Drawing.Size(284, 261);
            this.clbDownloadingGames.Sorted = true;
            this.clbDownloadingGames.TabIndex = 0;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.clbDownloadingGames);
            this.Name = "Main";
            this.ShowIcon = false;
            this.Text = "Steam Shutdown";
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.CheckedListBox clbDownloadingGames;
	}
}

