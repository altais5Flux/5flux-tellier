using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;
using System.IO;
using WebservicesSage.Object.DBObject;
using WebservicesSage.Object;
using Objets100cLib;
using WebservicesSage.Singleton;

namespace WebservicesSage.Utils
{
    class UtilsLinkedCommande
    {
        public static void addArticleToLocalDB(Article article)
        {
            // Open database (or create if doesn't exist)
            using (var db = new LiteDatabase(@"SageData.db"))
            {
                // Get a collection (or create, if doesn't exist)
                var ArticleCollection = db.GetCollection<LinkedProductDB>("Article");

                // Create your new customer instance
                var Article = new LinkedProductDB
                {
                    Designation = article.Designation,
                    Reference = article.Reference,
                    Resume = article.Resume,
                    PrixAchat = article.PrixAchat,
                    PrixVente = article.PrixVente,
                    Langue1 = article.Langue1,
                    Langue2 = article.Langue2,
                    CodeBarres = article.CodeBarres,
                    Poid = article.Poid,
                    Sommeil = article.Sommeil,
                    IsPriceTTC = article.IsPriceTTC,
                    isGamme = article.isGamme,
                    IsDoubleGamme = article.IsDoubleGamme,
                    Stock = article.Stock,
                    HaveNomenclature = article.HaveNomenclature

                };
                ArticleCollection.Insert(Article);
                if (article.isGamme)
                {
                    if (article.IsDoubleGamme)
                    {
                        
                        LiteCollection<LinkedProductGammes> GammesCollection = db.GetCollection<LinkedProductGammes>("Gamme");
                        foreach (Gamme ArticleGamme in article.Gammes)
                        {
                            var gamme = new LinkedProductGammes
                            {
                                ArticleReference = article.Reference,
                                CodeBarre = ArticleGamme.CodeBarre,
                                Intitule = ArticleGamme.Intitule,
                                Intitule2 = ArticleGamme.Intitule2,
                                Price = ArticleGamme.Price,
                                Reference = ArticleGamme.Reference,
                                Sommeil = ArticleGamme.Sommeil,
                                Stock = ArticleGamme.Stock,
                                Value_Intitule = ArticleGamme.Value_Intitule,
                                Value_Intitule2 = ArticleGamme.Value_Intitule2
                            };
                            GammesCollection.Insert(gamme);
                        }
                        var PrixGammeCollection = db.GetCollection<LinkedProductPrixGammes>("PrixGamme");
                        foreach (PrixGamme prixGamme in article.prixGammes)
                        {
                            var prixGammesObject = new LinkedProductPrixGammes
                            {
                                ArticleReference = article.Reference,
                                Categori_Intitule = prixGamme.Categori_Intitule,
                                Price = prixGamme.Price,
                                Gamme1_Intitule = prixGamme.Gamme1_Intitule,
                                Gamme1_Value = prixGamme.Gamme1_Value,
                                Gamme2_Intitule = prixGamme.Gamme2_Intitule,
                                Gamme2_Value = prixGamme.Gamme2_Value
                            };
                            PrixGammeCollection.Insert(prixGammesObject);
                        }
                    }
                    else
                    {
                        var GammesCollection = db.GetCollection<LinkedProductGammes>("Gamme");
                        foreach (Gamme ArticleGamme in article.Gammes)
                        {
                            var gamme = new LinkedProductGammes
                            {
                                ArticleReference = article.Reference,
                                CodeBarre = ArticleGamme.CodeBarre,
                                Intitule = ArticleGamme.Intitule,
                                Price = ArticleGamme.Price,
                                Reference = ArticleGamme.Reference,
                                Sommeil = ArticleGamme.Sommeil,
                                Stock = ArticleGamme.Stock,
                                Value_Intitule = ArticleGamme.Value_Intitule,
                            };
                            GammesCollection.Insert(gamme);
                        }
                        var PrixGammeCollection = db.GetCollection<LinkedProductPrixGammes>("PrixGamme");
                        foreach (PrixGamme prixGamme in article.prixGammes)
                        {
                            var prixGammesObject = new LinkedProductPrixGammes
                            {
                                ArticleReference = article.Reference,
                                Categori_Intitule = prixGamme.Categori_Intitule,
                                Price = prixGamme.Price,
                                Gamme1_Intitule = prixGamme.Gamme1_Intitule,
                                Gamme1_Value = prixGamme.Gamme1_Value
                            };
                            PrixGammeCollection.Insert(prixGammesObject);
                        }
                    }

                }

                var InfolibreCollection = db.GetCollection<LinkedProductInfoLibre>("Infolibre");
                foreach (InfoLibre infoLibre in article.infoLibre)
                {
                    var Infolibre = new LinkedProductInfoLibre
                    {
                        Libelle = infoLibre.Libelle,
                        Value = infoLibre.Value,
                        Id_feature = infoLibre.Id_feature,
                        ArticleReference = article.Reference
                    };
                    InfolibreCollection.Insert(Infolibre);
                }

                var PrixRemiseCollection = db.GetCollection<LinkedProductPrixRemises>("PrixRemise");
                foreach (PrixRemise prixRemise in article.prixRemises)
                {
                    var prixRemiseObject = new LinkedProductPrixRemises
                    {
                        ArticleReference = article.Reference,
                        Price = prixRemise.Price,
                        Born_Sup = prixRemise.Born_Sup,
                        CategorieTarifaire = prixRemise.CategorieTarifaire,
                        reduction_type = prixRemise.reduction_type,
                        RemisePercentage = prixRemise.RemisePercentage
                    };
                    PrixRemiseCollection.Insert(prixRemiseObject);
                }
                var PrixRemiseClientCollection = db.GetCollection<LinkedProductPrixRemisesClient>("PrixRemiseClient");
                foreach (PrixRemiseClient prixRemiseClient in article.prixRemisesClient)
                {
                    var PrixRemiseClientObject = new LinkedProductPrixRemisesClient
                    {
                        ArticleReference = article.Reference,
                        Born_Sup = prixRemiseClient.Born_Sup,
                        ClientCtNum = prixRemiseClient.ClientCtNum,
                        Price = prixRemiseClient.Price,
                        reduction_type = prixRemiseClient.reduction_type,
                        RemisePercentage = prixRemiseClient.RemisePercentage
                    };
                    PrixRemiseClientCollection.Insert(PrixRemiseClientObject);
                }
                var CategoriesCollection = db.GetCollection<LinkedProductCategories>("Categories");

            }
        }
        public static Article getArticleFromLocalDB(string reference)
        {
            Article article = new Article();
            List<Gamme> Gammes = new List<Gamme>();
            List<PrixGamme> prixGammes = new List<PrixGamme>();
            List<PrixRemise> prixRemises = new List<PrixRemise>();
            List<PrixRemiseClient> prixRemisesClient = new List<PrixRemiseClient>();
            List<InfoLibre> infoLibre = new List<InfoLibre>();
            List<Category> Categories = new List<Category>();
            using (var db = new LiteDatabase(@"SageData.db"))
            {
                // Get a collection (or create, if doesn't exist)
                var ArticleCollection = db.GetCollection<LinkedProductDB>("Article");
                var PrixRemiseClientCollection = db.GetCollection<LinkedProductPrixRemisesClient>("PrixRemiseClient");
                var PrixRemiseCollection = db.GetCollection<LinkedProductPrixRemises>("PrixRemise");
                var InfolibreCollection = db.GetCollection<LinkedProductInfoLibre>("Infolibre");
                var PrixGammeCollection = db.GetCollection<LinkedProductPrixGammes>("PrixGamme");
                var GammesCollection = db.GetCollection<LinkedProductGammes>("Gamme");
                var CategoryCollection = db.GetCollection<LinkedProductCategories>("Category");
                foreach (LinkedProductDB ProductDB in ArticleCollection.FindAll())
                {
                    if (ProductDB.Reference.Equals(reference))
                    {
                        article.Reference = ProductDB.Reference;
                        article.Resume = ProductDB.Resume;
                        article.Designation = ProductDB.Designation;
                        article.CodeBarres = ProductDB.CodeBarres;
                        article.PrixAchat = ProductDB.PrixAchat;
                        article.PrixVente = ProductDB.PrixVente;
                        article.IsPriceTTC = ProductDB.IsPriceTTC;
                        article.Poid = ProductDB.Poid;
                        article.Langue1 = ProductDB.Langue1;
                        article.Langue2 = ProductDB.Langue2;
                        article.Sommeil = ProductDB.Sommeil;
                        article.Stock = ProductDB.Stock;
                        article.isGamme = ProductDB.isGamme;
                        article.IsDoubleGamme = ProductDB.IsDoubleGamme;
                        article.HaveNomenclature = ProductDB.HaveNomenclature;
                        article.Largeur = ProductDB.Largeur;
                        article.Longueur = ProductDB.Longueur;
                        article.Taille = ProductDB.Taille;
                        //get Gamme and gamme price
                        if (article.isGamme)
                        {
                            if (article.IsDoubleGamme)
                            {
                                foreach (LinkedProductGammes linkedProductGammes in GammesCollection.FindAll())
                                {
                                    if (linkedProductGammes.ArticleReference.Equals(reference))
                                    {
                                        Gamme gamme = new Gamme();
                                        gamme.Intitule = linkedProductGammes.Intitule;
                                        gamme.Intitule2 = linkedProductGammes.Intitule2;
                                        gamme.Price = linkedProductGammes.Price;
                                        gamme.Reference = linkedProductGammes.Reference;
                                        gamme.Sommeil = linkedProductGammes.Sommeil;
                                        gamme.Stock = linkedProductGammes.Stock;
                                        gamme.Value_Intitule = linkedProductGammes.Value_Intitule;
                                        gamme.Value_Intitule2 = linkedProductGammes.Value_Intitule2;
                                        gamme.CodeBarre = linkedProductGammes.CodeBarre;
                                        Gammes.Add(gamme);
                                    }
                                }
                                article.Gammes = Gammes;
                                foreach (LinkedProductPrixGammes linkedProductPrixGammes in PrixGammeCollection.FindAll())
                                {
                                    if (linkedProductPrixGammes.ArticleReference.Equals(reference))
                                    {
                                        PrixGamme prix = new PrixGamme();
                                        prix.Categori_Intitule = linkedProductPrixGammes.Categori_Intitule;
                                        prix.Gamme1_Intitule = linkedProductPrixGammes.Gamme1_Intitule;
                                        prix.Gamme1_Value = linkedProductPrixGammes.Gamme1_Value;
                                        prix.Gamme2_Intitule = linkedProductPrixGammes.Gamme2_Intitule;
                                        prix.Gamme2_Value = linkedProductPrixGammes.Gamme2_Value;
                                        prix.Price = linkedProductPrixGammes.Price;
                                        prixGammes.Add(prix);
                                    }
                                }
                                article.prixGammes = prixGammes;
                            }
                            else
                            {
                                foreach (LinkedProductGammes linkedProductGammes in GammesCollection.FindAll())
                                {
                                    if (linkedProductGammes.ArticleReference.Equals(reference))
                                    {
                                        Gamme gamme = new Gamme();
                                        gamme.Intitule = linkedProductGammes.Intitule;
                                        gamme.Price = linkedProductGammes.Price;
                                        gamme.Reference = linkedProductGammes.Reference;
                                        gamme.Sommeil = linkedProductGammes.Sommeil;
                                        gamme.Stock = linkedProductGammes.Stock;
                                        gamme.Value_Intitule = linkedProductGammes.Value_Intitule;
                                        gamme.CodeBarre = linkedProductGammes.CodeBarre;
                                        Gammes.Add(gamme);

                                    }
                                }
                                article.Gammes = Gammes;
                                foreach (LinkedProductPrixGammes linkedProductPrixGammes in PrixGammeCollection.FindAll())
                                {
                                    if (linkedProductPrixGammes.ArticleReference.Equals(reference))
                                    {
                                        PrixGamme prix = new PrixGamme();
                                        prix.Categori_Intitule = linkedProductPrixGammes.Categori_Intitule;
                                        prix.Gamme1_Intitule = linkedProductPrixGammes.Gamme1_Intitule;
                                        prix.Gamme1_Value = linkedProductPrixGammes.Gamme1_Value;
                                        prix.Price = linkedProductPrixGammes.Price;
                                        prixGammes.Add(prix);
                                    }
                                }
                                article.prixGammes = prixGammes;
                            }
                        }
                        //get infolibre
                        foreach (LinkedProductInfoLibre linkedProductInfoLibre in InfolibreCollection.FindAll())
                        {
                            if (linkedProductInfoLibre.ArticleReference.Equals(reference))
                            {
                                InfoLibre info = new InfoLibre();
                                info.Libelle = linkedProductInfoLibre.Libelle;
                                info.Value = linkedProductInfoLibre.Value;
                                info.Id_feature = linkedProductInfoLibre.Id_feature;
                                infoLibre.Add(info);
                            }
                        }
                        article.infoLibre = infoLibre;
                        //get PrixRemise
                        foreach (LinkedProductPrixRemises linkedProductPrixRemises in PrixRemiseCollection.FindAll())
                        {
                            if (linkedProductPrixRemises.ArticleReference.Equals(reference))
                            {
                                PrixRemise prixR = new PrixRemise();
                                prixR.CategorieTarifaire = linkedProductPrixRemises.CategorieTarifaire;
                                prixR.Born_Sup = linkedProductPrixRemises.Born_Sup;
                                prixR.Price = linkedProductPrixRemises.Price;
                                prixR.reduction_type = linkedProductPrixRemises.reduction_type;
                                prixR.RemisePercentage = linkedProductPrixRemises.RemisePercentage;
                                prixRemises.Add(prixR);
                            }
                        }
                        article.prixRemises = prixRemises;
                        //get prixRemiseClient
                        //todo
                        foreach (LinkedProductPrixRemisesClient linkedProductPrixRemisesClient in PrixRemiseClientCollection.FindAll())
                        {
                            if (linkedProductPrixRemisesClient.ArticleReference.Equals(reference))
                            {
                                PrixRemiseClient prixremiseClient = new PrixRemiseClient();
                                prixremiseClient.Born_Sup = linkedProductPrixRemisesClient.Born_Sup;
                                prixremiseClient.ClientCtNum = linkedProductPrixRemisesClient.ClientCtNum;
                                prixremiseClient.Price = linkedProductPrixRemisesClient.Price;
                                prixremiseClient.reduction_type = linkedProductPrixRemisesClient.reduction_type;
                                prixremiseClient.RemisePercentage = linkedProductPrixRemisesClient.RemisePercentage;
                                prixRemisesClient.Add(prixremiseClient);
                            }
                        }
                        article.prixRemisesClient = prixRemisesClient;
                        foreach (LinkedProductCategories linkedProductCategories in CategoryCollection.FindAll())
                        {
                            if (!String.IsNullOrEmpty(linkedProductCategories.ArticleReference))
                            {
                                if (linkedProductCategories.ArticleReference.Equals(reference))
                                {
                                    Category category = new Category();
                                    category.Category_id = linkedProductCategories.Category_id;
                                    category.Name = linkedProductCategories.Name;
                                    category.Parent_Category_id = linkedProductCategories.Parent_Category_id;
                                    Categories.Add(category);
                                }
                            }
                        }
                        article.Categories = Categories;
                        break;
                    }
                }
            }
            return article;
        }
        public static List<Article> getAllArticleFromLocalDB()
        {
            List<Article> article = new List<Article>();
            using (var db = new LiteDatabase(@"SageData.db"))
            {
                // Get a collection (or create, if doesn't exist)
                var ArticleCollection = db.GetCollection<LinkedProductDB>("Article");

                foreach (LinkedProductDB ProductDB in ArticleCollection.FindAll())
                {
                    Article localArticle = new Article();
                    localArticle = getArticleFromLocalDB(ProductDB.Reference);
                    article.Add(localArticle);
                }
            }
            return article;
        }
        public static void createCategory()
        {
            using (var db = new LiteDatabase(@"SageData.db"))
            {
                var CategoryCollection = db.GetCollection<LinkedProductCategories>("Category");
                var Category = new LinkedProductCategories
                {
                    Category_id = "2",
                    Name = "Accueil",
                    Parent_Category_id = "0",
                    Is_root_category = "1"
                };
                CategoryCollection.Insert(Category);
                var Category3 = new LinkedProductCategories
                {
                    Category_id = "3",
                    Name = "Femmes",
                    Parent_Category_id = "2",
                    Is_root_category = "0"
                };
                CategoryCollection.Insert(Category3);
                var Category4 = new LinkedProductCategories
                {
                    Category_id = "4",
                    Name = "Tops",
                    Parent_Category_id = "3",
                    Is_root_category = "0"
                };
                CategoryCollection.Insert(Category4);
                var Category8 = new LinkedProductCategories
                {
                    Category_id = "8",
                    Name = "Robes",
                    Parent_Category_id = "3",
                    Is_root_category = "0"
                };
                CategoryCollection.Insert(Category8);
                var Category5 = new LinkedProductCategories
                {
                    Category_id = "5",
                    Name = "T-shirts",
                    Parent_Category_id = "4",
                    Is_root_category = "0"
                };
                CategoryCollection.Insert(Category5);
                var Category6 = new LinkedProductCategories
                {
                    Category_id = "6",
                    Name = "Tops",
                    Parent_Category_id = "4",
                    Is_root_category = "0"
                };
                CategoryCollection.Insert(Category6);
                var Category7 = new LinkedProductCategories
                {
                    Category_id = "7",
                    Name = "Chemisiers",
                    Parent_Category_id = "4",
                    Is_root_category = "0"
                };
                CategoryCollection.Insert(Category7);
                
                var Category9 = new LinkedProductCategories
                {
                    Category_id = "9",
                    Name = "Robes décontractées",
                    Parent_Category_id = "8",
                    Is_root_category = "0"
                };
                CategoryCollection.Insert(Category9);
                var Category10 = new LinkedProductCategories
                {
                    Category_id = "10",
                    Name = "Robes de soirée",
                    Parent_Category_id = "8",
                    Is_root_category = "0"
                };
                CategoryCollection.Insert(Category10);
                var Category11 = new LinkedProductCategories
                {
                    Category_id = "11",
                    Name = "Robes d'été",
                    Parent_Category_id = "8",
                    Is_root_category = "0"
                };
                CategoryCollection.Insert(Category11);
            }
        }
        private static Boolean UpdateProductlocalDB(Article article)
        {
            using (var db = new LiteDatabase(@"SageData.db"))
            {
                // Get a collection (or create, if doesn't exist)
                var ArticleCollection = db.GetCollection<LinkedProductDB>("Article");
                foreach (LinkedProductDB ProductDB in ArticleCollection.FindAll())
                {
                    if (ProductDB.Reference.Equals(article.Reference))
                    {
                        ProductDB.CodeBarres = article.CodeBarres;
                        ProductDB.PrixAchat = article.PrixAchat;
                        ProductDB.PrixVente = article.PrixVente ;
                        ProductDB.IsPriceTTC = article.IsPriceTTC ;
                        ProductDB.Poid = article.Poid;
                        ProductDB.Langue1 = article.Langue1 ;
                        ProductDB.Langue2 = article.Langue2 ;
                        ProductDB.Sommeil = article.Sommeil ;
                        ProductDB.Stock = article.Stock ;
                        ProductDB.isGamme = article.isGamme ;
                        ProductDB.IsDoubleGamme = article.IsDoubleGamme ;
                        ProductDB.HaveNomenclature = article.HaveNomenclature ;
                        //get Gamme and gamme price
                        if (article.isGamme)
                        {
                            if (article.IsDoubleGamme)
                            {

                                LiteCollection<LinkedProductGammes> GammesCollection = db.GetCollection<LinkedProductGammes>("Gamme");
                                foreach (Gamme ArticleGamme in article.Gammes)
                                {
                                    var gamme = new LinkedProductGammes
                                    {
                                        ArticleReference = article.Reference,
                                        CodeBarre = ArticleGamme.CodeBarre,
                                        Intitule = ArticleGamme.Intitule,
                                        Intitule2 = ArticleGamme.Intitule2,
                                        Price = ArticleGamme.Price,
                                        Reference = ArticleGamme.Reference,
                                        Sommeil = ArticleGamme.Sommeil,
                                        Stock = ArticleGamme.Stock,
                                        Value_Intitule = ArticleGamme.Value_Intitule,
                                        Value_Intitule2 = ArticleGamme.Value_Intitule2
                                    };
                                    GammesCollection.Insert(gamme);
                                }
                                var PrixGammeCollection = db.GetCollection<LinkedProductPrixGammes>("PrixGamme");
                                foreach (PrixGamme prixGamme in article.prixGammes)
                                {
                                    var prixGammesObject = new LinkedProductPrixGammes
                                    {
                                        ArticleReference = article.Reference,
                                        Categori_Intitule = prixGamme.Categori_Intitule,
                                        Price = prixGamme.Price,
                                        Gamme1_Intitule = prixGamme.Gamme1_Intitule,
                                        Gamme1_Value = prixGamme.Gamme1_Value,
                                        Gamme2_Intitule = prixGamme.Gamme2_Intitule,
                                        Gamme2_Value = prixGamme.Gamme2_Value
                                    };
                                    PrixGammeCollection.Insert(prixGammesObject);
                                }
                            }
                            else
                            {
                                var GammesCollection = db.GetCollection<LinkedProductGammes>("Gamme");
                                foreach (Gamme ArticleGamme in article.Gammes)
                                {
                                    var gamme = new LinkedProductGammes
                                    {
                                        ArticleReference = article.Reference,
                                        CodeBarre = ArticleGamme.CodeBarre,
                                        Intitule = ArticleGamme.Intitule,
                                        Price = ArticleGamme.Price,
                                        Reference = ArticleGamme.Reference,
                                        Sommeil = ArticleGamme.Sommeil,
                                        Stock = ArticleGamme.Stock,
                                        Value_Intitule = ArticleGamme.Value_Intitule,
                                    };
                                    GammesCollection.Insert(gamme);
                                }
                                var PrixGammeCollection = db.GetCollection<LinkedProductPrixGammes>("PrixGamme");
                                foreach (PrixGamme prixGamme in article.prixGammes)
                                {
                                    var prixGammesObject = new LinkedProductPrixGammes
                                    {
                                        ArticleReference = article.Reference,
                                        Categori_Intitule = prixGamme.Categori_Intitule,
                                        Price = prixGamme.Price,
                                        Gamme1_Intitule = prixGamme.Gamme1_Intitule,
                                        Gamme1_Value = prixGamme.Gamme1_Value
                                    };
                                    PrixGammeCollection.Insert(prixGammesObject);
                                }
                            }

                        }

                        var InfolibreCollection = db.GetCollection<LinkedProductInfoLibre>("Infolibre");
                        foreach (InfoLibre infoLibre in article.infoLibre)
                        {
                            var Infolibre = new LinkedProductInfoLibre
                            {
                                Libelle = infoLibre.Libelle,
                                Value = infoLibre.Value,
                                Id_feature = infoLibre.Id_feature,
                                ArticleReference = article.Reference
                            };
                            InfolibreCollection.Insert(Infolibre);
                        }

                        var PrixRemiseCollection = db.GetCollection<LinkedProductPrixRemises>("PrixRemise");
                        foreach (PrixRemise prixRemise in article.prixRemises)
                        {
                            var prixRemiseObject = new LinkedProductPrixRemises
                            {
                                ArticleReference = article.Reference,
                                Price = prixRemise.Price,
                                Born_Sup = prixRemise.Born_Sup,
                                CategorieTarifaire = prixRemise.CategorieTarifaire,
                                reduction_type = prixRemise.reduction_type,
                                RemisePercentage = prixRemise.RemisePercentage
                            };
                            PrixRemiseCollection.Insert(prixRemiseObject);
                        }
                        var PrixRemiseClientCollection = db.GetCollection<LinkedProductPrixRemisesClient>("PrixRemiseClient");
                        foreach (PrixRemiseClient prixRemiseClient in article.prixRemisesClient)
                        {
                            var PrixRemiseClientObject = new LinkedProductPrixRemisesClient
                            {
                                ArticleReference = article.Reference,
                                Born_Sup = prixRemiseClient.Born_Sup,
                                ClientCtNum = prixRemiseClient.ClientCtNum,
                                Price = prixRemiseClient.Price,
                                reduction_type = prixRemiseClient.reduction_type,
                                RemisePercentage = prixRemiseClient.RemisePercentage
                            };
                            PrixRemiseClientCollection.Insert(PrixRemiseClientObject);
                        }
                        var CategoriesCollection = db.GetCollection<LinkedProductCategories>("Categories");
                        return true;
                    }
                }
            }
            return false;
        }

        public static int UpdateLocalDB()
        {
            var gescom = SingletonConnection.Instance.Gescom;
            int updatedArticle = 0;
            //List<Article> articleToProcess = new List<Article>();
            if (!gescom.IsOpen)
            {
                gescom.Open();
            }
            using (var db = new LiteDatabase(@"SageData.db"))
            {
                var articleSageObj = gescom.FactoryArticle.List;
                var PrixRemiseClientCollection = db.GetCollection<LinkedProductPrixRemisesClient>("PrixRemiseClient");
                var PrixRemiseCollection = db.GetCollection<LinkedProductPrixRemises>("PrixRemise").Delete(1 == 1);
                var InfolibreCollection = db.GetCollection<LinkedProductInfoLibre>("Infolibre").Delete(1 == 1);
                var PrixGammeCollection = db.GetCollection<LinkedProductPrixGammes>("PrixGamme").Delete(1 == 1);
                var GammesCollection = db.GetCollection<LinkedProductGammes>("Gamme").Delete(1 == 1);
                foreach (IBOArticle3 articleSage in articleSageObj)
                {
                    //CurrentRefArticle = articleSage.AR_Ref;
                    // on check si l'article est cocher en publier sur le site marchand
                    if (!articleSage.AR_Publie)
                        continue;
                    Article article = new Article(articleSage);
                    if (UpdateProductlocalDB(article))
                    {
                        updatedArticle++;
                    }
                    else
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append(DateTime.Now + "    Erreur synchronisation d'Article : " + article.Reference + Environment.NewLine);
                        File.AppendAllText("Log\\UpdateProductDB.txt", sb.ToString());
                    }
                    //articleToProcess.Add(article);
                }
            }
            return updatedArticle;
        }
    }
}
