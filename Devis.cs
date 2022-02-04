using LiteDB;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Objets100cLib;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebservicesSage.Cotnroller;
using WebservicesSage.Object;
using WebservicesSage.Object.Categories;
using WebservicesSage.Object.DBObject;
using WebservicesSage.Singleton;
using WebservicesSage.Utils;
using WebservicesSage.Utils.Enums;

namespace WebservicesSage
{

    public partial class Devis : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
                         int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        DataTable dtSales = new DataTable();
        Article articleData;
        public void Categories(ProductCategories ProdCat, long level, long ParentId, TreeNode Root, Article RefArticle=null)
        {
            if (ProdCat.Level == 1)
            {
                Root.Name = ProdCat.Id.ToString();
                Root.Text = ProdCat.Name;
            }
            foreach (ProductCategories child in ProdCat.ChildrenData)
            {
                TreeNode node = new TreeNode();
                node.Name = child.Id.ToString();
                node.Text = child.Name;
                Root.Nodes.Add(node);
                Categories(child, child.Level, child.ParentId,node);
            }
            //return TreeView;
        }
       /* public void CreateNode(TreeNode node)
        {
            if (ds.Tables[0].Rows.Count == 0)
            {
                return;
            }
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                TreeNode tnode = new TreeNode(ds.Tables[0].Rows[i][1].ToString(), ds.Tables[0].Rows[i][0].ToString());
                tnode.SelectAction = TreeNodeSelectAction.Expand;
                node.ChildNodes.Add(tnode);
                CreateNode(tnode);
            }

        }*/
        public Devis(Article article = null)
        {
            InitializeComponent();
            articleData = article;
            SingletonUI.Instance.DevisList = DevisList;
            SingletonUI.Instance.ArticleName = ArticleName;
            this.DevisLoad();
            //EditArticleLoad(article.Reference);
            /*List<LinkedProductCategories> categoryList = new List<LinkedProductCategories>();
            using (var db = new LiteDatabase(@"SageData.db"))
            {
                var CategoryCollection = db.GetCollection<LinkedProductCategories>("Category");
                foreach (LinkedProductCategories category in CategoryCollection.FindAll())
                {
                    categoryList.Add(category);
                    
                }
                this.treeView1.Nodes.Add(categoryList[0].Name);
                int niveau = 0;
                for (int i = 1; i < categoryList.Count(); i++)
                {
                    if (categoryList[i].Parent_Category_id.Equals("0"))
                    {
                        this.treeView1.Nodes.Add(categoryList[i].Name);
                    }
                    else
                    {
                        if (!categoryList[i].Parent_Category_id.Equals(categoryList[i-1].))
                        {

                        }
                    }
                }
            }*/

            
        }
        private void DevisLoad()
        {
            dtSales.Columns.Add("Importer", typeof(Boolean));
            dtSales.Columns[0].ReadOnly = false;
            dtSales.Columns.Add("Id Devis", typeof(string));
            dtSales.Columns[1].ReadOnly = true;
            dtSales.Columns.Add("Date de Création", typeof(string));
            dtSales.Columns[2].ReadOnly = true;
            dtSales.Columns.Add("Date de modification", typeof(string));
            dtSales.Columns[3].ReadOnly = true;
            dtSales.Columns.Add("Active?", typeof(Boolean));
            dtSales.Columns[4].ReadOnly = true;
            dtSales.Columns.Add("Virtuel?", typeof(Boolean));
            dtSales.Columns[5].ReadOnly = true;
            dtSales.Columns.Add("Nbr d'article", typeof(string));
            dtSales.Columns[6].ReadOnly = true;
            dtSales.Columns.Add("Quantité articles", typeof(string));
            dtSales.Columns[7].ReadOnly = true;
            dtSales.Columns.Add("Nom Client", typeof(string));
            dtSales.Columns[8].ReadOnly = true;
            
            SingletonUI.Instance.DevisList.ReadOnly = false;
            int x = 0;
            var DevisSearch = Object.Devis.Devis.FromJson(UtilsWebservices.GetMagentoData("rest/V1/amasty_quote/search?searchCriteria"));
            if (DevisSearch.TotalCount > 0)
            {
                foreach (var item in DevisSearch.Items)
                {
                    StringBuilder s = new StringBuilder();
                    s.Append(false);
                    s.Append(";");
                    s.Append(item.Id.ToString());
                    s.Append(";");
                    s.Append(item.CreatedAt.Date.ToString());
                    s.Append(";");
                    s.Append(item.UpdatedAt.Date.ToString());
                    s.Append(";");
                    s.Append(item.IsActive);
                    s.Append(";");
                    s.Append(item.IsVirtual);
                    s.Append(";");
                    s.Append(item.ItemsCount.ToString());
                    s.Append(";");
                    s.Append(item.ItemsQty.ToString());
                    s.Append(";");
                    s.Append(item.Customer.Firstname+ " " + item.Customer.Lastname);
                    string[] row = s.ToString().Split(';');
                    dtSales.Rows.Add(row);
                    x++;
                }
                SingletonUI.Instance.DevisList.DataSource = dtSales;
            }
            else
            {
                //var json = JsonConvert.SerializeObject(DevisSearch);
                //string response = UtilsWebservices.SendDataJson(json, @"rest/V1/categories");
            }
            //get devis from Magento2
        }
        private void EditArticleLoad(string articleCT)
        {
            string response="";//= UtilsWebservices.SendDataNoParse(UtilsConfig.BaseUrl + EnumEndPoint.ArticleDetails.Value, "getArticle&articleCtNum=" + articleCT);
            if (!response.Equals("") && !response.Equals("[]"))
            {
                JArray infos = JArray.Parse(response);
                foreach (var info in infos)
                {
                    SingletonUI.Instance.NameArticle.Text = info["name"].ToString();
                    if (!string.IsNullOrEmpty(info["description_short"].ToString()))
                    {
                        string[] description_short1 = info["description_short"].ToString().Split(new string[] { "<p>" }, StringSplitOptions.None);
                        string[] description_short = description_short1[1].ToString().Split(new string[] { "</p>" }, StringSplitOptions.None);
                        SingletonUI.Instance.ShortDescription.Text = description_short[0].ToString();
                    }
                    if (!string.IsNullOrEmpty(info["description"].ToString()))
                    {
                        string[] description1 = info["description"].ToString().Split(new string[] { "<p>" }, StringSplitOptions.None);
                        string[] description = description1[1].ToString().Split(new string[] { "</p>" }, StringSplitOptions.None);
                        SingletonUI.Instance.Description.Text = description[0].ToString();
                    }
                    if (!string.IsNullOrEmpty(info["link_rewrite"].ToString()))
                    {
                        string linkRewrite = info["link_rewrite"].ToString().Replace('-', ' ');
                        SingletonUI.Instance.LinkRewrite.Text = linkRewrite;
                    }

                    SingletonUI.Instance.MetaDescription.Text = info["meta_description"].ToString();
                    SingletonUI.Instance.MetaTitle.Text = info["meta_title"].ToString();
                    if (info["sommeil"].ToString().Equals("1"))
                    {
                        SingletonUI.Instance.IsActif.Value = true;
                    }
                    else
                    {
                        SingletonUI.Instance.IsActif.Value = false;
                    }
                    foreach (JToken tag in info["tags"].Children())
                    {
                        SingletonUI.Instance.Tags.Text += tag["tagname"] + ", ";
                    }
                    SingletonUI.Instance.PrixAchat.Text = info["prixAchat"].ToString();
                    SingletonUI.Instance.PrixVendre.Text = info["prixVendre"].ToString();
                    SingletonUI.Instance.PrixTTC.Text = (((double)info["taxRate"] * (double)info["prixVendre"] /100)+((double)info["prixVendre"])).ToString();
                }
            }
        }
        private void GammeLoad(Article article)
        {
            
            if (article.isGamme)
            {
                if (article.IsDoubleGamme)
                {
                    dtSales.Columns.Add("Reférence", typeof(string));
                    dtSales.Columns[0].ReadOnly = true;
                    dtSales.Columns.Add("Code à Barre", typeof(string));
                    dtSales.Columns[1].ReadOnly = true;
                    dtSales.Columns.Add("Quantité", typeof(string));
                    dtSales.Columns[2].ReadOnly = true;
                    dtSales.Columns.Add("Prix Gamme", typeof(string));
                    dtSales.Columns[3].ReadOnly = true;
                    dtSales.Columns.Add("Intitulé Gamme 1", typeof(string));
                    dtSales.Columns[4].ReadOnly = true;
                    dtSales.Columns.Add("Valeur Gamme 1", typeof(string));
                    dtSales.Columns[5].ReadOnly = true;
                    dtSales.Columns.Add("Intitulé Gamme 2", typeof(string));
                    dtSales.Columns[6].ReadOnly = true;
                    dtSales.Columns.Add("Valeur Gamme 2", typeof(string));
                    dtSales.Columns[7].ReadOnly = true;
                    dtSales.Columns.Add("Sommeil", typeof(Boolean));
                    dtSales.Columns[8].ReadOnly = false;
                    SingletonUI.Instance.GammesDataGrid.ReadOnly = false;
                    int x = 0;
                    foreach (Gamme gamme in article.Gammes)
                    {
                        StringBuilder s = new StringBuilder();
                        s.Append(gamme.Reference);
                        s.Append(";");
                        s.Append(gamme.CodeBarre);
                        s.Append(";");
                        s.Append(gamme.Stock);
                        s.Append(";");
                        s.Append(gamme.Price);
                        s.Append(";");
                        s.Append(gamme.Intitule);
                        s.Append(";");
                        s.Append(gamme.Value_Intitule);
                        s.Append(";");
                        s.Append(gamme.Intitule2);
                        s.Append(";");
                        s.Append(gamme.Value_Intitule2);
                        s.Append(";");
                        s.Append(gamme.Sommeil);
                        string[] row = s.ToString().Split(';');
                        dtSales.Rows.Add(row);
                        x++;
                    }
                }
                else
                {
                    dtSales.Columns.Add("Reférence", typeof(string));
                    dtSales.Columns[0].ReadOnly = true;
                    dtSales.Columns.Add("Code à Barre", typeof(string));
                    dtSales.Columns[1].ReadOnly = true;
                    dtSales.Columns.Add("Quantité", typeof(string));
                    dtSales.Columns[2].ReadOnly = true;
                    dtSales.Columns.Add("Prix Gamme", typeof(string));
                    dtSales.Columns[3].ReadOnly = true;
                    dtSales.Columns.Add("Intitulé Gamme 1", typeof(string));
                    dtSales.Columns[4].ReadOnly = true;
                    dtSales.Columns.Add("Valeur Gamme 1", typeof(string));
                    dtSales.Columns[5].ReadOnly = true;
                    dtSales.Columns.Add("Sommeil", typeof(Boolean));
                    dtSales.Columns[6].ReadOnly = false;
                    SingletonUI.Instance.GammesDataGrid.ReadOnly = false;
                    int x = 0;
                    foreach (Gamme gamme in article.Gammes)
                    {
                        StringBuilder s = new StringBuilder();
                        s.Append(gamme.Reference);
                        s.Append(";");
                        s.Append(gamme.CodeBarre);
                        s.Append(";");
                        s.Append(gamme.Stock);
                        s.Append(";");
                        s.Append(gamme.Price);
                        s.Append(";");
                        s.Append(gamme.Intitule);
                        s.Append(";");
                        s.Append(gamme.Value_Intitule);
                        s.Append(";");
                        s.Append(gamme.Sommeil);
                        string[] row = s.ToString().Split(';');
                        dtSales.Rows.Add(row);
                        x++;
                    }
                }
            }
            SingletonUI.Instance.GammesDataGrid.DataSource = dtSales;

        }
        private void CheckAllChildNodes(TreeNode treeNode, bool nodeChecked)
        {
            foreach (TreeNode node in treeNode.Nodes)
            {
                node.Checked = nodeChecked;
                if (node.Nodes.Count > 0)
                {
                    // If the current node has child nodes, call the CheckAllChildsNodes method recursively.
                    this.CheckAllChildNodes(node, nodeChecked);
                }
            }
        }
        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            // The code only executes if the user caused the checked state to change.
            if (e.Action != TreeViewAction.Unknown)
            {
                if (e.Node.Nodes.Count > 0)
                {
                    /* Calls the CheckAllChildNodes method, passing in the current
                    Checked value of the TreeNode whose checked state changed. */
                    this.CheckAllChildNodes(e.Node, e.Node.Checked);
                }
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            /*var gescom = SingletonConnection.Instance.Gescom;
            var orders = gescom.FactoryDocumentVente.List;
            foreach (IBODocumentVente3 order in orders)
            {
                var x = order.FactoryDocumentVente ;
                if (order.DO_Piece.Equals("BC00073"))
                {
                    // order.FactoryDocument.InfoLibreFields[0].Name.ToString();
                     foreach (IBIValues item in order.InfoLibre)
                    { 
                    string test = item.ToString() ;
                }
                }
            }
            */
            this.Close();
        }
        private void EditArticle_MouseDown(object sender,
        System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }


        private void SaveInfos_Click(object sender, EventArgs e)
        {
            try
            {
                ControllerCommande.CreateDevis(DevisList);
                MessageBox.Show("Devis importer", "end", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


    }
}

