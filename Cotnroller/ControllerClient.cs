using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Objets100cLib;
using WebservicesSage.Object;
using WebservicesSage.Singleton;
using WebservicesSage.Utils;
using WebservicesSage.Utils.Enums;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Xml;
using WebservicesSage.Object.CustomerSearch;
using WebservicesSage.Object.Order;
using System.Globalization;
using System.Timers;
using WebservicesSage.Object.CustomerSearchByEmail;
using WebservicesSage.Object.CatTarSearch;
using System.Data.SqlClient;
using System.Collections;

namespace WebservicesSage.Cotnroller
{
    public static class ControllerClient
    {
        public static void LaunchService()
        {
            System.Timers.Timer timerUpdateStatut = new System.Timers.Timer();
            timerUpdateStatut.Elapsed += new ElapsedEventHandler(SendAllClientsCron);
            timerUpdateStatut.Interval = 20000;
            timerUpdateStatut.Enabled = true;
        }
        public static void SendAllClientsCron(object source, ElapsedEventArgs e)
        {
            DateTime now1 = DateTime.Now;
            DateTime NowNoSecond2 = new DateTime(now1.Year, now1.Month, now1.Day, now1.Hour, now1.Minute, 0, 0);
            DateTime firstRun2 = new DateTime(NowNoSecond2.Year, NowNoSecond2.Month, NowNoSecond2.Day, 0, 0, 0, 0);
            if (NowNoSecond2 == firstRun2)
            {
                UtilsConfig.CRONSYNCHROCLIENTDONE = "FALSE";
            }
            string[] date = UtilsConfig.CRONSYNCHROCLIENT.ToString().Split(':');
            int hours, minutes;
            hours = Int32.Parse(date[0]);
            minutes = Int32.Parse(date[1]);
            DateTime now = DateTime.Now;
            DateTime NowNoSecond = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0, 0);
            DateTime firstRun = new DateTime(now.Year, now.Month, now.Day, hours, minutes, 0, 0);
            if (NowNoSecond == firstRun && UtilsConfig.CRONSYNCHROCLIENTDONE.ToString().Equals("FALSE"))
            {
                UtilsConfig.CRONSYNCHROCLIENTDONE = "TRUE";
                var compta = SingletonConnection.Instance.Gescom.CptaApplication;

                //var clients = GetListOfClientToProcess(clientsSageObj);
                string sql = "SELECT [CT_Num] FROM [" + System.Configuration.ConfigurationManager.AppSettings["DBNAME"].ToString() + "].[dbo].[F_COMPTET] where TIERS_WEB like 'OUI'";
                SqlDataReader AllClients = SingletonConnection.Instance.dB.Select(sql);
                while (AllClients.Read())
                {
                    string testClient = AllClients.GetValue(0).ToString();
                    try
                    {
                        
                        var clientsSageObj = compta.FactoryClient.ReadNumero(AllClients.GetValue(0).ToString());
                        SendCustomClient(clientsSageObj);
                        File.AppendAllText("Log\\SendClientCronTest.txt", DateTime.Now + " send client OK : " + AllClients.GetValue(0).ToString());
                    }
                    catch (Exception s)
                    {
                        File.AppendAllText("Log\\SendClientCronTest.txt", DateTime.Now + " erreur avec ce client : "+ testClient.ToString());
                        StringBuilder sb = new StringBuilder();
                        sb.Append(DateTime.Now + " "+ AllClients.GetValue(0).ToString() + Environment.NewLine);
                        sb.Append(DateTime.Now + s.Message + Environment.NewLine);
                        sb.Append(DateTime.Now + s.StackTrace + Environment.NewLine);
                        File.AppendAllText("Log\\SendAllClientCronError.txt", sb.ToString());
                        sb.Clear();
                    }
                }
                AllClients.Close();
                SendPriceBtoC();
            }
        }

        /// <summary>
        /// Permets de remonter toute la base clients de SAGE vers Prestashop
        /// Ne remonte que les clients avec un mail 
        /// </summary>
        public static void SendAllClients()
        {
            try
            {
                var compta = SingletonConnection.Instance.Gescom.CptaApplication;
                
                //var clients = GetListOfClientToProcess(clientsSageObj);
                string sql = "SELECT [CT_Num] FROM [" + System.Configuration.ConfigurationManager.AppSettings["DBNAME"].ToString() + "].[dbo].[F_COMPTET] where TIERS_WEB like 'OUI'";
                SqlDataReader AllClients = SingletonConnection.Instance.dB.Select(sql);
                while (AllClients.Read())
                {
                    try
                    {
                        var clientsSageObj = compta.FactoryClient.ReadNumero(AllClients.GetValue(0).ToString());
                        SendCustomClient(clientsSageObj);
                    }
                    catch (Exception s)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append(DateTime.Now + s.Message + Environment.NewLine);
                        sb.Append(DateTime.Now + s.StackTrace + Environment.NewLine);
                        File.AppendAllText("Log\\SendAllClientError.txt", sb.ToString());
                        sb.Clear();
                    }
                }
                AllClients.Close();

                MessageBox.Show("end client sync", "ok",
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Information);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Information);
            }
        }
        public static void SendCustomClient(IBOClient3 clientsSageObj)
        {
            #region OLD Version
            /*
            var clients = GetListOfClientToProcess(clientsSageObj);
            foreach (Client client in clients)
            {

                CustomerSearchByEmail ClientSearch = CustomerSearchByEmail.FromJson(UtilsWebservices.GetMagentoData("rest/V1/customers/search" + UtilsWebservices.SearchOrderCriteria("email", client.Email, "eq")));
                Customer customerMagento = new Customer();
                var jsonClient = JsonConvert.SerializeObject(customerMagento.NewCustomer(client, clientsSageObj, ClientSearch));
                if (ClientSearch.TotalCount > 0)
                {
                    UtilsWebservices.SendDataJson(jsonClient, @"rest/all/V1/customers/" + ClientSearch.Items[0].Id.ToString(), "PUT");
                }
                else
                {
                    UtilsWebservices.SendDataJson(jsonClient, @"rest/all/V1/customers/");
                }
                // Create specific price for each client/central
                Product product = new Product();
                ClientSearch = CustomerSearchByEmail.FromJson(UtilsWebservices.GetMagentoData("rest/V1/customers/search" + UtilsWebservices.SearchOrderCriteria("email", client.Email, "eq")));
                string sql = "SELECT [AR_Ref],[AR_PrixVen] FROM [" + System.Configuration.ConfigurationManager.AppSettings["DBNAME"].ToString() + "].[dbo].[F_ARTICLE] where AR_Publie = 1";

                SqlDataReader AllProduct = SingletonConnection.Instance.dB.Select(sql);
                int j = 0;
                List<object> Todelete = product.DeletePrixRemiseClientAllProducts(client, AllProduct);
                AllProduct.Close();

                foreach (var item in Todelete)
                {
                    var json = JsonConvert.SerializeObject(item);
                    string response = UtilsWebservices.SendDataJson(json, @"rest/V1/products/tier-prices-delete");
                    
                }

                foreach (SpecialPriceClient specialPriceClient in client.SpecialPrices)
                {
                    if (specialPriceClient.price > 0)
                    {
                        ProductSearchCriteria productMagento = UtilsWebservices.GetMagentoProduct("rest/all/V1/products", UtilsWebservices.SearchCriteria("sku", specialPriceClient.articleReference, "eq"));
                        if (productMagento.TotalCount > 0)
                        {
                            foreach (TiersPrice item in productMagento.Items[0].TierPrices)
                            {
                                if (item.CustomerGroupId.Equals(ClientSearch.Items[0].GroupId.ToString()))
                                {
                                    string deleteResponse = UtilsWebservices.SendDataJson("", @"/rest/V1/products/" + specialPriceClient.articleReference + "/group-prices/" + item.CustomerGroupId + "/tiers/" + item.Qty + "", "DELETE");
                                }
                            }
                        }
                        var json1 = JsonConvert.SerializeObject(product.PrixCustomGroupe(specialPriceClient));
                        string response1 = UtilsWebservices.SendDataJson(json1, @"rest/V1/products/tier-prices");
                    }
                }
                if (client.RemiseGlobal > 0)
                {
                    string sql1 = "SELECT [AR_Ref] ,[AR_PrixVen] FROM [" + System.Configuration.ConfigurationManager.AppSettings["DBNAME"].ToString() + "].[dbo].[F_ARTICLE] where AR_Publie = 1";
                    SqlDataReader AllProduct1 = SingletonConnection.Instance.dB.Select(sql1);
                    var json1 = JsonConvert.SerializeObject(product.PrixRemiseClientAllProducts(client, AllProduct1));
                    AllProduct1.Close();
                    string response1 = UtilsWebservices.SendDataJson(json1, @"rest/V1/products/tier-prices");

                }

            }*/
            #endregion
            Client client = new Client(clientsSageObj);
            CustomerSearchByEmail ClientSearch = CustomerSearchByEmail.FromJson(UtilsWebservices.GetMagentoData("rest/V1/customers/search" + UtilsWebservices.SearchOrderCriteria("email", client.Email, "eq")));
            Customer customerMagento = new Customer();
            var jsonClient = JsonConvert.SerializeObject(customerMagento.NewCustomer(client, clientsSageObj, ClientSearch));
            File.AppendAllText("Log\\testClient.txt", DateTime.Now + " " + jsonClient.ToString() + Environment.NewLine);
            if (ClientSearch.TotalCount > 0)
            {
                UtilsWebservices.SendDataJson(jsonClient, @"rest/all/V1/customers/" + ClientSearch.Items[0].Id.ToString(), "PUT");
            }
            else
            {
                UtilsWebservices.SendDataJson(jsonClient, @"rest/all/V1/customers/");
            }
            // Create specific price for each client/central
            Product product = new Product();
            ClientSearch = CustomerSearchByEmail.FromJson(UtilsWebservices.GetMagentoData("rest/V1/customers/search" + UtilsWebservices.SearchOrderCriteria("email", client.Email, "eq")));
            string sql = "SELECT [AR_Ref],[AR_PrixVen] FROM [" + System.Configuration.ConfigurationManager.AppSettings["DBNAME"].ToString() + "].[dbo].[F_ARTICLE] where AR_Publie = 1";

            SqlDataReader AllProduct = SingletonConnection.Instance.dB.Select(sql);
            List<object> Todelete = product.DeletePrixRemiseClientAllProducts(client, AllProduct);
            AllProduct.Close();
            foreach (var item in Todelete)
            {
                var json = JsonConvert.SerializeObject(item);
                string response = UtilsWebservices.SendDataJson(json, @"rest/V1/products/tier-prices-delete");
            }


            //a tester :

            var groupPrice = product.PrixAllCustomGroupe(client.SpecialPrices);
            foreach (var item in groupPrice)
            {
                var json1 = JsonConvert.SerializeObject(item);
                string response1 = UtilsWebservices.SendDataJson(json1, @"rest/V1/products/tier-prices");
            }
            if (client.RemiseGlobal > 0)
            {
                string sql1 = "SELECT [AR_Ref] ,[AR_PrixVen] FROM [" + System.Configuration.ConfigurationManager.AppSettings["DBNAME"].ToString() + "].[dbo].[F_ARTICLE] where AR_Publie = 1";
                SqlDataReader AllProduct1 = SingletonConnection.Instance.dB.Select(sql1);
                var json2 = JsonConvert.SerializeObject(product.PrixRemiseClientAllProducts(client, AllProduct1));
                string response2 = UtilsWebservices.SendDataJson(json2, @"rest/V1/products/tier-prices");
                AllProduct1.Close();
            }
            File.AppendAllText("Log\\test.txt", DateTime.Now + " fin de traitement" + Environment.NewLine);
        }
        public static void SendPriceBtoC()
        {
            string sql2 = "Select article.AR_Ref,artClient.AC_PrixVen from " + System.Configuration.ConfigurationManager.AppSettings["DBNAME"].ToString() + ".dbo.F_ARTICLE AS article left join " + System.Configuration.ConfigurationManager.AppSettings["DBNAME"].ToString() + ".dbo.F_ARTCLIENT AS artClient on article.AR_Ref = artClient.AR_Ref  where article.AR_Publie = 1 and artClient.AC_Categorie = "+UtilsConfig.CatTarifB2C.ToString();
            Product product = new Product();
            SqlDataReader BtoCFixedPrice = SingletonConnection.Instance.dB.Select(sql2);
            ArrayList priceBtoC = product.PrixRemiseBtoC(BtoCFixedPrice);
            foreach (var item in priceBtoC)
            {
                var json1 = JsonConvert.SerializeObject(item);
                File.AppendAllText("Log\\BtoC.txt", DateTime.Now + " " + json1.ToString() + Environment.NewLine);
                string response1 = UtilsWebservices.SendDataJsonB2C(json1, @"rest/V1/products/tier-prices");
            }
            BtoCFixedPrice.Close();
        }
        public static void SendClient(string ct_num)
        {
            try
            {
                string ClientSQL = "SELECT CT_Num from " + System.Configuration.ConfigurationManager.AppSettings["DBNAME"].ToString() + ".dbo.F_COMPTET where CT_Num = '" + ct_num + "' and TIERS_WEB = 'OUI'";
                SqlDataReader clientToSynch = SingletonConnection.Instance.dB.Select(ClientSQL);
                string toSynch="";
                while (clientToSynch.Read())
                {
                    toSynch = clientToSynch.GetValue(0).ToString();
                }
                clientToSynch.Close();
                if (!String.IsNullOrEmpty(toSynch))
                {
                    var compta = SingletonConnection.Instance.Gescom.CptaApplication;
                    var clientsSageObj = compta.FactoryClient.ReadNumero(ct_num);
                    Client client = new Client(clientsSageObj);
                    CustomerSearchByEmail ClientSearch = CustomerSearchByEmail.FromJson(UtilsWebservices.GetMagentoData("rest/V1/customers/search" + UtilsWebservices.SearchOrderCriteria("email", client.Email, "eq")));
                    Customer customerMagento = new Customer();
                    var jsonClient = JsonConvert.SerializeObject(customerMagento.NewCustomer(client, clientsSageObj, ClientSearch));
                    File.AppendAllText("Log\\test.txt", DateTime.Now +" "+ jsonClient.ToString() + Environment.NewLine);
                    if (ClientSearch.TotalCount > 0)
                    {
                        UtilsWebservices.SendDataJson(jsonClient, @"rest/all/V1/customers/" + ClientSearch.Items[0].Id.ToString(), "PUT");
                    }
                    else
                    {
                        UtilsWebservices.SendDataJson(jsonClient, @"rest/all/V1/customers/");
                    }
                    // Create specific price for each client/central
                    Product product = new Product();
                    ClientSearch = CustomerSearchByEmail.FromJson(UtilsWebservices.GetMagentoData("rest/V1/customers/search" + UtilsWebservices.SearchOrderCriteria("email", client.Email, "eq")));
                    string sql = "SELECT [AR_Ref],[AR_PrixVen] FROM [" + System.Configuration.ConfigurationManager.AppSettings["DBNAME"].ToString() + "].[dbo].[F_ARTICLE] where AR_Publie = 1";

                    SqlDataReader AllProduct = SingletonConnection.Instance.dB.Select(sql);
                    List<object> Todelete = product.DeletePrixRemiseClientAllProducts(client, AllProduct);
                    AllProduct.Close();
                    foreach (var item in Todelete)
                    {
                        var json = JsonConvert.SerializeObject(item);
                        string response = UtilsWebservices.SendDataJson(json, @"rest/V1/products/tier-prices-delete");
                    }


                    //a tester :

                    var groupPrice = product.PrixAllCustomGroupe(client.SpecialPrices);
                    foreach (var item in groupPrice)
                    {
                        var json1 = JsonConvert.SerializeObject(item);
                        string response1 = UtilsWebservices.SendDataJson(json1, @"rest/V1/products/tier-prices");
                    }

                    if (client.RemiseGlobal > 0)
                    {
                        string sql1 = "SELECT [AR_Ref] ,[AR_PrixVen] FROM [" + System.Configuration.ConfigurationManager.AppSettings["DBNAME"].ToString() + "].[dbo].[F_ARTICLE] where AR_Publie = 1";
                        SqlDataReader AllProduct1 = SingletonConnection.Instance.dB.Select(sql1);
                        var json2 = JsonConvert.SerializeObject(product.PrixRemiseClientAllProducts(client, AllProduct1));
                        string response2 = UtilsWebservices.SendDataJson(json2, @"rest/V1/products/tier-prices");
                        AllProduct1.Close();
                    }
                    File.AppendAllText("Log\\test.txt", DateTime.Now + " fin de traitement" + Environment.NewLine);
                    MessageBox.Show("end client sync", "ok",
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("le client n'est pas à synchroniser", "Error",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Information);
                }              
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Information);
                File.AppendAllText("Log\\errorSendClient.txt", DateTime.Now + e.Message + Environment.NewLine);
                File.AppendAllText("Log\\errorSendClient.txt", DateTime.Now + e.Source + Environment.NewLine);
                File.AppendAllText("Log\\errorSendClient.txt", DateTime.Now + e.StackTrace + Environment.NewLine);
            }
        }

        /// <summary>
        /// Permet de vérifier si un client comporte des erreur ou non
        /// </summary>
        /// <param name="client">Client à tester</param>
        /// <returns></returns>
        private static bool HandleClientError(Client client)
        {
            bool error = false;

            if (String.IsNullOrEmpty(client.Email))
            {
                error = true;
                // SingletonUI.Instance.LogBox.Invoke((MethodInvoker)(() => SingletonUI.Instance.LogBox.AppendText("Client :  " + client.Intitule + " No Mail Found" + Environment.NewLine)));


                // on affiche une erreur + log 
            }

            return error;
        }

        /// <summary>
        /// Permet de récupérer une liste de Client depuis une liste de Client SAGE
        /// </summary>
        /// <param name="clientsSageObj">List de client SAGE</param>
        /// <returns></returns>
        private static List<Client> GetListOfClientToProcess(IBOClient3 clientSageObj)
        {
            List<Client> clientToProcess = new List<Client>();
            //var gescom = SingletonConnection.Instance.Gescom;
            var infolibreField = Singleton.SingletonConnection.Instance.Compta.FactoryTiers.InfoLibreFields;
            int compteur = 1;
            Boolean sendClient = false;

            foreach (var infoLibreValue in clientSageObj.InfoLibre)
            {
                //File.AppendAllText("Log\\infolibre.txt", " infolibre value + name : " + infoLibreValue.ToString() + " | " + infolibreField[compteur].Name + Environment.NewLine);

                if (infolibreField[compteur].Name.ToUpper().Equals("TIERS_WEB"))
                {
                    if (infoLibreValue.ToString().ToUpper().Equals("OUI"))
                    {
                     sendClient = true;
                    }
                    else
                    {
                        //File.AppendAllText("Log\\ClientBlocke.txt", " ct_num blocked : " + clientSageObj.CT_Num + Environment.NewLine);
                    }
                }
                compteur++;
                //var infolibreField = Singleton.SingletonConnection.Instance.Compta.FactoryTiers.InfoLibreFields;
            }

            if (!String.IsNullOrEmpty(clientSageObj.Telecom.EMail) && sendClient)
                    {
                        Client client = new Client(clientSageObj);
                        //client.setClientLivraisonAdresse();
                //File.AppendAllText("Log\\verif.txt", " ct_num non blocked : " + clientSageObj.CT_Num + Environment.NewLine);
                clientToProcess.Add(client);
                    }
            return clientToProcess;
        }

        private static List<Client> GetClientToProcess(IBOClient3 clientsSageObj)
        {
            List<Client> clientToProcess = new List<Client>();

            Client client = new Client(clientsSageObj);

            if (!HandleClientError(client))
            {
                //client.setClientLivraisonAdresse();
                clientToProcess.Add(client);
            }
            return clientToProcess;
        }

        /// <summary>
        /// Permet de vérifier si un Client existe dans SAGE
        /// </summary>
        /// <param name="CT_num"></param>
        /// <returns></returns>
        public static bool CheckIfClientExist(string CT_num)
        {
            if (String.IsNullOrEmpty(CT_num))
            {
                return false;
            }
            else
            {
                var compta = SingletonConnection.Instance.Gescom.CptaApplication;
                if (compta.FactoryTiers.ExistNumero(CT_num))// FactoryClient.ExistNumero(CT_num))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }

        }

        /// <summary>
        /// ToDo
        /// </summary>
        public static void CreateNewClient()
        {

        }

        /// <summary>
        /// Permet de crée un Client dans la base SAGE depuis un objet client et un objet commande Magento
        /// </summary>
        /// <param name="jsonClient">json du Client à crée</param>
        /// <returns></returns>
        public static string CreateNewClient(CustomerSearch customer, OrderSearchItem order)
        {
            //JObject customer = JObject.Parse(jsonClient);
            var compta = SingletonConnection.Instance.Gescom.CptaApplication;
            var gescom = SingletonConnection.Instance.Gescom;
            IBOClient3 clientSage = (IBOClient3)compta.FactoryClient.Create();
            clientSage.SetDefault();
            
            //clientSage.comp
                /*
            var test = clientSage.FactoryTiersContact.Create();
            test.Factory.List[0].*/
            try
            {
                Object.CustomerSearch.Address defaultAddress = new Object.CustomerSearch.Address();
                foreach (Object.CustomerSearch.Address addressCustomer in customer.Addresses)
                {
                    if (addressCustomer.Id == customer.DefaultBilling)
                    {
                        defaultAddress = addressCustomer;
                        break;
                    }
                }
                if (String.IsNullOrEmpty(UtilsConfig.PrefixClient))
                {
                    // pas de configuration renseigner pour le prefix client
                    // todo log
                    //int iterID = Int32.Parse(UtilsWebservices.SendDataNoParse(UtilsConfig.BaseUrl + EnumEndPoint.Client.Value, "getClientIterationSage&clientID=" + customer["id"].ToString()));
                    int iterID = Int32.Parse(customer.Id.ToString());
                    while (compta.FactoryClient.ExistNumero(iterID.ToString()))
                    {
                        iterID++;
                    }
                    clientSage.CT_Num = iterID.ToString();
                }
                else
                {
                    clientSage.CT_Num = UtilsConfig.PrefixClient + customer.Id.ToString();
                }
                string intitule;//TODO TAKE CARE OF NULL VALUE
                if (!String.IsNullOrEmpty(defaultAddress.company))
                {
                    intitule = defaultAddress.company.ToUpper();
                }
                else
                {
                    intitule = defaultAddress.Firstname.ToString().ToUpper() + " " + defaultAddress.Lastname.ToString().ToUpper();
                }
                clientSage.Write();
                
                if (defaultAddress.Street.Count >0)
                {
                    if (defaultAddress.Street[0].Length > 35)
                    {
                        clientSage.Adresse.Adresse = defaultAddress.Street[0].ToString().Substring(0, 35);
                    }
                    else
                    {
                        clientSage.Adresse.Adresse = defaultAddress.Street[0].ToString();
                    }
                }
                    
                    if (defaultAddress.Street.Count > 1)
                    {
                        clientSage.Adresse.Complement = defaultAddress.Street[1].ToString();
                    }
                    
                    clientSage.Adresse.CodePostal = defaultAddress.Postcode.ToString();
                    clientSage.Adresse.Ville = defaultAddress.City.ToString();
                    var region = CultureInfo
                                    .GetCultures(CultureTypes.SpecificCultures)
                                    .Select(ci => new RegionInfo(ci.LCID))
                                    .FirstOrDefault(rg => rg.TwoLetterISORegionName == defaultAddress.CountryId);
                    clientSage.Adresse.Pays = region.DisplayName.ToString();
                    clientSage.Telecom.Telephone = defaultAddress.Telephone.ToString();
                //clientSage.Telecom.Telecopie = customer.Addresses[0]..ToString();


                /*
                if (String.IsNullOrEmpty(UtilsConfig.CatTarif))
                {
                    // pas de configuration renseigner pour la cat tarif par defaut
                    // todo log
                }
                else
                {
                    clientSage.CatTarif = gescom.FactoryCategorieTarif.ReadIntitule(UtilsConfig.CatTarif);
                }
                if (String.IsNullOrEmpty(UtilsConfig.CompteG))
                {
                    // pas de configuration renseigner pour la cat tarif par defaut
                    // todo log
                }
                else
                {
                    clientSage.CompteGPrinc = compta.FactoryCompteG.ReadNumero(UtilsConfig.CompteGnum);
                }
                */
                string contactS = defaultAddress.Firstname.ToString() + " " + defaultAddress.Lastname.ToString();
                if (contactS.Length > 35)
                {
                    clientSage.CT_Contact = contactS.Substring(0, 35);
                }
                else
                {
                    clientSage.CT_Contact = contactS;
                }

                clientSage.Telecom.EMail = customer.Email.ToString();
                if (intitule.Length > 35)
                {
                    clientSage.CT_Intitule = intitule.Substring(0, 35);
                }
                else
                {
                    clientSage.CT_Intitule = intitule;
                }

                // abrégé client 
                if (intitule.Length > 17)
                {
                    clientSage.CT_Classement = intitule.Substring(0, 17);
                }
                else
                {
                    clientSage.CT_Classement = intitule;
                }
                if (!String.IsNullOrEmpty(customer.taxvat))
                {
                    clientSage.CT_Identifiant = customer.taxvat;
                }
                else
                {
                    clientSage.CT_Identifiant = "";
                }
                

                if (region.DisplayName.ToString().ToUpper() != "FRANCE" && !clientSage.CT_Identifiant.ToString().Equals(""))
                {
                    try
                    {
                        IBICategorieCompta categorieCompta = gescom.FactoryCategorieComptaVente.ReadIntitule(UtilsConfig.CategorieComptableForeigner);
                        clientSage.CategorieCompta = categorieCompta;
                    }
                    catch (Exception e)
                    {
                        UtilsMail.SendErrorMail(DateTime.Now + e.Message + Environment.NewLine + e.StackTrace + Environment.NewLine, "CREATE CLIENT CATEGORIE COMPTABLE");
                    }
                }
            }
            catch (Exception e)
            {
                UtilsMail.SendErrorMail(DateTime.Now + e.Message + Environment.NewLine + e.StackTrace + Environment.NewLine, "CREATE CLIENT INFO G");
            }

            clientSage.Write();

            try
            {
                IBOClientLivraison3 addrprinc = (IBOClientLivraison3)clientSage.FactoryClientLivraison.Create();

                if (!String.IsNullOrEmpty(order.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.company))
                {
                    addrprinc.LI_Intitule = order.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.company.ToUpper();
                }
                else
                {
                    string intitule = "";
                    intitule = order.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.Firstname + " " + order.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.Lastname;
                    if (intitule.Length >35)
                    {
                        intitule.Substring(0, 35);
                    }
                    addrprinc.LI_Intitule = intitule.ToUpper();
                }

                if (order.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.Street[0].ToString().Length > 35)
                {
                    addrprinc.Adresse.Adresse = order.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.Street[0].ToString().Substring(0, 35);
                }
                else
                {
                    addrprinc.Adresse.Adresse = order.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.Street[0].ToString();
                }
                if (order.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.Street.Count > 1)
                {
                    addrprinc.Adresse.Complement = order.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.Street[1].ToString();
                }
                addrprinc.Adresse.CodePostal = order.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.Postcode;
                addrprinc.Adresse.Ville = order.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.City;
                var region = CultureInfo
                                    .GetCultures(CultureTypes.SpecificCultures)
                                    .Select(ci => new RegionInfo(ci.LCID))
                                    .FirstOrDefault(rg => rg.TwoLetterISORegionName == order.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.CountryId);
                addrprinc.Adresse.Pays = region.DisplayName.ToString();
                addrprinc.Telecom.Telephone = order.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.Telephone.ToString();
                //addrprinc.Telecom.Telecopie = order.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.Telecopie.ToString();

                if (String.IsNullOrEmpty(UtilsConfig.CondLivraison))
                {
                    // pas de configuration renseigner pour CondLivraison par defaut
                    // todo log
                }
                else
                {
                    addrprinc.ConditionLivraison = gescom.FactoryConditionLivraison.ReadIntitule(UtilsConfig.CondLivraison);
                }
                if (String.IsNullOrEmpty(UtilsConfig.Expedition))
                {
                    // pas de configuration renseigner pour Expedition par defaut
                    // todo log
                }
                else
                {
                    addrprinc.Expedition = gescom.FactoryExpedition.ReadIntitule(UtilsConfig.Expedition);
                }
                clientSage.LivraisonPrincipal = addrprinc;
                addrprinc.Write();
            }
            catch (Exception e)
            {
                UtilsMail.SendErrorMail(DateTime.Now + e.Message + Environment.NewLine + e.StackTrace + Environment.NewLine, "CREATE CLIENT ADRESS P");
            }
            return clientSage.CT_Num;
        }

        public static string CreateNewClientDevis(CustomerSearch customer, Object.Devis.Devis order)
        {
            //JObject customer = JObject.Parse(jsonClient);
            var compta = SingletonConnection.Instance.Gescom.CptaApplication;
            var gescom = SingletonConnection.Instance.Gescom;
            IBOClient3 clientSage = (IBOClient3)compta.FactoryClient.Create();
            clientSage.SetDefault();/*
            var test = clientSage.FactoryTiersContact.Create();
            test.Factory.List[0].*/
            try
            {
                if (customer.Addresses.Count >0)
                {
                    Object.CustomerSearch.Address defaultAddress = new Object.CustomerSearch.Address();
                    foreach (Object.CustomerSearch.Address addressCustomer in customer.Addresses)
                    {
                        if (addressCustomer.Id == customer.DefaultBilling)
                        {
                            defaultAddress = addressCustomer;
                            break;
                        }
                    }
                    if (defaultAddress.Street[0].Length > 35)
                    {
                        clientSage.Adresse.Adresse = defaultAddress.Street[0].ToString().Substring(0, 35);
                    }
                    else
                    {
                        clientSage.Adresse.Adresse = defaultAddress.Street[0].ToString();
                    }
                    if (defaultAddress.Street.Count > 1)
                    {
                        clientSage.Adresse.Complement = defaultAddress.Street[1].ToString();
                    }

                    clientSage.Adresse.CodePostal = defaultAddress.Postcode.ToString();
                    clientSage.Adresse.Ville = defaultAddress.City.ToString();
                    var region = CultureInfo
                                    .GetCultures(CultureTypes.SpecificCultures)
                                    .Select(ci => new RegionInfo(ci.LCID))
                                    .FirstOrDefault(rg => rg.TwoLetterISORegionName == defaultAddress.CountryId);
                    clientSage.Adresse.Pays = region.DisplayName.ToString();
                    clientSage.Telecom.Telephone = defaultAddress.Telephone.ToString();
                    string intitule;//TODO TAKE CARE OF NULL VALUE
                    if (!String.IsNullOrEmpty(defaultAddress.company))
                    {
                        intitule = defaultAddress.company.ToUpper();
                    }
                    else
                    {
                        intitule = defaultAddress.Firstname.ToString().ToUpper() + " " + defaultAddress.Lastname.ToString().ToUpper();
                    }
                    string contactS = defaultAddress.Firstname.ToString() + " " + defaultAddress.Lastname.ToString();
                    if (contactS.Length > 35)
                    {
                        clientSage.CT_Contact = contactS.Substring(0, 35);
                    }
                    else
                    {
                        clientSage.CT_Contact = contactS;
                    }

                    
                    if (intitule.Length > 35)
                    {
                        clientSage.CT_Intitule = intitule.Substring(0, 35);
                    }
                    else
                    {
                        clientSage.CT_Intitule = intitule;
                    }

                    // abrégé client 
                    if (intitule.Length > 17)
                    {
                        clientSage.CT_Classement = intitule.Substring(0, 17);
                    }
                    else
                    {
                        clientSage.CT_Classement = intitule;
                    }
                    /*
                    if (region.DisplayName.ToString().ToUpper() != "FRANCE" && !clientSage.CT_Identifiant.ToString().Equals(""))
                    {
                        try
                        {
                            IBICategorieCompta categorieCompta = gescom.FactoryCategorieComptaVente.ReadIntitule(UtilsConfig.CategorieComptableForeigner);
                            clientSage.CategorieCompta = categorieCompta;
                        }
                        catch (Exception e)
                        {
                            UtilsMail.SendErrorMail(DateTime.Now + e.Message + Environment.NewLine + e.StackTrace + Environment.NewLine, "CREATE CLIENT CATEGORIE COMPTABLE");
                        }
                    }*/
                }
                else
                {
                    string intitule;//TODO TAKE CARE OF NULL VALUE
                    intitule = customer.Firstname.ToString().ToUpper() + " " + customer.Lastname.ToString().ToUpper();
                    if (intitule.Length > 35)
                    {
                        clientSage.CT_Intitule = intitule.Substring(0, 35);
                    }
                    else
                    {
                        clientSage.CT_Intitule = intitule;
                    }
                }


                //clientSage.Telecom.Telecopie = customer.Addresses[0]..ToString();

                clientSage.Telecom.EMail = customer.Email.ToString();

                if (String.IsNullOrEmpty(UtilsConfig.PrefixClient))
                {
                    // pas de configuration renseigner pour le prefix client
                    // todo log
                    //int iterID = Int32.Parse(UtilsWebservices.SendDataNoParse(UtilsConfig.BaseUrl + EnumEndPoint.Client.Value, "getClientIterationSage&clientID=" + customer["id"].ToString()));
                    int iterID = Int32.Parse(customer.Id.ToString());
                    while (compta.FactoryClient.ExistNumero(iterID.ToString()))
                    {
                        iterID++;
                    }
                    clientSage.CT_Num = iterID.ToString();
                }
                else
                {
                    clientSage.CT_Num = UtilsConfig.PrefixClient + customer.Id.ToString();
                }
                clientSage.Write();

                if (!String.IsNullOrEmpty(customer.taxvat))
                {
                    clientSage.CT_Identifiant = customer.taxvat;
                }
                else
                {
                    clientSage.CT_Identifiant = "";
                }
            }
            catch (Exception e)
            {
                UtilsMail.SendErrorMail(DateTime.Now + e.Message + Environment.NewLine + e.StackTrace + Environment.NewLine, "CREATE CLIENT INFO G");
            }
            clientSage.Write();

            return clientSage.CT_Num;
        }
        public static IBOClient3 CheckIfClientEmailExist(string email)
        {
            //var compta = SingletonConnection.Instance.Gescom.CptaApplication;
            foreach (IBOClient3 client3 in SingletonConnection.Instance.Compta.FactoryClient.List)
            {
                if (client3.Telecom.EMail.ToUpper().Equals(email.ToUpper()))
                {
                    return client3;
                }
            }
            return null;
        }
    }
}
