using System.Collections.Generic;
using System.Data.SqlClient;

namespace IOTStadiumWeb.Models
{
    /// <summary>
    /// This method contains the methods for accessing the data layer for the basket and basket related tasks
    /// </summary>
    public class BasketDAL
    {
        private readonly string defaultConnection = "Server=tcp:unswiot.database.windows.net,1433;Initial Catalog=iot_stadium;Persist Security Info=False;User ID=iot_admin;Password=Mko0(ijn; " +
                           "MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        /// <summary>
        /// This method determines which baskets contain enough quantity of the product to fulfill the order
        /// 
        /// Test Case:  Req-9
        /// </summary>
        /// <param name="order_id">The order id</param>
        /// <returns>List of KeyValuePairs (basket id, location_cell_id) that contain enough product to fulfill the order</returns>
        public List<KeyValuePair<int,int>> BasketHasStock(int order_id)
        {
            var basketList = new List<KeyValuePair<int, int>>();

            using (SqlConnection conn = new SqlConnection(defaultConnection))
            {
                string query = "; with order_items as (" +
                               "select product_id, qty from cust_order_item where order_id = @order_id), " +
                               "baskets as ( " +
                               "select bp.basket_id, b.location_cell_id, bp.product_id, bp.qty from Basket_Products bp inner join basket b on bp.basket_id = b.basket_id), " +
                               "available as (" +
                               "select b.basket_id, b.location_cell_id, b.product_id, b.qty as basket_qty, o.qty order_qty, " +
                               "case when b.qty >= o.qty then 'Y' else 'N' end as available " +
                               "from baskets b inner join order_items o on b.product_id = o.product_id) " +
                               "select distinct basket_id, location_cell_id from available where basket_id not in (select basket_id from available where available = 'N') ";

                using (SqlCommand myCmd = new SqlCommand(query, conn))
                {
                    SqlDataReader dr = null;
                    myCmd.Parameters.AddWithValue("@order_id", order_id);
                    conn.Open();
                    dr = myCmd.ExecuteReader();

                    while (dr.Read())
                    {                           
                        basketList.Add(new KeyValuePair<int, int>(int.Parse(dr["basket_id"].ToString().Trim()), int.Parse(dr["location_cell_id"].ToString().Trim())));
                    }
                    return basketList;
                }
            }            
        }

        /// <summary>
        /// This method updates the basket current location
        /// 
        /// Test Case:  Req-9
        /// </summary>
        /// <param name="basket_id">the ide for the basket</param>
        /// <param name="beacon_id">the id for the beacon</param>
        public void UpdateBasketLocation(int basket_id, string beacon_id)
        {
            using (SqlConnection conn = new SqlConnection(defaultConnection))
            {
                string query = "update basket set location_cell_id = @beacon_id where basket_id = @basket_id;";

                using (SqlCommand myCmd = new SqlCommand(query, conn))
                {                      
                    myCmd.Parameters.AddWithValue("@basket_id", basket_id);
                    myCmd.Parameters.AddWithValue("@beacon_id", beacon_id);
                    conn.Open();
                    myCmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// This method gets the order item data to update the contents of the basket
        /// 
        /// Test Case:  Req-7, Req-9
        /// </summary>
        /// <param name="basket_id">basket identifier</param>
        /// <param name="order_id">order identifier</param>
        public void UpdateBasketStock(int basket_id, int order_id)
        {
            CustOrderDAL orderDAL = new CustOrderDAL();
            List<CustOrderItem> listOrderItem = new List<CustOrderItem>();

            listOrderItem = orderDAL.GetOrderItems(order_id);

            foreach(CustOrderItem order_item in listOrderItem)
            {
                DeductBasketItem(basket_id, order_item.product_id, order_item.qty);
            }
        }

        /// <summary>
        /// This method deducts the qty of product from the basket by the amount in the accepted order
        /// 
        /// Test Case:  Req-9
        /// </summary>
        /// <param name="basket_id">basket identifier</param>
        /// <param name="product_id">product identifier</param>
        /// <param name="qty">quantity to reduce by</param>
        public void DeductBasketItem(int basket_id, int product_id, int qty)
        {
            using (SqlConnection conn = new SqlConnection(defaultConnection))
            {
                string query = "update basket_product set qty = (qty - @qty) where basket_id = @basket_id and product_id = @product_id;";

                using (SqlCommand myCmd = new SqlCommand(query, conn))
                {
                    myCmd.Parameters.AddWithValue("@basket_id", basket_id);
                    myCmd.Parameters.AddWithValue("@product_id", product_id);
                    myCmd.Parameters.AddWithValue("@qty", qty);
                    conn.Open();
                    myCmd.ExecuteNonQuery();
                }
            }
        }


        /// <summary>
        /// This method returns the qty of product to the basket by the amount in the order
        /// This will be used in case of cancelled orders
        /// 
        /// Test Case:  Req-9
        /// </summary>
        /// <param name="basket_id">basket identifier</param>
        /// <param name="product_id">product identifier</param>
        /// <param name="qty">quantity to reduce by</param>
        public void ReturnBasketItem(int basket_id, int product_id, int qty)
        {
            using (SqlConnection conn = new SqlConnection(defaultConnection))
            {
                string query = "update basket_products set qty = qty + @qty where basket_id = @basket_id and product_id = @product_id";

                using (SqlCommand myCmd = new SqlCommand(query, conn))
                {
                    myCmd.Parameters.AddWithValue("@basket_id", basket_id);
                    myCmd.Parameters.AddWithValue("@product_id", product_id);
                    myCmd.Parameters.AddWithValue("@qty", qty);
                    conn.Open();
                    myCmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// This method restocks the basket to its full capacity
        /// 
        /// Test Case:  Req-9
        /// </summary>
        /// <param name="basket_id"></param>
        /// <returns>Message indicating basket has been restocked</returns>
        public string RestockBasket(int basket_id)
        {
            List<Product> prodList = new List<Product>();

            foreach (Product product in prodList)
            {
                using (SqlConnection conn = new SqlConnection(defaultConnection))
                {
                    string query = "update basket_product set qty = @max_qty where basket_id = @basket_id and product_id = @product_id;";

                    using (SqlCommand myCmd = new SqlCommand(query, conn))
                    {
                        myCmd.Parameters.AddWithValue("@basket_id", basket_id);
                        myCmd.Parameters.AddWithValue("@product_id", product.product_id);
                        myCmd.Parameters.AddWithValue("@qty", product.max_qty_in_basket);
                        conn.Open();
                        myCmd.ExecuteNonQuery();
                    }
                }
            }
            return "Your basket has been restocked";
        }
    }
}
