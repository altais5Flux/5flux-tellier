namespace WebservicesSage
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.CloseButton = new WindowsFormsControlLibrary1.CustomImageButto();
            this.AllArticleDataGrid = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.Filter = new System.Windows.Forms.TextBox();
            this.Action = new System.Windows.Forms.Button();
            this.ActionList = new System.Windows.Forms.ComboBox();
            this.FilterField = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ArticleCounter = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.CloseButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AllArticleDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // CloseButton
            // 
            this.CloseButton.Image = ((System.Drawing.Image)(resources.GetObject("CloseButton.Image")));
            this.CloseButton.ImageHover = ((System.Drawing.Image)(resources.GetObject("CloseButton.ImageHover")));
            this.CloseButton.ImageNormale = ((System.Drawing.Image)(resources.GetObject("CloseButton.ImageNormale")));
            this.CloseButton.Location = new System.Drawing.Point(893, 12);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(28, 32);
            this.CloseButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.CloseButton.TabIndex = 2;
            this.CloseButton.TabStop = false;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // AllArticleDataGrid
            // 
            this.AllArticleDataGrid.AllowUserToAddRows = false;
            this.AllArticleDataGrid.AllowUserToDeleteRows = false;
            this.AllArticleDataGrid.AllowUserToOrderColumns = true;
            this.AllArticleDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.AllArticleDataGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnF2;
            this.AllArticleDataGrid.Location = new System.Drawing.Point(34, 110);
            this.AllArticleDataGrid.Name = "AllArticleDataGrid";
            this.AllArticleDataGrid.ReadOnly = true;
            this.AllArticleDataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.AllArticleDataGrid.Size = new System.Drawing.Size(860, 382);
            this.AllArticleDataGrid.TabIndex = 3;
            this.AllArticleDataGrid.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView1_CellContentDoubleClick);
            this.AllArticleDataGrid.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.AllArticleDataGrid_RowPrePaint);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(612, 526);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(761, 517);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(160, 41);
            this.button2.TabIndex = 5;
            this.button2.Text = "Mise a jours db a partir de sage";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // Filter
            // 
            this.Filter.Location = new System.Drawing.Point(208, 73);
            this.Filter.Name = "Filter";
            this.Filter.Size = new System.Drawing.Size(121, 20);
            this.Filter.TabIndex = 6;
            this.Filter.TextChanged += new System.EventHandler(this.Filter_TextChanged);
            // 
            // Action
            // 
            this.Action.Location = new System.Drawing.Point(612, 71);
            this.Action.Name = "Action";
            this.Action.Size = new System.Drawing.Size(75, 23);
            this.Action.TabIndex = 8;
            this.Action.Text = "Action";
            this.Action.UseVisualStyleBackColor = true;
            this.Action.Click += new System.EventHandler(this.Action_Click);
            // 
            // ActionList
            // 
            this.ActionList.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ActionList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ActionList.FormattingEnabled = true;
            this.ActionList.Items.AddRange(new object[] {
            "send file",
            "test",
            "test2",
            "test3"});
            this.ActionList.Location = new System.Drawing.Point(595, 43);
            this.ActionList.Name = "ActionList";
            this.ActionList.Size = new System.Drawing.Size(121, 21);
            this.ActionList.Sorted = true;
            this.ActionList.TabIndex = 9;
            // 
            // FilterField
            // 
            this.FilterField.Cursor = System.Windows.Forms.Cursors.Hand;
            this.FilterField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FilterField.FormattingEnabled = true;
            this.FilterField.Items.AddRange(new object[] {
            "Catégorie article",
            "Reférence article"});
            this.FilterField.Location = new System.Drawing.Point(208, 40);
            this.FilterField.Name = "FilterField";
            this.FilterField.Size = new System.Drawing.Size(121, 21);
            this.FilterField.Sorted = true;
            this.FilterField.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(152, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Filtrer la liste avec ce champs :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "rechercher :";
            // 
            // ArticleCounter
            // 
            this.ArticleCounter.AutoSize = true;
            this.ArticleCounter.Location = new System.Drawing.Point(31, 517);
            this.ArticleCounter.Name = "ArticleCounter";
            this.ArticleCounter.Size = new System.Drawing.Size(83, 13);
            this.ArticleCounter.TabIndex = 13;
            this.ArticleCounter.Text = "Articles publiés :";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(958, 600);
            this.ControlBox = false;
            this.Controls.Add(this.ArticleCounter);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.FilterField);
            this.Controls.Add(this.ActionList);
            this.Controls.Add(this.Action);
            this.Controls.Add(this.Filter);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.AllArticleDataGrid);
            this.Controls.Add(this.CloseButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.Text = "All Articles Liste";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            ((System.ComponentModel.ISupportInitialize)(this.CloseButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AllArticleDataGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private WindowsFormsControlLibrary1.CustomImageButto CloseButton;
        private System.Windows.Forms.DataGridView AllArticleDataGrid;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox Filter;
        private System.Windows.Forms.Button Action;
        private System.Windows.Forms.ComboBox ActionList;
        private System.Windows.Forms.ComboBox FilterField;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label ArticleCounter;
    }
}