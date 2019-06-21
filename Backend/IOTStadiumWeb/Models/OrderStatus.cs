namespace IOTStadiumWeb.Models
{
    /// <summary>
    /// This class contains the attributes of the order status
    /// </summary>
    public class OrderStatus
    {
        public int order_id { get; set; }
        public string customer_name { get; set; }
        public string delivery_time { get; set; }
        public string delivery_location { get; set; }
        public string order_status { get; set; }
    }
}
