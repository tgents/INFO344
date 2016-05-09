using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace SqlApp
{
    /// <summary>
    /// Summary description for SqlService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class SqlService : System.Web.Services.WebService
    {
        /*
         * 
         * REMEMBER TO EDIT WEB CONFIG LOGIN
         * 
         * 
         */
        [WebMethod]
        public List<string> GetAllProducts()
        {
            List<string> products = new List<string>();

            using(SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["dbstring"])){
                SqlCommand command = new SqlCommand("SELECT * FROM eBayProducts;", connection);
                command.Connection.Open();

                using(SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        products.Add(reader["product_name"] + ": " + reader["product_price"]);
                    }
                }
            }
            return products;
        }

        [WebMethod]
        public List<string> GetPurchases(int userid)
        {
            List<string> purchases = new List<string>();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["dbstring"]))
            {
                string query = "SELECT * FROM eBayOrders o JOIN eBayProducts p ON o.product_id = p.product_id JOIN eBayUsers u ON u.id_user = o.id_user WHERE u.id_user = " + userid + ";";
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        purchases.Add(reader["first_name"] + ": " + reader["product_name"]);
                    }
                }
            }
            return purchases;
        }

        [WebMethod]
        public List<string> GetUsers()
        {
            List<string> users = new List<string>();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["dbstring"]))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM eBayUsers;", connection);
                command.Connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(reader["first_name"] + " " + reader["last_name"]);
                    }
                }
            }
            return users;
        }
    }
}
