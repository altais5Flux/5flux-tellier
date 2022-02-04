namespace WebservicesSage
{
    partial class EditArticle
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditArticle));
            this.CategoriesTree = new System.Windows.Forms.TreeView();
            this.label1 = new System.Windows.Forms.Label();
            this.ArticleInformation = new System.Windows.Forms.TabControl();
            this.Information = new System.Windows.Forms.TabPage();
            this.ArticleActifLabel = new System.Windows.Forms.Label();
            this.IsActif = new Bunifu.Framework.UI.BunifuiOSSwitch();
            this.RestLinkRewriteLabel = new System.Windows.Forms.Label();
            this.RestMetaDescriptionLabel = new System.Windows.Forms.Label();
            this.LinkRewrite = new System.Windows.Forms.TextBox();
            this.MetaDescription = new System.Windows.Forms.TextBox();
            this.RestMetaTitleLabel = new System.Windows.Forms.Label();
            this.MetaTitle = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.NameLabel = new System.Windows.Forms.Label();
            this.Description = new System.Windows.Forms.RichTextBox();
            this.ShortDescription = new System.Windows.Forms.RichTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.Tags = new System.Windows.Forms.TextBox();
            this.NameArticle = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Declinaison = new System.Windows.Forms.TabPage();
            this.label10 = new System.Windows.Forms.Label();
            this.GammesDataGrid = new System.Windows.Forms.DataGridView();
            this.Price = new System.Windows.Forms.TabPage();
            this.PrixTTC = new System.Windows.Forms.TextBox();
            this.PrixVendre = new System.Windows.Forms.TextBox();
            this.PrixAchat = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.UploadPdf = new System.Windows.Forms.Button();
            this.SelectPdf = new System.Windows.Forms.Button();
            this.ArticleName = new System.Windows.Forms.Label();
            this.CloseButton = new WindowsFormsControlLibrary1.CustomImageButto();
            this.button1 = new System.Windows.Forms.Button();
            this.SelectJpeg = new System.Windows.Forms.Button();
            this.UploadJpeg = new System.Windows.Forms.Button();
            this.ArticleInformation.SuspendLayout();
            this.Information.SuspendLayout();
            this.Declinaison.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GammesDataGrid)).BeginInit();
            this.Price.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CloseButton)).BeginInit();
            this.SuspendLayout();
            // 
            // CategoriesTree
            // 
            this.CategoriesTree.CheckBoxes = true;
            this.CategoriesTree.Location = new System.Drawing.Point(22, 45);
            this.CategoriesTree.Name = "CategoriesTree";
            this.CategoriesTree.Size = new System.Drawing.Size(154, 456);
            this.CategoriesTree.TabIndex = 0;
            this.CategoriesTree.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterCheck);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(19, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Categories de Prestashop";
            // 
            // ArticleInformation
            // 
            this.ArticleInformation.Controls.Add(this.Information);
            this.ArticleInformation.Controls.Add(this.Declinaison);
            this.ArticleInformation.Controls.Add(this.Price);
            this.ArticleInformation.Controls.Add(this.tabPage1);
            this.ArticleInformation.Location = new System.Drawing.Point(191, 73);
            this.ArticleInformation.Name = "ArticleInformation";
            this.ArticleInformation.SelectedIndex = 0;
            this.ArticleInformation.Size = new System.Drawing.Size(739, 428);
            this.ArticleInformation.TabIndex = 2;
            // 
            // Information
            // 
            this.Information.Controls.Add(this.ArticleActifLabel);
            this.Information.Controls.Add(this.IsActif);
            this.Information.Controls.Add(this.RestLinkRewriteLabel);
            this.Information.Controls.Add(this.RestMetaDescriptionLabel);
            this.Information.Controls.Add(this.LinkRewrite);
            this.Information.Controls.Add(this.MetaDescription);
            this.Information.Controls.Add(this.RestMetaTitleLabel);
            this.Information.Controls.Add(this.MetaTitle);
            this.Information.Controls.Add(this.label9);
            this.Information.Controls.Add(this.label8);
            this.Information.Controls.Add(this.label7);
            this.Information.Controls.Add(this.label6);
            this.Information.Controls.Add(this.NameLabel);
            this.Information.Controls.Add(this.Description);
            this.Information.Controls.Add(this.ShortDescription);
            this.Information.Controls.Add(this.label5);
            this.Information.Controls.Add(this.label4);
            this.Information.Controls.Add(this.Tags);
            this.Information.Controls.Add(this.NameArticle);
            this.Information.Controls.Add(this.label3);
            this.Information.Controls.Add(this.label2);
            this.Information.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Information.Location = new System.Drawing.Point(4, 22);
            this.Information.Name = "Information";
            this.Information.Padding = new System.Windows.Forms.Padding(3);
            this.Information.Size = new System.Drawing.Size(731, 402);
            this.Information.TabIndex = 0;
            this.Information.Text = "Informations";
            this.Information.UseVisualStyleBackColor = true;
            // 
            // ArticleActifLabel
            // 
            this.ArticleActifLabel.AutoSize = true;
            this.ArticleActifLabel.ForeColor = System.Drawing.Color.Orange;
            this.ArticleActifLabel.Location = new System.Drawing.Point(6, 365);
            this.ArticleActifLabel.Name = "ArticleActifLabel";
            this.ArticleActifLabel.Size = new System.Drawing.Size(69, 13);
            this.ArticleActifLabel.TabIndex = 20;
            this.ArticleActifLabel.Text = "Article Actif : ";
            // 
            // IsActif
            // 
            this.IsActif.BackColor = System.Drawing.Color.Transparent;
            this.IsActif.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("IsActif.BackgroundImage")));
            this.IsActif.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.IsActif.Cursor = System.Windows.Forms.Cursors.Hand;
            this.IsActif.Location = new System.Drawing.Point(114, 358);
            this.IsActif.Name = "IsActif";
            this.IsActif.OffColor = System.Drawing.Color.Gray;
            this.IsActif.OnColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(202)))), ((int)(((byte)(94)))));
            this.IsActif.Size = new System.Drawing.Size(35, 20);
            this.IsActif.TabIndex = 19;
            this.IsActif.Value = true;
            // 
            // RestLinkRewriteLabel
            // 
            this.RestLinkRewriteLabel.AutoSize = true;
            this.RestLinkRewriteLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RestLinkRewriteLabel.Location = new System.Drawing.Point(542, 321);
            this.RestLinkRewriteLabel.Name = "RestLinkRewriteLabel";
            this.RestLinkRewriteLabel.Size = new System.Drawing.Size(90, 15);
            this.RestLinkRewriteLabel.TabIndex = 18;
            this.RestLinkRewriteLabel.Text = "NB car restant :";
            // 
            // RestMetaDescriptionLabel
            // 
            this.RestMetaDescriptionLabel.AutoSize = true;
            this.RestMetaDescriptionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RestMetaDescriptionLabel.Location = new System.Drawing.Point(542, 280);
            this.RestMetaDescriptionLabel.Name = "RestMetaDescriptionLabel";
            this.RestMetaDescriptionLabel.Size = new System.Drawing.Size(90, 15);
            this.RestMetaDescriptionLabel.TabIndex = 17;
            this.RestMetaDescriptionLabel.Text = "NB car restant :";
            // 
            // LinkRewrite
            // 
            this.LinkRewrite.Location = new System.Drawing.Point(114, 320);
            this.LinkRewrite.MaxLength = 128;
            this.LinkRewrite.Name = "LinkRewrite";
            this.LinkRewrite.Size = new System.Drawing.Size(422, 20);
            this.LinkRewrite.TabIndex = 16;
            this.LinkRewrite.TextChanged += new System.EventHandler(this.LinkRewrite_TextChanged);
            // 
            // MetaDescription
            // 
            this.MetaDescription.Location = new System.Drawing.Point(114, 279);
            this.MetaDescription.MaxLength = 128;
            this.MetaDescription.Name = "MetaDescription";
            this.MetaDescription.Size = new System.Drawing.Size(422, 20);
            this.MetaDescription.TabIndex = 15;
            this.MetaDescription.TextChanged += new System.EventHandler(this.MetaDescription_TextChanged);
            // 
            // RestMetaTitleLabel
            // 
            this.RestMetaTitleLabel.AutoSize = true;
            this.RestMetaTitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RestMetaTitleLabel.Location = new System.Drawing.Point(542, 230);
            this.RestMetaTitleLabel.Name = "RestMetaTitleLabel";
            this.RestMetaTitleLabel.Size = new System.Drawing.Size(90, 15);
            this.RestMetaTitleLabel.TabIndex = 14;
            this.RestMetaTitleLabel.Text = "NB car restant :";
            // 
            // MetaTitle
            // 
            this.MetaTitle.Location = new System.Drawing.Point(114, 230);
            this.MetaTitle.MaxLength = 128;
            this.MetaTitle.Name = "MetaTitle";
            this.MetaTitle.Size = new System.Drawing.Size(422, 20);
            this.MetaTitle.TabIndex = 13;
            this.MetaTitle.TextChanged += new System.EventHandler(this.MetaTitle_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Orange;
            this.label9.Location = new System.Drawing.Point(6, 320);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(85, 13);
            this.label9.TabIndex = 12;
            this.label9.Text = "URL Simplifiée : ";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.Orange;
            this.label8.Location = new System.Drawing.Point(6, 279);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(93, 13);
            this.label8.TabIndex = 11;
            this.label8.Text = "Meta Description :";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Orange;
            this.label7.Location = new System.Drawing.Point(6, 233);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Balise Titre :";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label6.Location = new System.Drawing.Point(111, 178);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(484, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Chaque mot-clé doit être suivi d\'une virgule. Les caractères suivants sont interd" +
    "its : !<;>;?=+#\"°{}_$%.";
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = true;
            this.NameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NameLabel.Location = new System.Drawing.Point(542, 13);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(90, 15);
            this.NameLabel.TabIndex = 8;
            this.NameLabel.Text = "NB car restant :";
            // 
            // Description
            // 
            this.Description.Location = new System.Drawing.Point(382, 60);
            this.Description.Name = "Description";
            this.Description.Size = new System.Drawing.Size(297, 74);
            this.Description.TabIndex = 7;
            this.Description.Text = "";
            // 
            // ShortDescription
            // 
            this.ShortDescription.Location = new System.Drawing.Point(9, 60);
            this.ShortDescription.Name = "ShortDescription";
            this.ShortDescription.Size = new System.Drawing.Size(301, 74);
            this.ShortDescription.TabIndex = 6;
            this.ShortDescription.Text = "";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Orange;
            this.label5.Location = new System.Drawing.Point(6, 155);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 15);
            this.label5.TabIndex = 5;
            this.label5.Text = "Mots-clés";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Orange;
            this.label4.Location = new System.Drawing.Point(379, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 15);
            this.label4.TabIndex = 4;
            this.label4.Text = "Description";
            // 
            // Tags
            // 
            this.Tags.Location = new System.Drawing.Point(114, 155);
            this.Tags.Name = "Tags";
            this.Tags.Size = new System.Drawing.Size(576, 20);
            this.Tags.TabIndex = 3;
            // 
            // NameArticle
            // 
            this.NameArticle.Location = new System.Drawing.Point(114, 12);
            this.NameArticle.MaxLength = 128;
            this.NameArticle.Name = "NameArticle";
            this.NameArticle.Size = new System.Drawing.Size(422, 20);
            this.NameArticle.TabIndex = 2;
            this.NameArticle.TextChanged += new System.EventHandler(this.Name_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Orange;
            this.label3.Location = new System.Drawing.Point(6, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 15);
            this.label3.TabIndex = 1;
            this.label3.Text = "Résumé";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Orange;
            this.label2.Location = new System.Drawing.Point(6, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "Nom de l\'article : ";
            // 
            // Declinaison
            // 
            this.Declinaison.Controls.Add(this.label10);
            this.Declinaison.Controls.Add(this.GammesDataGrid);
            this.Declinaison.Location = new System.Drawing.Point(4, 22);
            this.Declinaison.Name = "Declinaison";
            this.Declinaison.Padding = new System.Windows.Forms.Padding(3);
            this.Declinaison.Size = new System.Drawing.Size(731, 402);
            this.Declinaison.TabIndex = 1;
            this.Declinaison.Text = "Déclinaison";
            this.Declinaison.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.label10.Location = new System.Drawing.Point(6, 19);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(141, 15);
            this.label10.TabIndex = 1;
            this.label10.Text = "Listes des Gammes :";
            // 
            // GammesDataGrid
            // 
            this.GammesDataGrid.AllowUserToAddRows = false;
            this.GammesDataGrid.AllowUserToDeleteRows = false;
            this.GammesDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GammesDataGrid.Location = new System.Drawing.Point(6, 46);
            this.GammesDataGrid.Name = "GammesDataGrid";
            this.GammesDataGrid.Size = new System.Drawing.Size(691, 281);
            this.GammesDataGrid.TabIndex = 0;
            // 
            // Price
            // 
            this.Price.Controls.Add(this.PrixTTC);
            this.Price.Controls.Add(this.PrixVendre);
            this.Price.Controls.Add(this.PrixAchat);
            this.Price.Controls.Add(this.label14);
            this.Price.Controls.Add(this.label13);
            this.Price.Controls.Add(this.label12);
            this.Price.Controls.Add(this.label11);
            this.Price.Location = new System.Drawing.Point(4, 22);
            this.Price.Name = "Price";
            this.Price.Padding = new System.Windows.Forms.Padding(3);
            this.Price.Size = new System.Drawing.Size(731, 402);
            this.Price.TabIndex = 2;
            this.Price.Text = "Prix";
            this.Price.UseVisualStyleBackColor = true;
            // 
            // PrixTTC
            // 
            this.PrixTTC.Enabled = false;
            this.PrixTTC.Location = new System.Drawing.Point(141, 71);
            this.PrixTTC.Name = "PrixTTC";
            this.PrixTTC.Size = new System.Drawing.Size(100, 20);
            this.PrixTTC.TabIndex = 6;
            // 
            // PrixVendre
            // 
            this.PrixVendre.Location = new System.Drawing.Point(141, 45);
            this.PrixVendre.Name = "PrixVendre";
            this.PrixVendre.Size = new System.Drawing.Size(100, 20);
            this.PrixVendre.TabIndex = 5;
            // 
            // PrixAchat
            // 
            this.PrixAchat.Location = new System.Drawing.Point(141, 19);
            this.PrixAchat.Name = "PrixAchat";
            this.PrixAchat.Size = new System.Drawing.Size(100, 20);
            this.PrixAchat.TabIndex = 4;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(18, 108);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(95, 15);
            this.label14.TabIndex = 3;
            this.label14.Text = "Prix Spécifique :";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(18, 72);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(59, 15);
            this.label13.TabIndex = 2;
            this.label13.Text = "Prix TTC :";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(18, 46);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(91, 15);
            this.label12.TabIndex = 1;
            this.label12.Text = "Prix de vendre :";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(18, 19);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(77, 15);
            this.label11.TabIndex = 0;
            this.label11.Text = "Prix d\'achat :";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.UploadJpeg);
            this.tabPage1.Controls.Add(this.SelectJpeg);
            this.tabPage1.Controls.Add(this.UploadPdf);
            this.tabPage1.Controls.Add(this.SelectPdf);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(731, 402);
            this.tabPage1.TabIndex = 3;
            this.tabPage1.Text = "Updload File";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // UploadPdf
            // 
            this.UploadPdf.Location = new System.Drawing.Point(197, 59);
            this.UploadPdf.Name = "UploadPdf";
            this.UploadPdf.Size = new System.Drawing.Size(75, 23);
            this.UploadPdf.TabIndex = 1;
            this.UploadPdf.Text = "Upload";
            this.UploadPdf.UseVisualStyleBackColor = true;
            this.UploadPdf.Click += new System.EventHandler(this.Button3_Click);
            // 
            // SelectPdf
            // 
            this.SelectPdf.Location = new System.Drawing.Point(44, 59);
            this.SelectPdf.Name = "SelectPdf";
            this.SelectPdf.Size = new System.Drawing.Size(75, 23);
            this.SelectPdf.TabIndex = 0;
            this.SelectPdf.Text = "Select file";
            this.SelectPdf.UseVisualStyleBackColor = true;
            this.SelectPdf.Click += new System.EventHandler(this.Button2_Click);
            // 
            // ArticleName
            // 
            this.ArticleName.AutoSize = true;
            this.ArticleName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ArticleName.Location = new System.Drawing.Point(213, 45);
            this.ArticleName.Name = "ArticleName";
            this.ArticleName.Size = new System.Drawing.Size(121, 13);
            this.ArticleName.TabIndex = 3;
            this.ArticleName.Text = "Edition de l\'article : ";
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
            this.button1.Text = "M.A.J  article";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.SaveInfos_Click);
            // 
            // SelectJpeg
            // 
            this.SelectJpeg.Location = new System.Drawing.Point(44, 174);
            this.SelectJpeg.Name = "SelectJpeg";
            this.SelectJpeg.Size = new System.Drawing.Size(75, 23);
            this.SelectJpeg.TabIndex = 2;
            this.SelectJpeg.Text = "Select file";
            this.SelectJpeg.UseVisualStyleBackColor = true;
            // 
            // UploadJpeg
            // 
            this.UploadJpeg.Location = new System.Drawing.Point(197, 174);
            this.UploadJpeg.Name = "UploadJpeg";
            this.UploadJpeg.Size = new System.Drawing.Size(75, 23);
            this.UploadJpeg.TabIndex = 3;
            this.UploadJpeg.Text = "Upload";
            this.UploadJpeg.UseVisualStyleBackColor = true;
            // 
            // EditArticle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(942, 561);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.ArticleName);
            this.Controls.Add(this.ArticleInformation);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CategoriesTree);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "EditArticle";
            this.Text = "Edit Article";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.EditArticle_MouseDown);
            this.ArticleInformation.ResumeLayout(false);
            this.Information.ResumeLayout(false);
            this.Information.PerformLayout();
            this.Declinaison.ResumeLayout(false);
            this.Declinaison.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GammesDataGrid)).EndInit();
            this.Price.ResumeLayout(false);
            this.Price.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CloseButton)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView CategoriesTree;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl ArticleInformation;
        private System.Windows.Forms.TabPage Information;
        private System.Windows.Forms.TabPage Declinaison;
        private System.Windows.Forms.Label ArticleName;
        private WindowsFormsControlLibrary1.CustomImageButto CloseButton;
        private System.Windows.Forms.TextBox Tags;
        private System.Windows.Forms.TextBox NameArticle;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RichTextBox ShortDescription;
        private System.Windows.Forms.Label NameLabel;
        private System.Windows.Forms.RichTextBox Description;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox MetaDescription;
        private System.Windows.Forms.Label RestMetaTitleLabel;
        private System.Windows.Forms.TextBox MetaTitle;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label RestLinkRewriteLabel;
        private System.Windows.Forms.Label RestMetaDescriptionLabel;
        private System.Windows.Forms.TextBox LinkRewrite;
        private System.Windows.Forms.TabPage Price;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DataGridView GammesDataGrid;
        private System.Windows.Forms.Label ArticleActifLabel;
        private Bunifu.Framework.UI.BunifuiOSSwitch IsActif;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox PrixTTC;
        private System.Windows.Forms.TextBox PrixVendre;
        private System.Windows.Forms.TextBox PrixAchat;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button SelectPdf;
        private System.Windows.Forms.Button UploadPdf;
        private System.Windows.Forms.Button UploadJpeg;
        private System.Windows.Forms.Button SelectJpeg;
    }
}