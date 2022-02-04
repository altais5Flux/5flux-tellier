using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebservicesSage.Services;
using WebservicesSage.Singleton;
using WebservicesSage.Utils;
using WebservicesSage.Utils.Enums;
using WebservicesSage.Cotnroller;
using Objets100cLib;
using Bunifu.Framework.UI;
using Newtonsoft.Json;
using WebservicesSage.Object.Categories;
using System.Net;
using WebservicesSage.Object;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Collections;
using static WebservicesSage.Object.Customer;
using System.Data.SqlClient;
using WebservicesSage.Object.CustomerSearch;
using Address = WebservicesSage.Object.Customer.Address;
using CustomAttribute = WebservicesSage.Object.Customer.CustomAttribute;

namespace WebservicesSage
{
    public partial class MainUI : Form
    {
        List<Panel> DataPanList = new List<Panel>();
        Thread lodaingAnimateThread;
        delegate void StringArgReturningVoidDelegate(int value);
        delegate void StringArgVoidDelegate(bool value);
        PrivateFontCollection pfc = new PrivateFontCollection();
        public MainUI()
        {
            InitializeComponent();
            InitCustomLabelFont();

            SingletonConnection.Instance.Gescom.Open();

            SingletonUI.Instance.ProgressBar = ProgressBar;
            SingletonUI.Instance.ArticleNumber = ArticleNumber;
            SingletonUI.Instance.ArticleConfigurationArrondiInput = ConfigurationArrondiInput;
            SingletonUI.Instance.ArticleConfigurationTVAInput = ConfigurationTvaInput;
            SingletonUI.Instance.NotificationLabel = NotificationLabel;
            //SingletonUI.Instance.StockNotification = StockNotification;
            //SingletonUI.Instance.DefaultStock = DefaultStock;
            SingletonUI.Instance.SoucheDropdownBtoB = SoucheDropdownBtoB;
            SingletonUI.Instance.SoucheDropdownBtoC = SoucheDropdownBtoC;
            SingletonUI.Instance.PrefixClient = PrefixClient;
            SingletonUI.Instance.BaseURLConfigurationB2B = BaseURLConfigurationB2B;
            SingletonUI.Instance.BaseURLConfigurationB2C = BaseURLConfigurationB2C;
            SingletonUI.Instance.UserConfiguration = UserConfiguration;
            SingletonUI.Instance.Gcm_User = Gcm_User;
            SingletonUI.Instance.Gcm_Pass = Gcm_Pass;
            SingletonUI.Instance.Gcm_Path = Gcm_Path;
            SingletonUI.Instance.GCM_set = GCM_set;
            SingletonUI.Instance.Mae_User = Mae_User;
            SingletonUI.Instance.Mae_Pass = Mae_Pass;
            SingletonUI.Instance.Mae_Path = Mae_Path;
            SingletonUI.Instance.MAE_set = MAE_set;
            SingletonUI.Instance.CronTaskCheckNewOrder = CronTaskCheckNewOrder;
            SingletonUI.Instance.CronTaskUpdateStatut = CronTaskUpdateStatut;
            SingletonUI.Instance.ErrorNotificationLabel = ErrorNotificationLabel;
            SingletonUI.Instance.MenuTitle = MenuTitle;

            SingletonUI.Instance.SynchroClientCron = SynchroClientCron;

            SingletonUI.Instance.MagentoToken = MagentoToken;
            SingletonUI.Instance.MagentoDefaultCategory = MagentoDefaultCategory;
            SingletonUI.Instance.MagentoDefaultStore = MagentoDefaultStore;
            SingletonUI.Instance.DefaultRemise = DefaultRemise;
            SingletonUI.Instance.DefaultTransportReference = DefaultTransportReference;
            //SingletonUI.Instance.Lang1 = Lang1;
            //SingletonUI.Instance.Lang2 = Lang2;
            //SingletonUI.Instance.Store1 = Store1;
            //SingletonUI.Instance.Store2 = Store2;
            SingletonUI.Instance.AddContactConfig = AddContactConfig;
            //SingletonUI.Instance.LocalDB = LocalDB;
            SingletonUI.Instance.DepotConfiguration1 = DepotConfiguration1;
            SingletonUI.Instance.DepotConfiguration2 = DepotConfiguration2;
            SingletonUI.Instance.DepotConfiguration3 = DepotConfiguration3;
            ControllerConfiguration.LoadConfiguration();
            InitServices();


            MenuPan.BackColor = Color.FromArgb(255, 255, 255);
            InfoPan.BackColor = Color.FromArgb(255, 255, 255);
            this.BackColor = Color.FromArgb(253, 253, 255);
            DataPan.BackColor = Color.FromArgb(253, 253, 255);

            DataPanList.Add(DataPan);
            DataPanList.Add(DashboardPanel);
            DataPanList.Add(ClientPan);
            DataPanList.Add(ConfigurationPan);
            DataPanList.Add(ArticleConfiguration);
            DataPanList.Add(ArticlePan);
            DataPanList.Add(PrefixClientConfiguration);
            DataPanList.Add(GeneralConfiguration);
            DataPanList.Add(CommandeConfiguration);

            // à corriger prends beaucoup de proccess
            /*lodaingAnimateThread = new Thread(new ThreadStart(AnimateLoading));
            lodaingAnimateThread.Start();
            */
        }

        private void InitServices()
        {

            

            SingletonServices.Instance.ServiceClient = new ServiceClient();
            SingletonServices.Instance.ServiceGroupeTarifaire = new ServiceGroupeTarrifaire();
            SingletonServices.Instance.ServiceCommande = new ServiceCommande();
            SingletonServices.Instance.ServiceArticle = new ServiceArticle();
            SingletonServices.Instance.ServiceGammes = new ServicesGammes();

            SingletonServices.Instance.ServiceCommande.ToDoOnLaunch();
            SingletonServices.Instance.ServiceArticle.ToDoOnLaunch();
            SingletonServices.Instance.ServiceClient.ToDoOnLaunch();

        }

        private void InitCustomLabelFont()
        {
            //PrivateFontCollection pfc = new PrivateFontCollection();
            int fontLength = Properties.Resources.Montserrat_Regular.Length;
            byte[] fontdata = Properties.Resources.Montserrat_Regular;
            System.IntPtr data = Marshal.AllocCoTaskMem(fontLength);
            Marshal.Copy(fontdata, 0, data, fontLength);
            pfc.AddMemoryFont(data, fontLength);

            MenuTitle.Font = new Font(pfc.Families[0], MenuTitle.Font.Size);
            DoneLabel.Font = new Font(pfc.Families[0], DoneLabel.Font.Size);
            SyncClientLab.Font = new Font(pfc.Families[0], SyncClientLab.Font.Size);
        }

        private void ClientButton_Click(object sender, EventArgs e)
        {
            hideAllPan();
            ClientPan.Visible = true;
            ChangeTitleText("Gestion des Clients");
            //this.ProgressBar.Value += 50;
        }

        private void hideAllPan()
        {
            foreach(Panel pan in DataPanList)
            {
                pan.Visible = false;
            }
        }

        private void ChangeTitleText(String title)
        {
            SingletonUI.Instance.MenuTitle.Text = title;
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            SingletonConnection.Instance.dB.Disconnect();
            Environment.Exit(404);
        }

        private void AnimateLoading()
        {
            while (Thread.CurrentThread.IsAlive)
            {
                if(this.ProgressBar.Value == 100){
                    this.ProgressBar.ProgressColor = Color.FromArgb(39, 174, 96);
                    this.SetVisible(true);

                    Thread.Sleep(3000);

                    this.SetVisible(false);
                    this.SetValue(0);
                    this.ProgressBar.ProgressColor = Color.FromArgb(238, 118, 32);
                }
            }
        }

        private void SetValue(int value)
        {
            if (this.ProgressBar.InvokeRequired)
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(SetValue);
                this.Invoke(d, new object[] { value });
            }
            else
            {
                this.ProgressBar.Value = value;
            }
        }

        private void SetVisible(bool value)
        {
            if (this.DoneLabel.InvokeRequired)
            {
                StringArgVoidDelegate d = new StringArgVoidDelegate(SetVisible);
                this.Invoke(d, new object[] { value });
            }
            else
            {
                this.DoneLabel.Visible = value;
            }
        }

        private void articleConfButton_Click(object sender, EventArgs e)
        {
            hideAllPan();
            ArticleConfiguration.Visible = true;
            ChangeTitleText("Configuration Des Articles");
        }

        private void configurationButton_Click(object sender, EventArgs e)
        {
            hideAllPan();
            ConfigurationPan.Visible = true;
            ChangeTitleText("Configuration");
        }

        private void ArticleButton_Click(object sender, EventArgs e)
        {
            hideAllPan();
            ArticlePan.Visible = true;
            ChangeTitleText("Gestion des Articles");
        }

        private void SyncClient_Click(object sender, EventArgs e)
        {
            string promptValue = ShowDialog("Laisser vide pour synchroniser tout les clients", "Clients");
            if (!promptValue.Equals("CLOSE"))
            {
                if (!String.IsNullOrEmpty(promptValue))
                {
                    SingletonServices.Instance.ServiceClient.SendClient(promptValue);
                }
                else
                {
                    SingletonServices.Instance.ServiceClient.ToDoOnFirstCommit();
                }
            }
        }

        private void SyncArticle_Click(object sender, EventArgs e)
        {
            string promptValue = ShowDialog("Laisser vide pour synchroniser tout les produits", "Articles");
            if (!promptValue.Equals("CLOSE"))
            {
                SingletonServices.Instance.ServiceArticle.SendProducts(promptValue);
            }
            else
            {
                SingletonServices.Instance.ServiceArticle.ToDoOnFirstCommit();
            }
        }

        private void SyncPriceArticle_Click(object sender, EventArgs e)
        {
            string promptValue = ShowDialog("Laisser vide pour synchroniser tout les produits", "Prix");
            if (!promptValue.Equals("CLOSE"))
            {
                SingletonServices.Instance.ServiceArticle.SendPriceProduct(promptValue);
            }
        }

        private void SyncStockArticle_Click(object sender, EventArgs e)
        {
            string promptValue = ShowDialog("Laisser vide pour synchroniser tout les produits", "Stock");
            if (!promptValue.Equals("CLOSE"))
            {
                SingletonServices.Instance.ServiceArticle.SendProducts(promptValue); //SendStock(promptValue);
            }
        }

        private void SyncGammes_Click(object sender, EventArgs e)
        {
            SingletonServices.Instance.ServiceGammes.ToDoOnFirstCommit();
        }


        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 300,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 10, Top = 20, Width = 300 ,Text = text };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 200 };
            Button confirmation = new Button() { Text = "Ok", Left = 50, Width = 200, Top = 70, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "CLOSE";
        }

        private void SaveArticleConfiguration_Click(object sender, EventArgs e)
        {
            ControllerConfiguration.UpdateArticleConfiguration();
        }

        private void OrderConfButton(object sender, EventArgs e)
        {
            hideAllPan();
            CommandeConfiguration.Visible = true;
            ChangeTitleText("Configuration Des Commandes");
        }

        private void UpdateSouche(object sender, EventArgs e)
        {
            ControllerConfiguration.UpdateOrderConfiguration();
        }
        
        private void ClientConfButton_Click(object sender, EventArgs e)
        {
            hideAllPan();
            PrefixClientConfiguration.Visible = true;
            ChangeTitleText("Configuration Client");
        }

        private void GenelaConfButton_Click(object sender, EventArgs e)
        {
            hideAllPan();
            GeneralConfiguration.Visible = true;
            ChangeTitleText("Configuration Générale");
        }

        private void UpdatePrefixClient(object sender, EventArgs e)
        {
            ControllerConfiguration.UpdateClientConfiguration();
        }
        private void UpdateGeneralConfiguration(object sender, EventArgs e)
        {
            ControllerConfiguration.UpdateGeneralConfiguration();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog MaeDialog = new OpenFileDialog())
            {
                MaeDialog.InitialDirectory = UtilsConfig.Mae_Path.ToString();
                MaeDialog.Filter = "MAE files(*.MAE)| *.MAE";
                MaeDialog.FilterIndex = 2;
                MaeDialog.RestoreDirectory = true;

                if (MaeDialog.ShowDialog() == DialogResult.OK)
                {
                    Mae_Path.Text = MaeDialog.FileName;
                }
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog GcmDialog = new OpenFileDialog())
            {
                GcmDialog.InitialDirectory = UtilsConfig.Gcm_Path.ToString();
                GcmDialog.Filter = "GCM files(*.GCM)| *.GCM";
                GcmDialog.FilterIndex = 2;
                GcmDialog.RestoreDirectory = true;

                if (GcmDialog.ShowDialog() == DialogResult.OK)
                {
                    Gcm_Path.Text = GcmDialog.FileName;
                }
            }
        }

        private void DashBoardButton_Click(object sender, EventArgs e)
        {
            hideAllPan();
            DashboardPanel.Visible = true;
            ChangeTitleText("Dashboard");
        }

        private void MenuPan_Paint(object sender, PaintEventArgs e)
        {
            //ControlPaint.DrawBorder(e.Graphics, this.MenuPan.ClientRectangle, Color.DarkBlue, ButtonBorderStyle.Solid);
            e.Graphics.DrawLine(new Pen(Color.Black, 3),
                            this.MenuPan.DisplayRectangle.X + this.MenuPan.DisplayRectangle.Width, this.MenuPan.DisplayRectangle.Top, this.MenuPan.DisplayRectangle.X + this.MenuPan.DisplayRectangle.Width, this.MenuPan.Top + this.MenuPan.DisplayRectangle.Height);
        }

        private void CheckForNewOrderMagento(object sender, EventArgs e)
        {
            //ControllerCommande.CheckForNewOrderMagento();
        }
        private void MappingOrdersStatutButton_Click(object sender, EventArgs e)
        {
            MappingOrdersStaut mappingOrdersStaut = new MappingOrdersStaut();
            mappingOrdersStaut.Show();
        }
        private void MappingExpeditionModeButton_Click(object sender, EventArgs e)
        {
            MappingExpeditionMode m = new MappingExpeditionMode();
            m.Show();
        }
        private void DefaultStock_OnValueChange(object sender, EventArgs e)
        {
            if (!SingletonUI.Instance.DefaultStock.Value)
            {
                SingletonUI.Instance.ShowStockNotification("Utilisation du stock a terme");
            }
        }
        private void AddContactConfig_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            for (int ix = 0; ix < AddContactConfig.Items.Count; ++ix)
            {
                if (ix != e.Index) AddContactConfig.SetItemChecked(ix, false);
            }
        }
        private void MappingInfosLibre_Click(object sender, EventArgs e)
        {
            MappingInfosLibre mappingInfosLibre = new MappingInfosLibre();
            mappingInfosLibre.Show();
        }

        private void MultiLang_Click(object sender, EventArgs e)
        {/*
            var gescom = SingletonConnection.Instance.Gescom;
            var compta = SingletonConnection.Instance.Compta;
            IBOArticle3 article = (IBOArticle3)gescom.FactoryArticle.Create();
            var article2 = gescom.FactoryArticle.ReadReference("TESTMAGENTO");
            article.SetDefault();
            article.AR_Ref = "TESTARTICLE";
            article.AR_Design = "test designiation";
            //article.Famille.FA_CodeFamille = "BIJOUXOR";
            article.Famille = gescom.FactoryFamille.ReadCode(0, "BIJOUXOR");
            article.Write();*/
        }

        private void EditArticle_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1();
            form.Show();
            /*var gescom = SingletonConnection.Instance.Gescom;
            IBOArticle3 article = gescom.FactoryArticle.ReadReference("COR1");
            try
            {
                IBOArticle3 article1 = gescom.FactoryArticle.ReadReference("TEST1");
            }
            catch (Exception exc)
            {

                Console.Write(exc);
            }
            
            foreach (IBPConditionnement item in gescom.FactoryConditionnement.List)
            {
                var test = item;
            }
            //UtilsLinkedCommande.addArticleToLocalDB(article);
            //Article article = UtilsLinkedCommande.getArticleFromLocalDB("I");
            //string articleXML = UtilsSerialize.SerializeObject<Article>(article);*/
        }
        private void SynchCategories_Click(object sender, EventArgs e)
        {
            
            
            try
            {
                ControllerCommande.CheckForNewOrderMagento();
                //ControllerCommande.UpdateStatuOrder();


                MessageBox.Show("end sync", "end", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void BunifuFlatButton8_Click(object sender, EventArgs e)
        {
            try { 
            ControllerClient.SendPriceBtoC();

                MessageBox.Show("end sync", "end", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //Devis devis = new Devis();
            //devis.Show();
            //ControllerCommande.CheckForNewOrderMagento();
            //MessageBox.Show("end sync", "end", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //MessageBox.Show("end sync", "end", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void CustomImageButto1_Click_1(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void BunifuFlatButton9_Click(object sender, EventArgs e)
        {
            try
            {
               
                ControllerCommande.UpdateStatuOrder();

                MessageBox.Show("end sync", "end", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
