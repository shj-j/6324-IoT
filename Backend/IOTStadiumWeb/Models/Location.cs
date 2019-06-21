namespace IOTStadiumWeb.Models
{
    /// <summary>
    /// This class describes the attributes of a location
    /// </summary>
    public class Location
    {
        public int location_id { get; set; }
        public string bay { get; set; }
        public string row_no { get; set; }
        public string seat_no { get; set; }
        public LocationCell location_cell { get; set; }
    }
}
