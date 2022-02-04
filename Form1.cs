using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebservicesSage.Cotnroller;
using WebservicesSage.Object;
using WebservicesSage.Singleton;
using WebservicesSage.Utils;
using WebservicesSage.Utils.Enums;
using System.Runtime.InteropServices;

namespace WebservicesSage
{
    public partial class Form1 : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
                         int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        DataTable dtSales = new DataTable();
        string filterField = "category";
        public Form1()
        {
            InitializeComponent();
            SingletonUI.Instance.AllArticlesdataGridView = AllArticleDataGrid;
            SingletonUI.Instance.ActionList = ActionList;
            SingletonUI.Instance.FilterField = FilterField;
            SingletonUI.Instance.ArticleCounter = ArticleCounter;
            this.FormLoad();
            // this.Load += new EventHandler(Form1_Load);
        }
        private void FormLoad()
        {
            List<Article> articles = UtilsLinkedCommande.getAllArticleFromLocalDB();

            dtSales.Columns.Add("Reférence", typeof(string));
            dtSales.Columns[0].ReadOnly = true;
            dtSales.Columns.Add("Quantité", typeof(string));
            dtSales.Columns[1].ReadOnly = false;
            dtSales.Columns.Add("Prix d'achat", typeof(string));
            dtSales.Columns[2].ReadOnly = false;
            dtSales.Columns.Add("Prix de vente", typeof(string));
            dtSales.Columns[3].ReadOnly = false;
            dtSales.Columns.Add("Gamme", typeof(Boolean));
            dtSales.Columns[4].ReadOnly = true;
            dtSales.Columns.Add("Double Gamme", typeof(Boolean));
            dtSales.Columns[5].ReadOnly = true;
            dtSales.Columns.Add("Sommeil", typeof(Boolean));
            dtSales.Columns[6].ReadOnly = false;
            dtSales.Columns.Add("Categorie", typeof(string));
            dtSales.Columns[7].ReadOnly = true;
            SingletonUI.Instance.AllArticlesdataGridView.ReadOnly = false;
            int x = 0;
            foreach (Article article in articles)
            {
                StringBuilder s = new StringBuilder();
                s.Append(article.Reference);
                s.Append(";");
                s.Append(article.Stock);
                s.Append(";");
                s.Append(article.PrixAchat);
                s.Append(";");
                s.Append(article.PrixVente);
                s.Append(";");
                s.Append(article.isGamme);
                s.Append(";");
                s.Append(article.IsDoubleGamme);
                s.Append(";");
                s.Append(article.Sommeil);
                s.Append(";");
                s.Append("category");
                s.Append(x);
                string[] row = s.ToString().Split(';');
                dtSales.Rows.Add(row);
                x++;
            }
            SingletonUI.Instance.ArticleCounter.Text = SingletonUI.Instance.ArticleCounter.Text + x.ToString();
            SingletonUI.Instance.AllArticlesdataGridView.DataSource = dtSales;
            for (int i = 0; i < SingletonUI.Instance.AllArticlesdataGridView.Rows.Count; i++)
            {
                if (i % 2 != 0)
                {
                  //  AllArticleDataGrid.Rows[i].DefaultCellStyle.BackColor = Color.Orange;
                }
            }
        }

       

        /*
        private void Form1_Load(System.Object sender, System.EventArgs e)
        {
            SetupLayout();
            SetupDataGridView();
            PopulateDataGridView();
        }

        private void songsDataGridView_CellFormatting(object sender,
            System.Windows.Forms.DataGridViewCellFormattingEventArgs e)
        {
            if (e != null)
            {
                if (this.songsDataGridView.Columns[e.ColumnIndex].Name == "Release Date")
                {
                    if (e.Value != null)
                    {
                        try
                        {
                            e.Value = DateTime.Parse(e.Value.ToString())
                                .ToLongDateString();
                            e.FormattingApplied = true;
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("{0} is not a valid date.", e.Value.ToString());
                        }
                    }
                }
            }
        }

        private void addNewRowButton_Click(object sender, EventArgs e)
        {
            this.songsDataGridView.Rows.Add();
        }
        private void editRowButton_Click(object sender, EventArgs e)
        {
           int x = this.songsDataGridView.SelectedRows[0].Index;
            //this.songsDataGridView.Rows.Clear(); //clear the collection;
            var category = Category.FromJson(UtilsWebservices.SendData(UtilsConfig.BaseUrl + EnumEndPoint.Article.Value, "getArticle"));
            foreach (KeyValuePair<string, Dictionary<string,Category>> kvp in category)
            {
                foreach (KeyValuePair<string,Category> item in kvp.Value)
                {
                    string name = item.Value.Infos.Name;
                    long idCat = item.Value.Infos.IdCategory;
                    long idCatPar = item.Value.Infos.IdParent;
                }
                x++;
            }
        }

        private void deleteRowButton_Click(object sender, EventArgs e)
        {
            if (this.songsDataGridView.SelectedRows.Count > 0 &&
                this.songsDataGridView.SelectedRows[0].Index !=
                this.songsDataGridView.Rows.Count - 1)
            {
                this.songsDataGridView.Rows.RemoveAt(
                    this.songsDataGridView.SelectedRows[0].Index);
            }
        }

        private void SetupLayout()
        {
            this.Size = new Size(600, 500);

            addNewRowButton.Text = "Add Row";
            addNewRowButton.Location = new Point(10, 10);
            addNewRowButton.Click += new EventHandler(addNewRowButton_Click);

            deleteRowButton.Text = "Delete Row";
            deleteRowButton.Location = new Point(100, 10);
            deleteRowButton.Click += new EventHandler(deleteRowButton_Click);

            editRowButton.Text = "Clear Rows";
            editRowButton.Location = new Point(200, 10);
            editRowButton.Click += new EventHandler(editRowButton_Click);

            buttonPanel.Controls.Add(addNewRowButton);
            buttonPanel.Controls.Add(deleteRowButton);
            buttonPanel.Controls.Add(editRowButton);

            buttonPanel.Height = 50;
            buttonPanel.Dock = DockStyle.Bottom;
            this.Controls.Add(this.buttonPanel);
        }

        private void SetupDataGridView()
        {
            this.Controls.Add(songsDataGridView);

            songsDataGridView.ColumnCount = 5;

            songsDataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            songsDataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            songsDataGridView.ColumnHeadersDefaultCellStyle.Font =
                new Font(songsDataGridView.Font, FontStyle.Bold);

            songsDataGridView.Name = "songsDataGridView";
            songsDataGridView.Location = new Point(8, 8);
            songsDataGridView.Size = new Size(500, 250);
            songsDataGridView.AutoSizeRowsMode =
                DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            songsDataGridView.ColumnHeadersBorderStyle =
                DataGridViewHeaderBorderStyle.Single;
            songsDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            songsDataGridView.GridColor = Color.Black;
            songsDataGridView.RowHeadersVisible = false;
            
            songsDataGridView.Columns[0].Name = "Release Date";
            songsDataGridView.Columns[1].Name = "Track";
            songsDataGridView.Columns[2].Name = "Title";
            songsDataGridView.Columns[3].Name = "Artist";
            songsDataGridView.Columns[4].Name = "Album";
            songsDataGridView.Columns[4].DefaultCellStyle.Font =
                new Font(songsDataGridView.DefaultCellStyle.Font, FontStyle.Italic);

            songsDataGridView.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            songsDataGridView.MultiSelect = false;
            songsDataGridView.Dock = DockStyle.Fill;

            songsDataGridView.CellFormatting += new
                DataGridViewCellFormattingEventHandler(
                songsDataGridView_CellFormatting);
        }

        private void PopulateDataGridView()
        {

            string[] row0 = { "11/22/1968", "29", "Revolution 9",
            "Beatles", "The Beatles [White Album]" };
            string[] row1 = { "1960", "6", "Fools Rush In",
            "Frank Sinatra", "Nice 'N' Easy" };
            string[] row2 = { "11/11/1971", "1", "One of These Days",
            "Pink Floyd", "Meddle" };
            string[] row3 = { "1988", "7", "Where Is My Mind?",
            "Pixies", "Surfer Rosa" };
            string[] row4 = { "5/1981", "9", "Can't Find My Mind",
            "Cramps", "Psychedelic Jungle" };
            string[] row5 = { "6/10/2003", "13",
            "Scatterbrain. (As Dead As Leaves.)",
            "Radiohead", "Hail to the Thief" };
            string[] row6 = { "6/30/1992", "3", "Dress", "P J Harvey", "Dry" };
            //testing maximum number of line
            /*for (int i = 0; i < 100; i++)
            {
                string[] row7 = { i.ToString(), "3", "Dress", "P J Harvey", "Dry" };
                songsDataGridView.Rows.Add(row7);
            }*/
        /*songsDataGridView.Rows.Add(10);




        songsDataGridView.Rows.Add(row0);
        songsDataGridView.Rows.Add(row1);
        songsDataGridView.Rows.Add(row2);
        songsDataGridView.Rows.Add(row3);
        songsDataGridView.Rows.Add(row4);
        songsDataGridView.Rows.Add(row5);
        songsDataGridView.Rows.Add(row6);

        songsDataGridView.Columns[0].DisplayIndex = 3;
        songsDataGridView.Columns[1].DisplayIndex = 4;
        songsDataGridView.Columns[2].DisplayIndex = 0;
        songsDataGridView.Columns[3].DisplayIndex = 1;
        songsDataGridView.Columns[4].DisplayIndex = 2;
    }
    */
        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            //dtSales.DefaultView.RowFilter = string.Format("[{0}] LIKE '%{1}%'", this.filterField, this.Filter.Text);
            int x = SingletonUI.Instance.AllArticlesdataGridView.RowCount ;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            List<ArticleNomenclature> ArticleNomenclature = new List<ArticleNomenclature>();
            var gescom = SingletonConnection.Instance.Gescom;

            var articleSageObj = gescom.FactoryArticle.List;

            var articles = ControllerArticle.GetListOfClientToProcess(articleSageObj);



            foreach (Article article in articles)
            {
                UtilsLinkedCommande.addArticleToLocalDB(article);
            }
            if (SingletonUI.Instance.AllArticlesdataGridView.Rows.Count >0)
            {
                SingletonUI.Instance.AllArticlesdataGridView.DataSource = null;
                //SingletonUI.Instance.AllArticlesdataGridView.Rows.Clear();
            }
            this.FormLoad();
        }

        private void DataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex>=0 && e.ColumnIndex == 0)
            {
                var dataIndexNo = SingletonUI.Instance.AllArticlesdataGridView.Rows[e.RowIndex].Index;
                string cellValue = SingletonUI.Instance.AllArticlesdataGridView.Rows[e.RowIndex].Cells[0].Value.ToString();
                Article article = UtilsLinkedCommande.getArticleFromLocalDB(cellValue);
                EditArticle editArticle = new EditArticle(article);
                editArticle.Show();
               // MessageBox.Show("index du ligne = " + (dataIndexNo+1).ToString() + " la reference de l'article selectionné est : "+ cellValue.ToString());
            }
            
        }

        private void Filter_TextChanged(object sender, EventArgs e)
        {
            switch (SingletonUI.Instance.FilterField.Text)
            {
                case "Reférence article":
                    filterField = "Reférence";
                    break;
                case "Catégorie article":
                    filterField = "Categorie";
                    break;
                default:
                    filterField = "Categorie";
                    break;
            }
            dtSales.DefaultView.RowFilter = string.Format("[{0}] LIKE '%{1}%'", this.filterField, this.Filter.Text);
        }
        private void Action_Click(object sender, EventArgs e)
        {
            //ToDo les methodes pour chaque action
            switch (SingletonUI.Instance.ActionList.Text)
            {
                case "test":
                    //MessageBox.Show("l'action selectioner est : " + SingletonUI.Instance.ActionList.Text);
                    UtilsLinkedCommande.createCategory();
                   //EditArticle editArticle = new EditArticle();
                    //editArticle.Show();
                    break;
                case "test2":
                    MessageBox.Show("l'action selectioner est : " + SingletonUI.Instance.ActionList.Text);
                    break;
                case "test3":
                    MessageBox.Show("l'action selectioner est : " + SingletonUI.Instance.ActionList.Text);
                    break;
                case "send file":
                    
                    //MessageBox.Show("l'action selectioner est : " + SingletonUI.Instance.ActionList.Text);
                    break;
                default:
                    break;
            }
            
        }
        private void Form1_MouseDown(object sender,
        System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void AllArticleDataGrid_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {

        }
    }
}
