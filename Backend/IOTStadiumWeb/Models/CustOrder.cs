using System;
using System.Collections.Generic;

namespace IOTStadiumWeb.Models
{
    /// <summary>
    /// This class contains the relevant attributes for the customer order
    /// </summary>
    public class CustOrder
    {
        public int? order_id { get; set; }
        public int customer_id { get; set; }
        public string customer { get; set; }
     //   public Location location { get; set; }
        public int? location_id { get; set; }
        public string bay { get; set; }
        public string row_no { get; set; }
        public string seat_no { get; set; }
        public int? basket_id { get; set; }
        public decimal? total_price { get; set; }        
        public int? expected_delivery_time { get; set; }
        public int? actual_delivery_time { get; set; }
        public DateTime? submit_ts { get; set; }
        public DateTime? vendor_accepted_ts { get; set; }
        public DateTime? delivered_ts { get; set; }
        public string order_notes { get; set; }
        public int? feedback_stars { get; set; }
        public string feedback_notes { get; set; }
        public string vendor_notes { get; set; }
        public List<CustOrderItem> order_items;
    }
}
