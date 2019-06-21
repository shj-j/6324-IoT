using System.Data.SqlClient;

namespace IOTStadiumWeb.Models
{
    /// <summary>
    /// This class defines the methods used for acessing the data layer for customers
    /// </summary>
    public class CustomerDAL
    {
        private readonly string defaultConnection = "Server=tcp:unswiot.database.windows.net,1433;Initial Catalog=iot_stadium;Persist Security Info=False;User ID=iot_admin;Password=Mko0(ijn; " +
                   "MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        private Utility util = new Utility();

        /// <summary>
        /// This method retrieves the customer id after successfully logging in.
        /// 
        /// Test Case:  Req-4
        /// </summary>
        /// <param name="username">The user name of the customer</param>
        /// <param name="password">The password of the customer</param>
        /// <returns>Successful: Customer Id; Unsuccessful: -1;</returns>
        public int Login(string username, string password)
        {
            using (SqlConnection conn = new SqlConnection(defaultConnection))
            {
                string query = "select c.customer_id from customer c " +
                               "where c.username = @username and c.password = @password";
                int customer_id = -1;

                using (SqlCommand myCmd = new SqlCommand(query, conn))
                {
                    SqlDataReader dr = null;
                    conn.Open();
                    myCmd.Parameters.AddWithValue("@username", username);
                    myCmd.Parameters.AddWithValue("@password", password);
                    dr = myCmd.ExecuteReader();
                        
                    while (dr.Read())
                    {
                        if (util.IsNumeric(dr["customer_id"].ToString().Trim()))
                            customer_id = int.Parse(dr["customer_id"].ToString().Trim());        
                    }
                    return customer_id;
                }
            }
        }
    }
}
