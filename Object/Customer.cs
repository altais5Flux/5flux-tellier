using Objets100cLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using WebservicesSage.Singleton;
using WebservicesSage.Utils;
using Newtonsoft.Json.Linq;
using WebservicesSage.Object.CatTarSearch;
using Newtonsoft.Json;
using System.IO;
using System.Data.SqlClient;

namespace WebservicesSage.Object
{
    public class Customer
    {
        public long GroupId { get; set; }
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public long Gender { get; set; }
        public long StoreId { get; set; }
        public long WebsiteId { get; set; }
        public List<Address> Addresses { get; set; }
        public List<CustomAttribute> CustomAttributes { get; set; }
        public partial class CustomAttribute
        {
            public string attribute_code { get; set; }
            public string value { get; set; }
        }
        public partial class Address
        {
            public Region Region { get; set; }
            public long RegionId { get; set; }
            public string CountryId { get; set; }
            public List<string> Street { get; set; }
            public string Telephone { get; set; }
            public string Postcode { get; set; }
            public string City { get; set; }
            public string Company { get; set; }
            public string Firstname { get; set; }
            public string Lastname { get; set; }
            public bool DefaultShipping { get; set; }
            public bool DefaultBilling { get; set; }
            public List<CustomAttribute> CustomAttributes { get; set; }
        }

        public partial class Region
        {
            public object RegionCode { get; set; }
            public object region { get; set; }
            public long RegionId { get; set; }
        }

        public Customer()
        {

        }
        public object UpdateCustomer(string ct_Num,string clienttype,string idCustomer)
        {
            var compta = SingletonConnection.Instance.Gescom.CptaApplication;
            var clientsSageObj = compta.FactoryClient.ReadNumero(ct_Num);

            Client client = new Client(clientsSageObj);
            CustomAttribute custom_attribute = new CustomAttribute();
            CustomAttributes = new List<CustomAttribute>();
            custom_attribute.attribute_code = "sage_number";
            custom_attribute.value = client.CT_NUM.ToString();
            CustomAttributes.Add(custom_attribute);
            CustomAttribute custom_attribute1 = new CustomAttribute();
            custom_attribute1.attribute_code = "enseigne";
            custom_attribute1.value = client.Intitule.ToString();
            CustomAttributes.Add(custom_attribute1);
            CustomAttribute custom_attribute2 = new CustomAttribute();
            custom_attribute2.attribute_code = "immatriculation";
            custom_attribute2.value = client.Siret.ToString();
            CustomAttributes.Add(custom_attribute2);

            Firstname = client.Intitule;
            Lastname = "(" + client.CT_NUM + ")";
            WebservicesSage.Object.CustomerSearch.CustomerSearch Customer = UtilsWebservices.GetClientCtNum(idCustomer);
            var tarifsSearch = UtilsWebservices.GetMagentoData("rest/V1/customerGroups/search" + UtilsWebservices.SearchCategoriesCriteria("code", client.Central, "eq"));
            CatTarifSearch catTarifSearch = CatTarifSearch.FromJson(tarifsSearch);
            string group_id = "";
            if (catTarifSearch.TotalCount > 0 )
            {
                if (Customer.GroupId.ToString().Equals(catTarifSearch.Items[0].Id.ToString()))
                {
                    group_id = catTarifSearch.Items[0].Id.ToString();
                }
                else
                {
                    group_id = Customer.GroupId.ToString();
                }
            }
            var customerJson = new
            {
                customer = new
                {
                    group_id = group_id.ToString(),
                    email = client.Email.ToString(),
                    id = idCustomer,
                    firstname = Firstname,
                    lastname = Lastname,
                    website_id = Utils.UtilsConfig.StoreBtoB.ToString(),
                    disable_auto_group_change = "0",
                    extension_attributes = new { is_subscribed = false },
                    custom_attributes = CustomAttributes
                }
                
            };
            return customerJson;
        }
        public object NewCustomer(Client client, IBOClient3 clientsg, CustomerSearchByEmail.CustomerSearchByEmail customerSearchByEmail=null)
        {
            try
            {
                string group_id = "";
                var tarifsSearch = UtilsWebservices.GetMagentoData("rest/V1/customerGroups/search" + UtilsWebservices.SearchCategoriesCriteria("code", client.Central, "eq"));
                //File.AppendAllText("Log\\groupe.txt", " groupe client 1:" + tarifsSearch.ToString() + Environment.NewLine);
                CatTarifSearch catTarifSearch = CatTarifSearch.FromJson(tarifsSearch);
                if (catTarifSearch.TotalCount > 0)
                {
                    group_id = catTarifSearch.Items[0].Id.ToString();
                    //File.AppendAllText("Log\\groupe.txt", " groupe_id client :" + group_id + Environment.NewLine);
                }
                else
                {
                    var groupeJson = new
                    {
                        group = new
                        {
                            code = client.Central.ToString(),
                            tax_class_id = 3
                        }

                    };
                    var jsonClient = JsonConvert.SerializeObject(groupeJson);
                    //File.AppendAllText("Log\\groupe.txt", " groupe client :" + jsonClient.ToString() + Environment.NewLine);
                    UtilsWebservices.SendDataJson(jsonClient, @"rest/V1/customerGroups/");
                    var tarifsSearch1 = UtilsWebservices.GetMagentoData("rest/V1/customerGroups/search" + UtilsWebservices.SearchCategoriesCriteria("code", client.Central, "eq"));
                    //File.AppendAllText("Log\\groupe.txt", " tarif search :" + tarifsSearch1.ToString() + Environment.NewLine);
                    CatTarifSearch catTarifSearch1 = CatTarifSearch.FromJson(tarifsSearch1);
                    if (catTarifSearch1.TotalCount >0)
                    {
                        group_id = catTarifSearch1.Items[0].Id.ToString();
                    }
                    //create customer groupe
                }
                //File.AppendAllText("Log\\groupe.txt", " create custom attribute:"  + Environment.NewLine);
                CustomAttributes = new List<CustomAttribute>();
                CustomAttribute custom_attribute = new CustomAttribute();
                custom_attribute.attribute_code = "sage_number";
                custom_attribute.value = client.CT_NUM.ToString();
                CustomAttributes.Add(custom_attribute);
                //File.AppendAllText("Log\\groupe.txt", " create custom attribute 1: "+ custom_attribute.value +" code "+ custom_attribute.attribute_code + Environment.NewLine);
                CustomAttribute custom_attribute1 = new CustomAttribute();
                custom_attribute1.attribute_code = "enseigne";
                custom_attribute1.value = client.Intitule.ToString();
                CustomAttributes.Add(custom_attribute1);
                //File.AppendAllText("Log\\groupe.txt", " create custom attribute 2: " + custom_attribute1.value + " code " + custom_attribute1.attribute_code + Environment.NewLine);
                if (!String.IsNullOrEmpty(client.Siret))
                {
                    CustomAttribute custom_attribute2 = new CustomAttribute();
                    custom_attribute2.attribute_code = "immatriculation";
                    custom_attribute2.value = client.Siret.ToString();
                    CustomAttributes.Add(custom_attribute2);
                    //File.AppendAllText("Log\\groupe.txt", " create custom attribute 3: " + custom_attribute2.value + " code " + custom_attribute2.attribute_code + Environment.NewLine);
                }
                
                Firstname = client.Intitule;
                Lastname = "(" + client.CT_NUM + ")";
                if (!String.IsNullOrEmpty(clientsg.Telecom.Telephone))
                {
                    CustomAttribute custom_attribute3 = new CustomAttribute();
                    custom_attribute3.attribute_code = "telephone";
                    custom_attribute3.value = clientsg.Telecom.Telephone.ToString();
                    CustomAttributes.Add(custom_attribute3);
                    //File.AppendAllText("Log\\groupe.txt", " create custom attribute 4: " + custom_attribute3.value + " code " + custom_attribute3.attribute_code + Environment.NewLine);
                }
                string tva = "";
                if (!String.IsNullOrEmpty(clientsg.CT_Identifiant))
                {
                    tva = clientsg.CT_Identifiant;
                    //File.AppendAllText("Log\\groupe.txt", " tva: " + tva + Environment.NewLine);
                }
                
                var tarifsSearch2 = UtilsWebservices.GetMagentoData("rest/V1/customerGroups/search" + UtilsWebservices.SearchCategoriesCriteria("code", client.Central, "eq"));
                //File.AppendAllText("Log\\error.txt", DateTime.Now+"groupe search tarif 2 : " +tarifsSearch2.ToString() + Environment.NewLine);
                CatTarifSearch catTarifSearch2 = CatTarifSearch.FromJson(tarifsSearch2);
                string group_id1 = catTarifSearch2.Items[0].Id.ToString();

                if (customerSearchByEmail.TotalCount>0)
                {
                    //File.AppendAllText("Log\\error.txt", DateTime.Now + "client existe : " + customerSearchByEmail.Items[0].Id.ToString() + Environment.NewLine);
                    if (client.AddressesList.Count >0)
                    {
                        var customerJson = new
                        {
                            customer = new
                            {
                                id = customerSearchByEmail.Items[0].Id.ToString(),
                                group_id = group_id1,
                                email = client.Email.ToString(),
                                firstname = Firstname,
                                lastname = Lastname,
                                taxvat = tva,
                                website_id = Utils.UtilsConfig.StoreBtoB.ToString(),
                                addresses = client.AddressesList,// Addresses,
                                disable_auto_group_change = "0",
                                extension_attributes = new { is_subscribed = false },
                                custom_attributes = CustomAttributes
                            }
                        };
                        return customerJson;
                    }
                    else
                    {
                        var customerJson = new
                        {
                            customer = new
                            {
                                id = customerSearchByEmail.Items[0].Id.ToString(),
                                group_id = group_id1,
                                email = client.Email.ToString(),
                                firstname = Firstname,
                                lastname = Lastname,
                                taxvat = tva,
                                website_id = Utils.UtilsConfig.StoreBtoB.ToString(),
                                disable_auto_group_change = "0",
                                extension_attributes = new { is_subscribed = false },
                                custom_attributes = CustomAttributes
                            }
                        };
                        return customerJson;
                    }
                    
                }
                else
                {
                    //File.AppendAllText("Log\\error.txt", DateTime.Now + "client n'existe pas: " + client.Email.ToString() + Environment.NewLine);
                    if (client.AddressesList.Count > 0)
                    {
                        var customerJson = new
                        {
                            customer = new
                            {
                                group_id = group_id1,
                                email = client.Email.ToString(),
                                firstname = Firstname,
                                lastname = Lastname,
                                taxvat = tva,
                                website_id = Utils.UtilsConfig.StoreBtoB.ToString(),
                                addresses = client.AddressesList,// Addresses,
                                disable_auto_group_change = "0",
                                extension_attributes = new { is_subscribed = false },
                                custom_attributes = CustomAttributes
                            },
                        };
                        return customerJson;
                    }
                    else
                    {
                        var customerJson = new
                        {
                            customer = new
                            {
                                group_id = group_id1,
                                email = client.Email.ToString(),
                                firstname = Firstname,
                                lastname = Lastname,
                                taxvat = tva,
                                website_id = Utils.UtilsConfig.StoreBtoB.ToString(),
                                disable_auto_group_change = "0",
                                extension_attributes = new { is_subscribed = false },
                                custom_attributes = CustomAttributes
                            },
                        };
                        return customerJson;
                    }
                    
                }
            }
            catch (Exception e)
            {

                File.AppendAllText("Log\\error.txt", DateTime.Now + e.Message + Environment.NewLine);
                File.AppendAllText("Log\\error.txt", DateTime.Now + e.StackTrace + Environment.NewLine);
                File.AppendAllText("Log\\error.txt", DateTime.Now + e.InnerException.Source + Environment.NewLine);
                return null;
            }
        }

        public object UpdateBtoCCustomer(CustomerSearch.CustomerSearch customerSearch, CustomerSearch.Address address)
        {
            int j = 0;
            foreach (CustomerSearch.Address item in customerSearch.Addresses)
            {
                if (item.Street[0].Equals(address.Street[0]))
                {
                    List<CustomerSearch.CustomAttribute> CustomAttributes = new List<CustomerSearch.CustomAttribute>();
                    CustomerSearch.CustomAttribute custom_attribute = new CustomerSearch.CustomAttribute();
                    custom_attribute.AttributeCode = "li_no";
                    custom_attribute.Value = address.CustomAttributes[0].Value.ToString();
                    CustomAttributes.Add(custom_attribute);
                    customerSearch.Addresses[j].CustomAttributes = CustomAttributes;
                }
                j++;
            }
            var customerJson = new
            {
                customer = new
                {
                    group_id = customerSearch.GroupId.ToString(),
                    email = customerSearch.Email.ToString(),
                    id = customerSearch.Id,
                    firstname = customerSearch.Firstname,
                    lastname = customerSearch.Lastname,
                    website_id = customerSearch.WebsiteId,
                    disable_auto_group_change = "0",
                    addresses = customerSearch.Addresses,
                    extension_attributes = new { is_subscribed = false },
                    custom_attributes = customerSearch.CustomAttributes
                }

            };
            return customerJson;
        }

    }
}
