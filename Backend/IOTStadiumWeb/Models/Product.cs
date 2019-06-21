namespace IOTStadiumWeb.Models
{
    /// <summary>
    /// This class contains the attributes of the product
    /// </summary>
    public class Product
    {
        public int product_id { get; set; }
        public string product_name { get; set; }
        public string product_desc { get; set; }
        public int product_category_id{get;set;}
        public string product_category { get; set; }
        public string product_image { get; set; }
        public decimal cost_price { get; set; }
        public decimal sale_price { get; set; }
        public int max_qty_in_basket { get; set; }
    }
}
