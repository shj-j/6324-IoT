using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace IOTStadiumWeb.Models
{
    /// <summary>
    /// This class contains the methods for accessing the data layer and other order centred methods
    /// </summary>
    public class CustOrderDAL
    {
        private Utility util = new Utility();

        private readonly string defaultConnection = "Server=tcp:unswiot.database.windows.net,1433;Initial Catalog=iot_stadium;Persist Security Info=False;User ID=iot_admin;Password=Mko0(ijn; " +
                                   "MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        /// <summary>
        /// This method retrieves the cust order including the order items from the database for the specific order
        /// 
        /// Test Case:   Req-8
        /// </summary>
        /// <param name="order_id">identifier for the specific order</param>
        /// <returns>CustOrder object</returns>
        public CustOrder GetOrder(int order_id)
        {
            using (SqlConnection conn = new SqlConnection(defaultConnection))
            {
             /*   string query = "select co.order_id, co.customer_id, co.location_id, co.total_price, co.basket_id, " +
                               "co.expected_delivery_time, co.actual_delivery_time, co.submit_ts, co.vendor_accepted_ts, " +
                               "co.delivered_ts, co.order_notes, co.feedback_stars, co.feedback_notes, co.vendor_notes " +
                               "from cust_order co " +
                               "where order_id = @order_id";
                               */
                string query = "select co.order_id, co.customer_id, co.location_id, l.bay, l.row_no, l.seat_no, co.total_price, co.basket_id, " +
                               "co.expected_delivery_time, co.actual_delivery_time, co.submit_ts, co.vendor_accepted_ts, " +
                               "co.delivered_ts, co.order_notes, co.feedback_stars, co.feedback_notes, co.vendor_notes " +
                               "from cust_order co inner join  Location l on co.location_id	= l.location_id " +
                               "where order_id = @order_id";

                using (SqlCommand myCmd = new SqlCommand(query, conn))
                {
                    SqlDataReader dr = null;
                    conn.Open();
                    myCmd.Parameters.AddWithValue("@order_id", order_id);
                    dr = myCmd.ExecuteReader();

                    CustOrder cust_order = new CustOrder();

                    while (dr.Read())
                    {
                        if (util.IsNumeric(dr["order_id"].ToString().Trim()))
                            cust_order.order_id = int.Parse(dr["order_id"].ToString().Trim());
                        if (util.IsNumeric(dr["customer_id"].ToString().Trim()))
                        {
                            cust_order.customer_id = int.Parse(dr["customer_id"].ToString().Trim());
                            cust_order.customer = GetCustomerName(int.Parse(dr["customer_id"].ToString().Trim()));
                        }
                        if (util.IsNumeric(dr["location_id"].ToString().Trim()))
                            cust_order.location_id = int.Parse(dr["location_id"].ToString().Trim());
                        cust_order.bay = dr["bay"].ToString().Trim();
                        cust_order.row_no = dr["row_no"].ToString().Trim();
                        cust_order.seat_no = dr["seat_no"].ToString().Trim();
                        if (util.IsNumeric(dr["total_price"].ToString().Trim()))
                            cust_order.total_price = decimal.Parse(dr["total_price"].ToString().Trim());
                        if (util.IsNumeric(dr["basket_id"].ToString().Trim()))
                            cust_order.basket_id = int.Parse(dr["basket_id"].ToString().Trim());
                        if (util.IsNumeric(dr["expected_delivery_time"].ToString().Trim()))
                            cust_order.expected_delivery_time = int.Parse(dr["expected_delivery_time"].ToString().Trim());
                        if (util.IsNumeric(dr["actual_delivery_time"].ToString().Trim()))
                            cust_order.actual_delivery_time = int.Parse(dr["actual_delivery_time"].ToString().Trim());
                        if (util.IsDate(dr["submit_ts"].ToString().Trim()))
                            cust_order.submit_ts = DateTime.Parse(dr["submit_ts"].ToString().Trim());
                        if (util.IsDate(dr["vendor_accepted_ts"].ToString().Trim()))
                            cust_order.vendor_accepted_ts = DateTime.Parse(dr["vendor_accepted_ts"].ToString().Trim());
                        if (util.IsDate(dr["delivered_ts"].ToString().Trim()))
                            cust_order.delivered_ts = DateTime.Parse(dr["delivered_ts"].ToString().Trim());
                        if (util.IsNumeric(dr["feedback_stars"].ToString().Trim()))
                            cust_order.feedback_stars = int.Parse(dr["feedback_stars"].ToString().Trim());
                        cust_order.order_notes = dr["order_notes"].ToString().Trim();
                        cust_order.feedback_notes = dr["feedback_notes"].ToString().Trim();
                        cust_order.vendor_notes = dr["vendor_notes"].ToString().Trim();

                        cust_order.order_items = GetOrderItems(cust_order.order_id);
                    }
                    return cust_order;
                }
            }
        }

        /// <summary>
        /// This method returns the list of orders for the basket that have not yet been delivered 
        /// 
        /// Test Case:   Req-9
        /// </summary>
        /// <param name="basket_id">The basket id requesting list</param>
        /// <returns>List of undelivered CustOrder(s)</returns>
        public List<CustOrder> GetUndeliveredBasketOrders(int basket_id)
        {
            List<CustOrder> orderList = new List<CustOrder>();
            var locDAL = new LocationDAL();

            using (SqlConnection conn = new SqlConnection(defaultConnection))
            {
                string query = "select co.order_id, co.customer_id, co.location_id, l.bay, l.row_no, l.seat_no, co.total_price, co.basket_id, " +
                               "co.expected_delivery_time, co.actual_delivery_time, co.submit_ts, co.vendor_accepted_ts, " +
                               "co.delivered_ts, co.order_notes, co.feedback_stars, co.feedback_notes, co.vendor_notes " +
                               "from cust_order co inner join  Location l on co.location_id	= l.location_id " +
                               "where co.basket_id = @basket_id and co.delivered_ts is null";

                using (SqlCommand myCmd = new SqlCommand(query, conn))
                {
                    SqlDataReader dr = null;
                    conn.Open();
                    myCmd.Parameters.AddWithValue("@basket_id", basket_id);
                    dr = myCmd.ExecuteReader();                    

                    while (dr.Read())
                    {
                        CustOrder cust_order = new CustOrder();

                        if (util.IsNumeric(dr["order_id"].ToString().Trim()))
                            cust_order.order_id = int.Parse(dr["order_id"].ToString().Trim());
                        if (util.IsNumeric(dr["customer_id"].ToString().Trim()))
                        {
                            cust_order.customer_id = int.Parse(dr["customer_id"].ToString().Trim());
                            cust_order.customer = GetCustomerName(int.Parse(dr["customer_id"].ToString().Trim()));
                        }
                        if (util.IsNumeric(dr["location_id"].ToString().Trim()))
                            cust_order.location_id = int.Parse(dr["location_id"].ToString().Trim());
                        cust_order.bay = dr["bay"].ToString().Trim();
                        cust_order.row_no = dr["row_no"].ToString().Trim();
                        cust_order.seat_no = dr["seat_no"].ToString().Trim();
                        if (util.IsNumeric(dr["total_price"].ToString().Trim()))
                            cust_order.total_price = decimal.Parse(dr["total_price"].ToString().Trim());
                        if (util.IsNumeric(dr["basket_id"].ToString().Trim()))
                            cust_order.basket_id = int.Parse(dr["basket_id"].ToString().Trim());
                        if (util.IsNumeric(dr["expected_delivery_time"].ToString().Trim()))
                            cust_order.expected_delivery_time = int.Parse(dr["expected_delivery_time"].ToString().Trim());
                        if (util.IsNumeric(dr["actual_delivery_time"].ToString().Trim()))
                            cust_order.actual_delivery_time = int.Parse(dr["actual_delivery_time"].ToString().Trim());
                        if (util.IsDate(dr["submit_ts"].ToString().Trim()))
                            cust_order.submit_ts = DateTime.Parse(dr["submit_ts"].ToString().Trim());
                        if (util.IsDate(dr["vendor_accepted_ts"].ToString().Trim()))
                            cust_order.vendor_accepted_ts = DateTime.Parse(dr["vendor_accepted_ts"].ToString().Trim());
                        if (util.IsDate(dr["delivered_ts"].ToString().Trim()))
                            cust_order.delivered_ts = DateTime.Parse(dr["delivered_ts"].ToString().Trim());
                        if (util.IsNumeric(dr["feedback_stars"].ToString().Trim()))
                            cust_order.feedback_stars = int.Parse(dr["feedback_stars"].ToString().Trim());
                        cust_order.order_notes = dr["order_notes"].ToString().Trim();
                        cust_order.feedback_notes = dr["feedback_notes"].ToString().Trim();
                        cust_order.vendor_notes = dr["vendor_notes"].ToString().Trim();

                        cust_order.order_items = GetOrderItems(cust_order.order_id);

                        orderList.Add(cust_order);
                    }         
                    return orderList;
                }
            }
        }

        /// <summary>
        /// This method retieves the order items from the specified order
        /// 
        /// Test Case:  Req-8, Req-9
        /// </summary>
        /// <param name="order_id">The order id</param>
        /// <returns>List of order items</returns>
        public List<CustOrderItem> GetOrderItems(int? order_id)
        {
            List<CustOrderItem> orderItemList = new List<CustOrderItem>();

            using (SqlConnection conn = new SqlConnection(defaultConnection))
            {
                string query = "select coi.order_item_id, coi.order_id, coi.product_id, coi.qty, coi.sub_total " +
                               "from Cust_Order_Item coi " +
                               "where coi.order_id = @order_id";

                using (SqlCommand myCmd = new SqlCommand(query, conn))
                {
                    SqlDataReader dr = null;
                    conn.Open();
                    myCmd.Parameters.AddWithValue("@order_id", order_id);
                    dr = myCmd.ExecuteReader();

                    while (dr.Read())
                    {
                        CustOrderItem cust_order_item = new CustOrderItem();
                        if (util.IsNumeric(dr["order_id"].ToString().Trim()))
                            cust_order_item.order_id = int.Parse(dr["order_id"].ToString().Trim());
                        if (util.IsNumeric(dr["order_item_id"].ToString().Trim()))
                            cust_order_item.order_item_id = int.Parse(dr["order_item_id"].ToString().Trim());
                        if (util.IsNumeric(dr["product_id"].ToString().Trim()))
                            cust_order_item.product_id = int.Parse(dr["product_id"].ToString().Trim());
                        if (util.IsNumeric(dr["qty"].ToString().Trim()))
                            cust_order_item.qty = int.Parse(dr["qty"].ToString().Trim());
                        if (util.IsNumeric(dr["sub_total"].ToString().Trim()))
                            cust_order_item.sub_total = decimal.Parse(dr["sub_total"].ToString().Trim());

                        orderItemList.Add(cust_order_item);
                    }
                    return orderItemList;
                }
            }
        }

        /// <summary>
        /// This method gets the status of the selected order
        /// 
        /// Test Case:  Req-8, Req-9
        /// </summary>
        /// <param name="order_id">The identifier for the order</param>
        /// <returns>Object with order status information</returns>
        public OrderStatus GetOrderStatus(int order_id)
        {
            using (SqlConnection conn = new SqlConnection(defaultConnection))
            {
                string query = "select co.order_id, c.customer_name, l.bay + '-' + l.row_no + '-' + l.seat_no as delivery_location, " +
                               "case when co.delivered_ts is not null then co.delivered_ts " +
                               "when co.vendor_accepted_ts is not null then convert(varchar(19), dateadd(minute, co.expected_delivery_time, vendor_accepted_ts)) end as delivery_time, " +
                               "case when co.delivered_ts is not null then 'Order delivered' when co.vendor_accepted_ts is not null then 'Vendor on the way' else 'Order being processed' end as order_status " +
                               "from Cust_Order co inner join Location l on co.location_id = l.location_id " +
                               "inner join Customer c on co.customer_id = c.customer_id " +
                               "where co.order_id = @order_id";

                using (SqlCommand myCmd = new SqlCommand(query, conn))
                {
                    SqlDataReader dr = null;
                    conn.Open();
                    myCmd.Parameters.AddWithValue("@order_id", order_id);
                    dr = myCmd.ExecuteReader();

                    OrderStatus ord_status = new OrderStatus();

                    while (dr.Read())
                    {
                        if (util.IsNumeric(dr["order_id"].ToString().Trim()))
                            ord_status.order_id = int.Parse(dr["order_id"].ToString().Trim());
                        ord_status.customer_name = dr["customer_name"].ToString().Trim();
                        ord_status.delivery_time = dr["delivery_time"].ToString().Trim();
                        ord_status.delivery_location = dr["delivery_location"].ToString().Trim();
                        ord_status.order_status = dr["order_status"].ToString().Trim();
                    }
                    return ord_status;
                }
            }
        }

        /// <summary>
        /// This method creates the order details in the database
        /// 
        /// Test Case:  Req-6, Req-7
        /// </summary>
        /// <param name="new_order">New Cust Order object with new order information</param>
        /// <returns>Status of the order</returns>       
        public OrderStatus CreateOrder(NewCustOrder new_order)
        { 
            ProductDAL prodDAL = new ProductDAL();

            using (SqlConnection conn = new SqlConnection(defaultConnection))
            {               
                decimal total_price = 0;

                string query = "insert into Cust_Order (customer_id, location_id, total_price, submit_ts, order_notes) " +
                        "VALUES (@customer_id, @location_id, @total_price, @submit_ts, @order_notes) " +
                        "select scope_identity()";

                SqlCommand myCmd = new SqlCommand(query, conn);

                if (util.IsNumeric(new_order.customer_id))
                    myCmd.Parameters.AddWithValue("@customer_id", new_order.customer_id);
                else
                    myCmd.Parameters.AddWithValue("@customer_id", 0);   // indicates direct customer

                LocationCoordinates delivery_location = GetNewOrderDeliveryLocation(new_order.location);
                myCmd.Parameters.AddWithValue("@location_id", delivery_location.location_id);

                // generate the total price of the order
                foreach (NewCustOrderItem oi in new_order.order_items)
                {
                    total_price = total_price + oi.qty*prodDAL.GetProductSalePrice(oi.product_id);
                }

                myCmd.Parameters.AddWithValue("@total_price", total_price);                  
                myCmd.Parameters.AddWithValue("@submit_ts", DateTime.Now);
                myCmd.Parameters.AddWithValue("@order_notes", new_order.order_notes);

                conn.Open();

                // id is the newly generated order id
                int order_id = (int)(decimal)myCmd.ExecuteScalar();
                                
                foreach(NewCustOrderItem oi in new_order.order_items)
                {
                    InsertOrderItem(oi, order_id);
                }
                // Key = basket_id; Value = distance to delivery location
                KeyValuePair<int, int> basket_kvp = FindAvailableBasket(delivery_location, order_id);

                // update the order with basket and delivery info
                UpdateCustOrderWithBasket(order_id, basket_kvp.Key, basket_kvp.Value);

                // if this is a Direct Customer include the delivery information
                if (new_order.customer_id == 0)
                    DeliverOrder(order_id);

                // build a staus update to send to the customer
                OrderStatus ord_status = new OrderStatus();

                ord_status.order_id = order_id;
                if (new_order.customer_id == 0)
                    ord_status.order_status = "Your order has been delivered";
                else
                    ord_status.order_status = "Your order is being processed";
                ord_status.customer_name = GetCustomerName((int)new_order.customer_id);
                //ord_status.delivery_location = "Bay: " + new_order.location.bay + " Row: " + new_order.location.row_no + " Seat: " + new_order.location.seat_no;
                ord_status.delivery_time = CalculateDeliveryTime(basket_kvp.Value).ToString(); 

                return ord_status;
            }
        }

        /// <summary>
        /// This method creates a new order item for the specified order
        /// 
        /// Test Case:  Req-7
        /// </summary>
        /// <param name="new_order_item">New Cust Order Item object</param>
        /// <param name="order_id">The order id related to the products</param>
        private void InsertOrderItem(NewCustOrderItem new_order_item, int order_id)
        {
            ProductDAL prodDAL = new ProductDAL();

            using (SqlConnection conn = new SqlConnection(defaultConnection))
            {
                string query = "insert into Cust_Order_Item (order_id, product_id, qty, sub_total) " +
                        "VALUES (@order_id, @product_id, @qty, @sub_total)";

                SqlCommand myCmd = new SqlCommand(query, conn);

                if (util.IsNumeric(order_id))
                    myCmd.Parameters.AddWithValue("@order_id", order_id);
                else
                    myCmd.Parameters.AddWithValue("@order_id", DBNull.Value);
                if (util.IsNumeric(new_order_item.product_id))
                    myCmd.Parameters.AddWithValue("@product_id", new_order_item.product_id);
                else
                    myCmd.Parameters.AddWithValue("@product_id", DBNull.Value);
                if (util.IsNumeric(new_order_item.qty))
                {
                    myCmd.Parameters.AddWithValue("@qty", new_order_item.qty);
                    myCmd.Parameters.AddWithValue("@sub_total", new_order_item.qty * prodDAL.GetProductSalePrice(new_order_item.product_id));
                }
                else
                    myCmd.Parameters.AddWithValue("@qty", DBNull.Value);

                conn.Open();

                myCmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// This method calculates the expected delivery time of the order
        /// 
        /// Test Case:  Req-6, Req-8
        /// </summary>
        /// <param name="distance_to_customer">The distance the vendor is away from the customer</param>
        /// <returns>Time in minutes to deliver the order</returns>
        private int CalculateDeliveryTime(int distance_to_customer)
        {
            int delivery_time = (int)Math.Ceiling(distance_to_customer * (decimal)0.75);

            return delivery_time;
        }

        /// <summary>
        /// This method returns the customer name
        /// 
        /// Test Case:  Req-4, Req-7, Req-8
        /// </summary>
        /// <param name="id">customer id</param>
        /// <returns>The nameof the customer</returns>
        public string GetCustomerName(int id)
        {
            string customer_name = "";

            using (SqlConnection conn = new SqlConnection(defaultConnection))
            {
                string query = "select c.customer_name " +
                               "from customer c " +
                               "where c.customer_id = @customer_id";

                using (SqlCommand myCmd = new SqlCommand(query, conn))
                {
                    SqlDataReader dr = null;
                    conn.Open();
                    myCmd.Parameters.AddWithValue("@customer_id", id);
                    dr = myCmd.ExecuteReader();

                    while (dr.Read())
                    {
                        customer_name = dr["customer_name"].ToString();
                    }

                    return customer_name;
                }
            }
        }

        /// <summary>
        /// This method takes the bay, row no and seat no to determine the location id, location cell id, x and y coordinates
        /// 
        /// Test Case:  Req-5, Req-6
        /// </summary>
        /// <param name="location">New Order Location object that contains the bay, row_no and seat_no</param>
        /// <returns>Location Coordinates (i.e. location_id, location_cell_id</returns>
        private LocationCoordinates GetNewOrderDeliveryLocation(NewOrderLocation location)
        {
            var delivery_location = new LocationCoordinates();

            using (SqlConnection conn = new SqlConnection(defaultConnection))
            {
                string query = "select l.location_id, l.location_cell_id, lc.x_coord, lc.y_coord " +
                               "from location l inner join location_cell lc on l.location_cell_id = lc.location_cell_id " +
                               "where l.bay = @bay and l.row_no = @row_no and l.seat_no = @seat_no";

                using (SqlCommand myCmd = new SqlCommand(query, conn))
                {
                    SqlDataReader dr = null;
                    conn.Open();
                    myCmd.Parameters.AddWithValue("@bay", location.bay);
                    myCmd.Parameters.AddWithValue("@row_no", location.row_no);
                    myCmd.Parameters.AddWithValue("@seat_no", location.seat_no);
                    dr = myCmd.ExecuteReader();

                    while (dr.Read())
                    {
                        delivery_location.location_id = int.Parse(dr["location_id"].ToString());
                        delivery_location.location_cell_id = int.Parse(dr["location_cell_id"].ToString());
                        delivery_location.x = int.Parse(dr["x_coord"].ToString());
                        delivery_location.y = int.Parse(dr["y_coord"].ToString());
                    }

                    return delivery_location;
                }
            }
        }

        /// <summary>
        /// This method determines which basket to give the order to.
        /// 
        /// Test Case:  Req-8, Req-9, Req-11
        /// </summary>
        /// <param name="delivery_location">Object containing the delivery location</param>
        /// <param name="order_id">The related order id</param>
        /// <returns>KeyValuePair: basket_id, distance to customer</returns>
        private KeyValuePair<int, int> FindAvailableBasket(LocationCoordinates delivery_location, int order_id)
        {
            int basket_id=0;
            int min_distance = 9999;
            // KeyValuePair - Key = bsaket_id; Value = location_cell_id
            var basketList = new List<KeyValuePair<int, int>>();
            var basket = new BasketDAL();
            var locationDAL = new LocationDAL();

            basketList = basket.BasketHasStock(order_id);

            // get basket that is closest to delivery location
            foreach(KeyValuePair<int, int> kvp in basketList)
            {
                var basket_location = locationDAL.GetLocationCell(kvp.Value);
                int basket_distance = locationDAL.CalculateDistanceToCustomer(delivery_location.x, delivery_location.y, basket_location.x_coord, basket_location.y_coord);
                if (basket_distance < min_distance)
                {
                    min_distance = basket_distance;
                    basket_id = kvp.Key;
                }
            }
            return new KeyValuePair<int, int>(basket_id, min_distance);
        }

        /// <summary>
        /// This method add the basket id to the order and updates the relevant timestamps
        /// 
        /// Test Case:  Req-7, Req-8, Req-9
        /// </summary>
        /// <param name="order_id">The order id being updated</param>
        /// <param name="basket_id">The basket id being attached to the order</param>
        /// <param name="basket_distance">The distance the basket is away from the customer (used for determining time to deliver)</param>
        public void UpdateCustOrderWithBasket(int order_id, int basket_id, int basket_distance)
        {
            using (SqlConnection conn = new SqlConnection(defaultConnection))
            {
                string query = "update cust_order set basket_id = @basket_id, vendor_accepted_ts = getdate(), expected_delivery_time = @expected_delivery_time " +
                                "where order_id = @order_id";

                SqlCommand myCmd = new SqlCommand(query, conn);

                myCmd.Parameters.AddWithValue("@order_id", order_id);
                myCmd.Parameters.AddWithValue("@basket_id", basket_id);
                myCmd.Parameters.AddWithValue("@expected_delivery_time", CalculateDeliveryTime(basket_distance));

                conn.Open();

                myCmd.ExecuteNonQuery();
            }
        }


        /// <summary>
        /// This method updates the feedback for the particular order
        /// 
        /// Test Case:  Req-10
        /// </summary>
        /// <param name="CustomerFeedback">Object to hold the customer feedback for the order</param>
        /// <returns>Message</returns>
        public string UpdateCustomerFeedback(CustomerFeedback feedback)
        {
            using (SqlConnection conn = new SqlConnection(defaultConnection))
            {
                string query = "update cust_order set feedback_stars = @feedback_stars, feedback_notes = @feedback_notes " +
                                "where order_id = @order_id";

                SqlCommand myCmd = new SqlCommand(query, conn);

                myCmd.Parameters.AddWithValue("@order_id", feedback.order_id);
                myCmd.Parameters.AddWithValue("@feedback_stars", feedback.feedback_stars);                  
                myCmd.Parameters.AddWithValue("@feedback_notes", feedback.feedback);

                conn.Open();

                myCmd.ExecuteNonQuery();                    

                return "Thank you for your feedback";                  
            }
        }

        /// <summary>
        /// This method updates the order delivery_ts once the order has been delivered
        /// 
        /// Test Case:  Req-7, Req-8, Req-9
        /// </summary>
        /// <param name="order_id">order id for the order</param>
        /// <returns>string containing message about the order</returns>
        public string UpdateDeliveryTime(int order_id)
        {
            string order_status = ""; 

            using (SqlConnection conn = new SqlConnection(defaultConnection))
            {
                string query = "update cust_order set delivered_ts = getdate() where order_id = @order_id";

                SqlCommand myCmd = new SqlCommand(query, conn);

                myCmd.Parameters.AddWithValue("@order_id", order_id);
                 
                conn.Open();

                myCmd.ExecuteNonQuery();
                 
                order_status = "Thank you, your order has been delivered";                    

                return order_status;
            }
        }

        /// <summary>
        /// This method gets the list of orders that have not been accepted yet
        /// 
        /// Test Case:  Req-9
        /// </summary>
        /// <returns>List of orders not yet accepted by vendor/basket</returns>
        public List<CustOrder> DisplayAvailableOrders()
        {
            List<CustOrder> orderList = new List<CustOrder>();

            using (SqlConnection conn = new SqlConnection(defaultConnection))
            {
                string query = "select co.order_id, co.customer_id, co.location_id, co.total_price, co.basket_id, " +
                               "co.expected_delivery_time, co.actual_delivery_time, co.submit_ts, co.vendor_accepted_ts, " +
                               "co.delivered_ts, co.order_notes, co.feedback_stars, co.feedback_notes, co.vendor_notes " +
                               "from cust_order co " +
                               "where co.vendor_accepted_ts is null";

                using (SqlCommand myCmd = new SqlCommand(query, conn))
                {
                    SqlDataReader dr = null;
                    conn.Open();                
                    dr = myCmd.ExecuteReader();                    

                    while (dr.Read())
                    {
                        CustOrder cust_order = new CustOrder();

                        if (util.IsNumeric(dr["order_id"].ToString().Trim()))
                            cust_order.order_id = int.Parse(dr["order_id"].ToString().Trim());
                        if (util.IsNumeric(dr["customer_id"].ToString().Trim()))
                            cust_order.customer_id = int.Parse(dr["customer_id"].ToString().Trim());
                        if (util.IsNumeric(dr["location_id"].ToString().Trim()))
                            cust_order.location_id = int.Parse(dr["location_id"].ToString().Trim());
                        if (util.IsNumeric(dr["total_price"].ToString().Trim()))
                            cust_order.total_price = decimal.Parse(dr["total_price"].ToString().Trim());
                        if (util.IsNumeric(dr["basket_id"].ToString().Trim()))
                            cust_order.basket_id = int.Parse(dr["basket_id"].ToString().Trim());
                        if (util.IsNumeric(dr["expected_delivery_time"].ToString().Trim()))
                            cust_order.expected_delivery_time = int.Parse(dr["expected_delivery_time"].ToString().Trim());
                        if (util.IsNumeric(dr["actual_delivery_time"].ToString().Trim()))
                            cust_order.actual_delivery_time = int.Parse(dr["actual_delivery_time"].ToString().Trim());
                        if (util.IsDate(dr["submit_ts"].ToString().Trim()))
                            cust_order.submit_ts = DateTime.Parse(dr["submit_ts"].ToString().Trim());
                        if (util.IsDate(dr["vendor_accepted_ts"].ToString().Trim()))
                            cust_order.vendor_accepted_ts = DateTime.Parse(dr["vendor_accepted_ts"].ToString().Trim());
                        if (util.IsDate(dr["delivered_ts"].ToString().Trim()))
                            cust_order.delivered_ts = DateTime.Parse(dr["delivered_ts"].ToString().Trim());
                        if (util.IsNumeric(dr["feedback_stars"].ToString().Trim()))
                            cust_order.feedback_stars = int.Parse(dr["feedback_stars"].ToString().Trim());
                        cust_order.order_notes = dr["order_notes"].ToString().Trim();
                        cust_order.feedback_notes = dr["feedback_notes"].ToString().Trim();
                        cust_order.vendor_notes = dr["vendor_notes"].ToString().Trim();

                        cust_order.order_items = GetOrderItems(cust_order.order_id);

                        orderList.Add(cust_order);
                    }                       
                    return orderList;
                }
            }
        }

        /// <summary>
        /// This method provides the mechanism for a vendor to accept an order 
        /// and updates the customer order with the vendor's basket id
        /// and update the vendor accepted time stamp
        /// 
        /// Test Case:  Req-8
        /// </summary>
        /// <param name="order_id">The selected order</param>
        /// <param name="basket_id">The basket id</param>
        /// <returns>Message notifying the order was accepted</returns>
        public string AcceptOrder(int order_id, int basket_id)
        {
            OrderStatus order_status = new OrderStatus();

            using (SqlConnection conn = new SqlConnection(defaultConnection))
            {
                string query = "update Cust_Order set basket_id = @basket_id, vendor_accepted_ts = getdate() where order_id = @order_id ";                            

                SqlCommand myCmd = new SqlCommand(query, conn);

                myCmd.Parameters.AddWithValue("@basket_id", basket_id);                   
                myCmd.Parameters.AddWithValue("@order_id", order_id);

                conn.Open();

                myCmd.ExecuteNonQuery();

                return "Order has been accepted";
            }
        }
        
        /// <summary>
        /// This method gets the basket id for the specified order
        /// 
        /// Test Case:  Req-8
        /// </summary>
        /// <param name="order_id">The order id</param>
        /// <returns>The basket id for the specified order</returns>
        public int GetOrderBasketId(int order_id)
        {
            int basket_id = 0;

            using (SqlConnection conn = new SqlConnection(defaultConnection))
            {
                string query = "select case when o.basket_id is null then 0 else o.basket_id end as basket_id from cust_order o " +
                               "where o.order_id = @order_id";

                using (SqlCommand myCmd = new SqlCommand(query, conn))
                {
                    SqlDataReader dr = null;
                    conn.Open();
                    myCmd.Parameters.AddWithValue("@order_id", order_id);
                    dr = myCmd.ExecuteReader();

                    while (dr.Read())
                    {
                        if (util.IsNumeric(dr["basket_id"].ToString().Trim()))
                            basket_id = int.Parse(dr["basket_id"].ToString().Trim());
                    }
                    return basket_id;
                }
            }
        }

        /// <summary>
        /// This method cancels an order. Once the order is cancelled it will replace the goods back to the basket that were held against that order.
        /// 
        /// Test Case:  Req-7
        /// </summary>
        /// <param name="order_id">The order id of the cancelled order</param>
        /// <returns>Message indicating order has been cancelled</returns>
        public string CancelOrder(int order_id)
        {
            using (SqlConnection conn = new SqlConnection(defaultConnection))
            {                   
                string query = "update cust_order set cancelled_ts = getdate() " +
                                "where order_id = @order_id";

                SqlCommand myCmd = new SqlCommand(query, conn);

                myCmd.Parameters.AddWithValue("@order_id", order_id);

                conn.Open();

                myCmd.ExecuteNonQuery();
                        
                // get the order items
                List<CustOrderItem> coiList = new List<CustOrderItem>();
                BasketDAL basket = new BasketDAL();
                int basket_id = GetOrderBasketId(order_id);
                coiList = GetOrderItems(order_id);

                // add stock back to basket (if order was already assigned to a basket. basket_id = 0 means not assigned yet)
                if (basket_id > 0)
                {
                    foreach (CustOrderItem coi in coiList)
                    {
                        basket.ReturnBasketItem(basket_id, coi.product_id, coi.qty);
                    }
                }
                    
                return "Your order has been cancelled";           
            }
        }

        /// <summary>
        /// This method processes the order to the delivered status by updating the delivered_ts
        /// 
        /// Test Case:  Req-7, Req-8, Req-9
        /// </summary>
        /// <param name="order_id">Order id delivered</param>
        /// <returns>Message indicating order was delivered</returns>
        public string DeliverOrder(int order_id)
        {
            using (SqlConnection conn = new SqlConnection(defaultConnection))
            {
                string query = "update cust_order set delivered_ts = getdate() " +
                                "where order_id = @order_id";

                SqlCommand myCmd = new SqlCommand(query, conn);

                myCmd.Parameters.AddWithValue("@order_id", order_id);

                conn.Open();

                myCmd.ExecuteNonQuery();

                return "Thank you, your order has been delivered";
            }
        }

    }
}
