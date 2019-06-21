using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using IOTStadiumWeb.Models;
using Newtonsoft.Json.Linq;

/// <summary>
/// This class contains the api and methods associated with the Customer Orders
/// How to call APIs
/// {website}/{controller}/{action}/(optional){parameter}
/// {website} =  https://iotstadium.azurewebsites.net/api
/// {controller} = products
/// {action} = getmenu
/// {parameter} = optional parameter
/// [Formbody] = data is in the form body
/// The resulting call will look like this:
/// https://iotstadium.azurewebsites.net/api/order/getorder/102
///
/// 
/// Summary of Methods
/// GetOrder        -	Get the specific order
/// Undelivered	    -   Get a list of undelivered orders from a particular basket
/// GetOrderStatus  -	Get the status of a particular order
/// CreateOrder     -	Create a new order
/// DeliverOrder    -   Vendor completes delivery of the order
/// UpdateCustomerFeedback	-   Updates the feedback details for a particular order
/// UpdateDeliveryTime  -	Update the delivery time stamp for a particular order
/// AcceptOrder	    -   For the Basket to accept an order. Update the order with basket_id
/// CancelOrder     -   Customer cancels the order, stock is replaced into the basket
/// 
/// </summary>
namespace IOTStadiumWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        /// <summary>
        /// HTTP GET 
        /// usage: api/order/getorder/102 (this will retrieve the order with id of 102)
        /// 
        /// Sample JSON CustOrder:
        /// {
        ///     "order_id":102,
        ///     "customer_id":249,
        ///     "customer":null,
        ///     "location": null,
        ///     "location_id": 10627,    
        ///     "basket_id":5,
        ///     "total_price":28.00,
        ///     "expected_delivery_time":1,
        ///     "actual_delivery_time":14,
        ///     "submit_ts":"2019-03-30T15:19:34",
        ///     "vendor_accepted_ts":"2019-03-30T15:23:01",
        ///     "delivered_ts":"2019-03-30T15:34:43",
        ///     "order_notes":"",
        ///     "feedback_stars":0,
        ///     "feedback_notes":"",
        ///     "vendor_notes":"",
        ///     "order_items":
        ///     [{"order_item_id":192,"order_id":102,"product_id":8,"qty":2,"sub_total":13.00},
        ///     {"order_item_id":193,"order_id":102,"product_id":2,"qty":1,"sub_total":4.50},
        ///     {"order_item_id":194,"order_id":102,"product_id":5,"qty":3,"sub_total":10.50}]
        /// }
        /// </summary>
        /// <param name="id">The order id</param>
        /// <returns>Object that contains the order in JSON format</returns>
        [HttpGet("GetOrder/{id}")]
        public CustOrder GetOrder(int id)
        {
            CustOrderDAL api = new CustOrderDAL();
            return api.GetOrder(id);
        }

        /// <summary>
        /// HTTP GET 
        /// usage: api/order/undelivered/3 (this will retrieve the order for the basket id 3 that have not been delivered)
        /// Sample JSON can be seen above
        /// </summary>
        /// <param name="id">the basket_id (i.e. attached to the vendor)</param>
        /// <returns>List of orders that have not been delivered for the specified basket</returns>
        [HttpGet("Undelivered/{id}")]       
        public ICollection<CustOrder> Undelivered(int id)
        {
            CustOrderDAL api = new CustOrderDAL();
            return api.GetUndeliveredBasketOrders(id);
        }

        /// <summary>
        /// HTTP GET
        /// usage: api/order/getorderstatus/974 (this will retrieve the order status for the order id 974)
        /// Sample JSON OrderStatus:
        /// {
        /// "order_id":994,
        /// "customer_name":"Amy",
        /// "delivery_time":"2019/04/06 8:22:00 PM",
        /// "delivery_location":"11-4-5",
        /// "order_status":"Vendor on the way"
        /// }
        /// </summary>
        /// <param name="id">Order Id</param>
        /// <returns>Object containing the details of the order status in JSON format</returns>
        [HttpGet("GetOrderStatus/{id}")]
        public OrderStatus GetOrderStatus(int id)
        {
            CustOrderDAL api = new CustOrderDAL();
            return api.GetOrderStatus(id);
        }


        /// <summary>
        /// HTTP POST
        /// This will submit a new order for creation, may contain null values
        /// usage: api/order/createorder/ (data is in the form body)
        /// Sample JSON CustOrder:
        /// {
        ///    "customer_id": 249,
        ///    "location": {"bay": 2,"row_no": 4, "seat_no": 3},  
        ///    "order_notes": "Wearing big red hat",
        ///    "order_items": [
        ///     {"product_id": 8,"qty": 2},
        ///     {"product_id": 2,"qty": 1},
        ///     {"product_id": 5,"qty": 3}]
        /// }
        /// 
        /// </summary>
        /// <NOTE>If the customer Id is NULL then it refers to a Direct Customer</NOTE>
        /// <param name="value">JSON formatted Cust Order including Cust Order Items</param>
        /// <returns>The Order Status</returns>
        [HttpPost("CreateOrder")]
        public OrderStatus CreateOrder([FromBody] NewCustOrder value)
        {
            CustOrderDAL api = new CustOrderDAL();

            return api.CreateOrder(value);
        }

        /// <summary>
        /// HTTP POST
        /// usage: api/order/updatecustomerfeedback/ 
        /// Sample JSON CustomerFeedback
        /// {
        ///     "order_id": 5008,
        ///     "feedback_stars": 3
        ///     "feedback": "fantastic service"
        /// }
        /// </summary>
        /// <param name="value">JSON formatted Customer Feedback</param>
        [HttpPost("UpdateCustomerFeedback")]
        public string UpdateCustomerFeedback([FromBody] CustomerFeedback feedback)
        {
            CustOrderDAL orderDAL = new CustOrderDAL();

            return orderDAL.UpdateCustomerFeedback(feedback);
        }

        /// <summary>
        /// HTTP POST
        /// This will update the delivered_ts for the order
        /// usage: api/order/DeliverOrder/ (data in the form body)
        /// </summary>
        /// <param name="order_id">The order id</param>
        [HttpPost("DeliverOrder")]
        public string DeliverOrder([FromBody] JToken json_order)
        {
            CustOrderDAL orderDAL = new CustOrderDAL();
            int order_id = int.Parse(json_order["order_id"].ToString());

            return orderDAL.DeliverOrder(order_id);
        }

        /// <summary>
        /// HTTP POST
        /// This will update the cancelled_ts for the order
        /// usage: api/order/CancelOrder/ (data in the form body)
        /// </summary>
        /// <param name="order_id">The order id</param>
        [HttpPost("CancelOrder")]
        public string CancelOrder([FromBody] JToken json_order)
        {
            CustOrderDAL orderDAL = new CustOrderDAL();
            int order_id = int.Parse(json_order["order_id"].ToString());

            return orderDAL.CancelOrder(order_id);
        }

        /// <summary>
        /// HTTP POST
        /// This will update the delivered_ts for the order
        /// usage: api/order/UpdateDeliveryTime/ (data in the form body)
        /// </summary>
        /// <param name="order_id">The order id</param>
        /// <param name="value">JSON formatted Cust Order including Cust Order Items</param>
        [HttpPost("UpdateDeliveryTime")]
        public void UpdateDeliveryTime([FromBody] JToken json_order)
        {
            CustOrderDAL orderDAL = new CustOrderDAL();
            int order_id = int.Parse(json_order["order_id"].ToString());

            orderDAL.UpdateDeliveryTime(order_id);
        }

        /// <summary>
        /// HTTP POST
        /// This will assign the basket id to the order
        /// usage: api/order/acceptorder/ (data in the form body)
        /// This will update the cust order with the basket id
        /// Sample JSON OrderAccept:
        /// {
        ///    "order_id": 5008,
        ///    "basket_id": 4
        /// }
        /// </summary>
        /// <param name="OrderAccept">Order Accept object {order_id, basket_id}</param>
        /// <returns>String containing message</returns>
        [HttpPost("AcceptOrder")]
        public string AcceptOrder([FromBody] OrderAccept order_accept)
        {
            CustOrderDAL orderDAL = new CustOrderDAL();

            return orderDAL.AcceptOrder(order_accept.order_id, order_accept.basket_id);
        }
    }
}
