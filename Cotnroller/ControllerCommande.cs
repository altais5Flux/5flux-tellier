using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using WebservicesSage.Utils;
using WebservicesSage.Utils.Enums;
using WebservicesSage.Singleton;
using Objets100cLib;
using WebservicesSage.Object;
using WebservicesSage.Object.DBObject;
using System.Windows.Forms;
using LiteDB;
using System.IO;
using Newtonsoft.Json;
using WebservicesSage.Object.Order;
using WebservicesSage.Object.CustomerSearch;
using System.Globalization;
using WebservicesSage.Object.Devis;
using Customer = WebservicesSage.Object.Customer;
using WebservicesSage.Object.CustomerSearchByEmail;
using System.Data.SqlClient;
using CustomAttribute = WebservicesSage.Object.CustomerSearch.CustomAttribute;

namespace WebservicesSage.Cotnroller
{
    public static class ControllerCommande
    {

        /// <summary>
        /// Lance le service de check des nouvelles commandes prestashop
        /// Définir le temps de passage de la tâche dans la config
        /// </summary>
        public static void LaunchService()
        {
            // SingletonUI.Instance.LogBox.Invoke((MethodInvoker)(() => SingletonUI.Instance.LogBox.AppendText("Commande Services Launched " + Environment.NewLine)));
            
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Elapsed += new ElapsedEventHandler(CheckForNewOrderMagento);
            timer.Interval = UtilsConfig.CronTaskCheckForNewOrder;
            timer.Enabled = true;
            
            System.Timers.Timer timerUpdateStatut = new System.Timers.Timer();
            timerUpdateStatut.Elapsed += new ElapsedEventHandler(UpdateStatuOrder);
            timerUpdateStatut.Interval = UtilsConfig.CronTaskUpdateStatut;
            timerUpdateStatut.Enabled = true;
            
            
        }

        /// <summary>
        /// Event levé par une nouvelle commande dans prestashop
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        /*
         private static void CheckForNewOrder(object source, ElapsedEventArgs e)
        {

            string currentIdOrder = "0";
            try
            {
                string response = UtilsWebservices.SendData(UtilsConfig.BaseUrl + EnumEndPoint.Commande.Value, "checkOrder");
                if (!response.Equals("none") && !response.Equals("[]"))
                {
                    JArray orders = JArray.Parse(response);
                    List<int> currentCustomer_IDs = new List<int>();
                    foreach (var order in orders)
                    {
                        currentIdOrder = order["id_order"].ToString();
                        if (ControllerClient.CheckIfClientExist(order["ALTAIS_CT_NUM"].ToString()))
                        {
                            // si le client existe on associé la commande à son compte
                            AddNewOrderForCustomer(order, order["ALTAIS_CT_NUM"].ToString());
                        }
                        else
                        {
                            // si le client n'existe pas on récupère les info de presta et on le crée dans la base sage 
                            string client = UtilsWebservices.SendDataNoParse(UtilsConfig.BaseUrl + EnumEndPoint.Client.Value, "getClient&clientID=" + order["id_customer"]);
                            string ct_num = ControllerClient.CreateNewClient(client, order);

                            if (!String.IsNullOrEmpty(ct_num))
                            {
                                // le client à bien été crée on peut intégrer la commande sur son compte sage
                                AddNewOrderForCustomer(order, ct_num);
                            }
                        }
                    }
                }
            }
            catch (Exception s)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(DateTime.Now+ s.Message + Environment.NewLine);
                sb.Append(DateTime.Now + s.StackTrace + Environment.NewLine);
                UtilsMail.SendErrorMail(DateTime.Now + s.Message + Environment.NewLine + s.StackTrace + Environment.NewLine, "COMMANDE");
                File.AppendAllText("Log\\order.txt", sb.ToString());
                sb.Clear();
                UtilsWebservices.SendDataNoParse(UtilsConfig.BaseUrl + EnumEndPoint.Commande.Value, "errorOrder&orderID=" + currentIdOrder);
            }

        }*/
        public static void CheckForNewOrderMagento(object source, ElapsedEventArgs e)
        {
            File.AppendAllText("Log\\cron.txt", DateTime.Now + " start cron commande" + Environment.NewLine);
            CheckForNewOrderMagentoB2C();
            CheckForNewOrderMagentoB2B();
            File.AppendAllText("Log\\cron.txt", DateTime.Now + " end cron commande" + Environment.NewLine);
            
        }

        public static void CheckForNewOrderMagentoB2BFlag2()
        {
            string currentIdOrder = "";
            string currentIncrementedId = "";
            try
            {
                string response = UtilsWebservices.SearchOrder(UtilsConfig.BaseUrlB2B + @"rest/V1/orders", UtilsWebservices.SearchOrderCriteria("order_flag", "2", "eq"));
                //File.AppendAllText("Log\\commande.txt", response.ToString() + Environment.NewLine);
                OrderSearch orderSearch = OrderSearch.FromJson(response);
                if (orderSearch.TotalCount > 0)
                {
                    //todo create BC sage
                    for (int i = 0; i < orderSearch.TotalCount; i++)
                    {
                        string clientCtNum = "";
                        string clienttype = "";
                        currentIdOrder = orderSearch.Items[i].EntityId.ToString();
                        currentIncrementedId = orderSearch.Items[i].IncrementId.ToString();
                        if (orderSearch.Items[i].Status.Equals("canceled") || orderSearch.Items[i].State.Equals("canceled") || orderSearch.Items[i].Status.Equals("holded") || orderSearch.Items[i].State.Equals("holded"))
                        {
                            var jsonFlag = JsonConvert.SerializeObject(UpdateOrderFlag(currentIdOrder, currentIncrementedId, "3"));
                            UtilsWebservices.SendDataJson(jsonFlag, @"rest/all/V1/orders/", "POST");
                            continue;
                        }
                        CustomerSearch client = UtilsWebservices.GetClientCtNum(orderSearch.Items[i].CustomerId.ToString());
                        // création des commande à partir du BtoC
                        if (orderSearch.Items[i].StoreId.ToString().Equals("4") || orderSearch.Items[i].StoreId.ToString().Equals("5") || orderSearch.Items[i].StoreId.ToString().Equals("6"))
                        {
                            continue;
                            //AddNewOrderForCustomerESHOP(orderSearch.Items[i], client);
                        }
                        else //Creation de commande à partir de BtoB
                        {
                            try
                            {
                                for (int j = 0; j < client.CustomAttributes.Count; j++)
                                {
                                    if (client.CustomAttributes[j].AttributeCode.Equals("sage_number"))
                                    {
                                        clientCtNum = client.CustomAttributes[j].Value.ToString();
                                        break;
                                    }
                                }
                            }
                            catch (Exception exception)
                            {

                                clientCtNum = "";
                            }
                            if (ControllerClient.CheckIfClientExist(clientCtNum))
                            {

                                // si le client existe on associé la commande à son compte
                                AddNewOrderForCustomer(orderSearch.Items[i], clientCtNum, client);

                            }
                        }


                    }
                }
            }
            catch (Exception s)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(DateTime.Now + s.Message + Environment.NewLine);
                sb.Append(DateTime.Now + s.StackTrace + Environment.NewLine);
                //UtilsMail.SendErrorMail(DateTime.Now + s.Message + Environment.NewLine + s.StackTrace + Environment.NewLine, "COMMANDE");
                File.AppendAllText("Log\\order.txt", sb.ToString());
                sb.Clear();
                var jsonFlag = JsonConvert.SerializeObject(UpdateOrderFlag(currentIdOrder, currentIncrementedId, "2"));
                UtilsWebservices.SendDataJson(jsonFlag, @"rest/all/V1/orders/", "POST");
            }

        }


        public static void CheckForNewOrderMagentoB2B()
        {
            string currentIdOrder = "";
            string currentIncrementedId = "";
            try
            {
                string response = UtilsWebservices.SearchOrder(UtilsConfig.BaseUrlB2B + @"rest/V1/orders", UtilsWebservices.SearchOrderCriteria("order_flag", UtilsConfig.Flag.ToString(), "eq"));
                //File.AppendAllText("Log\\commande.txt", response.ToString() + Environment.NewLine);
                OrderSearch orderSearch = OrderSearch.FromJson(response);
                if (orderSearch.TotalCount > 0)
                {
                    //todo create BC sage
                    for (int i = 0; i < orderSearch.TotalCount; i++)
                    {
                        string clientCtNum = "";
                        string clienttype = "";
                        currentIdOrder = orderSearch.Items[i].EntityId.ToString();
                        currentIncrementedId = orderSearch.Items[i].IncrementId.ToString();
                        if (orderSearch.Items[i].Status.Equals("canceled") || orderSearch.Items[i].State.Equals("canceled") || orderSearch.Items[i].Status.Equals("holded") || orderSearch.Items[i].State.Equals("holded"))
                        {
                            var jsonFlag = JsonConvert.SerializeObject(UpdateOrderFlag(currentIdOrder, currentIncrementedId, "3"));
                            UtilsWebservices.SendDataJson(jsonFlag, @"rest/all/V1/orders/", "POST");
                            continue;
                        }
                        CustomerSearch client = UtilsWebservices.GetClientCtNum(orderSearch.Items[i].CustomerId.ToString());
                        // création des commande à partir du BtoC
                        if (orderSearch.Items[i].StoreId.ToString().Equals("4") || orderSearch.Items[i].StoreId.ToString().Equals("5") || orderSearch.Items[i].StoreId.ToString().Equals("6"))
                        {
                            continue;
                            //AddNewOrderForCustomerESHOP(orderSearch.Items[i], client);
                        }
                        else //Creation de commande à partir de BtoB
                        {
                            try
                            {
                                for (int j = 0; j < client.CustomAttributes.Count; j++)
                                {
                                    if (client.CustomAttributes[j].AttributeCode.Equals("sage_number"))
                                    {
                                        clientCtNum = client.CustomAttributes[j].Value.ToString();
                                        break;
                                    }
                                }
                            }
                            catch (Exception exception)
                            {

                                clientCtNum = "";
                            }
                            if (ControllerClient.CheckIfClientExist(clientCtNum))
                            {

                                // si le client existe on associé la commande à son compte
                                AddNewOrderForCustomer(orderSearch.Items[i], clientCtNum, client);

                            }
                        }


                    }
                }
            }
            catch (Exception s)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(DateTime.Now + s.Message + Environment.NewLine);
                sb.Append(DateTime.Now + s.StackTrace + Environment.NewLine);
                //UtilsMail.SendErrorMail(DateTime.Now + s.Message + Environment.NewLine + s.StackTrace + Environment.NewLine, "COMMANDE");
                File.AppendAllText("Log\\order.txt", sb.ToString());
                sb.Clear();
                var jsonFlag = JsonConvert.SerializeObject(UpdateOrderFlag(currentIdOrder, currentIncrementedId, "2"));
                UtilsWebservices.SendDataJson(jsonFlag, @"rest/all/V1/orders/", "POST");
            }

        }

        public static void CheckForNewOrderMagentoB2CFlag2()
        {
            string currentIdOrder = "";
            string currentIncrementedId = "";
            try
            {
                string response = UtilsWebservices.SearchOrder(UtilsConfig.BaseUrlB2C + @"rest/V1/orders", UtilsWebservices.SearchOrderCriteria("order_flag", "2", "eq"), "B2C");
                //File.AppendAllText("Log\\commande.txt", response.ToString() + Environment.NewLine);
                OrderSearch orderSearch = OrderSearch.FromJson(response);
                if (orderSearch.TotalCount > 0)
                {
                    //todo create BC sage
                    for (int i = 0; i < orderSearch.TotalCount; i++)
                    {
                        string clientCtNum = "";
                        string clienttype = "";
                        currentIdOrder = orderSearch.Items[i].EntityId.ToString();
                        currentIncrementedId = orderSearch.Items[i].IncrementId.ToString();
                        if (orderSearch.Items[i].Status.Equals("canceled") || orderSearch.Items[i].State.Equals("canceled") || orderSearch.Items[i].Status.Equals("holded") || orderSearch.Items[i].State.Equals("holded") || orderSearch.Items[i].Status.Equals("pending") || orderSearch.Items[i].State.Equals("pending"))
                        {
                            var jsonFlag = JsonConvert.SerializeObject(UpdateOrderFlag(currentIdOrder, currentIncrementedId, "3"));
                            UtilsWebservices.SendDataJsonB2C(jsonFlag, @"rest/all/V1/orders/", "POST");

                            //UtilsWebservices.UpdateOrderFlag(currentIdOrder, "3");
                            continue;
                        }

                        //CustomerSearch client = UtilsWebservices.GetClientCtNum(orderSearch.Items[i].CustomerId.ToString(),"B2C");
                        // création des commande à partir du BtoC
                        if (orderSearch.Items[i].StoreId.ToString().Equals("4") || orderSearch.Items[i].StoreId.ToString().Equals("5") || orderSearch.Items[i].StoreId.ToString().Equals("6"))
                        {
                            AddNewOrderForCustomerESHOP(orderSearch.Items[i]);
                        }


                    }
                }
            }
            catch (Exception s)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(DateTime.Now + s.Message + Environment.NewLine);
                sb.Append(DateTime.Now + s.StackTrace + Environment.NewLine);
                //UtilsMail.SendErrorMail(DateTime.Now + s.Message + Environment.NewLine + s.StackTrace + Environment.NewLine, "COMMANDE");
                File.AppendAllText("Log\\order.txt", sb.ToString());
                sb.Clear();
                var jsonFlag = JsonConvert.SerializeObject(UpdateOrderFlag(currentIdOrder, currentIncrementedId, "2"));
                UtilsWebservices.SendDataJsonB2C(jsonFlag, @"rest/all/V1/orders/", "POST");
            }

        }

        public static void CheckForNewOrderMagentoB2C()
        {
            string currentIdOrder = "";
            string currentIncrementedId = "";
            try
            {
                string response = UtilsWebservices.SearchOrder(UtilsConfig.BaseUrlB2C + @"rest/V1/orders", UtilsWebservices.SearchOrderCriteria("order_flag", UtilsConfig.Flag.ToString(), "eq"),"B2C");
                //File.AppendAllText("Log\\commande.txt", response.ToString() + Environment.NewLine);
                OrderSearch orderSearch = OrderSearch.FromJson(response);
                if (orderSearch.TotalCount > 0)
                {
                    //todo create BC sage
                    for (int i = 0; i < orderSearch.TotalCount; i++)
                    {
                        string clientCtNum = "";
                        string clienttype = "";
                        currentIdOrder = orderSearch.Items[i].EntityId.ToString();
                        currentIncrementedId = orderSearch.Items[i].IncrementId.ToString();
                        if (orderSearch.Items[i].Status.Equals("canceled") || orderSearch.Items[i].State.Equals("canceled") || orderSearch.Items[i].Status.Equals("holded") || orderSearch.Items[i].State.Equals("holded") || orderSearch.Items[i].Status.Equals("pending") || orderSearch.Items[i].State.Equals("pending") )
                        {
                            if(orderSearch.Items[i].StoreId.ToString().Equals("4") || orderSearch.Items[i].StoreId.ToString().Equals("5") || orderSearch.Items[i].StoreId.ToString().Equals("6"))
                            {
                                var jsonFlag = JsonConvert.SerializeObject(UpdateOrderFlag(currentIdOrder, currentIncrementedId, "3"));
                                UtilsWebservices.SendDataJsonB2C(jsonFlag, @"rest/all/V1/orders/", "POST");

                                //UtilsWebservices.UpdateOrderFlag(currentIdOrder, "3");
                                continue;
                            }
                            
                        }

                        //CustomerSearch client = UtilsWebservices.GetClientCtNum(orderSearch.Items[i].CustomerId.ToString(),"B2C");
                        // création des commande à partir du BtoC
                        if (orderSearch.Items[i].StoreId.ToString().Equals("4") || orderSearch.Items[i].StoreId.ToString().Equals("5") || orderSearch.Items[i].StoreId.ToString().Equals("6"))
                        {
                            AddNewOrderForCustomerESHOP(orderSearch.Items[i]);
                        }


                    }
                }
            }
            catch (Exception s)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(DateTime.Now + s.Message + Environment.NewLine);
                sb.Append(DateTime.Now + s.StackTrace + Environment.NewLine);
                //UtilsMail.SendErrorMail(DateTime.Now + s.Message + Environment.NewLine + s.StackTrace + Environment.NewLine, "COMMANDE");
                File.AppendAllText("Log\\order.txt", sb.ToString());
                sb.Clear();
                var jsonFlag = JsonConvert.SerializeObject(UpdateOrderFlag(currentIdOrder, currentIncrementedId, "2"));
                UtilsWebservices.SendDataJsonB2C(jsonFlag, @"rest/all/V1/orders/", "POST");
            }

        }

        public static void CheckForNewOrderMagento()//object source, ElapsedEventArgs e)
        {
            File.AppendAllText("Log\\cron.txt", DateTime.Now + " start import order" + Environment.NewLine);
            CheckForNewOrderMagentoB2C();
            CheckForNewOrderMagentoB2CFlag2();
            CheckForNewOrderMagentoB2B();
            CheckForNewOrderMagentoB2BFlag2();
            File.AppendAllText("Log\\cron.txt", DateTime.Now + " end import order" + Environment.NewLine);
        }

        public static void UpdateStatuOrder(object source, ElapsedEventArgs e)
        {
            //File.AppendAllText("Log\\Process.txt", DateTime.Now + " start UpdateStatuOrder  " + Environment.NewLine);
            File.AppendAllText("Log\\cron.txt", DateTime.Now + " start UpdateStatuOrder" + Environment.NewLine);
            string test = "";
            try
            {
                var gescom = SingletonConnection.Instance.Gescom;
                var compta = SingletonConnection.Instance.Compta;
                
                //IBICollection AllOrders = gescom.FactoryDocumentVente.List;
                using (var db = new LiteDatabase(@"MyData.db"))
                {
                    // Get OrderMapping from Config

                    string MagentoStatutId, orderStatutId, statut1, statut2, statut3;
                    string[] MagentoID, orderStatut;
                    UtilsConfig.MagentoStatutId.TryGetValue("default", out MagentoStatutId);
                    MagentoID = MagentoStatutId.Split('_');
                    string statutMagento1, statutMagento2, statutMagento3;
                    UtilsConfig.MagentoStatutId.TryGetValue(MagentoID[0], out statutMagento1);
                    UtilsConfig.MagentoStatutId.TryGetValue(MagentoID[1], out statutMagento2);
                    UtilsConfig.MagentoStatutId.TryGetValue(MagentoID[2], out statutMagento3);
                    UtilsConfig.OrderMapping.TryGetValue("default", out orderStatutId);
                    orderStatut = orderStatutId.Split('_');
                    UtilsConfig.OrderMapping.TryGetValue(orderStatut[0], out statut1);
                    UtilsConfig.OrderMapping.TryGetValue(orderStatut[1], out statut2);
                    UtilsConfig.OrderMapping.TryGetValue(orderStatut[2], out statut3);
                    //statut1 =UtilsConfig.OrderMapping. //orderStatut[0];
                    //statut2 = orderStatut[1];
                    //statut3 = orderStatut[2];
                    //File.AppendAllText("Log\\statuShip.txt","Nbr total des document à parcourir :  "+ gescom.FactoryDocumentVente.List.Count.ToString() + Environment.NewLine);
                    // Get a collection (or create, if doesn't exist)
                    var col = db.GetCollection<LinkedCommandeDB>("Commande");
                    foreach (LinkedCommandeDB item in col.FindAll())
                    {
                        try
                        {
                            if (String.IsNullOrEmpty(item.Site))
                            {
                                item.Site = "B2B";
                            }
                        }
                        catch (Exception Exc)
                        {
                            item.Site = "B2B";
                        }
                        
                        DocumentType OrderDocumentType =DocumentType.DocumentTypeVenteCommande;
                        string sql = "SELECT DO_Type FROM [" + System.Configuration.ConfigurationManager.AppSettings["DBNAME"].ToString() + "].[dbo].[F_DOCENTETE] WHERE DO_Ref = '" + item.DO_Ref + "' ";// And DO_Souche ='1'"; ;
                        //File.AppendAllText("Log\\SQL.txt", DateTime.Now + sql.ToString() + Environment.NewLine);
                        SqlDataReader orderType = SingletonConnection.Instance.dB.Select(sql);
                        while (orderType.Read())
                        {
                           
                            //File.AppendAllText("Log\\SQL.txt", DateTime.Now + orderType.GetValue(0).ToString() + Environment.NewLine);
                            if (orderType.GetValue(0).ToString().Equals("2"))
                            {
                                OrderDocumentType = DocumentType.DocumentTypeVentePrepaLivraison;
                            }
                            else if (orderType.GetValue(0).ToString().Equals("3"))
                            {
                                OrderDocumentType = DocumentType.DocumentTypeVenteLivraison;
                            }
                            else if (orderType.GetValue(0).ToString().Equals("6"))
                            {
                                OrderDocumentType = DocumentType.DocumentTypeVenteFacture;
                            }
                            else if (orderType.GetValue(0).ToString().Equals("7"))
                            {
                                OrderDocumentType = DocumentType.DocumentTypeVenteFactureCpta;
                            }
                        }
                        test = item.OrderID;
                        orderType.Close();
                        if (OrderDocumentType.ToString().Equals("DocumentTypeVenteCommande"))
                        {
                            continue;
                        }else if (OrderDocumentType.ToString().Equals(item.OrderType.ToString()))
                        {
                            //File.AppendAllText("Log\\statut.txt", DateTime.Now + " la commande n'as pas changer de statut  " + item.DO_Ref + " " + item.OrderType + Environment.NewLine);
                            continue;
                        }
                        else
                        {
                            try
                            {
                                if (OrderDocumentType.ToString().Equals(DocumentType.DocumentTypeVenteLivraison.ToString()) && item.Shipped.Equals("1") && item.Site.Equals("B2B"))
                                {
                                    string response = UtilsWebservices.SearchOrder(UtilsConfig.BaseUrlB2B + @"rest/V1/orders", UtilsWebservices.SearchOrderCriteria("increment_id", item.incremented_id, "eq"));
                                    OrderSearch orderSearch = OrderSearch.FromJson(response);
                                    if (orderSearch.TotalCount > 0)
                                    {
                                        List<object> items = new List<object>();

                                        foreach (ParentItemElement OrderItem in orderSearch.Items[0].Items)
                                        {
                                            var ObjectItem = new
                                            {
                                                order_item_id = OrderItem.ItemId,
                                                qty = OrderItem.QtyOrdered
                                            };
                                            items.Add(ObjectItem);
                                        }
                                        var data = new
                                        {
                                            items = items,
                                            notify = true
                                        };

                                        var jsonData = JsonConvert.SerializeObject(data);
                                        string responseShipping = UtilsWebservices.SendDataJson(jsonData, @"rest/all/V1/order/" + orderSearch.Items[0].EntityId.ToString() + "/ship");
                                        item.Shipped = "0";
                                        col.Update(item);
                                        //File.AppendAllText("Log\\statuShip.txt", DateTime.Now + " Expédition id :  " + responseShipping.ToString() + " incremented id : "+ item.incremented_id +Environment.NewLine);
                                    }
                                }
                                if (OrderDocumentType.ToString().Equals(DocumentType.DocumentTypeVenteLivraison.ToString()) && item.Shipped.Equals("1") && item.Site.Equals("B2C"))
                                {
                                    string response = UtilsWebservices.SearchOrder(UtilsConfig.BaseUrlB2C + @"rest/V1/orders", UtilsWebservices.SearchOrderCriteria("increment_id", item.incremented_id, "eq"),"B2C");
                                    OrderSearch orderSearch = OrderSearch.FromJson(response);
                                    if (orderSearch.TotalCount > 0)
                                    {
                                        List<object> items = new List<object>();

                                        foreach (ParentItemElement OrderItem in orderSearch.Items[0].Items)
                                        {
                                            var ObjectItem = new
                                            {
                                                order_item_id = OrderItem.ItemId,
                                                qty = OrderItem.QtyOrdered
                                            };
                                            items.Add(ObjectItem);
                                        }
                                        var data = new
                                        {
                                            items = items,
                                            notify = true
                                        };

                                        var jsonData = JsonConvert.SerializeObject(data);
                                        string responseShipping = UtilsWebservices.SendDataJsonB2C(jsonData, @"/rest/all/V1/order/" + orderSearch.Items[0].EntityId.ToString() + "/ship");
                                        item.Shipped = "0";
                                        col.Update(item);
                                        //File.AppendAllText("Log\\statuShip.txt", DateTime.Now + " Expédition id :  " + responseShipping.ToString() + " incremented id : "+ item.incremented_id +Environment.NewLine);
                                    }
                                }
                                
                                if (OrderDocumentType.ToString().Equals(statut1.Split('_')[0]))
                                {
                                    if (item.Site.Equals("B2C"))
                                    {
                                        continue;
                                    }
                                    UtilsWebservices.SendDataJson(UpdateStatusOnMagento(item.OrderID, item.incremented_id, statutMagento1), @"rest/V1/orders");
                                    item.OrderType = statut1.Split('_')[0];
                                    col.Update(item);
                                    
                                    //col.Update()
                                    //File.AppendAllText("Log\\statut.txt", DateTime.Now + " stat1  " + item.DO_Ref + " "+ item.OrderType + Environment.NewLine);
                                    continue;
                                }
                                if (OrderDocumentType.ToString().Equals(statut2.Split('_')[0]))
                                {
                                    if (item.Site.Equals("B2C"))
                                    {
                                        UtilsWebservices.SendDataJsonB2C(UpdateStatusOnMagento(item.OrderID, item.incremented_id, statutMagento2), @"rest/V1/orders");
                                    }
                                    else
                                    {
                                        UtilsWebservices.SendDataJson(UpdateStatusOnMagento(item.OrderID, item.incremented_id, statutMagento2), @"rest/V1/orders");
                                    }
                                    
                                    item.OrderType = statut2.Split('_')[0];
                                    col.Update(item);
                                    
                                    //File.AppendAllText("Log\\statut.txt", DateTime.Now + " stat2  " + item.DO_Ref + " "+ item.OrderType + Environment.NewLine);
                                    //col.Update()
                                    continue;
                                }
                                if (OrderDocumentType.ToString().Equals(statut3.Split('_')[0]))
                                {
                                    if (item.Site.Equals("B2C"))
                                    {
                                        UtilsWebservices.SendDataJsonB2C(UpdateStatusOnMagento(item.OrderID, item.incremented_id, statutMagento3), @"rest/V1/orders");
                                    }
                                    else
                                    {
                                        UtilsWebservices.SendDataJson(UpdateStatusOnMagento(item.OrderID, item.incremented_id, statutMagento3), @"rest/V1/orders");
                                    }
                                    
                                    col.Delete(item.Id);

                                    //File.AppendAllText("Log\\statut.txt", DateTime.Now + " stat3  " + item.DO_Ref +" " + item.OrderType + Environment.NewLine);
                                    continue;
                                }
                            }
                            catch (Exception s)
                            {
                                StringBuilder sb = new StringBuilder();
                                sb.Append(DateTime.Now + s.Message +" Erreur incremented_id : " +item.incremented_id + Environment.NewLine);
                                sb.Append(DateTime.Now + s.StackTrace + Environment.NewLine);
                                File.AppendAllText("Log\\statutError.txt", sb.ToString());
                                sb.Clear();
                                //UtilsMail.SendErrorMail(DateTime.Now + s.Message + Environment.NewLine + s.StackTrace + Environment.NewLine, "UPDATE STATUT ORDER " + test);

                            }
                        }
                    }

                }
            }
            catch(Exception s)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(DateTime.Now + s.Message + Environment.NewLine);
                sb.Append(DateTime.Now + s.StackTrace + Environment.NewLine);
                File.AppendAllText("Log\\statutError.txt", sb.ToString());
                sb.Clear();
                UtilsMail.SendErrorMail(DateTime.Now + s.Message + Environment.NewLine + s.StackTrace + Environment.NewLine, "UPDATE STATUT ORDER "+ test);
                
            }
            //File.AppendAllText("Log\\Process.txt", DateTime.Now + " end UpdateStatuOrder  " + Environment.NewLine);
            File.AppendAllText("Log\\cron.txt", DateTime.Now + " end UpdateStatuOrder" + Environment.NewLine);
        }

        public static void UpdateStatuOrder()//object source, ElapsedEventArgs e)
        {
            File.AppendAllText("Log\\cron.txt", DateTime.Now + " start UpdateStatu" + Environment.NewLine);
            //File.AppendAllText("Log\\Process.txt", DateTime.Now + " start UpdateStatuOrder  " + Environment.NewLine);
            string test = "";
            try
            {
                var gescom = SingletonConnection.Instance.Gescom;
                var compta = SingletonConnection.Instance.Compta;

                //IBICollection AllOrders = gescom.FactoryDocumentVente.List;
                using (var db = new LiteDatabase(@"MyData.db"))
                {
                    // Get OrderMapping from Config

                    string MagentoStatutId, orderStatutId, statut1, statut2, statut3;
                    string[] MagentoID, orderStatut;
                    UtilsConfig.MagentoStatutId.TryGetValue("default", out MagentoStatutId);
                    MagentoID = MagentoStatutId.Split('_');
                    string statutMagento1, statutMagento2, statutMagento3;
                    UtilsConfig.MagentoStatutId.TryGetValue(MagentoID[0], out statutMagento1);
                    UtilsConfig.MagentoStatutId.TryGetValue(MagentoID[1], out statutMagento2);
                    UtilsConfig.MagentoStatutId.TryGetValue(MagentoID[2], out statutMagento3);
                    UtilsConfig.OrderMapping.TryGetValue("default", out orderStatutId);
                    orderStatut = orderStatutId.Split('_');
                    UtilsConfig.OrderMapping.TryGetValue(orderStatut[0], out statut1);
                    UtilsConfig.OrderMapping.TryGetValue(orderStatut[1], out statut2);
                    UtilsConfig.OrderMapping.TryGetValue(orderStatut[2], out statut3);
                    //statut1 =UtilsConfig.OrderMapping. //orderStatut[0];
                    //statut2 = orderStatut[1];
                    //statut3 = orderStatut[2];
                    //File.AppendAllText("Log\\statuShip.txt","Nbr total des document à parcourir :  "+ gescom.FactoryDocumentVente.List.Count.ToString() + Environment.NewLine);
                    // Get a collection (or create, if doesn't exist)
                    var col = db.GetCollection<LinkedCommandeDB>("Commande");
                    foreach (LinkedCommandeDB item in col.FindAll())
                    {
                        try
                        {
                            if (String.IsNullOrEmpty(item.Site))
                            {
                                item.Site = "B2B";
                            }
                        }
                        catch (Exception Exc)
                        {
                            item.Site = "B2B";
                        }

                        DocumentType OrderDocumentType = DocumentType.DocumentTypeVenteCommande;
                        string sql = "SELECT DO_Type FROM [" + System.Configuration.ConfigurationManager.AppSettings["DBNAME"].ToString() + "].[dbo].[F_DOCENTETE] WHERE DO_Ref = '" + item.DO_Ref + "' ";// And DO_Souche ='1'"; ;
                        //File.AppendAllText("Log\\SQL.txt", DateTime.Now + sql.ToString() + Environment.NewLine);
                        SqlDataReader orderType = SingletonConnection.Instance.dB.Select(sql);
                        while (orderType.Read())
                        {

                            //File.AppendAllText("Log\\SQL.txt", DateTime.Now + orderType.GetValue(0).ToString() + Environment.NewLine);
                            if (orderType.GetValue(0).ToString().Equals("2"))
                            {
                                OrderDocumentType = DocumentType.DocumentTypeVentePrepaLivraison;
                            }
                            else if (orderType.GetValue(0).ToString().Equals("3"))
                            {
                                OrderDocumentType = DocumentType.DocumentTypeVenteLivraison;
                            }
                            else if (orderType.GetValue(0).ToString().Equals("6"))
                            {
                                OrderDocumentType = DocumentType.DocumentTypeVenteFacture;
                            }
                            else if (orderType.GetValue(0).ToString().Equals("7"))
                            {
                                OrderDocumentType = DocumentType.DocumentTypeVenteFactureCpta;
                            }
                        }
                        test = item.OrderID;
                        orderType.Close();
                        if (OrderDocumentType.ToString().Equals("DocumentTypeVenteCommande"))
                        {
                            continue;
                        }
                        else if (OrderDocumentType.ToString().Equals(item.OrderType.ToString()))
                        {
                            //File.AppendAllText("Log\\statut.txt", DateTime.Now + " la commande n'as pas changer de statut  " + item.DO_Ref + " " + item.OrderType + Environment.NewLine);
                            continue;
                        }
                        else
                        {
                            try
                            {
                                if (OrderDocumentType.ToString().Equals(DocumentType.DocumentTypeVenteLivraison.ToString()) && item.Shipped.Equals("1") && item.Site.Equals("B2B"))
                                {
                                    string response = UtilsWebservices.SearchOrder(UtilsConfig.BaseUrlB2B + @"rest/V1/orders", UtilsWebservices.SearchOrderCriteria("increment_id", item.incremented_id, "eq"));
                                    OrderSearch orderSearch = OrderSearch.FromJson(response);
                                    if (orderSearch.TotalCount > 0)
                                    {
                                        List<object> items = new List<object>();

                                        foreach (ParentItemElement OrderItem in orderSearch.Items[0].Items)
                                        {
                                            var ObjectItem = new
                                            {
                                                order_item_id = OrderItem.ItemId,
                                                qty = OrderItem.QtyOrdered
                                            };
                                            items.Add(ObjectItem);
                                        }
                                        var data = new
                                        {
                                            items = items,
                                            notify = true
                                        };

                                        var jsonData = JsonConvert.SerializeObject(data);
                                        string responseShipping = UtilsWebservices.SendDataJson(jsonData, @"rest/all/V1/order/" + orderSearch.Items[0].EntityId.ToString() + "/ship");
                                        item.Shipped = "0";
                                        col.Update(item);
                                        //File.AppendAllText("Log\\statuShip.txt", DateTime.Now + " Expédition id :  " + responseShipping.ToString() + " incremented id : "+ item.incremented_id +Environment.NewLine);
                                    }
                                }
                                if (OrderDocumentType.ToString().Equals(DocumentType.DocumentTypeVenteLivraison.ToString()) && item.Shipped.Equals("1") && item.Site.Equals("B2C"))
                                {
                                    string response = UtilsWebservices.SearchOrder(UtilsConfig.BaseUrlB2C + @"rest/V1/orders", UtilsWebservices.SearchOrderCriteria("increment_id", item.incremented_id, "eq"), "B2C");
                                    OrderSearch orderSearch = OrderSearch.FromJson(response);
                                    if (orderSearch.TotalCount > 0)
                                    {
                                        List<object> items = new List<object>();

                                        foreach (ParentItemElement OrderItem in orderSearch.Items[0].Items)
                                        {
                                            var ObjectItem = new
                                            {
                                                order_item_id = OrderItem.ItemId,
                                                qty = OrderItem.QtyOrdered
                                            };
                                            items.Add(ObjectItem);
                                        }
                                        var data = new
                                        {
                                            items = items,
                                            notify = true
                                        };

                                        var jsonData = JsonConvert.SerializeObject(data);
                                        string responseShipping = UtilsWebservices.SendDataJsonB2C(jsonData, @"/rest/all/V1/order/" + orderSearch.Items[0].EntityId.ToString() + "/ship");
                                        item.Shipped = "0";
                                        col.Update(item);
                                        //File.AppendAllText("Log\\statuShip.txt", DateTime.Now + " Expédition id :  " + responseShipping.ToString() + " incremented id : "+ item.incremented_id +Environment.NewLine);
                                    }
                                }

                                if (OrderDocumentType.ToString().Equals(statut1.Split('_')[0]))
                                {
                                    if (item.Site.Equals("B2C"))
                                    {
                                        continue;
                                    }
                                    UtilsWebservices.SendDataJson(UpdateStatusOnMagento(item.OrderID, item.incremented_id, statutMagento1), @"rest/V1/orders");
                                    item.OrderType = statut1.Split('_')[0];
                                    col.Update(item);

                                    //col.Update()
                                    //File.AppendAllText("Log\\statut.txt", DateTime.Now + " stat1  " + item.DO_Ref + " "+ item.OrderType + Environment.NewLine);
                                    continue;
                                }
                                if (OrderDocumentType.ToString().Equals(statut2.Split('_')[0]))
                                {
                                    if (item.Site.Equals("B2C"))
                                    {
                                        UtilsWebservices.SendDataJsonB2C(UpdateStatusOnMagento(item.OrderID, item.incremented_id, statutMagento2), @"rest/V1/orders");
                                    }
                                    else
                                    {
                                        UtilsWebservices.SendDataJson(UpdateStatusOnMagento(item.OrderID, item.incremented_id, statutMagento2), @"rest/V1/orders");
                                    }

                                    item.OrderType = statut2.Split('_')[0];
                                    col.Update(item);

                                    //File.AppendAllText("Log\\statut.txt", DateTime.Now + " stat2  " + item.DO_Ref + " "+ item.OrderType + Environment.NewLine);
                                    //col.Update()
                                    continue;
                                }
                                if (OrderDocumentType.ToString().Equals(statut3.Split('_')[0]))
                                {
                                    if (item.Site.Equals("B2C"))
                                    {
                                        UtilsWebservices.SendDataJsonB2C(UpdateStatusOnMagento(item.OrderID, item.incremented_id, statutMagento3), @"rest/V1/orders");
                                    }
                                    else
                                    {
                                        UtilsWebservices.SendDataJson(UpdateStatusOnMagento(item.OrderID, item.incremented_id, statutMagento3), @"rest/V1/orders");
                                    }

                                    col.Delete(item.Id);

                                    //File.AppendAllText("Log\\statut.txt", DateTime.Now + " stat3  " + item.DO_Ref +" " + item.OrderType + Environment.NewLine);
                                    continue;
                                }
                            }
                            catch (Exception s)
                            {
                                StringBuilder sb = new StringBuilder();
                                sb.Append(DateTime.Now + s.Message + " Erreur incremented_id : " + item.incremented_id + Environment.NewLine);
                                sb.Append(DateTime.Now + s.StackTrace + Environment.NewLine);
                                File.AppendAllText("Log\\statutError.txt", sb.ToString());
                                sb.Clear();
                                //UtilsMail.SendErrorMail(DateTime.Now + s.Message + Environment.NewLine + s.StackTrace + Environment.NewLine, "UPDATE STATUT ORDER " + test);

                            }
                        }
                    }

                }
            }
            catch (Exception s)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(DateTime.Now + s.Message + Environment.NewLine);
                sb.Append(DateTime.Now + s.StackTrace + Environment.NewLine);
                File.AppendAllText("Log\\statutError.txt", sb.ToString());
                sb.Clear();
                UtilsMail.SendErrorMail(DateTime.Now + s.Message + Environment.NewLine + s.StackTrace + Environment.NewLine, "UPDATE STATUT ORDER " + test);

            }
            //File.AppendAllText("Log\\Process.txt", DateTime.Now + " end UpdateStatuOrder  " + Environment.NewLine);
            File.AppendAllText("Log\\cron.txt", DateTime.Now + " end UpdateStatu" + Environment.NewLine);
        }


        /// <summary>
        /// Crée une nouvelle commande pour un utilisateur
        /// </summary>
        /// <param name="jsonOrder">Order à crée</param>
        /// <param name="CT_Num">Client</param>
        public static void AddNewOrderForCustomer(OrderSearchItem orderMagento, string CT_Num,CustomerSearch customerMagento)
        {
            var gescom = SingletonConnection.Instance.Gescom;

            // création de l'entête de la commande 

            IBOClient3 customer = gescom.CptaApplication.FactoryClient.ReadNumero(CT_Num);
            IBODocumentVente3 order = gescom.FactoryDocumentVente.CreateType(DocumentType.DocumentTypeVenteCommande);
            order.SetDefault();
            order.SetDefaultClient(customer);
            order.DO_Date = DateTime.Now;

            order.Souche = gescom.FactorySoucheVente.ReadIntitule(UtilsConfig.Souche);
            order.DO_Ref = "WEB " + orderMagento.IncrementId.ToString();// orderMagento.Payment.Po_number.ToString(); //
            order.SetDefaultDO_Piece();
            order.Write();
            //à modifier pour récupérer l'adresse en se basant sur le champs Li_No dans la table F_LIVRAISON
            #region Update/Create shipping Address
            /*
            bool asAdressMatch = false;
            IBOClientLivraison3 currentAdress = null;
            foreach (IBOClientLivraison3 tmpAdress in customer.FactoryClientLivraison.List)
            {
                if (!String.IsNullOrEmpty(orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.company))
                {
                    if (tmpAdress.LI_Intitule.Equals(orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.company.ToUpper()))
                    {
                        currentAdress = tmpAdress;
                        asAdressMatch = true;
                        break;
                    }
                }
                string intitule = "";
                intitule = orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.Firstname + " " + orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.Lastname;
                if (intitule.Length > 35)
                {
                    intitule.Substring(0, 35);
                }
                if (tmpAdress.LI_Intitule.Equals(intitule.ToUpper()))
                {
                    currentAdress = tmpAdress;
                    asAdressMatch = true;
                    break;
                }
                
            }


            // si on a trouver aucune adresse coresspondante sur le client alors on la crée
            IBOClientLivraison3 adress;
            if (!asAdressMatch)
            {
                adress = (IBOClientLivraison3)customer.FactoryClientLivraison.Create();
                adress.SetDefault();
            }
            else
            {
                adress = currentAdress;
            }

            adress.Telecom.EMail = customer.Telecom.EMail;
            /*
            try
            {
                string carrier_id = jsonOrder["order_carriere"].ToString();
                adress.Expedition = gescom.FactoryExpedition.ReadIntitule(UtilsConfig.OrderCarrierMapping[carrier_id]);
            }
            catch (Exception s)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(DateTime.Now + s.Message + Environment.NewLine);
                sb.Append(DateTime.Now + s.StackTrace + Environment.NewLine);
                UtilsMail.SendErrorMail(DateTime.Now + s.Message + Environment.NewLine + s.StackTrace + Environment.NewLine, "TRANSPORTEUR");
                File.AppendAllText("Log\\order.txt", sb.ToString());
                sb.Clear();
            }
            *//*
            if (!String.IsNullOrEmpty(orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.company))
            {
                adress.LI_Intitule = orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.company.ToUpper();
            }
            else
            {
                string intitule = "";
                intitule = orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.Firstname + " " + orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.Lastname;
                if (intitule.Length > 35)
                {
                    intitule.Substring(0, 35);
                }
                adress.LI_Intitule = intitule.ToUpper();
            }

            // Setup champ contact dans adress
            if ((orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.Firstname + " " + orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.Lastname).Length > 35)
            {
                adress.LI_Contact = (orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.Firstname + " " + orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.Lastname).Substring(0, 35);
            }
            else
            {
                adress.LI_Contact = orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.Firstname + " " + orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.Lastname;
            }

            if (orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.Street[0].ToString().Length > 35)
            {
                adress.Adresse.Adresse = orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.Street[0].ToString().Substring(0, 35);
            }
            else
            {
                adress.Adresse.Adresse = orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.Street[0].ToString();
            }
            if (orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.Street.Count >1)
            {
                if (orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.Street[1].ToString().Length >35)
                {
                    adress.Adresse.Complement = orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.Street[1].ToString().Substring(0, 35);
                }
                else
                {
                    adress.Adresse.Complement = orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.Street[1].ToString();
                }
                
            }
            
            adress.Adresse.CodePostal = orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.Postcode.ToString();
            adress.Adresse.Ville = orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.City.ToString();
            var region = CultureInfo
                                    .GetCultures(CultureTypes.SpecificCultures)
                                    .Select(ci => new RegionInfo(ci.LCID))
                                    .FirstOrDefault(rg => rg.TwoLetterISORegionName == orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.CountryId);
            adress.Adresse.Pays = region.DisplayName.ToString();
            adress.Telecom.Telephone = orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.Telephone;
            //adress.Telecom.Telecopie = jsonOrder["shipping_phone_mobile"].ToString();

            if (String.IsNullOrEmpty(UtilsConfig.CondLivraison))
            {
                // pas de configuration renseigner pour CondLivraison par defaut
                // todo log
            }
            else
            {
                adress.ConditionLivraison = gescom.FactoryConditionLivraison.ReadIntitule(UtilsConfig.CondLivraison);
            }
            if (String.IsNullOrEmpty(UtilsConfig.Expedition))
            {
                // pas de configuration renseigner pour Expedition par defaut
                // todo log
            }
            else
            {
                adress.Expedition = gescom.FactoryExpedition.ReadIntitule(UtilsConfig.Expedition);
            }
            adress.Write();*/
            #endregion
            // on ajoute une adresse par defaut sur la fiche client si il y en a pas

            // désactiver la mise à jour de l'adresse de facturation ==> demande client
            // On met à jour l'adresse de facturation du client
            #region Update invoice Adress
            /*
            Object.CustomerSearch.Address defaultAddress = new Object.CustomerSearch.Address();
            foreach (Object.CustomerSearch.Address addressCustomer in customerMagento.Addresses)
            {
                if (addressCustomer.Id == customerMagento.DefaultBilling)
                {
                    defaultAddress = addressCustomer;
                    break;
                }
            }
            if (defaultAddress.Street[0].Length > 35)
            {
                customer.Adresse.Adresse = defaultAddress.Street[0].ToString().Substring(0, 35);
            }
            else
            {
                customer.Adresse.Adresse = defaultAddress.Street[0].ToString();
            }
            if (defaultAddress.Street.Count > 1)
            {
                customer.Adresse.Complement = defaultAddress.Street[1].ToString();
            }

            customer.Adresse.CodePostal = defaultAddress.Postcode.ToString();
            customer.Adresse.Ville = defaultAddress.City.ToString();
            var region1 = CultureInfo
                            .GetCultures(CultureTypes.SpecificCultures)
                            .Select(ci => new RegionInfo(ci.LCID))
                            .FirstOrDefault(rg => rg.TwoLetterISORegionName == defaultAddress.CountryId);
            customer.Adresse.Pays = region1.DisplayName.ToString();
            customer.Telecom.Telephone = defaultAddress.Telephone.ToString();
            customer.Write();
            */
            #endregion

            order.FraisExpedition = orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Total.ShippingAmount;
            //order.LieuLivraison = adress;


            order.Write();

            string intitulAdressLivraison = "";
            intitulAdressLivraison = GetIntitulAdressBtoB(orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.Street[0], orderMagento.CustomerEmail,CT_Num);

            
            foreach (IBOClientLivraison3 item in customer.FactoryClientLivraison.List)
            {
                File.AppendAllText("Log\\test.txt", "client list of adress " + item.LI_Intitule + Environment.NewLine);
                if (item.LI_Intitule.Equals(intitulAdressLivraison))
                {
                    order.LieuLivraison = item;
                    order.Write();
                    File.AppendAllText("Log\\test.txt", "found " + intitulAdressLivraison + Environment.NewLine);
                    break;
                }
            }


            #region infolibre commande
            try
            {
                //take care of infolibre commande
                if (!String.IsNullOrEmpty(orderMagento.Payment.Po_number.ToString()))
                {
                    order.InfoLibre[16] = orderMagento.Payment.Po_number.ToString();
                    order.Write();
                }
            }
            catch (Exception exception)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(DateTime.Now + exception.Message + Environment.NewLine);
                sb.Append(DateTime.Now + exception.StackTrace + Environment.NewLine);
                File.AppendAllText("Log\\order.txt", sb.ToString());
                sb.Clear();
            }

            #endregion

            // création des lignes de la commandes
            try
            {
                foreach (Object.Order.ParentItemElement product in orderMagento.Items)
                {
                    if (product.ProductType.Equals("configurable"))
                    {
                        continue;
                    }
                    IBODocumentVenteLigne3 docLigne = (IBODocumentVenteLigne3)order.FactoryDocumentLigne.Create();
                    var ArticleExist = gescom.FactoryArticle.ExistReference(product.Sku);
                    if (ArticleExist)
                    {
                        //= double.Parse(product.RowTotal.ToString(), System.Globalization.CultureInfo.InvariantCulture);

                        // produit simple
                        docLigne.SetDefaultArticle(gescom.FactoryArticle.ReadReference(product.Sku), Int32.Parse(product.QtyOrdered.ToString()));
                        //docLigne.DL_PrixUnitaire = product.Price;
                        docLigne.SetDefaultRemise();
                        //docLigne.DL_PrixUnitaire = product.Price;//.ToString(), System.Globalization.CultureInfo.InvariantCulture);
                        //docLigne.SetDefaultArticleReferenceClient(customer.CT_Num, Int32.Parse(product.QtyOrdered.ToString()));
                        //SHipping price

                        /*if (product["product_ref"].ToString().Equals("TRANSPORT"))
                        {
                            docLigne.DL_PrixUnitaire = Convert.ToDouble(orderMagento.ShippingAmount.ToString().Replace('.', ','));
                        }
                        else if (product["product_ref"].ToString().Equals("REMISE"))
                        {
                            docLigne.DL_PrixUnitaire = Convert.ToDouble(product.Price.ToString().Replace('.', ','));
                        }*/
                    }
                    else
                    {
                        // on récupère la chaine de gammages d'un produit
                        string product_attribut_string = GetParentProductDetails(product.Sku).ToString();
                        String[] subgamme = product_attribut_string.Split('|');
                        IBOArticle3 article = gescom.FactoryArticle.ReadReference(subgamme[0].ToString());
                        if (subgamme.Length == 3)
                        {
                            // produit à simple gamme
                            IBOArticleGammeEnum3 articleEnum = ControllerArticle.GetArticleGammeEnum1(article, new Gamme(subgamme[1], subgamme[2]));
                            docLigne.SetDefaultArticleMonoGamme(articleEnum, Int32.Parse(product.QtyOrdered.ToString()));
                            //docLigne.DL_PrixUnitaire = product.Price;
                            docLigne.SetDefaultRemise();
                        }
                        else if (subgamme.Length == 5)
                        {
                            // produit à double gamme
                            IBOArticleGammeEnum3 articleEnum = ControllerArticle.GetArticleGammeEnum1(article, new Gamme(subgamme[1], subgamme[2], subgamme[3], subgamme[4]));
                            IBOArticleGammeEnum3 articleEnum2 = ControllerArticle.GetArticleGammeEnum2(article, new Gamme(subgamme[1], subgamme[2], subgamme[3], subgamme[4]));
                            docLigne.SetDefaultArticleDoubleGamme(articleEnum, articleEnum2, Int32.Parse(product.QtyOrdered.ToString()));
                            //docLigne.DL_PrixUnitaire = product.Price;
                            docLigne.SetDefaultRemise();
                        }
                    }
                    docLigne.Write();
                }
                /*IBODocumentLigne3 docLigne1 = (IBODocumentLigne3)order.FactoryDocumentLigne.Create();
                docLigne1.SetDefaultArticle(gescom.FactoryArticle.ReadReference(UtilsConfig.DefaultTransportReference), 1);
                docLigne1.DL_PrixUnitaire = Convert.ToDouble(orderMagento.ShippingAmount.ToString().Replace('.', ','));
                docLigne1.Write();*/
            }
            catch (Exception e)
            {
                //UtilsWebservices.UpdateOrderFlag(orderMagento.EntityId.ToString(), "2");
                var jsonFlag2 = JsonConvert.SerializeObject(UpdateOrderFlag(orderMagento.EntityId.ToString(), orderMagento.IncrementId.ToString(), "2"));
                UtilsWebservices.SendDataJson(jsonFlag2, @"rest/all/V1/orders", "POST");
                StringBuilder sb = new StringBuilder();
                sb.Append(DateTime.Now + e.Message + Environment.NewLine);
                sb.Append(DateTime.Now + e.StackTrace + Environment.NewLine);
                UtilsMail.SendErrorMail(DateTime.Now + e.Message + Environment.NewLine + e.StackTrace + Environment.NewLine, "COMMANDE LIGNE");
                File.AppendAllText("Log\\order.txt", sb.ToString());
                sb.Clear();
                // UtilsWebservices.SendDataNoParse(UtilsConfig.BaseUrl + EnumEndPoint.Commande.Value, "errorOrder&orderID=" + jsonOrder["id_order"]);
                order.Remove();
                return;
            }
            order.FraisExpedition = orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Total.ShippingAmount;
            order.Write();
            addOrderToLocalDB(orderMagento.EntityId.ToString(), order.Client.CT_Num, order.DO_Piece, order.DO_Ref, orderMagento.IncrementId);
            // TODO updateOrderFlag using custom PHP script
            
            var jsonFlag = JsonConvert.SerializeObject(UpdateOrderFlag(orderMagento.EntityId.ToString(), orderMagento.IncrementId.ToString(), "0"));
            UtilsWebservices.SendDataJson(jsonFlag, @"rest/all/V1/orders", "POST");
            //UtilsWebservices.UpdateOrderFlag(orderMagento.EntityId.ToString(), "0");
            /*if (UtilsWebservices.UpdateOrderFlag(orderMagento.EntityId.ToString(),"0").Equals("error"))
            {
                UtilsMail.SendErrorMail(DateTime.Now + "Commande non importé : "+ orderMagento.EntityId.ToString() + Environment.NewLine , "COMMANDE LIGNE");
            }*/
            // on notfie prestashop que la commande à bien été crée dans SAGE
            //UtilsWebservices.SendDataNoParse(UtilsConfig.BaseUrl + EnumEndPoint.Commande.Value, "validateOrder&orderID=" + jsonOrder["id_order"]);
        }


        /// <summary>
        /// Crée une nouvelle commande pour un utilisateur
        /// </summary>
        /// <param name="jsonOrder">Order à crée</param>
        /// <param name="CT_Num">Client</param>
        public static void AddNewOrderForCustomerESHOP(OrderSearchItem orderMagento)
        {
            var gescom = SingletonConnection.Instance.Gescom;

            // création de l'entête de la commande 
            try
            {
                IBOClient3 customer = gescom.CptaApplication.FactoryClient.ReadNumero("1ESHOP");
                IBODocumentVente3 order = gescom.FactoryDocumentVente.CreateType(DocumentType.DocumentTypeVenteCommande);
                int BillingLI_NO = 0;
                int ShippingLi_No = 0;
                string intitulAdressLivraison = "";
                string intitulAdressFacturation = "";
                order.SetDefault();
                order.SetDefaultClient(customer);
                order.DO_Date = DateTime.Now;

                order.Souche = gescom.FactorySoucheVente.ReadIntitule(UtilsConfig.SoucheBtoC);
                order.DO_Ref = "WEB " + orderMagento.IncrementId.ToString();
                order.SetDefaultDO_Piece();

                order.FraisExpedition = orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Total.ShippingAmount;

                if (ExistAdress(orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.Street[0], orderMagento.CustomerEmail))
                {
                    intitulAdressLivraison = GetIntitulAdress(orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.Street[0], orderMagento.CustomerEmail);
                }
                if (ExistAdress(orderMagento.BillingAddress.Street[0], orderMagento.CustomerEmail))
                {
                    intitulAdressFacturation = GetIntitulAdress(orderMagento.BillingAddress.Street[0], orderMagento.CustomerEmail);
                }
                if (String.IsNullOrEmpty(intitulAdressFacturation) && String.IsNullOrEmpty(intitulAdressLivraison))
                {
                    if (orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.Street[0].Equals(orderMagento.BillingAddress.Street[0]))
                    {
                        File.AppendAllText("Log\\test.txt", DateTime.Now + " Création addresse livraison/facturation : "  + Environment.NewLine);
                        createBtoCAdress(orderMagento, customer, "Livraison/Facturation");
                        intitulAdressFacturation = GetIntitulAdress(orderMagento.BillingAddress.Street[0], orderMagento.CustomerEmail);
                        intitulAdressLivraison = GetIntitulAdress(orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.Street[0], orderMagento.CustomerEmail);
                        File.AppendAllText("Log\\test.txt", DateTime.Now + " Création addresse livraison/facturation : " + intitulAdressLivraison + Environment.NewLine);
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(intitulAdressFacturation))
                        {
                            createBtoCBillingAdress(orderMagento, customer, "Facturation");
                            File.AppendAllText("Log\\test.txt", DateTime.Now + " Création addresse facturation : "  + Environment.NewLine);
                        }
                        if (String.IsNullOrEmpty(intitulAdressLivraison))
                        {
                            createBtoCShippingAdress(orderMagento, customer, "Livraison");
                            File.AppendAllText("Log\\test.txt", DateTime.Now + " Création addresse livraison : " + Environment.NewLine);
                        }
                    }
                }
                #region test LI_NO
                /*
                if (orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.Street[0].Equals(orderMagento.BillingAddress.Street[0]))
                {
                    if (String.IsNullOrEmpty(intitulAdressFacturation) && String.IsNullOrEmpty(intitulAdressLivraison))
                    {
                        createBtoCAdress(orderMagento, customerMagento, customer, "Livraison/Facturation");
                        intitulAdressFacturation = GetIntitulAdress(orderMagento.BillingAddress.Street[0], customerMagento.Email);
                        intitulAdressLivraison = GetIntitulAdress(orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.Street[0], customerMagento.Email);
                    }
                    else
                    {

                    }

                }

                if (HasMethod(orderMagento.BillingAddress, "CustomerAddressId"))
                {
                    foreach (Object.CustomerSearch.Address addressClient in customerMagento.Addresses)
                    {
                        if (HasMethod(addressClient, "CustomAttributes"))
                        {
                            if (orderMagento.BillingAddress.CustomerAddressId == addressClient.Id)
                            {
                                BillingLI_NO = Int32.Parse(addressClient.CustomAttributes[0].Value);
                            }
                            if (HasMethod(orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Address, "CustomerAddressId"))
                            {
                                if (orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.CustomerAddressId == addressClient.Id)
                                {
                                    ShippingLi_No = Int32.Parse(addressClient.CustomAttributes[0].Value);
                                }
                            }
                        }
                    }
                }




                if (orderMagento.BillingAddress.Street[0].ToUpper().Equals(orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.Street[0].ToUpper()))
                {
                    //Création adresse Facturation/livraison
                    if (BillingLI_NO == 0 && ShippingLi_No == 0)
                    {
                        Object.CustomerSearch.Address address = createBtoCAdress(orderMagento, customerMagento, customer, "Livraison/Facturation");
                        Customer customerupdate = new Customer();

                        var jsonClient = JsonConvert.SerializeObject(customerupdate.UpdateBtoCCustomer(customerMagento, address));
                        UtilsWebservices.SendDataJson(jsonClient, @"rest/all/V1/customers/" + customerMagento.Id.ToString(), "PUT");
                    }
                    else
                    {

                    }

                }
                else
                {

                }

                //gestion des adresse de livraison et facturation
                if (BillingLI_NO == 0)
                {
                    Object.CustomerSearch.Address address = createBtoCBillingAdress(orderMagento, customerMagento, customer, "Facturation");
                    Customer customerupdate = new Customer();

                    var jsonClient = JsonConvert.SerializeObject(customerupdate.UpdateBtoCCustomer(customerMagento, address));
                    UtilsWebservices.SendDataJson(jsonClient, @"rest/all/V1/customers/" + customerMagento.Id.ToString(), "PUT");
                }
                else
                {
                    //récupération à partir de la DB l'adresse avec le num Li_No et le mettre à jour


                }
                if (ShippingLi_No == 0)
                {
                    //Création d'une nouvelle adresse de livraison
                    Object.CustomerSearch.Address address = createBtoCShippingAdress(orderMagento, customerMagento, customer, "Livraison");
                    Customer customerupdate = new Customer();

                    var jsonClient = JsonConvert.SerializeObject(customerupdate.UpdateBtoCCustomer(customerMagento, address));
                    UtilsWebservices.SendDataJson(jsonClient, @"rest/all/V1/customers/" + customerMagento.Id.ToString(), "PUT");
                }
                else
                {
                    //récupération à partir de la DB l'adresse avec le num Li_No et le mettre à jour

                }
                */
                #endregion
                //affectation de l'adress de livraison à la commande
                intitulAdressLivraison = GetIntitulAdress(orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.Street[0], orderMagento.CustomerEmail);
                File.AppendAllText("Log\\test.txt", "order adress to find : " + intitulAdressLivraison + Environment.NewLine);
                if (String.IsNullOrEmpty(intitulAdressLivraison))
                {
                    createBtoCShippingAdress(orderMagento, customer, "Livraison");
                    File.AppendAllText("Log\\test.txt", DateTime.Now + " Création addresse livraison avant affectation à la commande: " + Environment.NewLine);
                }
                IBOClient3 customer2 = gescom.CptaApplication.FactoryClient.ReadNumero("1ESHOP");
                foreach (IBOClientLivraison3 item in customer2.FactoryClientLivraison.List)
                {
                    File.AppendAllText("Log\\test.txt", "client list of adress " + item.LI_Intitule + Environment.NewLine);
                    if (item.LI_Intitule.Equals(intitulAdressLivraison))
                    {
                        order.LieuLivraison = item;
                        order.Write();
                        File.AppendAllText("Log\\test.txt", "found " + intitulAdressLivraison + Environment.NewLine);
                        break;
                    }
                }
                // order.LieuLivraison = adress;
                File.AppendAllText("Log\\test.txt", "order adress " + order.LieuLivraison.LI_Intitule + Environment.NewLine);

                order.Write();
            // création des lignes de la commandes
            try
            {
                foreach (Object.Order.ParentItemElement product in orderMagento.Items)
                {
                    if (product.ProductType.Equals("configurable"))
                    {
                        continue;
                    }
                    IBODocumentVenteLigne3 docLigne = (IBODocumentVenteLigne3)order.FactoryDocumentLigne.Create();
                    var ArticleExist = gescom.FactoryArticle.ExistReference(product.Sku);
                    if (ArticleExist)
                    {
                            //= double.Parse(product.RowTotal.ToString(), System.Globalization.CultureInfo.InvariantCulture);
                            File.AppendAllText("Log\\test.txt", "doc line " + product.Sku + " Price :" + product.Price.ToString() + " base price " +product.BasePrice.ToString()+" row total " + product.RowTotal.ToString() + Environment.NewLine);
                            // produit simple
                            docLigne.SetDefaultArticle(gescom.FactoryArticle.ReadReference(product.Sku), Int32.Parse(product.QtyOrdered.ToString()));
                            //docLigne.DL_PrixRU = product.Price;
                            docLigne.DL_PrixUnitaire =product.Price;
                            //docLigne.DL_PUTTC = product.BasePriceInclTax;
                            
                            //docLigne.DL_PUTTC = product.BasePriceInclTax;
                            //docLigne.SetDefaultRemise();
                            File.AppendAllText("Log\\test.txt", "doc line " + product.Sku + " Price :" + product.Price.ToString() + " base price " + product.BasePrice.ToString() + " row total " + product.RowTotal.ToString() + " prix doc line " + docLigne.DL_PrixUnitaire.ToString() + Environment.NewLine);

                            //docLigne.DL_PrixUnitaire = product.Price;//.ToString(), System.Globalization.CultureInfo.InvariantCulture);
                            //docLigne.SetDefaultArticleReferenceClient(customer.CT_Num, Int32.Parse(product.QtyOrdered.ToString()));
                            //SHipping price

                            /*if (product["product_ref"].ToString().Equals("TRANSPORT"))
                            {
                                docLigne.DL_PrixUnitaire = Convert.ToDouble(orderMagento.ShippingAmount.ToString().Replace('.', ','));
                            }
                            else if (product["product_ref"].ToString().Equals("REMISE"))
                            {
                                docLigne.DL_PrixUnitaire = Convert.ToDouble(product.Price.ToString().Replace('.', ','));
                            }*/
                        }
                        else
                    {
                        // on récupère la chaine de gammages d'un produit
                        string product_attribut_string = GetParentProductDetails(product.Sku).ToString();
                        String[] subgamme = product_attribut_string.Split('|');
                        IBOArticle3 article = gescom.FactoryArticle.ReadReference(subgamme[0].ToString());
                        if (subgamme.Length == 3)
                        {
                            // produit à simple gamme
                            IBOArticleGammeEnum3 articleEnum = ControllerArticle.GetArticleGammeEnum1(article, new Gamme(subgamme[1], subgamme[2]));
                            docLigne.SetDefaultArticleMonoGamme(articleEnum, Int32.Parse(product.QtyOrdered.ToString()));
                            docLigne.DL_PrixUnitaire = product.Price;
                            }
                        else if (subgamme.Length == 5)
                        {
                            // produit à double gamme
                            IBOArticleGammeEnum3 articleEnum = ControllerArticle.GetArticleGammeEnum1(article, new Gamme(subgamme[1], subgamme[2], subgamme[3], subgamme[4]));
                            IBOArticleGammeEnum3 articleEnum2 = ControllerArticle.GetArticleGammeEnum2(article, new Gamme(subgamme[1], subgamme[2], subgamme[3], subgamme[4]));
                            docLigne.SetDefaultArticleDoubleGamme(articleEnum, articleEnum2, Int32.Parse(product.QtyOrdered.ToString()));
                            docLigne.DL_PrixUnitaire = product.Price;
                            
                            }
                    }
                    docLigne.Write();
                }
                
            }
            catch (Exception e)
            {
                //UtilsWebservices.UpdateOrderFlag(orderMagento.EntityId.ToString(), "2");
                var jsonFlag2 = JsonConvert.SerializeObject(UpdateOrderFlag(orderMagento.EntityId.ToString(), orderMagento.IncrementId.ToString(), "2"));
                UtilsWebservices.SendDataJsonB2C(jsonFlag2, @"rest/all/V1/orders", "POST");
                StringBuilder sb = new StringBuilder();
                sb.Append(DateTime.Now + e.Message + Environment.NewLine);
                sb.Append(DateTime.Now + e.StackTrace + Environment.NewLine);
                UtilsMail.SendErrorMail(DateTime.Now + e.Message + Environment.NewLine + e.StackTrace + Environment.NewLine, "COMMANDE LIGNE");
                File.AppendAllText("Log\\order.txt", sb.ToString());
                sb.Clear();
                // UtilsWebservices.SendDataNoParse(UtilsConfig.BaseUrl + EnumEndPoint.Commande.Value, "errorOrder&orderID=" + jsonOrder["id_order"]);
                order.Remove();
                return;
            }
            order.FraisExpedition = orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Total.ShippingAmount;
            order.Write();
            addOrderToLocalDB(orderMagento.EntityId.ToString(), order.Client.CT_Num, order.DO_Piece, order.DO_Ref, orderMagento.IncrementId,"B2C");
            // TODO updateOrderFlag using custom PHP script

            var jsonFlag = JsonConvert.SerializeObject(UpdateOrderFlag(orderMagento.EntityId.ToString(), orderMagento.IncrementId.ToString(), "0"));
            UtilsWebservices.SendDataJsonB2C(jsonFlag, @"rest/all/V1/orders", "POST");
            }
            catch (Exception s)
            {
                var jsonFlag2 = JsonConvert.SerializeObject(UpdateOrderFlag(orderMagento.EntityId.ToString(), orderMagento.IncrementId.ToString(), "2"));
                UtilsWebservices.SendDataJson(jsonFlag2, @"rest/all/V1/orders", "POST");
                StringBuilder sb = new StringBuilder();
                sb.Append(DateTime.Now + s.Message + Environment.NewLine);
                sb.Append(DateTime.Now + s.StackTrace + Environment.NewLine);
                UtilsMail.SendErrorMail(DateTime.Now + s.Message + Environment.NewLine + s.StackTrace + Environment.NewLine, "COMMANDE LIGNE");
                File.AppendAllText("Log\\order.txt", sb.ToString());
                sb.Clear();
                // UtilsWebservices.SendDataNoParse(UtilsConfig.BaseUrl + EnumEndPoint.Commande.Value, "errorOrder&orderID=" + jsonOrder["id_order"]);
                return;
            }
        }

        public static void CreateDevis(DataGridView devisList)
        {
            string clientCtNum="";
            string clienttype = "";
            foreach (DataGridViewRow item in devisList.Rows)
            {
                if (item.Cells[0].Value.ToString().Equals("True"))
                {

                    string currentIdDevis = item.Cells[1].Value.ToString(); //orderSearch.Items[i].EntityId.ToString();
                    var DevisSearch = Object.Devis.Devis.FromJson(UtilsWebservices.GetMagentoData("rest/V1/amasty_quote/search"+ UtilsWebservices.SearchOrderCriteria("quote_id",currentIdDevis,"eq")));
                    if (DevisSearch.TotalCount > 0 & !String.IsNullOrEmpty(DevisSearch.Items[0].Customer.Id.ToString()))
                    {
                        CustomerSearch client = UtilsWebservices.GetClientCtNum(DevisSearch.Items[0].Customer.Id.ToString());
                        try
                        {
                            for (int j = 0; j < client.CustomAttributes.Count; j++)
                            {
                                if (client.CustomAttributes[j].AttributeCode.Equals("sage_number"))
                                {
                                    clientCtNum = client.CustomAttributes[j].Value.ToString();
                                }
                                    if (client.CustomAttributes[j].AttributeCode.Equals("customer_type"))
                                    {
                                        clienttype = client.CustomAttributes[j].Value.ToString();
                                    }
                            }
                        }
                        catch (Exception e)
                        {

                            clientCtNum = "";
                        }
                        if (ControllerClient.CheckIfClientExist(clientCtNum))
                        {

                            // si le client existe on associé la devis à son compte
                            AddNewDevisForCustomer(DevisSearch.Items[0], clientCtNum, client);

                        }
                        else
                        {/*
                            // si le client n'existe pas on récupère les info de magento et on le crée dans la base sage 
                            //string client = UtilsWebservices.SendDataNoParse(UtilsConfig.BaseUrl + EnumEndPoint.Client.Value, "getClient&clientID=" + order["id_customer"]);
                            string ct_num = ControllerClient.CreateNewClientDevis(client, DevisSearch);
                            Object.Customer customerMagento = new Object.Customer();
                            var jsonClient = JsonConvert.SerializeObject(customerMagento.UpdateCustomer(ct_num, clienttype, client.Id.ToString()));
                            UtilsWebservices.SendDataJson(jsonClient, @"rest/all/V1/customers/" + client.Id.ToString(), "PUT");
                            if (!String.IsNullOrEmpty(ct_num))
                            {
                                // le client à bien été crée on peut intégrer la commande sur son compte sage
                                AddNewDevisForCustomer(DevisSearch.Items[0], ct_num, client);
                            }*/


                            CustomerSearchByEmail ClientSearch = CustomerSearchByEmail.FromJson(UtilsWebservices.GetMagentoData("rest/V1/customers/search" + UtilsWebservices.SearchOrderCriteria("email", client.Email, "eq")));
                            Customer customerMagento = new Customer();
                            IBOClient3 clientSageObj = ControllerClient.CheckIfClientEmailExist(client.Email);
                            if (clientSageObj != null)
                            {
                                Client ClientData = new Client(clientSageObj);
                                var jsonClient = JsonConvert.SerializeObject(customerMagento.UpdateCustomer(clientSageObj.CT_Num, clienttype, client.Id.ToString()));
                                UtilsWebservices.SendDataJson(jsonClient, @"rest/all/V1/customers/" + ClientSearch.Items[0].Id.ToString(), "PUT");
                                AddNewDevisForCustomer(DevisSearch.Items[0], clientSageObj.CT_Num, client);
                            }
                            else
                            {
                                string ct_num = ControllerClient.CreateNewClientDevis(client, DevisSearch);//.Items[0]);

                                if (!String.IsNullOrEmpty(ct_num))
                                {
                                    var jsonClient = JsonConvert.SerializeObject(customerMagento.UpdateCustomer(ct_num, clienttype, client.Id.ToString()));
                                    UtilsWebservices.SendDataJson(jsonClient, @"rest/all/V1/customers/" + client.Id.ToString(), "PUT");
                                    // le client à bien été crée on peut intégrer la commande sur son compte sage
                                    AddNewDevisForCustomer(DevisSearch.Items[0], ct_num, client);
                                }
                            }
                        }
                    }
 
                }   
               
            }
        }

        private static void AddNewDevisForCustomer(DevisItem devisItem, string ct_num, CustomerSearch client)
        {
            var gescom = SingletonConnection.Instance.Gescom;

            // création de l'entête de la commande 

            IBOClient3 customer = gescom.CptaApplication.FactoryClient.ReadNumero(ct_num);
            IBODocumentVente3 order = gescom.FactoryDocumentVente.CreateType(DocumentType.DocumentTypeVenteDevis);
            order.SetDefault();
            order.SetDefaultClient(customer);
            order.DO_Date = DateTime.Now;
            order.Souche = gescom.FactorySoucheVente.ReadIntitule(UtilsConfig.Souche);
            order.DO_Ref = "WEB " + devisItem.Id.ToString();//orderMagento.EntityId.ToString();
            order.SetDefaultDO_Piece();

            order.Write();
            // création des lignes de la commandes
            try
            {
                foreach (Object.Devis.ItemItem product in devisItem.Items)
                {
                    if (product.ProductType.Equals("configurable"))
                    {
                        continue;
                    }
                    IBODocumentLigne3 docLigne = (IBODocumentLigne3)order.FactoryDocumentLigne.Create();
                    var ArticleExist = gescom.FactoryArticle.ExistReference(product.Sku);
                    if (ArticleExist)
                    {

                        docLigne.DL_PrixUnitaire = double.Parse(product.Price.ToString(), System.Globalization.CultureInfo.InvariantCulture);
                        // produit simple
                        docLigne.SetDefaultArticle(gescom.FactoryArticle.ReadReference(product.Sku), Int32.Parse(product.Qty.ToString()));
                        //SHipping price

                        /*if (product["product_ref"].ToString().Equals("TRANSPORT"))
                        {
                            docLigne.DL_PrixUnitaire = Convert.ToDouble(orderMagento.ShippingAmount.ToString().Replace('.', ','));
                        }
                        else if (product["product_ref"].ToString().Equals("REMISE"))
                        {
                            docLigne.DL_PrixUnitaire = Convert.ToDouble(product.Price.ToString().Replace('.', ','));
                        }*/
                    }
                    else
                    {
                        // on récupère la chaine de gammages d'un produit
                        string product_attribut_string = GetParentProductDetails(product.Sku).ToString();
                        String[] subgamme = product_attribut_string.Split('|');
                        IBOArticle3 article = gescom.FactoryArticle.ReadReference(subgamme[0].ToString());
                        if (subgamme.Length == 3)
                        {
                            // produit à simple gamme
                            IBOArticleGammeEnum3 articleEnum = ControllerArticle.GetArticleGammeEnum1(article, new Gamme(subgamme[1], subgamme[2]));
                            docLigne.SetDefaultArticleMonoGamme(articleEnum, Int32.Parse(product.Qty.ToString()));
                        }
                        else if (subgamme.Length == 5)
                        {
                            // produit à double gamme
                            IBOArticleGammeEnum3 articleEnum = ControllerArticle.GetArticleGammeEnum1(article, new Gamme(subgamme[1], subgamme[2], subgamme[3], subgamme[4]));
                            IBOArticleGammeEnum3 articleEnum2 = ControllerArticle.GetArticleGammeEnum2(article, new Gamme(subgamme[1], subgamme[2], subgamme[3], subgamme[4]));
                            docLigne.SetDefaultArticleDoubleGamme(articleEnum, articleEnum2, Int32.Parse(product.Qty.ToString()));
                        }
                    }
                    docLigne.Write();
                }
                
            }
            catch (Exception e)
            {
                //UtilsWebservices.UpdateOrderFlag(order.EntityId.ToString(), "2");
                StringBuilder sb = new StringBuilder();
                sb.Append(DateTime.Now + e.Message + Environment.NewLine);
                sb.Append(DateTime.Now + e.StackTrace + Environment.NewLine);
                //UtilsMail.SendErrorMail(DateTime.Now + e.Message + Environment.NewLine + e.StackTrace + Environment.NewLine, "COMMANDE LIGNE");
                File.AppendAllText("Log\\Devis.txt", sb.ToString());
                sb.Clear();
                // UtilsWebservices.SendDataNoParse(UtilsConfig.BaseUrl + EnumEndPoint.Commande.Value, "errorOrder&orderID=" + jsonOrder["id_order"]);
                order.Remove();
                return;
            }
        }

        private static void addOrderToLocalDB(string orderID, string CT_Num, string DO_piece, string DO_Ref, string incremented_id, string Site = "B2B")
        {
            // Open database (or create if doesn't exist)
            using (var db = new LiteDatabase(@"MyData.db"))
            {
                // Get a collection (or create, if doesn't exist)
                var col = db.GetCollection<LinkedCommandeDB>("Commande");

                // Create your new customer instance
                var commande = new LinkedCommandeDB
                {
                    OrderID = orderID,
                    OrderType = "DocumentTypeVenteCommande",
                    CT_Num = CT_Num,
                    DO_piece = DO_piece,
                    DO_Ref = DO_Ref,
                    incremented_id = incremented_id,
                    Shipped = "1",
                    Site = Site

                };
                col.Insert(commande);
            }
        }

        public static string GetPrestaOrderStatutFromMapping(DocumentType orderSageType)
        {
            string prestaType;
            if(UtilsConfig.OrderMapping.TryGetValue(orderSageType.ToString(), out prestaType))
            {
                return prestaType;
            }
            else
            {
                return null;
            }
        }

        public static string UpdateStatusOnMagento(string orderID, string incremented_id , string status)
        {
            var UpdateOrder = new
            {
                entity = new
                {
                    entity_id = orderID,
                    increment_id = incremented_id,
                    state = "processing",
                    status = status
                }
            };
            return JsonConvert.SerializeObject(UpdateOrder);
        }
        public static object UpdateOrderFlag(string orderID, string incremented_id, string flag)
        {
            //string test = '{"entity":{"entity_id":"17","increment_id":"000000016","extension_attributes":{"order_flag":"0"}}}';
            var updateFlag = new
            {
                entity = new
                {
                    entity_id = orderID,
                    increment_id = incremented_id,
                    extension_attributes = new
                    {
                        order_flag = flag
                    }
                }
            };
            return updateFlag;
        }
        public static StringBuilder GetParentProductDetails(string sku)
        {
            //File.AppendAllText("Log\\Process.txt", DateTime.Now + " start GetParentProductDetails  " + Environment.NewLine);
            var gescom = SingletonConnection.Instance.Gescom;
            var articlesSageObj = gescom.FactoryArticle.List;
            StringBuilder results = new StringBuilder();
            results.Append("");
            foreach (IBOArticle3 articleSage in articlesSageObj)
            {
                // on check si l'article est cocher en publier sur le site marchand
                if (!articleSage.AR_Publie)
                    continue;
                Article article = new Article(articleSage);
                if (article.isGamme)
                {
                    foreach (Gamme doubleGamme in article.Gammes)
                    {
                        if (article.IsDoubleGamme)
                        {
                            if (doubleGamme.Reference.Equals(sku))
                            {
                                results.Append(article.Reference);
                                results.Append("|");
                                results.Append(doubleGamme.Intitule);
                                results.Append("|");
                                results.Append(doubleGamme.Value_Intitule);
                                results.Append("|");
                                results.Append(doubleGamme.Intitule2);
                                results.Append("|");
                                results.Append(doubleGamme.Value_Intitule2);
                                return results;
                            }
                        }
                        else
                        {
                            if (doubleGamme.Reference.Equals(sku))
                            {
                                results.Append(article.Reference);
                                results.Append("|");
                                results.Append(doubleGamme.Intitule);
                                results.Append("|");
                                results.Append(doubleGamme.Value_Intitule);
                                return results;
                            }
                        }
                    }
                }
            }
            //File.AppendAllText("Log\\Process.txt", DateTime.Now + " end GetParentProductDetails  " + Environment.NewLine);
            return results;
        }
        public static bool HasMethod(this object objectToCheck, string methodName)
        {
            var type = objectToCheck.GetType();
            return type.GetMethod(methodName) != null;
        }
        public static void createBtoCShippingAdress(OrderSearchItem orderMagento,IBOClient3 customer, string AdressType)
        {
            File.AppendAllText("Log\\test.txt", DateTime.Now + " begin create shipping adress : " + orderMagento.IncrementId.ToString() + Environment.NewLine);
            int AdressNumber = 0;
            SqlDataReader DbAdressNumber = SingletonConnection.Instance.dB.Select("select count(*) from [" + System.Configuration.ConfigurationManager.AppSettings["DBNAME"].ToString() + "].[dbo].[F_LIVRAISON] where LI_EMail like '" + orderMagento.CustomerEmail.ToString() + "'");
            while (DbAdressNumber.Read())
            {
                AdressNumber = Int32.Parse(DbAdressNumber.GetValue(0).ToString());
            }
            DbAdressNumber.Close();
            // requete SQL
            if (AdressNumber == 0)
            {
                AdressNumber = 1;
            }
            else
            {
                AdressNumber++;
            }
            IBOClientLivraison3 adress = (IBOClientLivraison3)customer.FactoryClientLivraison.Create();
            adress.SetDefault();
            string intitule;
            if (AdressNumber > 9)
            {
                intitule = orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.Lastname+ " " + orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.Firstname.ToUpper() + " " + AdressNumber.ToString();
            }
            else
            {
                intitule = orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.Lastname + " " + orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.Firstname.ToUpper() + " 0" + AdressNumber.ToString();
            }
            adress.LI_Intitule = intitule;
            adress.LI_Contact = AdressType;
            adress.Adresse.Adresse = orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.Street[0].ToString();
            if (orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Method.Equals("dpdrelais_dpdrelais"))
            {
                adress.Adresse.Complement = orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.company.ToString();
            }
            else
            {
                if (orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.Street.Count > 1)
                {
                    adress.Adresse.Complement = orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.Street[1].ToString();
                }
            }
            
            adress.Telecom.EMail = orderMagento.CustomerEmail.ToString();
            adress.Telecom.Telephone = orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.Telephone.ToString();
            adress.Adresse.CodePostal = orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.Postcode.ToString();
            adress.Adresse.Ville = orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.City.ToString();
            if (HasMethod(orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Address, "Region"))
            {
                adress.Adresse.CodeRegion = orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.Region.ToString();
            }
            adress.Adresse.Pays = ISO3166.FromAlpha2(orderMagento.ExtensionAttributes.ShippingAssignments[0].Shipping.Address.CountryId.ToString()).Name;
            adress.Write();
            File.AppendAllText("Log\\test.txt", DateTime.Now + " end create shipping adress : " + intitule + Environment.NewLine);
        }

        public static void createBtoCBillingAdress(OrderSearchItem orderMagento, IBOClient3 customer, string AdressType)
        {
            File.AppendAllText("Log\\test.txt", DateTime.Now + " begin create billing adress : " + orderMagento.IncrementId.ToString() + Environment.NewLine);
            int AdressNumber = 0;
            SqlDataReader DbAdressNumber = SingletonConnection.Instance.dB.Select("select count(*) from [" + System.Configuration.ConfigurationManager.AppSettings["DBNAME"].ToString() + "].[dbo].[F_LIVRAISON] where LI_EMail like '" + orderMagento.BillingAddress.Email.ToString() + "'");
            while (DbAdressNumber.Read())
            {
                AdressNumber = Int32.Parse(DbAdressNumber.GetValue(0).ToString());
            }
            DbAdressNumber.Close();
            // requete SQL
            if (AdressNumber == 0)
            {
                AdressNumber = 1;
            }
            else
            {
                AdressNumber++;
            }
            IBOClientLivraison3 adress = (IBOClientLivraison3)customer.FactoryClientLivraison.Create();
            adress.SetDefault();
            string intitule = "";
            if (AdressNumber > 9)
            {
                intitule = orderMagento.BillingAddress.Lastname + " " + orderMagento.BillingAddress.Firstname.ToUpper() + " " + AdressNumber.ToString();
            }
            else
            {
                intitule = orderMagento.BillingAddress.Lastname + " " + orderMagento.BillingAddress.Firstname.ToUpper() + " 0" + AdressNumber.ToString();
            }
            adress.LI_Intitule = intitule;
            adress.LI_Contact = AdressType;
            adress.Adresse.Adresse = orderMagento.BillingAddress.Street[0].ToString();
            if (orderMagento.BillingAddress.Street.Count > 1 )
            {
                adress.Adresse.Complement = orderMagento.BillingAddress.Street[1].ToString();
            }
            adress.Telecom.EMail = orderMagento.BillingAddress.Email.ToString();
            adress.Telecom.Telephone = orderMagento.BillingAddress.Telephone.ToString();
            adress.Adresse.CodePostal = orderMagento.BillingAddress.Postcode.ToString();
            adress.Adresse.Ville = orderMagento.BillingAddress.City.ToString();
            if (HasMethod(orderMagento.BillingAddress, "Region"))
            {
                adress.Adresse.CodeRegion = orderMagento.BillingAddress.Region.ToString();
            }
            adress.Adresse.Pays = ISO3166.FromAlpha2(orderMagento.BillingAddress.CountryId.ToString()).Name;
            adress.Write();
            File.AppendAllText("Log\\test.txt", DateTime.Now + " end create billing adress : " + intitule + Environment.NewLine);
            /*SqlDataReader DbAdressLiNo = SingletonConnection.Instance.dB.Select("select [LI_No] from [" + System.Configuration.ConfigurationManager.AppSettings["DBNAME"].ToString() + "].[dbo].[F_LIVRAISON] where [LI_Intitule] like '" + intitule.ToString() + "'");
            int AdressLiNo = 0;
            while (DbAdressLiNo.Read())
            {
                AdressLiNo = Int32.Parse(DbAdressLiNo.GetValue(0).ToString());
            }
            DbAdressLiNo.Close();
            List<CustomAttribute> CustomAttributes = new List<CustomAttribute>();
            CustomAttribute custom_attribute = new CustomAttribute();
            custom_attribute.AttributeCode = "li_no";
            custom_attribute.Value = AdressLiNo.ToString();
            CustomAttributes.Add(custom_attribute);

            foreach (Object.CustomerSearch.Address addressClient in customerMagento.Addresses)
            {
                if (orderMagento.BillingAddress.CustomerAddressId == addressClient.Id)
                {
                    addressClient.CustomAttributes = CustomAttributes;
                    return addressClient;
                }
            }
            Object.CustomerSearch.Address Addresses = new Object.CustomerSearch.Address();
            Addresses.Firstname = orderMagento.BillingAddress.Firstname;
            Addresses.Lastname = orderMagento.BillingAddress.Lastname;
            Addresses.Postcode = orderMagento.BillingAddress.Postcode;
            Addresses.CountryId = orderMagento.BillingAddress.CountryId;
            Addresses.City = orderMagento.BillingAddress.City;
            Addresses.company= orderMagento.BillingAddress.company;
            Addresses.Street = orderMagento.BillingAddress.Street;
            Addresses.Telephone = orderMagento.BillingAddress.Telephone;
            Addresses.CustomAttributes = CustomAttributes;
            return Addresses;*/
        }

        public static void createBtoCAdress(OrderSearchItem orderMagento, IBOClient3 customer, string AdressType)
        {
            int AdressNumber = 0;
            SqlDataReader DbAdressNumber = SingletonConnection.Instance.dB.Select("select count(*) from [" + System.Configuration.ConfigurationManager.AppSettings["DBNAME"].ToString() + "].[dbo].[F_LIVRAISON] where LI_EMail like '" + orderMagento.BillingAddress.Email.ToString() + "'");
            while (DbAdressNumber.Read())
            {
                AdressNumber = Int32.Parse(DbAdressNumber.GetValue(0).ToString());
            }
            DbAdressNumber.Close();
            // requete SQL
            if (AdressNumber == 0)
            {
                AdressNumber = 1;
            }
            else
            {
                AdressNumber++;
            }
            IBOClientLivraison3 adress = (IBOClientLivraison3)customer.FactoryClientLivraison.Create();
            adress.SetDefault();
            string intitule = "";
            if (AdressNumber > 9)
            {
                intitule = orderMagento.BillingAddress.Lastname + " " + orderMagento.BillingAddress.Firstname.ToUpper() + " " + AdressNumber.ToString();
            }
            else
            {
                intitule = orderMagento.BillingAddress.Lastname + " " + orderMagento.BillingAddress.Firstname.ToUpper() + " 0" + AdressNumber.ToString();
            }
            adress.LI_Intitule = intitule;
            adress.LI_Contact = AdressType;
            adress.Adresse.Adresse = orderMagento.BillingAddress.Street[0].ToString();
            if (orderMagento.BillingAddress.Street.Count > 1)
            {
                adress.Adresse.Complement = orderMagento.BillingAddress.Street[1].ToString();
            }
            adress.Telecom.EMail = orderMagento.BillingAddress.Email.ToString();
            adress.Telecom.Telephone = orderMagento.BillingAddress.Telephone.ToString();
            adress.Adresse.CodePostal = orderMagento.BillingAddress.Postcode.ToString();
            adress.Adresse.Ville = orderMagento.BillingAddress.City.ToString();
            if (HasMethod(orderMagento.BillingAddress,"Region"))
            {
                adress.Adresse.CodeRegion = orderMagento.BillingAddress.Region.ToString();
            }
            adress.Adresse.Pays = ISO3166.FromAlpha2(orderMagento.BillingAddress.CountryId.ToString()).Name;
            adress.Write();
            /*SqlDataReader DbAdressLiNo = SingletonConnection.Instance.dB.Select("select [LI_No] from [" + System.Configuration.ConfigurationManager.AppSettings["DBNAME"].ToString() + "].[dbo].[F_LIVRAISON] where [LI_Intitule] like '" + intitule.ToString() + "'");
            int AdressLiNo = 0;
            while (DbAdressLiNo.Read())
            {
                AdressLiNo = int.Parse(DbAdressLiNo.GetValue(0).ToString());
            }
            DbAdressLiNo.Close();
            List<CustomAttribute> CustomAttributes = new List<CustomAttribute>();
            CustomAttribute custom_attribute = new CustomAttribute();
            custom_attribute.AttributeCode = "li_no";
            custom_attribute.Value = AdressLiNo.ToString();
            CustomAttributes.Add(custom_attribute);
            foreach (Object.CustomerSearch.Address addressClient in customerMagento.Addresses)
            {
                if (orderMagento.BillingAddress.CustomerAddressId == addressClient.Id)
                {
                    
                    addressClient.CustomAttributes = CustomAttributes;
                    return addressClient;
                }
            }
            Object.CustomerSearch.Address Addresses = new Object.CustomerSearch.Address();
            Addresses.Firstname = orderMagento.BillingAddress.Firstname;
            Addresses.Lastname = orderMagento.BillingAddress.Lastname;
            Addresses.Postcode = orderMagento.BillingAddress.Postcode;
            Addresses.CountryId = orderMagento.BillingAddress.CountryId;
            Addresses.City = orderMagento.BillingAddress.City;
            Addresses.company = orderMagento.BillingAddress.company;
            Addresses.Street = orderMagento.BillingAddress.Street;
            Addresses.Telephone = orderMagento.BillingAddress.Telephone;
            Addresses.CustomAttributes = CustomAttributes;
            return Addresses;*/
        }

        public static Boolean ExistAdress(string adress, string email)
        {
            string sql = "SELECT [LI_No] FROM [" + System.Configuration.ConfigurationManager.AppSettings["DBNAME"].ToString() + "].[dbo].[F_LIVRAISON] where CT_Num = '1ESHOP' and LI_Adresse ='"+adress.Replace("'", "''") + "' and LI_EMail = '"+email+"'";
            File.AppendAllText("Log\\test.txt", DateTime.Now + " requete SQL exist adress: " + sql + Environment.NewLine);
            SqlDataReader AddressLiNo= SingletonConnection.Instance.dB.Select(sql);
            while (AddressLiNo.Read())
            {
                return true;
            }
            return false;
        }
        public static string GetIntitulAdress(string adress, string email)
        {
            string sql = "SELECT [LI_Intitule] FROM [" + System.Configuration.ConfigurationManager.AppSettings["DBNAME"].ToString() + "].[dbo].[F_LIVRAISON] where CT_Num = '1ESHOP' and LI_Adresse ='" + adress.Replace("'", "''")+ "' and LI_EMail = '" + email + "'";
            File.AppendAllText("Log\\test.txt", DateTime.Now + " requete SQL get intitule : " + sql+ Environment.NewLine);
            SqlDataReader AddressLiNo = SingletonConnection.Instance.dB.Select(sql);
            while (AddressLiNo.Read())
            {
                return AddressLiNo.GetValue(0).ToString();
            }
            return "";
        }

        public static string GetIntitulAdressBtoB(string adress, string email,string CT_num)
        {
            string sql = "SELECT [LI_Intitule] FROM [" + System.Configuration.ConfigurationManager.AppSettings["DBNAME"].ToString() + "].[dbo].[F_LIVRAISON] where CT_Num = '"+ CT_num + "' and LI_Adresse ='" + adress.Replace("'", "''") + "' and LI_EMail = '" + email + "'";
            File.AppendAllText("Log\\test.txt", DateTime.Now + " requete SQL get intitule : " + sql + Environment.NewLine);
            SqlDataReader AddressLiNo = SingletonConnection.Instance.dB.Select(sql);
            while (AddressLiNo.Read())
            {
                return AddressLiNo.GetValue(0).ToString();
            }
            return "";
        }
    }
}
