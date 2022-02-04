using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebservicesSage.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections;
using System.Data.SqlClient;
using System.IO;

namespace WebservicesSage.Object
{
    class Product
    {

        public List<long> website_ids { get; set; }
        public int status { get; set; }
        public Double price { get; set; }
        public int visibility { get; set; }
        public string type_id { get; set; }
        public string sku { get; set; }
        public string name { get; set; }
        public bool is_in_stock { get; set; }
        public double stock { get; set; }
        public List<CustomAttribute> CustomAttributes { get; set; }
        public Product()
        {

        }
        public partial class CustomAttribute
        {
            [JsonProperty("attribute_code")]
            public string AttributeCode { get; set; }

            [JsonProperty("value")]
            public string Value { get; set; }
        }
        public object GroupedProduct(Article article, Conditionnement conditionnement, ProductSearchCriteria productMagento = null)
        {
            if (productMagento.TotalCount > 0)
            {
                status = 2;
                name = productMagento.Items[0].Name.ToString();
            }
            else
            {
                name = article.Designation + conditionnement.Enumere;
            }
            var product = new
            {
                product = new
                {
                    sku = conditionnement.Reference.ToString(),
                    name = name,
                    attribute_set_id = int.Parse(UtilsConfig.Attribute_set_id.ToString()),
                    type_id = "grouped",
                    status = status,
                    visibility = 4
                }
            };
            return product;
        }
        public object SimpleGroupedProduct(Article article, Conditionnement conditionnement)
        {

            var items = new
            {
                entity = new
                {
                    sku = article.Reference.ToString(),
                    link_type = "associated",
                    linked_product_sku = conditionnement.Reference.ToString(),
                    linked_product_type = "simple",
                    position = 0,
                    extension_attributes = new
                    {
                        qty = conditionnement.Quantity
                    }
                }
            };
            return items;
        }
        public object PrixCatTarif(Article article, PrixCatTarif prixCatTarif)
        {
            var prices = new
            {
                prices = new
                {
                    price = prixCatTarif.Price,
                    price_type = "fixed",
                    website_id = 0,
                    sku = article.Reference,
                    quantity = 1,
                    customer_groupe = prixCatTarif.CategorieTarifaire
                }
            };
            return prices;
        }
        public object PrixCustomGroupe(SpecialPriceClient specialPriceClient)
        {
            ArrayList price = new ArrayList();
            double priceF=specialPriceClient.price;
            if (specialPriceClient.remiseClient > 0)
            {
                priceF = priceF - (priceF * (specialPriceClient.remiseClient / 100));
            }
            if (specialPriceClient.remiseFamille>0)
            {
                priceF = priceF - (priceF * (specialPriceClient.remiseFamille / 100));
            }
            

            var prices = new
            {
                price = (Decimal)Math.Round(priceF, 2),
                price_type = "fixed",
                website_id = 0,
                sku = specialPriceClient.articleReference,
                quantity = 1,
                customer_group = specialPriceClient.groupeName
            };
            price.Add(prices);
            var priceRemise = new
            {
                prices = price
            };
            return priceRemise;
        }
        public ArrayList PrixAllCustomGroupe(List<SpecialPriceClient> specialPriceClient)
        {
            ArrayList price = new ArrayList();
            int count = 0;
            int nbr = 0;
            ArrayList FinalPrice = new ArrayList();
            foreach (SpecialPriceClient item in specialPriceClient)
            {
                count++;
                nbr++;
                double priceF = item.price;
                if (item.remiseClient > 0)
                {
                    priceF = priceF - (priceF * (item.remiseClient / 100));
                }
                if (item.remiseFamille > 0)
                {
                    priceF = priceF - (priceF * (item.remiseFamille / 100));
                }
                var prices = new
                {
                    price = (Decimal)Math.Round(priceF, 2),
                    price_type = "fixed",
                    website_id = 0,
                    sku = item.articleReference,
                    quantity = 1,
                    customer_group = item.groupeName
                };
                var pricesBtoB = new
                {
                    price = item.price,
                    price_type = "fixed",
                    website_id = 0,
                    sku = item.articleReference,
                    quantity = 1,
                    customer_group = "PrixBaseBtoB"
                };
                price.Add(prices);
                price.Add(pricesBtoB);


                if (count == 500 || nbr == specialPriceClient.Count())
                {
                    var priceRemise = new
                    {
                        prices = price

                    };
                    FinalPrice.Add(priceRemise);
                    price = new ArrayList();
                    count = 0;
                }
            }
            return FinalPrice;
        }
        #region old function 
        /*public object PrixAllCustomGroupe(List<SpecialPriceClient> specialPriceClient)
        {
            ArrayList price = new ArrayList();
            foreach (SpecialPriceClient item in specialPriceClient)
            {
                double priceF = item.price;
                if (item.remiseClient > 0)
                {
                    priceF = priceF - (priceF * (item.remiseClient / 100));
                }
                if (item.remiseFamille > 0)
                {
                    priceF = priceF - (priceF * (item.remiseFamille / 100));
                }


                var prices = new
                {
                    price = (Decimal)Math.Round(priceF, 2),
                    price_type = "fixed",
                    website_id = 0,
                    sku = item.articleReference,
                    quantity = 1,
                    customer_group = item.groupeName
                };
                price.Add(prices);
            }
            
            var priceRemise = new
            {
                prices = price
            };
            return priceRemise;
        }*/
        #endregion
        public object PrixRemise(Article article, PrixRemise prixRemise)
        {
            if (prixRemise.reduction_type.Equals("amount"))
            {
                ArrayList price = new ArrayList();
                var prices = new
                {
                    price = prixRemise.Price,
                    price_type = "fixed",
                    website_id = 0,
                    sku = article.Reference,
                    quantity = prixRemise.Born_Sup,
                    customer_group = prixRemise.CategorieTarifaire
                };
                price.Add(prices);
                var priceRemise = new
                {
                    prices = price
                };
                return priceRemise;
            }
            else
            {
                ArrayList price = new ArrayList();
                var prices = new
                {
                    prices = new
                    {
                        price = prixRemise.RemisePercentage * 100,
                        price_type = "discount",
                        website_id = 0,
                        sku = article.Reference,
                        quantity = prixRemise.Born_Sup,
                        customer_group = prixRemise.CategorieTarifaire
                    }
                };
                price.Add(prices);
                var priceRemise = new
                {
                    prices = price
                };
                return priceRemise;
            }
        }
        public List<object> DeletePrixRemiseClientAllProducts(Client client, SqlDataReader AllProduct)
        {
            ArrayList price = new ArrayList();
            List<object> todelete = new List<object>();
            while (AllProduct.Read())
            {

                Boolean found = false;
                foreach (SpecialPriceClient specialPriceClient in client.SpecialPrices)
                {
                    if (specialPriceClient.articleReference.Equals(AllProduct.GetValue(0).ToString()))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    double priceF = Double.Parse(AllProduct.GetValue(1).ToString());
                    priceF = priceF - (priceF * (client.RemiseGlobal / 100));
                    var prices = new
                    {
                        price = (Decimal)Math.Round(priceF, 2),
                        price_type = "fixed",
                        website_id = 0,
                        sku = AllProduct.GetValue(0),
                        quantity = 1,
                        customer_group = client.Central
                    };
                    price.Add(prices);
                }
            }
            int count = 0;

            ArrayList list = new ArrayList();
            foreach (var item in price)
            {
                list.Add(item);
                if (count == 1000)
                {

                    var x = list.Clone();
                    var prixremise = new
                    {
                        prices = x
                    };
                    todelete.Add(prixremise);
                    list.Clear();
                    count = -1;
                }
                count++;
            }
            var x1 = list.Clone();
            var prixremise1 = new
            {
                prices = x1
            };
            todelete.Add(prixremise1);

            return todelete;
        }

        public object PrixRemiseClientAllProducts(Client client , SqlDataReader AllProduct)
        {
            ArrayList price = new ArrayList();
            while (AllProduct.Read())
            {
                Boolean found = false;
                foreach (SpecialPriceClient specialPriceClient in client.SpecialPrices)
                {
                    if (specialPriceClient.articleReference.Equals(AllProduct.GetValue(0).ToString()))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    double priceF = Double.Parse(AllProduct.GetValue(1).ToString());
                    priceF = priceF - (priceF * (client.RemiseGlobal / 100));
                    var prices = new
                    {
                        price = (Decimal)Math.Round(priceF, 2),
                        price_type = "fixed",
                        website_id = 0,
                        sku = AllProduct.GetValue(0),
                        quantity = 1,
                        customer_group = client.Central
                    };
                    price.Add(prices);
                }
            }
            var priceRemise = new
            {
                prices = price
            };
            return priceRemise;
        }
        public ArrayList PrixRemiseBtoC(SqlDataReader AllProduct)
        {
            ArrayList price = new ArrayList();
            
            while (AllProduct.Read())
            {
                double priceF = Double.Parse(AllProduct.GetValue(1).ToString());
                if (priceF >0)
                {
                    var prices = new
                    {
                        price = (Decimal)Math.Round(priceF, 2),
                        price_type = "fixed",
                        website_id = 0,
                        sku = AllProduct.GetValue(0),
                        quantity = 1,
                        customer_group = UtilsConfig.BtoCGroupClient.ToString()
                    };
                    price.Add(prices);
                    var prices1 = new
                    {
                        price = (Decimal)Math.Round(priceF, 2),
                        price_type = "fixed",
                        website_id = 0,
                        sku = AllProduct.GetValue(0),
                        quantity = 1,
                        customer_group = "NOT LOGGED IN"
                    };
                    price.Add(prices1);
                }
            }
            int count = 0;
            ArrayList list = new ArrayList();
            ArrayList PriceBtoCSplited = new ArrayList();
            foreach (var item in price)
            {
                list.Add(item);
                if (count == 1000)
                {

                    var x = list.Clone();
                    var prixremise = new
                    {
                        prices = x
                    };
                    PriceBtoCSplited.Add(prixremise);
                    list.Clear();
                    count = -1;
                }
                count++;
            }
            var x1 = list.Clone();
            var prixremise1 = new
            {
                prices = x1
            };
            PriceBtoCSplited.Add(prixremise1);
            return PriceBtoCSplited;
        }

        public object PrixRemiseClient(double prixRemise,string reference,string groupe, double ArticlePrice)
        {
                ArrayList price = new ArrayList();
                double priceF = ArticlePrice;
                priceF = priceF - (priceF * (prixRemise / 100));
                var prices = new
                {
                    price = (Decimal)Math.Round(priceF, 2),
                    price_type = "fixed",
                    website_id = 0,
                    sku = reference,
                    quantity = 1,
                    customer_group = groupe
                };
                price.Add(prices);
                var priceRemise = new
                {
                    prices = price
                };
                return priceRemise;
        }
        public object SimpleProductjson(Article article, Gamme gamme = null, string value_index = null, string value_index2 = null, ProductSearchCriteria productMagento=null)
        {
            try
            {
                is_in_stock = false;
            
            CustomAttribute custom_attribute = new CustomAttribute();
            CustomAttribute custom_attribute1 = new CustomAttribute();
            CustomAttributes = new List<CustomAttribute>();

            website_ids = new List<long>();
            
            //website_ids.Add(int.Parse(UtilsConfig.StoreBtoC.ToString()));
            if (gamme != null)
            {
                custom_attribute.AttributeCode = gamme.Intitule;
                custom_attribute.Value = value_index;
                CustomAttributes.Add(custom_attribute);
                if (gamme.Reference != null)
                {
                    sku = gamme.Reference;
                    if (article.IsDoubleGamme)
                    {
                        custom_attribute1.AttributeCode = gamme.Intitule2;
                        custom_attribute1.Value = value_index2;
                        CustomAttributes.Add(custom_attribute1);
                    }
                }
                else
                {
                    if (article.IsDoubleGamme)
                    {
                        sku = gamme.Value_Intitule + gamme.Value_Intitule2;
                        custom_attribute1.AttributeCode = gamme.Intitule2;
                        custom_attribute1.Value = value_index2;
                        CustomAttributes.Add(custom_attribute1);
                    }
                    else
                    {
                        sku = gamme.Value_Intitule;
                    }
                }
                
                price = gamme.Price;
                stock = gamme.Stock;
                if (article.IsDoubleGamme)
                {
                    name = article.Designation + " " + gamme.Value_Intitule + " " + gamme.Value_Intitule2;
                }
                else
                {
                    name = article.Designation + " " + gamme.Value_Intitule;
                }
                if (gamme.Sommeil)
                {
                    status = 2;
                }
                else
                {
                    if (gamme.Stock > 0)
                    {
                        status = 1;
                        is_in_stock = true;
                    }
                }

            }
            else
            {
                if (article.Sommeil)
                {
                    status = 2;
                }
                else
                {
                    status = 1;
                }
                if (article.Stock > 0)
                {
                    is_in_stock = true;
                }
                else
                {
                    is_in_stock = false;
                }
                sku = article.Reference;

                    price = article.PrixVente;
                
                
                
                stock = article.Stock;
                name = article.Designation;
            }
            if (!String.IsNullOrEmpty(article.Ecotaxe))
            {

                var value = new
                {
                    website_id = int.Parse(UtilsConfig.StoreBtoB.ToString()),
                    country = "FR",
                    state = 0,
                    value = Double.Parse(article.Ecotaxe),
                    website_value = Double.Parse(article.Ecotaxe)
                };
                CustomAttribute ecotax = new CustomAttribute();
                ecotax.AttributeCode = "fpt_tax";
                ecotax.Value = value.ToString();
                CustomAttributes.Add(ecotax);
            }
            if (productMagento.TotalCount > 0)
            {
                status = int.Parse(productMagento.Items[0].Status.ToString());
                name = productMagento.Items[0].Name.ToString();
                website_ids = productMagento.Items[0].ExtensionAttributes.WebsiteIds;
            }
            else
            {
                status =2;
                website_ids.Add(int.Parse(UtilsConfig.StoreBtoB.ToString()));
            }
            

            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(DateTime.Now + e.Message + Environment.NewLine);
                sb.Append(DateTime.Now + e.StackTrace + Environment.NewLine);
                File.AppendAllText("Log\\errorsP.txt", sb.ToString());
                sb.Clear();
            }
            File.AppendAllText("Log\\err.txt", "worked");
            var product = new
            {
                product = new
                {
                    name = name,
                    sku = sku,
                    price = price,
                    status = status,
                    attribute_set_id = int.Parse(UtilsConfig.Attribute_set_id.ToString()),
                    visibility = 4,
                    type_id = "simple",
                    weight = article.Poid,
                    extension_attributes = new
                    {
                        website_ids = website_ids,


                        stock_item = new
                        {
                            qty = stock,
                            //min_qty get value from infolibre
                            min_qty = 1,
                            is_in_stock = is_in_stock
                        }
                    },
                    custom_attributes = CustomAttributes
                    /*new
                    {
                        attribute_code = custom_attribute.AttributeCode.ToString(),
                        value = custom_attribute.Value.ToString()
                    }*/
    },
                    saveOptions = true

            };
            return product;
        }
        public object BundleProductjson(Article article, Gamme gamme = null, string value_index = null, string value_index2 = null, ProductSearchCriteria productMagento=null)
        {
            is_in_stock = false;

            CustomAttribute custom_attribute = new CustomAttribute();
            CustomAttribute custom_attribute1 = new CustomAttribute();
            CustomAttributes = new List<CustomAttribute>();

            website_ids = new List<long>();
            
            //website_ids.Add(int.Parse(UtilsConfig.StoreBtoC.ToString()));
            if (article.Sommeil)
                {
                    status = 2;
                }
                else
                {
                    status = 1;
                }
                if (article.Stock > 0)
                {
                    is_in_stock = true;
                }
                else
                {
                    is_in_stock = false;
                }
                sku = article.Reference;
                price = article.PrixVente;
                stock = article.Stock;
                name = article.Designation;
            CustomAttribute sku_type = new CustomAttribute();
            sku_type.AttributeCode = "sku_type";
            sku_type.Value = "1";
            CustomAttributes.Add(sku_type);

            CustomAttribute price_type = new CustomAttribute();
            price_type.AttributeCode = "price_type";
            price_type.Value = "0";
            CustomAttributes.Add(price_type);

            CustomAttribute price_view = new CustomAttribute();
            sku_type.AttributeCode = "price_view";
            sku_type.Value = "0";
            CustomAttributes.Add(price_view);

            if (!String.IsNullOrEmpty(article.Ecotaxe))
            {

                var value = new
                {
                    website_id = int.Parse(UtilsConfig.StoreBtoB.ToString()),
                    country = "FR",
                    state = 0,
                    value = Double.Parse(article.Ecotaxe),
                    website_value = Double.Parse(article.Ecotaxe)
                };
                CustomAttribute ecotax = new CustomAttribute();
                ecotax.AttributeCode = "fpt_tax";
                ecotax.Value = value.ToString();
                CustomAttributes.Add(ecotax);
            }
            if (productMagento.TotalCount > 0)
            {
                status = int.Parse(productMagento.Items[0].Status.ToString());
                name = productMagento.Items[0].Name.ToString();
                website_ids = productMagento.Items[0].ExtensionAttributes.WebsiteIds;
            }
            else
            {
                status = 2;
                website_ids.Add(int.Parse(UtilsConfig.StoreBtoB.ToString()));
            }
            var product = new
            {
                product = new
                {
                    name = name,
                    sku = sku,
                    price = price,
                    status = status,
                    attribute_set_id = int.Parse(UtilsConfig.Attribute_set_id.ToString()),
                    visibility = 4,
                    type_id = "bundle",
                    weight = article.Poid,
                    extension_attributes = new
                    {
                        website_ids = website_ids,


                        stock_item = new
                        {
                            qty = 0,
                            //min_qty get value from infolibre
                            min_qty = 1,
                            is_in_stock = true
                        }
                    },
                    custom_attributes = CustomAttributes
                    /*new
                    {
                        attribute_code = custom_attribute.AttributeCode.ToString(),
                        value = custom_attribute.Value.ToString()
                    }*/
                },
                saveOptions = true

            };
            return product;
        }
        public object ConfigurableProductjson(Article article, ProductSearchCriteria productMagento = null)
        {

            website_ids = new List<long>();

            if (article.Sommeil)
            {
                status = 2;
            }
            else
            {
                status = 1;
            }
            if (productMagento.TotalCount > 0)
            {
                status = int.Parse(productMagento.Items[0].Status.ToString());
                name = productMagento.Items[0].Name.ToString();
                website_ids = productMagento.Items[0].ExtensionAttributes.WebsiteIds;
            }
            else
            {
                status = 2;
                name = article.Designation;
                website_ids.Add(int.Parse(UtilsConfig.StoreBtoB.ToString()));
            }
            var product = new
            {
                product = new
                {
                    name =name,
                    sku = article.Reference,
                    price = 0,
                    status = status,
                    attribute_set_id = int.Parse(UtilsConfig.Attribute_set_id.ToString()),
                    visibility = 4,
                    type_id = "configurable",
                    weight = article.Poid,
                    extension_attributes = new
                    {
                        website_ids = website_ids,
                        
                        stock_item = new
                        {
                            qty = 0,
                            //min_qty get value from infolibre
                            //min_qty = 1,
                            is_in_stock = true
                        }
                    }
                },
                saveOptions = true

            };
            return product;
        }
        public object CustomProductStock(Article article,Gamme gamme=null, ProductSearchCriteria productMagento = null)
        {
            if (gamme != null)
            {
                if (gamme.Reference != null)
                {
                    sku = gamme.Reference;
                }
                else
                {
                    if (article.IsDoubleGamme)
                    {
                        sku = gamme.Value_Intitule + gamme.Value_Intitule2;
                    }
                    else
                    {
                        sku = gamme.Value_Intitule;
                    }
                }
                stock = 0;
                if (article.IsDoubleGamme)
                {
                    name = article.Designation + " " + gamme.Value_Intitule + " " + gamme.Value_Intitule2;
                }
                else
                {
                    name = article.Designation + " " + gamme.Value_Intitule;
                }
                if (gamme.Sommeil)
                {
                    status = 2;
                }
                else
                {
                    if (gamme.Stock > 0)
                    {
                        stock = gamme.Stock;
                        status = 1;
                        is_in_stock = true;
                    }
                }

            }
            if (productMagento.TotalCount > 0)
            {
                status = int.Parse(productMagento.Items[0].Status.ToString());
                name = productMagento.Items[0].Name.ToString();
            }
            else
            {
                status = 2;
            }
            var CustomProductStock = new
            {
                product = new
                {
                    name = name,
                    sku = sku,
                    status = status,
                    attribute_set_id = int.Parse(UtilsConfig.Attribute_set_id.ToString()),
                    visibility = 4,
                    type_id = "simple",
                    weight = article.Poid,
                    extension_attributes = new
                    {
                        website_ids = website_ids,

                        stock_item = new
                        {
                            qty = stock,
                            is_in_stock = true
                        }
                    }
                },
                saveOptions = true
            };
            return CustomProductStock;
        }
        public object CustomProductPrice(Article article, Gamme gamme = null, ProductSearchCriteria productMagento = null)
        {
            if (gamme != null)
            {
                if (gamme.Reference != null)
                {
                    sku = gamme.Reference;
                }
                else
                {
                    if (article.IsDoubleGamme)
                    {
                        sku = gamme.Value_Intitule + gamme.Value_Intitule2;
                    }
                    else
                    {
                        sku = gamme.Value_Intitule;
                    }
                }
                price = gamme.Price;
                //stock = gamme.Stock;
                if (article.IsDoubleGamme)
                {
                    name = article.Designation + " " + gamme.Value_Intitule + " " + gamme.Value_Intitule2;
                }
                else
                {
                    name = article.Designation + " " + gamme.Value_Intitule;
                }
                if (gamme.Sommeil)
                {
                    status = 2;
                }
                else
                {
                    if (gamme.Stock > 0)
                    {
                        status = 1;
                        is_in_stock = true;
                    }
                }

            }
            if (productMagento.TotalCount > 0)
            {
                status = int.Parse(productMagento.Items[0].Status.ToString());
                name = productMagento.Items[0].Name.ToString();
                website_ids = productMagento.Items[0].ExtensionAttributes.WebsiteIds;
            }
            else
            {
                status = 2;
            }
            var CustomProductPrice = new
            {
                product = new
                {
                    name = name,
                    sku = sku,
                    status = status,
                    price = price,
                    attribute_set_id = int.Parse(UtilsConfig.Attribute_set_id.ToString()),
                    visibility = 4,
                    type_id = "simple",
                    weight = article.Poid,
                    extension_attributes = new
                    {
                        website_ids = website_ids
                    }
                },
                saveOptions = true
            };
            return CustomProductPrice;
        }
    }
}
