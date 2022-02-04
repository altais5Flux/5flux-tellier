using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebservicesSage.Object
{
    public class DB
    {
        public static SqlConnection cnn;
        public DB()
        {
            Connect();
        }
        private static void Connect()
        {
            string connetionString;
            connetionString = @"Data Source=" + ConfigurationManager.AppSettings["SERVER"].ToString() + ";Initial Catalog=" + ConfigurationManager.AppSettings["DBNAME"].ToString() + ";User ID=" + ConfigurationManager.AppSettings["SQLUSER"].ToString() + ";Password=" + ConfigurationManager.AppSettings["SQLPWD"].ToString()+ ";MultipleActiveResultSets=True";
            cnn = new SqlConnection(connetionString);
            cnn.Open();
        }

        public void Disconnect()
        {
            cnn.Close();
        }

        public  SqlDataReader Select(string sql)
        {
            //Connect();

            SqlCommand command = new SqlCommand(sql, cnn);
            SqlDataReader dataReader = command.ExecuteReader();

            //Disconnect();
            return dataReader;
        }
        

    }
}
