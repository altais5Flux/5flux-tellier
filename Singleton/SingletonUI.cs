using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace WebservicesSage.Singleton
{
    public sealed class SingletonUI
    {
        private static readonly Lazy<SingletonUI> lazy =
            new Lazy<SingletonUI>(() => new SingletonUI());

        public static SingletonUI Instance { get { return lazy.Value; } }

        public Label MenuTitle { get; set; }
        public Bunifu.Framework.UI.BunifuDropdown DepotConfiguration1 { get; set; }
        public Bunifu.Framework.UI.BunifuDropdown DepotConfiguration2 { get; set; }
        public Bunifu.Framework.UI.BunifuDropdown DepotConfiguration3 { get; set; }
        /*******          testing import from presta version          *******/
        public Bunifu.Framework.UI.BunifuDropdown PrestaId1 { get; set; }
        public Bunifu.Framework.UI.BunifuDropdown PrestaId2 { get; set; }
        public Bunifu.Framework.UI.BunifuDropdown PrestaId3 { get; set; }
        public Bunifu.Framework.UI.BunifuDropdown SageDoc1 { get; set; }
        public Bunifu.Framework.UI.BunifuDropdown SageDoc2 { get; set; }
        public Bunifu.Framework.UI.BunifuDropdown SageDoc3 { get; set; }
        public Dictionary<string, string> Transporteur { get; set; }
        public Bunifu.Framework.UI.BunifuDropdown DefaultExpedition { get; set; }
        public Panel ExpeditionOrderDataGrid { get; set; }
        public Bunifu.Framework.UI.BunifuiOSSwitch DefaultStock { get; set; }
        public Bunifu.Framework.UI.BunifuDropdown Lang1 { get; set; }
        public DataGridView DevisList { get; set; }
        public Bunifu.Framework.UI.BunifuDropdown Lang2 { get; set; }
        public WindowsFormsControlLibrary1.BunifuCustomTextbox Store1 { get; set; }
        public WindowsFormsControlLibrary1.BunifuCustomTextbox Store2 { get; set; }
        public CheckedListBox AddContactConfig { get; set; }
        public Bunifu.Framework.UI.BunifuDropdown SageInfos1 { get; set; }
        public Bunifu.Framework.UI.BunifuDropdown SageInfos2 { get; set; }
        public Bunifu.Framework.UI.BunifuDropdown SageInfos3 { get; set; }
        public Bunifu.Framework.UI.BunifuDropdown SageInfos4 { get; set; }
        public Bunifu.Framework.UI.BunifuDropdown SageInfos5 { get; set; }
        public WindowsFormsControlLibrary1.BunifuCustomTextbox MagentoInfos1 { get; set; }
        public WindowsFormsControlLibrary1.BunifuCustomTextbox MagentoInfos2 { get; set; }
        public WindowsFormsControlLibrary1.BunifuCustomTextbox MagentoInfos3 { get; set; }
        public WindowsFormsControlLibrary1.BunifuCustomTextbox MagentoInfos4 { get; set; }
        public WindowsFormsControlLibrary1.BunifuCustomTextbox MagentoInfos5 { get; set; }
        /***********/
        public Bunifu.Framework.UI.BunifuCircleProgressbar ClientCircleProgress { get; set; }
        public Bunifu.Framework.UI.BunifuCircleProgressbar catProgress { get; set; }
        public Bunifu.Framework.UI.BunifuProgressBar ProgressBar { get; set; }
        public Label ArticleNumber { get; set; }
        public Bunifu.Framework.UI.BunifuiOSSwitch LocalDB { get; set; }
        public WindowsFormsControlLibrary1.BunifuCustomTextbox LogBox { get; set; }

        public Bunifu.Framework.UI.BunifuDropdown ComptGConf { get; set; }
        public Bunifu.Framework.UI.BunifuDropdown CatTarifConf { get; set; }
        public Bunifu.Framework.UI.BunifuDropdown CondLivraisonConf { get; set; }
        public Bunifu.Framework.UI.BunifuDropdown ExpeditionConf { get; set; }
        public Bunifu.Framework.UI.BunifuMaterialTextbox PrefixClientConf { get; set; }

        public WindowsFormsControlLibrary1.BunifuCustomTextbox ArticleConfigurationArrondiInput { get; set; }

        public WindowsFormsControlLibrary1.BunifuCustomTextbox ArticleConfigurationTVAInput { get; set; }
        public Panel currentPanel { get; set; }

        public Bunifu.Framework.UI.BunifuCustomLabel StockNotification { get; set; }
        public Bunifu.Framework.UI.BunifuCustomLabel NotificationLabel { get; set; }
        public Bunifu.Framework.UI.BunifuCustomLabel ErrorNotificationLabel { get; set; }

        public WindowsFormsControlLibrary1.BunifuCustomTextbox SynchroClientCron { get; set; }
        public void ShowNotification(String NotificationMessage)
        {
            NotificationLabel.Visible = true;
            NotificationLabel.Text = NotificationMessage;
            Task.Delay(TimeSpan.FromSeconds(2)).ContinueWith(_ => HideNotification());
            
        }
        public void ShowStockNotification(String NotificationMessage)
        {
            StockNotification.Visible = true;
            StockNotification.Text = NotificationMessage;
            Task.Delay(TimeSpan.FromSeconds(2)).ContinueWith(_ => HideNotification());

        }
        public void ShowErrorNotification(String ErrorNotificationMessage)
        {
            ErrorNotificationLabel.Text = ErrorNotificationMessage;
            ErrorNotificationLabel.Visible = true;
            Task.Delay(TimeSpan.FromSeconds(2)).ContinueWith(_ => HideErrorNotification());

        }
        private void HideNotification()
        {
            SingletonUI.Instance.NotificationLabel.Invoke((MethodInvoker)(() => SingletonUI.Instance.NotificationLabel.Visible = false));
            //SingletonUI.Instance.StockNotification.Invoke((MethodInvoker)(() => SingletonUI.Instance.StockNotification.Visible = false));
        }
        private void HideErrorNotification()
        {
            SingletonUI.Instance.ErrorNotificationLabel.Invoke((MethodInvoker)(() => SingletonUI.Instance.ErrorNotificationLabel.Visible = false));
            SingletonUI.Instance.NotificationLabel.Invoke((MethodInvoker)(() => ShowNotification("Enregistrement effectuer avec Succée")));

        }
        public Bunifu.Framework.UI.BunifuDropdown SoucheDropdownBtoB { get; set; }
        public Bunifu.Framework.UI.BunifuDropdown SoucheDropdownBtoC { get; set; }
        public WindowsFormsControlLibrary1.BunifuCustomTextbox PrefixClient { get; set; }

        public WindowsFormsControlLibrary1.BunifuCustomTextbox BaseURLConfigurationB2B { get; set; }
        public WindowsFormsControlLibrary1.BunifuCustomTextbox BaseURLConfigurationB2C { get; set; }

        public WindowsFormsControlLibrary1.BunifuCustomTextbox UserConfiguration { get; set; }

        public WindowsFormsControlLibrary1.BunifuCustomTextbox Gcm_User { get; set; }

        public WindowsFormsControlLibrary1.BunifuCustomTextbox Gcm_Pass { get; set; }

        public WindowsFormsControlLibrary1.BunifuCustomTextbox Gcm_Path { get; set; }

        public WindowsFormsControlLibrary1.BunifuCustomTextbox Mae_User { get; set; }

        public WindowsFormsControlLibrary1.BunifuCustomTextbox Mae_Pass { get; set; }

        public WindowsFormsControlLibrary1.BunifuCustomTextbox Mae_Path { get; set; }

        public Bunifu.Framework.UI.BunifuiOSSwitch MAE_set { get; set; }

        public Bunifu.Framework.UI.BunifuiOSSwitch GCM_set { get; set; }

        public WindowsFormsControlLibrary1.BunifuCustomTextbox CronTaskUpdateStatut { get; set; }

        public WindowsFormsControlLibrary1.BunifuCustomTextbox CronTaskCheckNewOrder { get; set; }

        public WindowsFormsControlLibrary1.BunifuCustomTextbox MagentoToken { get; set; }

        public WindowsFormsControlLibrary1.BunifuCustomTextbox MagentoDefaultStore { get; set; }

        public WindowsFormsControlLibrary1.BunifuCustomTextbox MagentoDefaultCategory { get; set; }
        public WindowsFormsControlLibrary1.BunifuCustomTextbox DefaultRemise { get; set; }
        public WindowsFormsControlLibrary1.BunifuCustomTextbox DefaultTransportReference { get; set; }

        /****************** All Article Form ******************/
        public DataGridView AllArticlesdataGridView { get; set; }
        public ComboBox ActionList { get; set; }
        public ComboBox FilterField { get; set; }
        public Label ArticleCounter { get; set; }
        /****************** All Article Form ******************/

        /****************** Specifique Article Form ******************/
        public TreeView CategoriesTree { get; set; }

        // tab information
        public TabControl ArticleInformation { get; set; }
        public Label ArticleName { get; set; }
        public TextBox NameArticle { get; set; }
        public Label NameLabel { get; set; }
        public TextBox MetaTitle { get; set; }
        public TextBox MetaDescription { get; set; }
        public TextBox LinkRewrite { get; set; }
        public RichTextBox Description { get; set; }
        public RichTextBox ShortDescription { get; set; }
        public TextBox Tags { get; set; }
        public Label RestMetaDescriptionLabel { get; set; }
        public Label RestMetaTitleLabel { get; set; }
        public Label RestLinkRewriteLabel { get; set; }
        public Bunifu.Framework.UI.BunifuiOSSwitch IsActif { get; set; }
        //tab déclinaison
        public DataGridView GammesDataGrid { get; set; }
        // tab prix
        public TextBox PrixAchat { get; set; }
        public TextBox PrixVendre { get; set; }
        public TextBox PrixTTC { get; set; }
        public string FileLocation { get; set; }
        /****************** Specifique Article Form ******************/
        private SingletonUI()
        {

        }
    }
}
