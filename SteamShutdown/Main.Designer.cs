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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbUnwatched
            // 
            this.lbUnwatched.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbUnwatched.FormattingEnabled = true;
            this.lbUnwatched.ItemHeight = 20;
            this.lbUnwatched.Location = new System.Drawing.Point(12, 41);
            this.lbUnwatched.Name = "lbUnwatched";
            this.lbUnwatched.Size = new System.Drawing.Size(168, 284);
            this.lbUnwatched.TabIndex = 0;
            // 
            // lbWatching
            // 
            this.lbWatching.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbWatching.FormattingEnabled = true;
            this.lbWatching.ItemHeight = 20;
            this.lbWatching.Location = new System.Drawing.Point(301, 41);
            this.lbWatching.Name = "lbWatching";
            this.lbWatching.Size = new System.Drawing.Size(168, 284);
            this.lbWatching.TabIndex = 1;
            // 
            // btnSwitch
            // 
            this.btnSwitch.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSwitch.Location = new System.Drawing.Point(18, 120);
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
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSwitch);
            this.panel1.Location = new System.Drawing.Point(186, 41);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(109, 284);
            this.panel1.TabIndex = 6;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(482, 346);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblUnwatched);
            this.Controls.Add(this.lbWatching);
            this.Controls.Add(this.lbUnwatched);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Main";
            this.ShowIcon = false;
            this.Text = "Steam Shutdown";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

        #endregion

        private System.Windows.Forms.ListBox lbUnwatched;
        private System.Windows.Forms.ListBox lbWatching;
        private System.Windows.Forms.Button btnSwitch;
        private System.Windows.Forms.Label lblUnwatched;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
    }
}

