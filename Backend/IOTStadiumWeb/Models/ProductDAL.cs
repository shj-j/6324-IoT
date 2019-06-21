using System.Collections.Generic;
using System.Data.SqlClient;

namespace IOTStadiumWeb.Models
{
    /// <summary>
    /// This class contains the methods for accessing the data layer and other routines for products
    /// </summary>
    public class ProductDAL
    {
        // provides common utility methods
        private Utility util = new Utility();
        private readonly string defaultConnection = "Server=tcp:unswiot.database.windows.net,1433;Initial Catalog=iot_stadium;Persist Security Info=False;User ID=iot_admin;Password=Mko0(ijn; " +
                                           "MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        /// <summary>
        /// This method retrieves the list of products from the database.
        /// 
        /// Test Case:  Req-7
        /// </summary>
        /// <returns>A list containing all products in the database.</returns>
        public List<Product> GetProductList()
        {
            List<Product> productList = new List<Product>();
             
            using (SqlConnection conn = new SqlConnection(defaultConnection))
            {
                string query = "select p.product_id, p.product_name, p.product_category_id, pc.product_category, p.product_desc, " +
                               "p.product_image, p.cost_price, p.sale_price, p.max_qty_in_basket " +
                               "from product p inner join product_category pc on p.product_category_id = pc.product_category_id";

                using (SqlCommand myCmd = new SqlCommand(query, conn))
                {
                    SqlDataReader dr = null;
                    conn.Open();
                    dr = myCmd.ExecuteReader();

                    while (dr.Read())
                    {
                        Product prod = new Product();
                        if (util.IsNumeric(dr["product_id"].ToString().Trim()))
                            prod.product_id = int.Parse(dr["product_id"].ToString().Trim());
                        prod.product_name = dr["product_name"].ToString().Trim();
                        if (util.IsNumeric(dr["product_category_id"].ToString().Trim()))
                            prod.product_category_id = int.Parse(dr["product_category_id"].ToString().Trim());
                        prod.product_category = dr["product_category"].ToString().Trim();
                        prod.product_desc = dr["product_desc"].ToString().Trim();
                        prod.product_image = dr["product_image"].ToString().Trim();
                        if (util.IsNumeric(dr["cost_price"].ToString().Trim()))
                            prod.cost_price = decimal.Parse(dr["cost_price"].ToString().Trim());
                        if (util.IsNumeric(dr["sale_price"].ToString().Trim()))
                            prod.sale_price = decimal.Parse(dr["sale_price"].ToString().Trim());
                        if (util.IsNumeric(dr["max_qty_in_basket"].ToString().Trim()))
                            prod.max_qty_in_basket = int.Parse(dr["max_qty_in_basket"].ToString().Trim());

                        productList.Add(prod);
                    }
                    return productList;
                }
            }
        }

        /// <summary>
        /// This method retrieves the list of products from the database.
        /// 
        /// Test Case:  Req-7, Req-9
        /// </summary>
        /// <param name="product_id">The product id to get the details for</param>
        /// <returns>The Product object</returns>
        public Product GetProduct(int product_id)
        {          
            util = new Utility();

            using (SqlConnection conn = new SqlConnection(defaultConnection))
            {
                string query = "select p.product_id, p.product_name, p.product_category_id, pc.product_category, p.product_desc, " +
                               "p.product_image, p.cost_price, p.sale_price, p.max_qty_in_basket " +
                               "from product p inner join product_category pc on p.product_category_id = pc.product_category_id " +
                               "where p.product_id = @product_id";

                using (SqlCommand myCmd = new SqlCommand(query, conn))
                {
                    SqlDataReader dr = null;
                    conn.Open();
                    myCmd.Parameters.AddWithValue("@product_id", product_id);
                    dr = myCmd.ExecuteReader();

                    Product prod = new Product();

                    while (dr.Read())
                    {                            
                        if (util.IsNumeric(dr["product_id"].ToString().Trim()))
                            prod.product_id = int.Parse(dr["product_id"].ToString().Trim());
                        prod.product_name = dr["product_name"].ToString().Trim();
                        if (util.IsNumeric(dr["product_category_id"].ToString().Trim()))
                            prod.product_category_id = int.Parse(dr["product_category_id"].ToString().Trim());
                        prod.product_category = dr["product_category"].ToString().Trim();
                        prod.product_desc = dr["product_desc"].ToString().Trim();
                        prod.product_image = dr["product_image"].ToString().Trim();
                        if (util.IsNumeric(dr["cost_price"].ToString().Trim()))
                            prod.cost_price = decimal.Parse(dr["cost_price"].ToString().Trim());
                        if (util.IsNumeric(dr["sale_price"].ToString().Trim()))
                            prod.sale_price = decimal.Parse(dr["sale_price"].ToString().Trim());
                        if (util.IsNumeric(dr["max_qty_in_basket"].ToString().Trim()))
                            prod.max_qty_in_basket = int.Parse(dr["max_qty_in_basket"].ToString().Trim());
                    }
                    return prod;                
                }
            }
        }

        /// <summary>
        /// This method retrieves the sale price of a product from the database.
        /// 
        /// Test Case:  Req-7, Req-8
        /// </summary>
        /// <param name="product_id">The id of the product to get the sale price</param>
        /// <returns>The sale price of the specified product.</returns>
        public decimal GetProductSalePrice(int product_id)
        {
            util = new Utility();

            using (SqlConnection conn = new SqlConnection(defaultConnection))
            {
                string query = "select p.sale_price " +
                               "from product p " +
                               "where p.product_id = @product_id";

                using (SqlCommand myCmd = new SqlCommand(query, conn))
                {
                    decimal sale_price = 0;
                    SqlDataReader dr = null;
                    conn.Open();
                    myCmd.Parameters.AddWithValue("@product_id", product_id);
                    dr = myCmd.ExecuteReader();

                    while (dr.Read())
                    {
                        if (util.IsNumeric(dr["sale_price"].ToString()))
                            sale_price = decimal.Parse(dr["sale_price"].ToString());
                    }

                    return sale_price;
                }
            }
        }
    }
}
