using LiteDB;
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
using WebservicesSage.Object;
using WebservicesSage.Object.Categories;
using WebservicesSage.Object.DBObject;
using WebservicesSage.Singleton;
using WebservicesSage.Utils;
using WebservicesSage.Utils.Enums;

namespace WebservicesSage
{

    public partial class EditArticle : Form
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
        public EditArticle(Article article = null)
        {
            InitializeComponent();
            articleData = article;
            SingletonUI.Instance.ArticleInformation = ArticleInformation;
            SingletonUI.Instance.ArticleName = ArticleName;
            SingletonUI.Instance.NameLabel = NameLabel;
            SingletonUI.Instance.NameArticle = NameArticle;
            SingletonUI.Instance.MetaTitle = MetaTitle;
            SingletonUI.Instance.MetaDescription = MetaDescription;
            SingletonUI.Instance.LinkRewrite = LinkRewrite;
            SingletonUI.Instance.Description = Description;
            SingletonUI.Instance.ShortDescription = ShortDescription;
            SingletonUI.Instance.Tags = Tags;
            SingletonUI.Instance.RestMetaDescriptionLabel = RestMetaDescriptionLabel;
            SingletonUI.Instance.RestMetaTitleLabel = RestMetaTitleLabel;
            SingletonUI.Instance.RestLinkRewriteLabel = RestLinkRewriteLabel;
            SingletonUI.Instance.IsActif = IsActif;
            SingletonUI.Instance.PrixAchat = PrixAchat;
            SingletonUI.Instance.PrixVendre = PrixVendre;
            SingletonUI.Instance.PrixTTC = PrixTTC;
            EditArticleLoad(article.Reference);
            SingletonUI.Instance.CategoriesTree = CategoriesTree;
            SingletonUI.Instance.CategoriesTree.BeginUpdate();
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

            
            ProductCategories test = ProductCategories.FromJson(UtilsWebservices.GetMagentoData("rest/V1/categories"));
            
            SingletonUI.Instance.CategoriesTree.Nodes.Add(test.Name);
            SingletonUI.Instance.CategoriesTree.Nodes[0].Name = test.Id.ToString();

            Categories(test, test.Level, test.ParentId, SingletonUI.Instance.CategoriesTree.Nodes[0],article);
            //SingletonUI.Instance.CategoriesTree.Nodes SingletonUI.Instance.CategoriesTree.Nodes[0];
           /* SingletonUI.Instance.CategoriesTree.Nodes[0].Nodes.Add("Homme");
            SingletonUI.Instance.CategoriesTree.Nodes[0].Nodes[0].Name = "3";
            SingletonUI.Instance.CategoriesTree.Nodes[0].Nodes.Add("Femmes");
            SingletonUI.Instance.CategoriesTree.Nodes[0].Nodes[1].Name = "4";
            SingletonUI.Instance.CategoriesTree.Nodes[0].Nodes[1].Nodes.Add("Tops");
            SingletonUI.Instance.CategoriesTree.Nodes[0].Nodes[1].Nodes[0].Name = "5";
            SingletonUI.Instance.CategoriesTree.Nodes[0].Nodes[1].Nodes.Add("Robes");
            SingletonUI.Instance.CategoriesTree.Nodes[0].Nodes[1].Nodes[0].Name = "6";*/
            SingletonUI.Instance.CategoriesTree.EndUpdate();

            if (article.isGamme)
            {
                //fill gammes dataGrid
                SingletonUI.Instance.GammesDataGrid = GammesDataGrid;
                GammeLoad(article);
                //show name + reference of the article
            }
            else
            {
                SingletonUI.Instance.ArticleInformation.TabPages.RemoveAt(1);
            }
            SingletonUI.Instance.ArticleName.Text = SingletonUI.Instance.ArticleName.Text + article.Designation + " ("+article.Reference+")";
            
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

        private void Name_TextChanged(object sender, EventArgs e)
        {
            SingletonUI.Instance.NameLabel.Text = "NB caractéres restante : " + (128 - SingletonUI.Instance.NameArticle.Text.Length).ToString();
        }

        private void MetaTitle_TextChanged(object sender, EventArgs e)
        {
            SingletonUI.Instance.RestMetaTitleLabel.Text = "NB caractéres restante : " + (70 - SingletonUI.Instance.MetaTitle.Text.Length).ToString();
        }

        private void MetaDescription_TextChanged(object sender, EventArgs e)
        {
            SingletonUI.Instance.RestMetaDescriptionLabel.Text = "NB caractéres restante : " + (160 - SingletonUI.Instance.MetaDescription.Text.Length).ToString();
        }

        private void LinkRewrite_TextChanged(object sender, EventArgs e)
        {
            SingletonUI.Instance.RestLinkRewriteLabel.Text = "NB caractéres restante : " + (128 - SingletonUI.Instance.LinkRewrite.Text.Length).ToString();
        }

        private void SaveInfos_Click(object sender, EventArgs e)
        {
            int TabPageNB = SingletonUI.Instance.ArticleInformation.TabCount;
            TabControl.TabPageCollection tabs = SingletonUI.Instance.ArticleInformation.TabPages;
            //int nbElement = SingletonUI.Instance.ArticleInformation.TabPages[1].Controls.Count;
            for (int i = 0; i < TabPageNB; i++)
            {
                int nbElement = tabs[i].Controls.Count;
                if (true)
                {
                    for (int j = 0; j < nbElement; j++)
                    {
                        if (!string.IsNullOrEmpty(tabs[i].Controls[j].Name.ToString()))
                        {
                            switch (tabs[i].Controls[j].Name.ToString())
                            {
                                case "IsActif":
                                    Bunifu.Framework.UI.BunifuiOSSwitch test = (Bunifu.Framework.UI.BunifuiOSSwitch)tabs[i].Controls[j];
                                    articleData.Sommeil = !test.Value;
                                    break;
                                case "LinkRewrite":
                                    articleData.LinkRewrite = tabs[i].Controls[j].Text;
                                    break;
                                case "MetaDescription":
                                    articleData.MetaDescription = tabs[i].Controls[j].Text;
                                    break;
                                case "MetaTitle":
                                    articleData.MetaTitle = tabs[i].Controls[j].Text;
                                    break;
                                case "Tags":
                                    string[] tags = tabs[i].Controls[j].Text.ToString().Split(',');
                                    List<string> taglist = new List<string>();
                                    foreach (string tag in tags)
                                    {
                                        if (!string.IsNullOrEmpty(tag))
                                        {
                                            taglist.Add(tag);
                                        }
                                    }
                                    articleData.Tags = taglist;
                                    break;
                                case "ShortDescription":
                                    articleData.ShortDescription = tabs[i].Controls[j].Text;
                                    break;
                                case "Description":
                                    articleData.Description = tabs[i].Controls[j].Text;
                                    break;
                                case "NameArticle":
                                    articleData.Description = tabs[i].Controls[j].Text;
                                    break;
                                case "GammesDataGrid":
                                    DataGridView gammes = (DataGridView)tabs[i].Controls[j];
                                    foreach (DataGridViewRow row in gammes.Rows)
                                    {
                                        if (articleData.IsDoubleGamme)
                                        {
                                            for (int g = 0; g < articleData.Gammes.Count; g++)
                                            {
                                                if (articleData.Gammes[g].Reference.Equals(row.Cells[0].Value))
                                                {
                                                    articleData.Gammes[g].Sommeil = (Boolean)row.Cells[8].Value;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            for (int g = 0; g < articleData.Gammes.Count; g++)
                                            {
                                                if (articleData.Gammes[g].Reference.Equals(row.Cells[0].Value))
                                                {
                                                    articleData.Gammes[g].Sommeil = (Boolean)row.Cells[6].Value;
                                                }
                                            }
                                        }
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }

                    }
                }
            }
            string articlexml = UtilsSerialize.SerializeObject<Article>(articleData);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog GcmDialog = new OpenFileDialog())
            {
                GcmDialog.InitialDirectory = @"C:/";
                //GcmDialog.Filter = "PDF files(*.jpeg)| *.jpeg";
                //GcmDialog.FilterIndex = 2;
                GcmDialog.RestoreDirectory = true;

                if (GcmDialog.ShowDialog() == DialogResult.OK)
                {
                   SingletonUI.Instance.FileLocation = GcmDialog.FileName;
                }
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            
            try
            {
               /* try
                {
                    WebClient client = new WebClient();
                    string myFile = SingletonUI.Instance.FileLocation;
                    client.Credentials = CredentialCache.DefaultCredentials;
                    client.UploadFile(UtilsConfig.BaseUrl + EnumEndPoint.Article.Value, "POST", myFile);
                    client.Dispose();
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }
                /* string requestURL =UtilsConfig.BaseUrl+EnumEndPoint.EditeArticle.Value;
                 string fileName = SingletonUI.Instance.FileLocation;  
                 WebClient wc = new WebClient();
                 byte[] bytes = wc.DownloadData(fileName); // You need to do this download if your file is on any other server otherwise you can convert that file directly to bytes  
                 Dictionary<string, object> postParameters = new Dictionary<string, object>();
                 // Add your parameters here  
                 postParameters.Add("fileToUpload", new FormUpload.FileParameter(bytes, Path.GetFileName(fileName), "pdf/pdf"));
                 postParameters.Add("ArticleReference", articleData.Reference);
                 string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.88 Safari/537.36";
                 HttpWebResponse webResponse = FormUpload.MultipartFormPost(requestURL, userAgent, postParameters,"","");
                 // Process response  
                 //StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
                 //var returnResponseText = responseReader.ReadToEnd();
                 webResponse.Close();
                string fileName = SingletonUI.Instance.FileLocation;
                WebClient wc = new WebClient();
                byte[] bytes = wc.DownloadData(fileName);
               */
               /*
                NameValueCollection nvc = new NameValueCollection();
                nvc.Add("ProductId", articleData.Reference);
                HttpUploadFile(UtilsConfig.BaseUrl+EnumEndPoint.EditeArticle.Value,
                     SingletonUI.Instance.FileLocation, "file", "application/octet-stream", nvc);*/
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.ToString());
            }
        }
        public static void HttpUploadFile(string url, string file, string paramName, string contentType, NameValueCollection nvc)
        {
            //log.Debug(string.Format("Uploading {0} to {1}", file, url));
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

            Stream rs = wr.GetRequestStream();

            string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            foreach (string key in nvc.Keys)
            {
                rs.Write(boundarybytes, 0, boundarybytes.Length);
                string formitem = string.Format(formdataTemplate, key, nvc[key]);
                byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                rs.Write(formitembytes, 0, formitembytes.Length);
            }
            rs.Write(boundarybytes, 0, boundarybytes.Length);

            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string header = string.Format(headerTemplate, paramName, file, contentType);
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);

            FileStream fileStream = new FileStream(file, System.IO.FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                rs.Write(buffer, 0, bytesRead);
            }
            fileStream.Close();

            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);
            rs.Close();

            WebResponse wresp = null;
            try
            {
                wresp = wr.GetResponse();
                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
                //log.Debug(string.Format("File uploaded, server response is: {0}", reader2.ReadToEnd()));
            }
            catch (Exception ex)
            {
                //log.Error("Error uploading file", ex);
                if (wresp != null)
                {
                    wresp.Close();
                    wresp = null;
                }
            }
            finally
            {
                wr = null;
            }
        }

    }
}

