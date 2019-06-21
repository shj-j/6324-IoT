namespace IOTStadiumWeb.Models
{
    /// <summary>
    /// This class describes the attributes for the order item
    /// </summary>
    public class CustOrderItem
    {
        public int? order_item_id { get; set; }
        public int? order_id { get; set; }
        public int product_id { get; set; }
        public int qty { get; set; }
        public decimal? sub_total { get; set; }
    }
}
