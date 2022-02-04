namespace WebservicesSage
{
    partial class Devis
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Devis));
            this.ArticleName = new System.Windows.Forms.Label();
            this.CloseButton = new WindowsFormsControlLibrary1.CustomImageButto();
            this.button1 = new System.Windows.Forms.Button();
            this.DevisList = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.CloseButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DevisList)).BeginInit();
            this.SuspendLayout();
            // 
            // ArticleName
            // 
            this.ArticleName.AutoSize = true;
            this.ArticleName.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ArticleName.Location = new System.Drawing.Point(361, 19);
            this.ArticleName.Name = "ArticleName";
            this.ArticleName.Size = new System.Drawing.Size(214, 31);
            this.ArticleName.TabIndex = 3;
            this.ArticleName.Text = "Listes de Devis";
            // 
            // CloseButton
            // 
            this.CloseButton.BackColor = System.Drawing.SystemColors.Control;
            this.CloseButton.Image = ((System.Drawing.Image)(resources.GetObject("CloseButton.Image")));
            this.CloseButton.ImageHover = ((System.Drawing.Image)(resources.GetObject("CloseButton.ImageHover")));
            this.CloseButton.ImageNormale = ((System.Drawing.Image)(resources.GetObject("CloseButton.ImageNormale")));
            this.CloseButton.Location = new System.Drawing.Point(879, 18);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(28, 32);
            this.CloseButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.CloseButton.TabIndex = 4;
            this.CloseButton.TabStop = false;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(721, 516);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(173, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Importer Devis";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.SaveInfos_Click);
            // 
            // DevisList
            // 
            this.DevisList.AllowUserToAddRows = false;
            this.DevisList.AllowUserToDeleteRows = false;
            this.DevisList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DevisList.Location = new System.Drawing.Point(28, 67);
            this.DevisList.Name = "DevisList";
            this.DevisList.ReadOnly = true;
            this.DevisList.Size = new System.Drawing.Size(879, 421);
            this.DevisList.TabIndex = 6;
            // 
            // Devis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(942, 561);
            this.Controls.Add(this.DevisList);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.ArticleName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Devis";
            this.Text = "Edit Article";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.EditArticle_MouseDown);
            ((System.ComponentModel.ISupportInitialize)(this.CloseButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DevisList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label ArticleName;
        private WindowsFormsControlLibrary1.CustomImageButto CloseButton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView DevisList;
    }
}