using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebservicesSage.Object;
using WebservicesSage.Object.CustomerSearch;
//using WebservicesSage.Object.Customer;
using WebservicesSage.Object.Search;

namespace WebservicesSage.Utils
{
    public static class UtilsWebservices
    {
        public static string CreateAttribute(string gamme)
        {
                var attributeFormat = new
                {
                    attribute = new
                    {
                        attribute_code = gamme.ToLower(),
                        default_frontend_label = gamme,
                        frontend_labels = new
                        {
                            store_id = UtilsConfig.StoreBtoB.ToString(),
                            label = gamme
                        },
                        is_required = true,
                        default_value = "",
                        frontend_input = "select",
                        is_visible_on_front = true,
                        is_searchable = true,
                        is_visible_in_advanced_search = true,
                        is_filterable = true,
                        is_filterable_in_search = true,
                        entity_type_id = "4"
                    }
                };
                var jsonreq = JsonConvert.SerializeObject(attributeFormat);
                return SendDataJson(jsonreq, "rest/all/V1/products/attributes");
        }

        public static string CreateGammesProduct(Article article)
        {
                foreach (Gamme gamme in article.Gammes)
                {
                    bool first = false;
                    var searchatt = AttributeSearchCriteria.FromJson(UtilsWebservices.SearchAttribute(UtilsConfig.BaseUrlB2B + "rest/V1/products/attributes/", UtilsWebservices.SearchCriteria("attribute_code", gamme.Intitule, "eq")));
                    if (searchatt.TotalCount > 0)
                    {
                        CreateGammeProduct(article, gamme, first, searchatt);
                    }
                    else
                    {
                        if (UtilsWebservices.CreateAttribute(gamme.Intitule) != null)
                        {
                            var searchat = AttributeSearchCriteria.FromJson(UtilsWebservices.SearchAttribute(UtilsConfig.BaseUrlB2B + "rest/V1/products/attributes/", UtilsWebservices.SearchCriteria("attribute_code", gamme.Intitule, "eq")));
                            CreateGammeProduct(article, gamme, first, searchatt);
                        }
                    }
                }
                return "good";
        }
        public static bool CreateGammeProduct(Article article, Gamme gamme, bool first, AttributeSearchCriteria searchatt)
        {
            
                string value_index = "";
                value_index = UtilsWebservices.SearchOption(gamme.Value_Intitule, searchatt);
                if (value_index.Length == 0)
                {
                    value_index = UtilsWebservices.CreateAttributeOption(gamme.Intitule, gamme.Value_Intitule);
                }
                Product productGamme = new Product();
                var jsonGamme = JsonConvert.SerializeObject(productGamme.SimpleProductjson(article, gamme, value_index));
                string responseGamme = UtilsWebservices.SendDataJson(jsonGamme);
                if (!first)
                {
                    string responseOption = UtilsWebservices.CreateOption(article.Reference, searchatt.Items[0].AttributeCode, true, value_index, searchatt.Items[0].AttributeId.ToString());
                    UtilsWebservices.CreateChildSKU(article.Reference, gamme.Reference);
                    first = true;
                }
                else
                {
                    UtilsWebservices.CreateChildSKU(article.Reference, gamme.Reference);
                }
                return true;

        }
        public static string CreateDoublesGammeProduct(Article article)
        {
            bool first = false;
            foreach (Gamme gamme in article.Gammes)
            {
                var searchatt = AttributeSearchCriteria.FromJson(UtilsWebservices.SearchAttribute(UtilsConfig.BaseUrlB2B + "rest/V1/products/attributes/", UtilsWebservices.SearchCriteria("attribute_code", gamme.Intitule, "eq")));
                if (searchatt.TotalCount > 0)
                {
                    //CreateDoubleGammeProduct(article, gamme, first, searchatt);
                    var searchatt1 = AttributeSearchCriteria.FromJson(UtilsWebservices.SearchAttribute(UtilsConfig.BaseUrlB2B + "rest/V1/products/attributes/", UtilsWebservices.SearchCriteria("attribute_code", gamme.Intitule2, "eq")));
                    if (searchatt1.TotalCount > 0)
                    {
                        CreateDoubleGammeProduct(article, gamme, searchatt,searchatt1,first);
                    }
                    else
                    {
                        if (UtilsWebservices.CreateAttribute(gamme.Intitule2) != null)
                        {
                            var searchat1 = AttributeSearchCriteria.FromJson(UtilsWebservices.SearchAttribute(UtilsConfig.BaseUrlB2B + "rest/V1/products/attributes/", UtilsWebservices.SearchCriteria("attribute_code", gamme.Intitule2, "eq")));
                            CreateDoubleGammeProduct(article, gamme, searchatt, searchat1, first);
                        }
                    }
                }
                else
                {
                    if (UtilsWebservices.CreateAttribute(gamme.Intitule) != null)
                    {
                        var searchat = AttributeSearchCriteria.FromJson(UtilsWebservices.SearchAttribute(UtilsConfig.BaseUrlB2B + "rest/V1/products/attributes/", UtilsWebservices.SearchCriteria("attribute_code", gamme.Intitule, "eq")));
                        var searchatt1 = AttributeSearchCriteria.FromJson(UtilsWebservices.SearchAttribute(UtilsConfig.BaseUrlB2B + "rest/V1/products/attributes/", UtilsWebservices.SearchCriteria("attribute_code", gamme.Intitule2, "eq")));
                        if (searchatt1.TotalCount > 0)
                        {
                            CreateDoubleGammeProduct(article, gamme, searchat, searchatt1, first);
                        }
                        else
                        {
                            if (UtilsWebservices.CreateAttribute(gamme.Intitule2) != null)
                            {
                                var searchat1 = AttributeSearchCriteria.FromJson(UtilsWebservices.SearchAttribute(UtilsConfig.BaseUrlB2B + "rest/V1/products/attributes/", UtilsWebservices.SearchCriteria("attribute_code", gamme.Intitule2, "eq")));
                                CreateDoubleGammeProduct(article, gamme, searchat, searchat1, first);
                            }
                        }
                    }
                }
            }
            return "good";
        }
        public static Boolean CreateDoubleGammeProduct(Article article, Gamme gamme, AttributeSearchCriteria searchatt, AttributeSearchCriteria searchatt1, bool first )
        {
            string value_index = "";
            value_index = UtilsWebservices.SearchOption(gamme.Value_Intitule, searchatt);
            if (value_index.Length == 0)
            {
                value_index = UtilsWebservices.CreateAttributeOption(gamme.Intitule, gamme.Value_Intitule);
            }
            string value_index1 = "";
            value_index1 = UtilsWebservices.SearchOption(gamme.Value_Intitule2, searchatt1);
            if (value_index1.Length == 0)
            {
                value_index1 = UtilsWebservices.CreateAttributeOption(gamme.Intitule2, gamme.Value_Intitule2);
            }
            Product productGamme = new Product();
            var jsonGamme = JsonConvert.SerializeObject(productGamme.SimpleProductjson(article, gamme, value_index,value_index1));
            string responseGamme = UtilsWebservices.SendDataJson(jsonGamme, "rest/all/V1/products");
            if (!first)
            {
                string responseOption = UtilsWebservices.CreateOption(article.Reference, searchatt.Items[0].AttributeCode, true, value_index, searchatt.Items[0].AttributeId.ToString());
                string responseOption1 = UtilsWebservices.CreateOption(article.Reference, searchatt1.Items[0].AttributeCode, true, value_index1, searchatt1.Items[0].AttributeId.ToString(),1);
                UtilsWebservices.CreateChildSKU(article.Reference, gamme.Reference);
                first = true;
            }
            else
            {
                UtilsWebservices.CreateChildSKU(article.Reference, gamme.Reference);
            }
            return first;
        }
        public static String SearchCriteria(String FieldName, String FieldValue, String ConditionType)
        {
            StringBuilder SearchCriteria = new StringBuilder();
            SearchCriteria.Append("?searchCriteria[filter_groups][0][filters][0][field]=");
            SearchCriteria.Append(FieldName.ToString());
            SearchCriteria.Append("&searchCriteria[filter_groups][0][filters][0][value]=");
            SearchCriteria.Append(FieldValue.ToString());
            SearchCriteria.Append("&searchCriteria[filter_groups][0][filters][0][condition_type]=");
            SearchCriteria.Append(ConditionType.ToString());
            return SearchCriteria.ToString();
        }
        public static String SearchOrderCriteria(String FieldName, String FieldValue, String ConditionType)
        {
            StringBuilder SearchCriteria = new StringBuilder();
            SearchCriteria.Append("?searchCriteria[filter_groups][2][filters][0][field]=");
            SearchCriteria.Append(FieldName.ToString());
            SearchCriteria.Append("&searchCriteria[filter_groups][2][filters][0][value]=");
            SearchCriteria.Append(FieldValue.ToString());
            SearchCriteria.Append("&searchCriteria[filter_groups][2][filters][0][condition_type]=");
            SearchCriteria.Append(ConditionType.ToString());
            return SearchCriteria.ToString();
        }
        public static String SearchCategoriesCriteria(String FieldName, String FieldValue, String ConditionType)
        {
            StringBuilder SearchCriteria = new StringBuilder();
            SearchCriteria.Append("?searchCriteria[filterGroups][0][filters][0][field]=");
            SearchCriteria.Append(FieldName.ToString());
            SearchCriteria.Append("&searchCriteria[filterGroups][0][filters][0][value]=");
            SearchCriteria.Append(FieldValue.ToString());
            SearchCriteria.Append("&searchCriteria[filterGroups][0][filters][0][conditionType]=");
            SearchCriteria.Append(ConditionType.ToString());
            return SearchCriteria.ToString();
        }
        public static string SearchOption(String gammeName, AttributeSearchCriteria searchResult)
        {
            string value_index = "";
            foreach (Option option in searchResult.Items[0].Options)
            {
                if (option.Label.ToString().Equals(gammeName))
                {
                    value_index = option.Value.ToString();
                    break;
                }
            }
            return value_index;
        }
        public static string CreateAttributeOption(string gammeInitule, string gammeValueIntitule)
        {
            string postURL =UtilsConfig.BaseUrlB2B +  @"rest/V1/products/attributes/" + gammeInitule + "/options";
            var request = (HttpWebRequest)WebRequest.Create(postURL);
            var option = new
            {
                option = new
                {
                    label = gammeValueIntitule,
                    value = ""
                }
            };

            var jsonreq = JsonConvert.SerializeObject(option);
            request.Method = "POST";
            request.PreAuthenticate = true;
            request.ContentType = "application/json; charset=utf-8";
            request.Headers.Add("Authorization", "Bearer " + UtilsConfig.Token.ToString());
            using (var stream = new StreamWriter(request.GetRequestStream()))
            {
                stream.Write(jsonreq);
            }
            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                responseString.Remove(responseString.Length - 1, 1);
                responseString.Remove(0, responseString.IndexOf("_") + 1);
                return responseString;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                return null;
            }
        }
        public static string CreateOption(string ConfProdSku, string label, bool useDefault , string value_index, string attribute_id, int position = 0)
        {
            var value_indexstring = new
            {
                value_index = int.Parse(value_index)
            };
            List<object> value_indexlist = new List<object>();
            value_indexlist.Add(value_indexstring);
            string postURL = UtilsConfig.BaseUrlB2B + @"/rest/all/V1/configurable-products/" + ConfProdSku + "/options";
            var request = (HttpWebRequest)WebRequest.Create(postURL);
            var option = new
            {
                option = new
                {
                    attribute_id = attribute_id,
                    label = label,
                    position = position,
                    is_use_default = useDefault,
                    values = value_indexlist
                }
            };

            var jsonreq = JsonConvert.SerializeObject(option);
            request.Method = "POST";
            request.PreAuthenticate = true;
            request.ContentType = "application/json; charset=utf-8";
            request.Headers.Add("Authorization", "Bearer " + UtilsConfig.Token.ToString());
            using (var stream = new StreamWriter(request.GetRequestStream()))
            {
                stream.Write(jsonreq);
            }
            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                return responseString;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                return null;
            }
        }
        public static string CreateChildSKU(string ConfProdSku, string ChildProdSku = null)
        {
            string postURL = UtilsConfig.BaseUrlB2B +@"/rest/all/V1/configurable-products/" + ConfProdSku+"/child";
            var request = (HttpWebRequest)WebRequest.Create(postURL);
            var json = new { childSku = ChildProdSku };
            var jsonreq = JsonConvert.SerializeObject(json);
            request.Method = "POST";
            request.PreAuthenticate = true;
            request.ContentType = "application/json; charset=utf-8";
            request.Headers.Add("Authorization", "Bearer " + UtilsConfig.Token.ToString());
            using (var stream = new StreamWriter(request.GetRequestStream()))
            {
                stream.Write(jsonreq);
            }
            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                return responseString;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                return null;
            }
        }
        public static string SendDataJson(string json, string url = null,string methode = "POST")
        {
            string postURL = UtilsConfig.BaseUrlB2B + url;
            var request = (HttpWebRequest)WebRequest.Create(postURL);
            request.Method = methode;
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", "Bearer " + UtilsConfig.Token.ToString());
            request.PreAuthenticate = true;
            if (!String.IsNullOrEmpty(json))
            {
                using (var stream = new StreamWriter(request.GetRequestStream()))
                {
                    stream.Write(json);
                }
            }
            try
            {
                 var response = (HttpWebResponse)request.GetResponse();
                 var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                 return responseString;
            }
            catch (Exception s)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(DateTime.Now + s.Message + Environment.NewLine);
                sb.Append(DateTime.Now + s.StackTrace + Environment.NewLine);
                sb.Append(DateTime.Now + " " + json.ToString() + Environment.NewLine);
                sb.Append(DateTime.Now + " " + postURL.ToString() + Environment.NewLine);
                sb.Append(DateTime.Now + " " + methode.ToString() + Environment.NewLine);
                //UtilsMail.SendErrorMail(DateTime.Now + s.Message + Environment.NewLine + s.StackTrace + Environment.NewLine, "COMMANDE");
                File.AppendAllText("Log\\SendDataJson.txt", sb.ToString());
                sb.Clear();
                return null;
            }
        }
        public static string SendDataJsonB2C(string json, string url = null, string methode = "POST")
        {
            string postURL = UtilsConfig.BaseUrlB2C + url;
            var request = (HttpWebRequest)WebRequest.Create(postURL);
            request.Method = methode;
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", "Bearer " + UtilsConfig.TokenB2C.ToString());
            request.PreAuthenticate = true;
            if (!String.IsNullOrEmpty(json))
            {
                using (var stream = new StreamWriter(request.GetRequestStream()))
                {
                    stream.Write(json);
                }
            }
            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                return responseString;
            }
            catch (Exception s)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(DateTime.Now + s.Message + Environment.NewLine);
                sb.Append(DateTime.Now + s.StackTrace + Environment.NewLine);
                sb.Append(DateTime.Now + " " + json.ToString() + Environment.NewLine);
                sb.Append(DateTime.Now + " " + postURL.ToString() + Environment.NewLine);
                sb.Append(DateTime.Now + " " + methode.ToString() + Environment.NewLine);
                //UtilsMail.SendErrorMail(DateTime.Now + s.Message + Environment.NewLine + s.StackTrace + Environment.NewLine, "COMMANDE");
                File.AppendAllText("Log\\SendDataJsonB2C.txt", sb.ToString());
                sb.Clear();
                return null;
            }
        }
        public static string GetMagentoData(string url = null, string methode = "GET")
        {
            string postURL = UtilsConfig.BaseUrlB2B + url;
            var request = (HttpWebRequest)WebRequest.Create(@postURL);
            request.Method = methode;
            request.PreAuthenticate = true;
            request.ContentType = "application/json; charset=utf-8";
            request.Headers.Add("Authorization", "Bearer " + UtilsConfig.Token.ToString());
            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                return responseString;
            }
            catch (Exception s)
            {
                
                return null;
            }
        }
        public static ProductSearchCriteria GetMagentoProduct(string url = null, string searchcriteria=null)
        {
            string postURL = UtilsConfig.BaseUrlB2B + url+ searchcriteria;
            //postURL = postURL.Replace("//", "/");
            var request = (HttpWebRequest)WebRequest.Create(postURL);
            request.Method = "GET";
            request.PreAuthenticate = true;
            request.ContentType = "application/json; charset=utf-8";
            request.Headers.Add("Authorization", "Bearer " + UtilsConfig.Token.ToString());
            
            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
               
                ProductSearchCriteria product = ProductSearchCriteria.FromJson(responseString);
                return product;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                return null;
            }
        }
        public static string SearchAttribute(string postURL ,string searchCriteria)
        {
            string URL = postURL + searchCriteria;
            var request = (HttpWebRequest)WebRequest.Create(URL);
            string html;
            request.Method = "GET";
            request.PreAuthenticate = true;
            request.ContentType = "application/json; charset=utf-8";
            request.Headers.Add("Authorization", "Bearer " + UtilsConfig.Token.ToString());
            try
            {

                 using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                 using (Stream stream = response.GetResponseStream())
                 using (StreamReader reader = new StreamReader(stream))
                 {
                     html = reader.ReadToEnd();
                 }

                 return html;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                return null;
            }
        }
        public static CustomerSearch GetClientCtNum(string customerId,string site="B2B")
        {
            string URL = "";
            if (site.Equals("B2C"))
            {
                URL = UtilsConfig.BaseUrlB2C + @"rest/all/V1/customers/" + customerId;
            }
            else
            {
                URL = UtilsConfig.BaseUrlB2B + @"rest/all/V1/customers/" + customerId;
            }
            
            var request = (HttpWebRequest)WebRequest.Create(URL);
            string html;
            request.PreAuthenticate = true;
            request.Method = "GET";
            request.ContentType = "application/json; charset=utf-8";
            if (site.Equals("B2C"))
            {
                request.Headers.Add("Authorization", "Bearer " + UtilsConfig.TokenB2C.ToString());
            }
            else
            {
                request.Headers.Add("Authorization", "Bearer " + UtilsConfig.Token.ToString());
            }
            

            try
            {

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    html = reader.ReadToEnd();
                }
                CustomerSearch customerSearch = CustomerSearch.FromJson(html);
                return customerSearch;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                return null;
            }
        }
        public static string SearchOrder(string postURL, string searchCriteria,string site ="B2B")
        {
            string URL = @postURL + @searchCriteria;
            var request = (HttpWebRequest)WebRequest.Create(@URL);
            string html;
            request.PreAuthenticate = true;
            request.Method = "GET";
            request.ContentType = "application/json; charset=utf-8";
            if (site.Equals("B2C"))
            {
                request.Headers.Add("Authorization", "Bearer " + UtilsConfig.TokenB2C.ToString());
            }
            else
            {
                request.Headers.Add("Authorization", "Bearer " + UtilsConfig.Token.ToString());
            }
            
            try
            {

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    html = reader.ReadToEnd();
                }
                
                return html;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                return null;
            }
        }
        public static string SendData(string url, string xmlData)
        {
            Console.WriteLine(url);
            //System.Threading.Thread.Sleep(2000);
            string xml = xmlData;
            xml = xml.Replace(@"&", "mpa-_-");

            //Console.WriteLine(xml);

            var request = (HttpWebRequest)WebRequest.Create(url);
            var data = Encoding.UTF8.GetBytes("xml=" + xml);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                return responseString;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                return null;
            }

        }

        public static string SendDataNoParse(string url, string xmlData)
        {
            Console.WriteLine(url);
            //System.Threading.Thread.Sleep(2000);
            string xml = xmlData;
            //Console.WriteLine(xml);

            var request = (HttpWebRequest)WebRequest.Create(url);
            var data = Encoding.UTF8.GetBytes("xml=" + xml);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            return responseString;
        }
        public static string UpdateOrderFlag(string orderId, string value)
        {
            string postURL = UtilsConfig.BaseUrlB2B + @"/order.php";
            var request = (HttpWebRequest)WebRequest.Create(postURL);
            var data = Encoding.UTF8.GetBytes("xml=" + value + "-_-" + orderId);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            //request.Headers.Add("Authorization", "Bearer " + UtilsConfig.Token.ToString());
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                return responseString;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                return "error";
            }
            
        }

    }
}
