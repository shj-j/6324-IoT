using System.Collections.Generic;

namespace IOTStadiumWeb.Models
{
    /// <summary>
    /// This class describes the attributes of a new order 
    /// </summary>
    public class NewCustOrder
    {
        public int? customer_id { get; set; }
        public NewOrderLocation location { get; set; }
        public string order_notes { get; set; }
        public List<NewCustOrderItem> order_items;
    }
}
