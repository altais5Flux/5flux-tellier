namespace WebservicesSage
{
    partial class MappingExpeditionMode
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MappingExpeditionMode));
            this.Header = new System.Windows.Forms.Panel();
            this.CloseButton = new WindowsFormsControlLibrary1.CustomImageButto();
            this.bunifuCustomLabel1 = new Bunifu.Framework.UI.BunifuCustomLabel();
            this.DefaultExpeditinMode = new System.Windows.Forms.Panel();
            this.DefaultExpedition = new Bunifu.Framework.UI.BunifuDropdown();
            this.bunifuCustomLabel2 = new Bunifu.Framework.UI.BunifuCustomLabel();
            this.ExpeditionOrderDataGrid = new System.Windows.Forms.Panel();
            this.bunifuCustomLabel4 = new Bunifu.Framework.UI.BunifuCustomLabel();
            this.bunifuCustomLabel3 = new Bunifu.Framework.UI.BunifuCustomLabel();
            this.SaveExpidationMode = new System.Windows.Forms.Button();
            this.CancelExpéditionMode = new System.Windows.Forms.Button();
            this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            this.Header.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CloseButton)).BeginInit();
            this.DefaultExpeditinMode.SuspendLayout();
            this.ExpeditionOrderDataGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
            this.SuspendLayout();
            // 
            // Header
            // 
            this.Header.Controls.Add(this.CloseButton);
            this.Header.Controls.Add(this.bunifuCustomLabel1);
            this.Header.Dock = System.Windows.Forms.DockStyle.Top;
            this.Header.Location = new System.Drawing.Point(0, 0);
            this.Header.Name = "Header";
            this.Header.Size = new System.Drawing.Size(808, 100);
            this.Header.TabIndex = 0;
            // 
            // CloseButton
            // 
            this.CloseButton.Image = ((System.Drawing.Image)(resources.GetObject("CloseButton.Image")));
            this.CloseButton.ImageHover = ((System.Drawing.Image)(resources.GetObject("CloseButton.ImageHover")));
            this.CloseButton.ImageNormale = ((System.Drawing.Image)(resources.GetObject("CloseButton.ImageNormale")));
            this.CloseButton.Location = new System.Drawing.Point(757, 12);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(28, 32);
            this.CloseButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.CloseButton.TabIndex = 2;
            this.CloseButton.TabStop = false;
            this.CloseButton.Click += new System.EventHandler(this.MappingExpeditionModeCloseButton_Click);
            // 
            // bunifuCustomLabel1
            // 
            this.bunifuCustomLabel1.AutoSize = true;
            this.bunifuCustomLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bunifuCustomLabel1.Location = new System.Drawing.Point(12, 41);
            this.bunifuCustomLabel1.Name = "bunifuCustomLabel1";
            this.bunifuCustomLabel1.Size = new System.Drawing.Size(305, 25);
            this.bunifuCustomLabel1.TabIndex = 0;
            this.bunifuCustomLabel1.Text = "Mappage des modes d\'expédition";
            // 
            // DefaultExpeditinMode
            // 
            this.DefaultExpeditinMode.Controls.Add(this.DefaultExpedition);
            this.DefaultExpeditinMode.Controls.Add(this.bunifuCustomLabel2);
            this.DefaultExpeditinMode.Dock = System.Windows.Forms.DockStyle.Top;
            this.DefaultExpeditinMode.Location = new System.Drawing.Point(0, 100);
            this.DefaultExpeditinMode.Name = "DefaultExpeditinMode";
            this.DefaultExpeditinMode.Size = new System.Drawing.Size(808, 68);
            this.DefaultExpeditinMode.TabIndex = 1;
            // 
            // DefaultExpedition
            // 
            this.DefaultExpedition.BackColor = System.Drawing.Color.Transparent;
            this.DefaultExpedition.BorderRadius = 3;
            this.DefaultExpedition.DisabledColor = System.Drawing.Color.Gray;
            this.DefaultExpedition.ForeColor = System.Drawing.Color.White;
            this.DefaultExpedition.Items = new string[0];
            this.DefaultExpedition.Location = new System.Drawing.Point(424, 17);
            this.DefaultExpedition.Name = "DefaultExpedition";
            this.DefaultExpedition.NomalColor = System.Drawing.Color.Gainsboro;
            this.DefaultExpedition.onHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(129)))), ((int)(((byte)(77)))));
            this.DefaultExpedition.selectedIndex = -1;
            this.DefaultExpedition.Size = new System.Drawing.Size(277, 35);
            this.DefaultExpedition.TabIndex = 1;
            // 
            // bunifuCustomLabel2
            // 
            this.bunifuCustomLabel2.AutoSize = true;
            this.bunifuCustomLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bunifuCustomLabel2.Location = new System.Drawing.Point(12, 17);
            this.bunifuCustomLabel2.Name = "bunifuCustomLabel2";
            this.bunifuCustomLabel2.Size = new System.Drawing.Size(226, 20);
            this.bunifuCustomLabel2.TabIndex = 0;
            this.bunifuCustomLabel2.Text = "Mode d\'expédition par defaut : ";
            // 
            // ExpeditionOrderDataGrid
            // 
            this.ExpeditionOrderDataGrid.AutoScroll = true;
            this.ExpeditionOrderDataGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ExpeditionOrderDataGrid.Controls.Add(this.bunifuCustomLabel4);
            this.ExpeditionOrderDataGrid.Controls.Add(this.bunifuCustomLabel3);
            this.ExpeditionOrderDataGrid.Location = new System.Drawing.Point(0, 174);
            this.ExpeditionOrderDataGrid.Name = "ExpeditionOrderDataGrid";
            this.ExpeditionOrderDataGrid.Padding = new System.Windows.Forms.Padding(0, 0, 0, 300);
            this.ExpeditionOrderDataGrid.Size = new System.Drawing.Size(808, 360);
            this.ExpeditionOrderDataGrid.TabIndex = 2;
            // 
            // bunifuCustomLabel4
            // 
            this.bunifuCustomLabel4.AutoSize = true;
            this.bunifuCustomLabel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bunifuCustomLabel4.Location = new System.Drawing.Point(400, 16);
            this.bunifuCustomLabel4.Name = "bunifuCustomLabel4";
            this.bunifuCustomLabel4.Size = new System.Drawing.Size(223, 25);
            this.bunifuCustomLabel4.TabIndex = 2;
            this.bunifuCustomLabel4.Text = "Mode d\'expédition Sage";
            // 
            // bunifuCustomLabel3
            // 
            this.bunifuCustomLabel3.AutoSize = true;
            this.bunifuCustomLabel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bunifuCustomLabel3.Location = new System.Drawing.Point(36, 16);
            this.bunifuCustomLabel3.Name = "bunifuCustomLabel3";
            this.bunifuCustomLabel3.Size = new System.Drawing.Size(124, 25);
            this.bunifuCustomLabel3.TabIndex = 1;
            this.bunifuCustomLabel3.Text = "Transporteur";
            // 
            // SaveExpidationMode
            // 
            this.SaveExpidationMode.Location = new System.Drawing.Point(561, 558);
            this.SaveExpidationMode.Name = "SaveExpidationMode";
            this.SaveExpidationMode.Size = new System.Drawing.Size(75, 23);
            this.SaveExpidationMode.TabIndex = 3;
            this.SaveExpidationMode.Text = "Accepter";
            this.SaveExpidationMode.UseVisualStyleBackColor = true;
            this.SaveExpidationMode.Click += new System.EventHandler(this.SaveExpidationMode_Click);
            // 
            // CancelExpéditionMode
            // 
            this.CancelExpéditionMode.Location = new System.Drawing.Point(687, 558);
            this.CancelExpéditionMode.Name = "CancelExpéditionMode";
            this.CancelExpéditionMode.Size = new System.Drawing.Size(75, 23);
            this.CancelExpéditionMode.TabIndex = 4;
            this.CancelExpéditionMode.Text = "Annuler";
            this.CancelExpéditionMode.UseVisualStyleBackColor = true;
            this.CancelExpéditionMode.Click += new System.EventHandler(this.CancelExpéditionMode_Click);
            // 
            // fileSystemWatcher1
            // 
            this.fileSystemWatcher1.EnableRaisingEvents = true;
            this.fileSystemWatcher1.SynchronizingObject = this;
            // 
            // MappingExpeditionMode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(808, 603);
            this.Controls.Add(this.CancelExpéditionMode);
            this.Controls.Add(this.SaveExpidationMode);
            this.Controls.Add(this.ExpeditionOrderDataGrid);
            this.Controls.Add(this.DefaultExpeditinMode);
            this.Controls.Add(this.Header);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MappingExpeditionMode";
            this.Text = "Form1";
            this.Header.ResumeLayout(false);
            this.Header.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CloseButton)).EndInit();
            this.DefaultExpeditinMode.ResumeLayout(false);
            this.DefaultExpeditinMode.PerformLayout();
            this.ExpeditionOrderDataGrid.ResumeLayout(false);
            this.ExpeditionOrderDataGrid.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel Header;
        private Bunifu.Framework.UI.BunifuCustomLabel bunifuCustomLabel1;
        private System.Windows.Forms.Panel DefaultExpeditinMode;
        private Bunifu.Framework.UI.BunifuDropdown DefaultExpedition;
        private Bunifu.Framework.UI.BunifuCustomLabel bunifuCustomLabel2;
        private WindowsFormsControlLibrary1.CustomImageButto CloseButton;
        private System.Windows.Forms.Panel ExpeditionOrderDataGrid;
        private Bunifu.Framework.UI.BunifuCustomLabel bunifuCustomLabel4;
        private Bunifu.Framework.UI.BunifuCustomLabel bunifuCustomLabel3;
        private System.Windows.Forms.Button SaveExpidationMode;
        private System.Windows.Forms.Button CancelExpéditionMode;
        private System.IO.FileSystemWatcher fileSystemWatcher1;
    }
}