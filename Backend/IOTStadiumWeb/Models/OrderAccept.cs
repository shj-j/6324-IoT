namespace IOTStadiumWeb.Models
{
    /// <summary>
    /// This class contains the attributes of the order accept object, when a basket is assigned order
    /// </summary>
    public class OrderAccept
    {
        public int order_id { get; set; }
        public int basket_id { get; set; }
    }
}
