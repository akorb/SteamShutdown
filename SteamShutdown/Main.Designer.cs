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
            this.lbUnwatched = new System.Windows.Forms.ListBox();
            this.lbWatching = new System.Windows.Forms.ListBox();
            this.btnSwitch = new System.Windows.Forms.Button();
            this.lblUnwatched = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbUnwatched
            // 
            this.lbUnwatched.FormattingEnabled = true;
            this.lbUnwatched.Location = new System.Drawing.Point(12, 41);
            this.lbUnwatched.Name = "lbUnwatched";
            this.lbUnwatched.Size = new System.Drawing.Size(168, 290);
            this.lbUnwatched.TabIndex = 0;
            // 
            // lbWatching
            // 
            this.lbWatching.FormattingEnabled = true;
            this.lbWatching.Location = new System.Drawing.Point(301, 41);
            this.lbWatching.Name = "lbWatching";
            this.lbWatching.Size = new System.Drawing.Size(168, 290);
            this.lbWatching.TabIndex = 1;
            // 
            // btnSwitch
            // 
            this.btnSwitch.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSwitch.Location = new System.Drawing.Point(203, 159);
            this.btnSwitch.Name = "btnSwitch";
            this.btnSwitch.Size = new System.Drawing.Size(72, 45);
            this.btnSwitch.TabIndex = 2;
            this.btnSwitch.Text = ">>";
            this.btnSwitch.UseVisualStyleBackColor = true;
            this.btnSwitch.Click += new System.EventHandler(this.btnSwitch_Click);
            // 
            // lblUnwatched
            // 
            this.lblUnwatched.AutoSize = true;
            this.lblUnwatched.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUnwatched.Location = new System.Drawing.Point(8, 16);
            this.lblUnwatched.Name = "lblUnwatched";
            this.lblUnwatched.Size = new System.Drawing.Size(94, 20);
            this.lblUnwatched.TabIndex = 4;
            this.lblUnwatched.Text = "Unwatched:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(297, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "Watching:";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(482, 346);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblUnwatched);
            this.Controls.Add(this.btnSwitch);
            this.Controls.Add(this.lbWatching);
            this.Controls.Add(this.lbUnwatched);
            this.Name = "Main";
            this.ShowIcon = false;
            this.Text = "Steam Shutdown";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

        #endregion

        private System.Windows.Forms.ListBox lbUnwatched;
        private System.Windows.Forms.ListBox lbWatching;
        private System.Windows.Forms.Button btnSwitch;
        private System.Windows.Forms.Label lblUnwatched;
        private System.Windows.Forms.Label label1;
    }
}

