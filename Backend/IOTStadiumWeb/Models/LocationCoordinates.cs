namespace IOTStadiumWeb.Models
{
    // This class describes the attributes for the location coordinates
    public class LocationCoordinates
    {
        public int? location_id { get; set; }
        public int location_cell_id { get; set; }
        public int x { get; set; }
        public int y { get; set; }
    }
}
