using System;
using System.Data.SqlClient;

namespace IOTStadiumWeb.Models
{
    /// <summary>
    /// This class contains the methods for accessing the data layer and other routines for the Location
    /// </summary>
    public class LocationDAL
    {
        private Utility util = new Utility();

        private readonly string defaultConnection = "Server=tcp:unswiot.database.windows.net,1433;Initial Catalog=iot_stadium;Persist Security Info=False;User ID=iot_admin;Password=Mko0(ijn; " +
                                    "MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        /// <summary>
        /// This method gets the location id for the particular bay, row, seat
        /// 
        /// Test Case:  Req-5, Req-6
        /// </summary>
        /// <param name="bay">Bay number</param>
        /// <param name="row_no">Row number</param>
        /// <param name="seat_no">Saet number</param>
        /// <returns>Location object</returns>
        private Location GetLocation(string bay, string row_no, string seat_no)
        {
            using (SqlConnection conn = new SqlConnection(defaultConnection))
            {
                string query = "select l.location_id, l.bay, l.row_no, l.seat_no, l.location_cell_id, lc.location_cell_desc, lc.beacon_id " +
                               "from location l inner " +
                               "join location_cell lc on l.location_cell_id = lc.location_cell_id " +
                               "where l.bay = @bay and l.row = @row_no and l.seat_no = @seat_no";

                using (SqlCommand myCmd = new SqlCommand(query, conn))
                {
                    SqlDataReader dr = null;
                    conn.Open();
                    myCmd.Parameters.AddWithValue("@bay", bay);
                    myCmd.Parameters.AddWithValue("@row_no", row_no);
                    myCmd.Parameters.AddWithValue("@seat_no", seat_no);
                    dr = myCmd.ExecuteReader();

                    Location location = new Location();
                    while (dr.Read())
                    {
                        if (util.IsNumeric(dr["location_id"].ToString().Trim()))
                            location.location_id = int.Parse(dr["location_id"].ToString().Trim());
                        location.bay = dr["bay"].ToString().Trim();
                        location.row_no = dr["row_no"].ToString().Trim();
                        location.seat_no = dr["seat_no"].ToString().Trim();

                        if (util.IsNumeric(dr["location_cell_id"].ToString().Trim()))
                            location.location_cell = GetLocationCell(int.Parse(dr["location_cell_id"].ToString().Trim()));
                    }
                    return location;
                }
            }
        }

        /// <summary>
        /// This method gets the location for the particular location id
        /// 
        /// Test Case:  Req-5, Req-6
        /// </summary>
        /// <param name="Location_id">Location id</param>
        /// <returns>Location object</returns>
        public Location GetLocationFromId(int location_id)
        {
            using (SqlConnection conn = new SqlConnection(defaultConnection))
            {
                string query = "select l.location_id, l.bay, l.row_no, l.seat_no, l.location_cell_id, lc.location_cell_desc, lc.beacon_id " +
                               "from location l inner " +
                               "join location_cell lc on l.location_cell_id = lc.location_cell_id " +
                               "where l.location_id = @location_id";

                using (SqlCommand myCmd = new SqlCommand(query, conn))
                {
                    SqlDataReader dr = null;
                    conn.Open();
                    myCmd.Parameters.AddWithValue("@location_id", location_id);
                    dr = myCmd.ExecuteReader();

                    Location location = new Location();
                    while (dr.Read())
                    {
                        if (util.IsNumeric(dr["location_id"].ToString().Trim()))
                            location.location_id = int.Parse(dr["location_id"].ToString().Trim());
                        location.bay = dr["bay"].ToString().Trim();
                        location.row_no = dr["row_no"].ToString().Trim();
                        location.seat_no = dr["seat_no"].ToString().Trim();

                        if (util.IsNumeric(dr["location_cell_id"].ToString().Trim()))
                            location.location_cell = GetLocationCell(int.Parse(dr["location_cell_id"].ToString().Trim()));
                    }
                    return location;
                }
            }
        }

        /// <summary>
        /// This method gets the location cell that the beacon is attached to
        /// 
        /// Test Case:  Req-9
        /// </summary>
        /// <param name="beacon_id">The unique beacon id</param>
        /// <returns>Location Cell object</returns>
        private LocationCell GetLocationCellFromBeacon(string beacon_id)
        {
            using (SqlConnection conn = new SqlConnection(defaultConnection))
            {
                string query = "select lc.location_cell_id, lc.location_cell_desc, lc.beacon_id, lc.x_coord, lc.y_coord " +
                               "from location_cell lc " +
                               "where lc.beacon_id = @beacon_id";

                using (SqlCommand myCmd = new SqlCommand(query, conn))
                {
                    SqlDataReader dr = null;
                    conn.Open();
                    myCmd.Parameters.AddWithValue("@beacon_id", beacon_id);
                    dr = myCmd.ExecuteReader();

                    LocationCell location_cell = new LocationCell();

                    while (dr.Read())
                    {
                        if (util.IsNumeric(dr["location_cell_id"].ToString().Trim()))
                            location_cell.location_cell_id = int.Parse(dr["location_cell_id"].ToString().Trim());
                        location_cell.location_cell_desc = dr["locacion_cell_desc"].ToString().Trim();
                        location_cell.beacon_id = dr["beacon_id"].ToString().Trim();

                        if (util.IsNumeric(dr["x_coord"].ToString().Trim()))
                            location_cell.x_coord = int.Parse(dr["x_coord"].ToString().Trim());
                        if (util.IsNumeric(dr["y_coord"].ToString().Trim()))
                            location_cell.y_coord = int.Parse(dr["y_coord"].ToString().Trim());

                    }
                    return location_cell;
                }
            }
        }

        /// <summary>
        /// This method gets the location cell object from the location cell id
        /// 
        /// Test Case:  Req-5, Req-6, Req-9
        /// </summary>
        /// <param name="location_cell_id">location cell id</param>
        /// <returns>Location Cell object for the particular location cell id</returns>
        public LocationCell GetLocationCell(int location_cell_id)
        {
            using (SqlConnection conn = new SqlConnection(defaultConnection))
            {
                string query = "select lc.location_cell_id, lc.location_cell_desc, lc.beacon_id, lc.x_coord, lc.y_coord " +
                                "from location_cell lc " +
                                "where lc.location_cell_id = @location_cell_id";

                using (SqlCommand myCmd = new SqlCommand(query, conn))
                {
                    SqlDataReader dr = null;
                    conn.Open();
                    myCmd.Parameters.AddWithValue("@location_cell_id", location_cell_id);
                    dr = myCmd.ExecuteReader();

                    LocationCell location_cell = new LocationCell();

                    while (dr.Read())
                    {
                        if (util.IsNumeric(dr["location_cell_id"].ToString().Trim()))
                            location_cell.location_cell_id = int.Parse(dr["location_cell_id"].ToString().Trim());
                        location_cell.location_cell_desc = dr["location_cell_desc"].ToString().Trim();
                        location_cell.beacon_id = dr["beacon_id"].ToString().Trim();

                        if (util.IsNumeric(dr["x_coord"].ToString().Trim()))
                            location_cell.x_coord = int.Parse(dr["x_coord"].ToString().Trim());
                        if (util.IsNumeric(dr["y_coord"].ToString().Trim()))
                            location_cell.y_coord = int.Parse(dr["y_coord"].ToString().Trim());

                    }
                    return location_cell;
                }
            }
        }


        /// <summary>
        /// This method calculates the distance in units from the basket to the customer using the basket's current location.
        /// It uses the Manhattan distance method to calculate distance between 2 points in a grid
        /// 
        /// Test Case:  Req-9
        /// </summary>
        /// <param name="cust_x">customer x coordinate</param>
        /// <param name="basket_x">basket x coordinate</param>
        /// <param name="cust_y">customer y coordinate</param>
        /// <param name="basket_y">basket y coordinate</param>        
        /// <returns>distance in units from the basket to the customer</returns>
        public int CalculateDistanceToCustomer(int cust_x, int cust_y, int basket_x, int basket_y)
        {
            var a = Math.Abs(basket_x - cust_x);
            var b = Math.Abs(basket_y - cust_y);

            return a+b;
        }
    }
}
