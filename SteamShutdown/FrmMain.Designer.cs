namespace SteamShutdown
{
    partial class FrmMain
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.grdData = new System.Windows.Forms.DataGridView();
            this.cbModes = new System.Windows.Forms.ComboBox();
            this.lblAction = new System.Windows.Forms.Label();
            this.cbAll = new System.Windows.Forms.CheckBox();
            this.colWatch = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colGame = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIsDownloading = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).BeginInit();
            this.SuspendLayout();
            // 
            // grdData
            // 
            this.grdData.AllowUserToAddRows = false;
            this.grdData.AllowUserToDeleteRows = false;
            this.grdData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colWatch,
            this.colGame,
            this.colIsDownloading});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdData.DefaultCellStyle = dataGridViewCellStyle1;
            this.grdData.Location = new System.Drawing.Point(13, 40);
            this.grdData.Name = "grdData";
            this.grdData.RowHeadersWidth = 20;
            this.grdData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdData.ShowEditingIcon = false;
            this.grdData.Size = new System.Drawing.Size(582, 294);
            this.grdData.TabIndex = 0;
            // 
            // cbModes
            // 
            this.cbModes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbModes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbModes.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbModes.Location = new System.Drawing.Point(71, 5);
            this.cbModes.Name = "cbModes";
            this.cbModes.Size = new System.Drawing.Size(116, 28);
            this.cbModes.TabIndex = 13;
            // 
            // lblAction
            // 
            this.lblAction.AutoSize = true;
            this.lblAction.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.lblAction.Location = new System.Drawing.Point(12, 9);
            this.lblAction.Name = "lblAction";
            this.lblAction.Size = new System.Drawing.Size(53, 18);
            this.lblAction.TabIndex = 12;
            this.lblAction.Text = "Action:";
            // 
            // cbAll
            // 
            this.cbAll.AutoSize = true;
            this.cbAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbAll.Location = new System.Drawing.Point(444, 9);
            this.cbAll.Name = "cbAll";
            this.cbAll.Size = new System.Drawing.Size(151, 22);
            this.cbAll.TabIndex = 11;
            this.cbAll.Text = "Action if all finished";
            this.cbAll.UseVisualStyleBackColor = true;
            // 
            // colWatch
            // 
            this.colWatch.HeaderText = "Watched";
            this.colWatch.Name = "colWatch";
            // 
            // colGame
            // 
            this.colGame.DataPropertyName = "Name";
            this.colGame.HeaderText = "Game title";
            this.colGame.Name = "colGame";
            this.colGame.ReadOnly = true;
            this.colGame.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colGame.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colGame.Width = 340;
            // 
            // colIsDownloading
            // 
            this.colIsDownloading.DataPropertyName = "IsDownloading";
            this.colIsDownloading.HeaderText = "Is downloading";
            this.colIsDownloading.Name = "colIsDownloading";
            this.colIsDownloading.ReadOnly = true;
            this.colIsDownloading.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colIsDownloading.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colIsDownloading.Width = 120;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(607, 346);
            this.Controls.Add(this.cbModes);
            this.Controls.Add(this.lblAction);
            this.Controls.Add(this.cbAll);
            this.Controls.Add(this.grdData);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FrmMain";
            this.ShowIcon = false;
            this.Text = "Steam shutdown";
            this.Load += new System.EventHandler(this.FrmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView grdData;
        private System.Windows.Forms.ComboBox cbModes;
        private System.Windows.Forms.Label lblAction;
        private System.Windows.Forms.CheckBox cbAll;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colWatch;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGame;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colIsDownloading;
    }
}