namespace IOTStadiumWeb.Models
{
    /// <summary>
    /// This class describes the attributes of order feedback 
    /// </summary>
    public class CustomerFeedback
    {
        public int order_id { get; set; }
        public int feedback_stars { get; set; }
        public string feedback { get; set; }
    }
}
