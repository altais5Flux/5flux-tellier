using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Objets100cLib;
using WebservicesSage.Object;
using WebservicesSage.Cotnroller;
using WebservicesSage.Singleton;
using WebservicesSage.Utils;
using System.Data.SqlClient;
using static WebservicesSage.Object.Customer;
using System.Globalization;
using CustomAttribute = WebservicesSage.Object.CustomerSearch.CustomAttribute;
using System.IO;

namespace WebservicesSage.Object
{
    [Serializable()]
    public class Client
    {
        public string Intitule { get; set; }
        public string Contact { get; set; }
        public bool Sommeil { get; set; }
        public string Email { get; set; }
        public string GroupeTarifaireIntitule { get; set; }
        public string CT_NUM { get; set; }
        public string Central { get; set; }
        public string Siret { get; set; }
        public double RemiseGlobal { get; set; }
        public int IdCategory { get; set; }
        public List<Object.Customer.Address> AddressesList { get; set; }
        public List<ClientLivraisonAdress> clientLivraisonAdresses { get; set; }
        public List<SpecialPriceClient> SpecialPrices { get; set; }
        private IBOClient3 clientFC;

        public Client(IBOClient3 clientFC)
        {
            if (clientFC.CentraleAchat != null)
            {
                Central = clientFC.CentraleAchat.CT_Num;
            }
            else
            {
                Central = clientFC.CT_Num;
            }
            RemiseGlobal = 0;
            Siret = "";
            AddressesList = new List<Object.Customer.Address>();
            clientLivraisonAdresses = new List<ClientLivraisonAdress>();
            SpecialPrices = new List<SpecialPriceClient>();
            this.clientFC = clientFC;
            Email = clientFC.Telecom.EMail.Trim();
            Intitule = clientFC.CT_Intitule;
            Sommeil = clientFC.CT_Sommeil;
            Contact = clientFC.CT_Contact;
            GroupeTarifaireIntitule = clientFC.CatTarif.CT_Intitule;
            CT_NUM = clientFC.CT_Num;
            if (!String.IsNullOrEmpty(clientFC.CT_Siret))
            {
                Siret = clientFC.CT_Siret;
            }

            string sql2 = "SELECT Ar_Ref, AC_PrixVen FROM " + System.Configuration.ConfigurationManager.AppSettings["DBNAME"].ToString() + ".dbo.F_ARTCLIENT WHERE CT_Num = '" + Central + "' AND AC_Categorie = 0";

            SqlDataReader ClientFixedPrice = SingletonConnection.Instance.dB.Select(sql2);
            while (ClientFixedPrice.Read())
            {
                if (Convert.ToDouble(ClientFixedPrice.GetValue(1).ToString()) > 0)
                {
                    SpecialPriceClient price = new SpecialPriceClient();
                    //ProductSearchCriteria productMagento = UtilsWebservices.GetMagentoProduct("rest/all/V1/products", UtilsWebservices.SearchCriteria("sku", ClientFixedPrice.GetValue(0).ToString(), "eq"));
                    //if (productMagento.TotalCount > 0)
                    //{

                        price.groupeName = Central;
                        price.price = Convert.ToDouble(ClientFixedPrice.GetValue(1).ToString());
                        price.remiseClient = 0;
                        price.remiseFamille = 0;
                        price.articleReference = ClientFixedPrice.GetValue(0).ToString();
                    SpecialPrices.Add(price);
                    //}
                }
            }
            ClientFixedPrice.Close();

            string sql3 = "SELECT N_CatTarif, CT_Taux01 FROM " + System.Configuration.ConfigurationManager.AppSettings["DBNAME"].ToString() + ".dbo.F_COMPTET WHERE CT_Num = '" + Central + "' AND CT_Type = 0";
            SqlDataReader ClientCatTarifAndReduction = SingletonConnection.Instance.dB.Select(sql3);
            while (ClientCatTarifAndReduction.Read())
            {
                RemiseGlobal = Convert.ToDouble(ClientCatTarifAndReduction.GetValue(1).ToString());
                IdCategory = Convert.ToInt32(ClientCatTarifAndReduction.GetValue(0).ToString());
            }
            ClientCatTarifAndReduction.Close();
            string sql4 = "Select article.AR_Ref,articleClient.AC_PrixVen,article.FA_CodeFamille,Case when (Select FC_Remise as remise from " + System.Configuration.ConfigurationManager.AppSettings["DBNAME"].ToString() + ".dbo.F_FAMCLIENT as Famille where CT_Num = '" + Central+ "' and Famille.FA_CodeFamille = article.FA_CodeFamille) > 0 then (Select FC_Remise as remise from " + System.Configuration.ConfigurationManager.AppSettings["DBNAME"].ToString() + ".dbo.F_FAMCLIENT as famille where CT_Num = '" + Central + "' and Famille.FA_CodeFamille = article.FA_CodeFamille) else 0 end FROM " + System.Configuration.ConfigurationManager.AppSettings["DBNAME"].ToString() + ".dbo.F_ARTICLE as article left join  " + System.Configuration.ConfigurationManager.AppSettings["DBNAME"].ToString() + ".dbo.F_ARTCLIENT as articleClient on article.AR_Ref = articleClient.AR_Ref WHERE article.AR_Publie = 1 and articleClient.AC_Categorie = " + IdCategory + " order by FA_CodeFamille asc";
            SqlDataReader AllArticle = SingletonConnection.Instance.dB.Select(sql4);
            while (AllArticle.Read())
            {
                if (Convert.ToDouble(AllArticle.GetValue(1).ToString()) > 0)
                {
                    Boolean found = false;
                    foreach (SpecialPriceClient priceClient in SpecialPrices)
                    {
                        if (AllArticle.GetValue(0).ToString().Equals(priceClient.articleReference))
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        SpecialPriceClient priceF = new SpecialPriceClient();
                        priceF.groupeName = Central;
                        priceF.price = Convert.ToDouble(AllArticle.GetValue(1).ToString());
                        priceF.remiseClient = RemiseGlobal;
                        priceF.remiseFamille = Convert.ToDouble(AllArticle.GetValue(3).ToString());
                        priceF.articleReference = AllArticle.GetValue(0).ToString();
                        SpecialPrices.Add(priceF);
                    }
                }
            }
            AllArticle.Close();
            // récupération de l'adresse de facturation
            string sql = "SELECT CT.CT_Num ," + "PAY.CT_Intitule ," +"'(' + PAY.CT_Num + ')' ," +"PAY.CT_Adresse ," +"PAY.CT_Complement ," +"PAY.CT_CodePostal ," +
" PAY.CT_Ville ," +
" PAY.CT_CodeRegion," +
"PAY.CT_Pays ," +
"PAY.CT_Telephone ," +
"PAY.CT_Identifiant " +
"FROM " + System.Configuration.ConfigurationManager.AppSettings["DBNAME"].ToString() + ".dbo.F_COMPTET AS CT INNER JOIN " + System.Configuration.ConfigurationManager.AppSettings["DBNAME"].ToString() + ".dbo.F_COMPTET AS PAY ON CT.CT_NumPayeur = PAY.CT_Num " +
"WHERE  CT.CT_NUM = '" + CT_NUM + "' " +
"AND(PAY.CT_Adresse IS NOT NULL AND PAY.CT_Adresse <> '')" +
"AND(PAY.CT_Ville IS NOT NULL AND PAY.CT_Ville <> '')" +
"AND(PAY.CT_Pays IS NOT NULL AND PAY.CT_Pays <> '')" +
"AND(PAY.CT_CodePostal IS NOT NULL AND PAY.CT_CodePostal <> '')" +
"AND(PAY.CT_Telephone IS NOT NULL AND PAY.CT_Telephone <> '')" +
"ORDER BY CT.CT_Num";
            File.AppendAllText("Log\\test.txt", DateTime.Now + " SQL Facturation : " + Environment.NewLine);
            File.AppendAllText("Log\\test.txt", DateTime.Now + " " + sql.ToString() + Environment.NewLine);
            SqlDataReader AdressF = SingletonConnection.Instance.dB.Select(sql);
            while (AdressF.Read())
            {
                try
                {
                    var clientsSageObj = AdressF.GetValue(0).ToString();
                    Address address1 = new Address();
                    address1.Firstname = AdressF["CT_Intitule"].ToString();
                    //File.AppendAllText("Log\\test.txt", DateTime.Now + " firstname " + AdressF["CT_Intitule"].ToString() + Environment.NewLine);
                    address1.Lastname = "(" + Central + ")";
                    address1.Telephone = AdressF["CT_Telephone"].ToString();
                    //File.AppendAllText("Log\\test.txt", DateTime.Now + " tele " + AdressF["CT_Telephone"].ToString() + Environment.NewLine);
                    address1.Postcode = AdressF["CT_CodePostal"].ToString();
                    //File.AppendAllText("Log\\test.txt", DateTime.Now + " postal " + AdressF["CT_CodePostal"].ToString() + Environment.NewLine);
                    address1.City = AdressF["CT_Ville"].ToString();
                    //File.AppendAllText("Log\\test.txt", DateTime.Now + " ville " + AdressF["CT_Ville"].ToString() + Environment.NewLine);
                    string pays1 = AdressF["CT_Pays"].ToString();//clientsg.LivraisonPrincipal.AdressFe.Pays;
                    /*pays1 = pays1.Replace(" ", "-");
                    var regions1 = CultureInfo.GetCultures(CultureTypes.SpecificCultures).Select(x => new RegionInfo(x.LCID));
                    var englishRegion1 = regions1.FirstOrDefault(region1 => region1.EnglishName.ToLower().Contains(pays1.ToLower()));
                    if (englishRegion1 == null)
                    {
                        englishRegion1 = regions1.FirstOrDefault(region1 => region1.DisplayName.ToLower().Contains(pays1.ToLower()));
                    }
                    //ISO3166.FromName(AdressF["CT_Pays"].ToString()).Alpha2;
                    address1.CountryId = ISO3166.FromName(AdressF["CT_Pays"].ToString()).Alpha2;
                    */

                    string sqlPays = "SELECT PA_CodeISO2  FROM [" + System.Configuration.ConfigurationManager.AppSettings["DBNAME"].ToString() + "].[dbo].[F_PAYS] where PA_Intitule ='" + pays1.ToUpper() + "'";
                    SqlDataReader sqlDataReaderPays = SingletonConnection.Instance.dB.Select(sqlPays);
                    string codeISO = "";
                    while (sqlDataReaderPays.Read())
                    {
                        codeISO = sqlDataReaderPays.GetValue(0).ToString();
                    }
                    sqlDataReaderPays.Close();
                    if (!String.IsNullOrEmpty(codeISO))
                    {
                        address1.CountryId = codeISO;
                    }
                    else
                    {
                        address1.CountryId = ISO3166.FromName(pays1.ToString()).Alpha2;
                    }



                    //File.AppendAllText("Log\\test.txt", DateTime.Now + " countryid " + address1.CountryId.ToString() + Environment.NewLine);
                    Region regF = new Region();
                    regF.region= AdressF["CT_CodeRegion"].ToString();

                    address1.Region = regF;
                    address1.DefaultBilling = true;
                    address1.DefaultShipping = false;
                    address1.Street = new List<string>();
                    address1.Street.Add(AdressF["CT_Adresse"].ToString());
                    //File.AppendAllText("Log\\test.txt", DateTime.Now + " street 1 " + AdressF["CT_Adresse"].ToString() + Environment.NewLine);
                    if (AdressF["CT_Complement"].ToString().Length > 0)
                    {
                        address1.Street.Add(AdressF["CT_Complement"].ToString());
                        //File.AppendAllText("Log\\test.txt", DateTime.Now + " street 2 " + AdressF["CT_Complement"].ToString() + Environment.NewLine);
                    }
                    List<Customer.CustomAttribute> CustomAttributes = new List<Customer.CustomAttribute>();
                    Customer.CustomAttribute customAttribute = new Customer.CustomAttribute();
                    customAttribute.attribute_code = "li_no";
                    customAttribute.value = "";
                    CustomAttributes.Add(customAttribute);
                    address1.CustomAttributes = CustomAttributes;
                    AddressesList.Add(address1);
                }
                catch (Exception s)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(DateTime.Now + s.Message + Environment.NewLine);
                    sb.Append(DateTime.Now + s.StackTrace + Environment.NewLine);
                    System.IO.File.AppendAllText("Log\\SendAllClientCronError.txt", sb.ToString());
                    sb.Clear();
                }
            }
            AdressF.Close();
            // récupération de l'adresse de livraison
            string sqlLivraison = "SELECT CT.CT_Num ," +
       "LIV.LI_Intitule ," +
      " '(' + LIV.CT_Num + ')'," +
      " LIV.LI_Adresse ," +
      " LIV.LI_Complement ," +
      " LIV.LI_Ville ," +
      " LIV.LI_Pays ," +
      " LIV.LI_CodePostal," +
      " LIV.LI_Telephone ," +
      " LIV.LI_Contact ," +
      " LIV.LI_CodeRegion ," +
      " LIV.LI_Principal," +
      " LIV.LI_No" +
" FROM " + System.Configuration.ConfigurationManager.AppSettings["DBNAME"].ToString() + ".dbo.F_COMPTET AS CT LEFT JOIN " + System.Configuration.ConfigurationManager.AppSettings["DBNAME"].ToString() + ".dbo.F_LIVRAISON AS LIV ON CT.CT_Num = LIV.CT_Num" +
" WHERE CT.TIERS_WEB = 'OUI' and CT.CT_Num like '" + CT_NUM + "'" +
" AND(LIV.LI_Pays IS NOT NULL AND LIV.LI_Pays <> '')" +
" AND(LIV.LI_CodeRegion IS NOT NULL AND LIV.LI_CodeRegion <> '')" +
" AND(LIV.LI_Adresse IS NOT NULL AND LIV.LI_Adresse <> '')" +
" AND(LIV.LI_Ville IS NOT NULL AND LIV.LI_Ville <> '')" +
" AND(LIV.LI_CodePostal IS NOT NULL AND LIV.LI_CodePostal <> '')" +
" AND(LIV.LI_Telephone IS NOT NULL AND LIV.LI_Telephone <> '')" +
" ORDER BY LIV.CT_Num, LIV.LI_Principal DESC";
            File.AppendAllText("Log\\test.txt", DateTime.Now + " SQL Livraison : " + Environment.NewLine);
            File.AppendAllText("Log\\test.txt", DateTime.Now + " " + sqlLivraison.ToString() + Environment.NewLine);
            SqlDataReader AdressL = SingletonConnection.Instance.dB.Select(sqlLivraison);

            while (AdressL.Read())
            {
                try
                {
                    var clientsSageObj = AdressL.GetValue(0).ToString();
                    Address address1 = new Address();
                    address1.Firstname = AdressL["LI_Intitule"].ToString();
                    File.AppendAllText("Log\\test.txt", DateTime.Now + " LI_Intitule " + AdressL["LI_Intitule"].ToString() + Environment.NewLine);
                    address1.Lastname = "(" + Central + ")";
                    address1.Telephone = AdressL["LI_Telephone"].ToString();
                    File.AppendAllText("Log\\test.txt", DateTime.Now + " LI_Telephone " + AdressL["LI_Telephone"].ToString() + Environment.NewLine);
                    address1.Postcode = AdressL["LI_CodePostal"].ToString();
                    File.AppendAllText("Log\\test.txt", DateTime.Now + " LI_CodePostal " + AdressL["LI_CodePostal"].ToString() + Environment.NewLine);
                    if (!String.IsNullOrEmpty(AdressL["LI_CodeRegion"].ToString()))
                    {
                        Region regL = new Region();
                        regL.region = AdressL["LI_CodeRegion"].ToString();
                        File.AppendAllText("Log\\test.txt", DateTime.Now + " LI_CodeRegion " + AdressL["LI_CodeRegion"].ToString() + Environment.NewLine);
                        address1.Region = regL;
                    }
                    
                    address1.City = AdressL["LI_Ville"].ToString();
                    File.AppendAllText("Log\\test.txt", DateTime.Now + " LI_Ville " + AdressL["LI_Ville"].ToString() + Environment.NewLine);
                    string pays1 = AdressL["LI_Pays"].ToString().ToUpper();//clientsg.LivraisonPrincipal.AdressLe.Pays;
                    File.AppendAllText("Log\\test.txt", DateTime.Now + " LI_Pays " + AdressL["LI_Pays"].ToString() + Environment.NewLine);
                    //pays1 = pays1.Replace(" ", "-");
                    pays1 = RemoveDiacritics(pays1);
                    //pays1 = pays1.Substring(0, 1).ToUpper() + pays1.Substring(1).ToLower();
                    //File.AppendAllText("Log\\test.txt", DateTime.Now + " LI_Pays " + pays1.ToString() + Environment.NewLine);
                    string sqlPays = "SELECT PA_CodeISO2  FROM [" + System.Configuration.ConfigurationManager.AppSettings["DBNAME"].ToString() + "].[dbo].[F_PAYS] where PA_Intitule ='" + pays1.ToUpper() + "'";
                    //File.AppendAllText("Log\\test.txt", DateTime.Now + " sql " + sqlPays.ToString() + Environment.NewLine);
                    SqlDataReader sqlDataReaderPays = SingletonConnection.Instance.dB.Select(sqlPays);
                    string codeISO = "";
                    while (sqlDataReaderPays.Read())
                    {
                        codeISO = sqlDataReaderPays.GetValue(0).ToString();
                    }
                    sqlDataReaderPays.Close();
                    if (!String.IsNullOrEmpty(codeISO))
                    {
                        address1.CountryId = codeISO;
                    }
                    else
                    {
                        address1.CountryId = ISO3166.FromName(pays1.ToString()).Alpha2;
                    }
                    if (AdressL["LI_Principal"].ToString().Equals("1"))
                    {
                        address1.DefaultBilling = false;
                        address1.DefaultShipping = true;

                    }
                    else
                    {
                        address1.DefaultBilling = false;
                        address1.DefaultShipping = false;
                    }

                    address1.Street = new List<string>();
                    address1.Street.Add(AdressL["LI_Adresse"].ToString());
                    File.AppendAllText("Log\\test.txt", DateTime.Now + " LI_Adresse " + AdressL["LI_Adresse"].ToString() + Environment.NewLine);

                    if (!String.IsNullOrEmpty(AdressL["LI_Complement"].ToString()))
                    {
                        address1.Street.Add(AdressL["LI_Complement"].ToString());
                        File.AppendAllText("Log\\test.txt", DateTime.Now + " LI_Complement " + AdressL["LI_Complement"].ToString() + Environment.NewLine);
                    }
                    List<Customer.CustomAttribute> CustomAttributes = new List<Customer.CustomAttribute>();
                    Customer.CustomAttribute customAttribute = new Customer.CustomAttribute();
                    customAttribute.attribute_code = "li_no";
                    customAttribute.value = AdressL["LI_No"].ToString();
                    File.AppendAllText("Log\\test.txt", DateTime.Now + " li_no " + AdressL["LI_No"].ToString() + Environment.NewLine);
                    CustomAttributes.Add(customAttribute);
                    address1.CustomAttributes = CustomAttributes;
                    AddressesList.Add(address1);
                }
                catch (Exception s)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(DateTime.Now + s.Message + Environment.NewLine);
                    sb.Append(DateTime.Now + s.StackTrace + Environment.NewLine);
                    System.IO.File.AppendAllText("Log\\SendAllClientCronError.txt", sb.ToString());
                    sb.Clear();
                }
            }

            AdressL.Close();




            // applicaiton d'algorithme calcul de prix avec les OBjet métier
            #region algorithme Calcul prix avec Objet Métier
            /*
            if (CT_NUM.Equals(Central))
            {                
                
                foreach (IBOArticleTarifClient3 item in clientFC.FactoryClientTarif.List)
                {
                    SpecialPriceClient price = new SpecialPriceClient();
                    ProductSearchCriteria productMagento = UtilsWebservices.GetMagentoProduct("rest/all/V1/products", UtilsWebservices.SearchCriteria("sku", item.Article.AR_Ref, "eq"));
                    if (productMagento.TotalCount>0)
                    {
                        if (item.Prix != 0)
                        {
                            price.groupeName = CT_NUM;
                            price.price = item.Prix;
                            price.remiseClient = 0;
                            price.remiseFamille = 0;
                            price.articleReference = item.Article.AR_Ref;
                        }
                        else
                        {
                            price.groupeName = CT_NUM;
                            price.price = item.Article.AR_PrixVen;
                            price.remiseClient = 0;
                            price.remiseFamille = 0;
                            price.articleReference = item.Article.AR_Ref;
                        }
                        specialPrice.Add(price);
                    }
                }
                foreach (IBOFamilleTarifClient familleTarifClient in clientFC.FactoryClientTarifFamille.List)
                {
                    foreach (IBOArticle3 item in SingletonConnection.Instance.Gescom.FactoryArticle.QueryFamille(SingletonConnection.Instance.Gescom.FactoryFamille.ReadCode(familleTarifClient.Famille.FA_Type,familleTarifClient.Famille.FA_CodeFamille)))
                    {
                        int i = 0;
                        Boolean found = false;
                        double percentageFamille;
                        string[] remiseFa = familleTarifClient.Remise.ToString().Split('%');
                        Double.TryParse(remiseFa[0], out percentageFamille);
                        foreach (SpecialPriceClient priceClient in specialPrice)
                        {
                            if (item.AR_Ref.Equals(priceClient.articleReference))
                            {
                                found = true;
                                break;
                            }
                            i++;
                        }
                        if (!found)
                        {
                            SpecialPriceClient priceF = new SpecialPriceClient();
                            priceF.groupeName = CT_NUM;
                            priceF.price = item.AR_PrixVen;
                            priceF.remiseClient = RemiseGlobal;
                            priceF.remiseFamille = percentageFamille;
                            priceF.articleReference = item.AR_Ref;
                            specialPrice.Add(priceF);
                        }
                    }
                }
            }
            else
            {
                //var compta = SingletonConnection.Instance.Gescom.CptaApplication;
                IBOClient3 clientsSageObj = SingletonConnection.Instance.Gescom.CptaApplication.FactoryClient.ReadNumero(Central);
                RemiseGlobal = 0;
                if (clientsSageObj.TauxRemise > 0)
                {
                    RemiseGlobal = clientsSageObj.TauxRemise;
                }
                foreach (IBOArticleTarifClient3 item in clientsSageObj.FactoryClientTarif.List)
                {
                    SpecialPriceClient price = new SpecialPriceClient();
                    ProductSearchCriteria productMagento = UtilsWebservices.GetMagentoProduct("rest/all/V1/products", UtilsWebservices.SearchCriteria("sku", item.Article.AR_Ref, "eq"));
                    if (productMagento.TotalCount > 0)
                    {
                        if (item.Prix != 0)
                        {
                            price.groupeName = Central;
                            price.price = item.Prix;
                            price.remiseClient = 0;
                            price.remiseFamille = 0;
                            price.articleReference = item.Article.AR_Ref;
                        }
                        else
                        {
                            price.groupeName = Central;
                            price.price = item.Article.AR_PrixVen;
                            price.remiseClient = 0;
                            price.remiseFamille = 0;
                            price.articleReference = item.Article.AR_Ref;
                        }
                        specialPrice.Add(price);
                    }
                }
                foreach (IBOFamilleTarifClient familleTarifClient in clientsSageObj.FactoryClientTarifFamille.List)
                {
                    foreach (IBOArticle3 item in SingletonConnection.Instance.Gescom.FactoryArticle.QueryFamille(SingletonConnection.Instance.Gescom.FactoryFamille.ReadCode(familleTarifClient.Famille.FA_Type, familleTarifClient.Famille.FA_CodeFamille)))
                    {
                        int i = 0;
                        Boolean found = false;
                        double percentageFamille;
                        string[] remiseFa = familleTarifClient.Remise.ToString().Split('%');
                        Double.TryParse(remiseFa[0], out percentageFamille);
                        foreach (SpecialPriceClient priceClient in specialPrice)
                        {
                            if (item.AR_Ref.Equals(priceClient.articleReference))
                            {
                                found = true;
                                break;
                            }
                            i++;
                        }
                        if (!found)
                        {
                            SpecialPriceClient priceF = new SpecialPriceClient();
                            priceF.groupeName = Central;
                            priceF.price = item.AR_PrixVen;
                            priceF.remiseClient = RemiseGlobal;
                            priceF.remiseFamille = percentageFamille;
                            priceF.articleReference = item.AR_Ref;
                            specialPrice.Add(priceF);
                        }
                    }
                }
            }
            SpecialPrices = specialPrice;
            */
            #endregion
        }
        public Client()
        {

        }
        private string RemoveDiacritics(string stIn)
        {
            string stFormD = stIn.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }

            return (sb.ToString().Normalize(NormalizationForm.FormC));
        }
        public void setClientLivraisonAdresse()
        {
            //clientLivraisonAdresses = ControllerClientLivraisonAdress.getAllClientLivraisonAdressToProcess(clientFC.FactoryClientLivraison.List);
        }
    }
}
