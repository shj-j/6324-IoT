namespace IOTStadiumWeb.Models
{
    /// <summary>
    /// This class describes the attributes of a location cell
    /// </summary>
    public class LocationCell
    {
        public int location_cell_id { get; set; }
        public string location_cell_desc { get; set; }
        public string beacon_id { get; set; }
        public int x_coord { get; set; }
        public int y_coord { get; set; }
    }
}
