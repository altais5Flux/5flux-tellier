using WebservicesSage.Properties;

namespace WebservicesSage
{
    partial class MappingOrdersStaut
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MappingOrdersStaut));
            this.HeaderPanel = new System.Windows.Forms.Panel();
            this.bunifuCustomLabel1 = new Bunifu.Framework.UI.BunifuCustomLabel();
            this.CloseButton = new WindowsFormsControlLibrary1.CustomImageButto();
            this.BodyPanel = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SageDoc2 = new Bunifu.Framework.UI.BunifuDropdown();
            this.PrestaId1 = new Bunifu.Framework.UI.BunifuDropdown();
            this.PrestaId2 = new Bunifu.Framework.UI.BunifuDropdown();
            this.SageDoc1 = new Bunifu.Framework.UI.BunifuDropdown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.FooterPanel = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SageDoc3 = new Bunifu.Framework.UI.BunifuDropdown();
            this.PrestaId3 = new Bunifu.Framework.UI.BunifuDropdown();
            this.label3 = new System.Windows.Forms.Label();
            this.HeaderPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CloseButton)).BeginInit();
            this.BodyPanel.SuspendLayout();
            this.FooterPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // HeaderPanel
            // 
            this.HeaderPanel.Controls.Add(this.bunifuCustomLabel1);
            this.HeaderPanel.Controls.Add(this.CloseButton);
            this.HeaderPanel.Location = new System.Drawing.Point(0, 0);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new System.Drawing.Size(800, 100);
            this.HeaderPanel.TabIndex = 0;
            // 
            // bunifuCustomLabel1
            // 
            this.bunifuCustomLabel1.AutoSize = true;
            this.bunifuCustomLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bunifuCustomLabel1.Location = new System.Drawing.Point(29, 35);
            this.bunifuCustomLabel1.Name = "bunifuCustomLabel1";
            this.bunifuCustomLabel1.Size = new System.Drawing.Size(317, 25);
            this.bunifuCustomLabel1.TabIndex = 4;
            this.bunifuCustomLabel1.Text = "Mappage des Statut de commande";
            // 
            // CloseButton
            // 
            this.CloseButton.Image = ((System.Drawing.Image)(resources.GetObject("CloseButton.Image")));
            this.CloseButton.ImageHover = ((System.Drawing.Image)(resources.GetObject("CloseButton.ImageHover")));
            this.CloseButton.ImageNormale = ((System.Drawing.Image)(resources.GetObject("CloseButton.ImageNormale")));
            this.CloseButton.Location = new System.Drawing.Point(745, 28);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(28, 32);
            this.CloseButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.CloseButton.TabIndex = 3;
            this.CloseButton.TabStop = false;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // BodyPanel
            // 
            this.BodyPanel.Controls.Add(this.SageDoc3);
            this.BodyPanel.Controls.Add(this.PrestaId3);
            this.BodyPanel.Controls.Add(this.label3);
            this.BodyPanel.Controls.Add(this.label5);
            this.BodyPanel.Controls.Add(this.label4);
            this.BodyPanel.Controls.Add(this.SageDoc2);
            this.BodyPanel.Controls.Add(this.PrestaId1);
            this.BodyPanel.Controls.Add(this.PrestaId2);
            this.BodyPanel.Controls.Add(this.SageDoc1);
            this.BodyPanel.Controls.Add(this.label2);
            this.BodyPanel.Controls.Add(this.label1);
            this.BodyPanel.Location = new System.Drawing.Point(0, 98);
            this.BodyPanel.Name = "BodyPanel";
            this.BodyPanel.Size = new System.Drawing.Size(800, 296);
            this.BodyPanel.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(462, 17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(103, 20);
            this.label5.TabIndex = 15;
            this.label5.Text = "Statuts Sage";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(138, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(141, 20);
            this.label4.TabIndex = 14;
            this.label4.Text = "Statuts Magento 2";
            // 
            // SageDoc2
            // 
            this.SageDoc2.BackColor = System.Drawing.Color.Transparent;
            this.SageDoc2.BorderRadius = 3;
            this.SageDoc2.DisabledColor = System.Drawing.Color.Gray;
            this.SageDoc2.ForeColor = System.Drawing.Color.White;
            this.SageDoc2.Items = new string[0];
            this.SageDoc2.Location = new System.Drawing.Point(434, 140);
            this.SageDoc2.Name = "SageDoc2";
            this.SageDoc2.NomalColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.SageDoc2.onHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(129)))), ((int)(((byte)(77)))));
            this.SageDoc2.selectedIndex = -1;
            this.SageDoc2.Size = new System.Drawing.Size(217, 35);
            this.SageDoc2.TabIndex = 12;
            // 
            // PrestaId1
            // 
            this.PrestaId1.BackColor = System.Drawing.Color.Transparent;
            this.PrestaId1.BorderRadius = 3;
            this.PrestaId1.DisabledColor = System.Drawing.Color.Gray;
            this.PrestaId1.ForeColor = System.Drawing.Color.White;
            this.PrestaId1.Items = new string[0];
            this.PrestaId1.Location = new System.Drawing.Point(107, 65);
            this.PrestaId1.Name = "PrestaId1";
            this.PrestaId1.NomalColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.PrestaId1.onHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(129)))), ((int)(((byte)(77)))));
            this.PrestaId1.selectedIndex = -1;
            this.PrestaId1.Size = new System.Drawing.Size(217, 35);
            this.PrestaId1.TabIndex = 11;
            // 
            // PrestaId2
            // 
            this.PrestaId2.BackColor = System.Drawing.Color.Transparent;
            this.PrestaId2.BorderRadius = 3;
            this.PrestaId2.DisabledColor = System.Drawing.Color.Gray;
            this.PrestaId2.ForeColor = System.Drawing.Color.White;
            this.PrestaId2.Items = new string[0];
            this.PrestaId2.Location = new System.Drawing.Point(107, 140);
            this.PrestaId2.Name = "PrestaId2";
            this.PrestaId2.NomalColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.PrestaId2.onHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(129)))), ((int)(((byte)(77)))));
            this.PrestaId2.selectedIndex = -1;
            this.PrestaId2.Size = new System.Drawing.Size(217, 35);
            this.PrestaId2.TabIndex = 9;
            // 
            // SageDoc1
            // 
            this.SageDoc1.BackColor = System.Drawing.Color.Transparent;
            this.SageDoc1.BorderRadius = 3;
            this.SageDoc1.DisabledColor = System.Drawing.Color.Gray;
            this.SageDoc1.ForeColor = System.Drawing.Color.White;
            this.SageDoc1.Items = new string[0];
            this.SageDoc1.Location = new System.Drawing.Point(434, 65);
            this.SageDoc1.Name = "SageDoc1";
            this.SageDoc1.NomalColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.SageDoc1.onHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(129)))), ((int)(((byte)(77)))));
            this.SageDoc1.selectedIndex = -1;
            this.SageDoc1.Size = new System.Drawing.Size(217, 35);
            this.SageDoc1.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(42, 150);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(13, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(42, 76);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(13, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "1";
            // 
            // FooterPanel
            // 
            this.FooterPanel.Controls.Add(this.button2);
            this.FooterPanel.Controls.Add(this.button1);
            this.FooterPanel.Location = new System.Drawing.Point(0, 394);
            this.FooterPanel.Name = "FooterPanel";
            this.FooterPanel.Size = new System.Drawing.Size(800, 56);
            this.FooterPanel.TabIndex = 2;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(551, 21);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Annuler";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(672, 21);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Enregistrer";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // SageDoc3
            // 
            this.SageDoc3.BackColor = System.Drawing.Color.Transparent;
            this.SageDoc3.BorderRadius = 3;
            this.SageDoc3.DisabledColor = System.Drawing.Color.Gray;
            this.SageDoc3.ForeColor = System.Drawing.Color.White;
            this.SageDoc3.Items = new string[0];
            this.SageDoc3.Location = new System.Drawing.Point(434, 212);
            this.SageDoc3.Name = "SageDoc3";
            this.SageDoc3.NomalColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.SageDoc3.onHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(129)))), ((int)(((byte)(77)))));
            this.SageDoc3.selectedIndex = -1;
            this.SageDoc3.Size = new System.Drawing.Size(217, 35);
            this.SageDoc3.TabIndex = 18;
            // 
            // PrestaId3
            // 
            this.PrestaId3.BackColor = System.Drawing.Color.Transparent;
            this.PrestaId3.BorderRadius = 3;
            this.PrestaId3.DisabledColor = System.Drawing.Color.Gray;
            this.PrestaId3.ForeColor = System.Drawing.Color.White;
            this.PrestaId3.Items = new string[0];
            this.PrestaId3.Location = new System.Drawing.Point(107, 212);
            this.PrestaId3.Name = "PrestaId3";
            this.PrestaId3.NomalColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.PrestaId3.onHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(129)))), ((int)(((byte)(77)))));
            this.PrestaId3.selectedIndex = -1;
            this.PrestaId3.Size = new System.Drawing.Size(217, 35);
            this.PrestaId3.TabIndex = 17;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(42, 222);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(13, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "3";
            // 
            // MappingOrdersStaut
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.FooterPanel);
            this.Controls.Add(this.BodyPanel);
            this.Controls.Add(this.HeaderPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MappingOrdersStaut";
            this.Text = "MappingOrdersStaut";
            this.HeaderPanel.ResumeLayout(false);
            this.HeaderPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CloseButton)).EndInit();
            this.BodyPanel.ResumeLayout(false);
            this.BodyPanel.PerformLayout();
            this.FooterPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel HeaderPanel;
        private System.Windows.Forms.Panel BodyPanel;
        private System.Windows.Forms.Panel FooterPanel;
        private WindowsFormsControlLibrary1.CustomImageButto CloseButton;
        private Bunifu.Framework.UI.BunifuCustomLabel bunifuCustomLabel1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private Bunifu.Framework.UI.BunifuDropdown PrestaId2;
        private Bunifu.Framework.UI.BunifuDropdown SageDoc1;
        private Bunifu.Framework.UI.BunifuDropdown SageDoc2;
        private Bunifu.Framework.UI.BunifuDropdown PrestaId1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private Bunifu.Framework.UI.BunifuDropdown SageDoc3;
        private Bunifu.Framework.UI.BunifuDropdown PrestaId3;
        private System.Windows.Forms.Label label3;
    }
}