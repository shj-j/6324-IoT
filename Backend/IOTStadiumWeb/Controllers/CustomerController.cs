using Microsoft.AspNetCore.Mvc;
using IOTStadiumWeb.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

/// <summary>
/// This class contains the api and methods associated with the Customer
/// How to call APIs
/// {website}/{controller}/{action}/(optional){parameter}
/// {website} =  https://iotstadium.azurewebsites.net/api
/// {controller} = customer
/// {action} = login
/// [Formbody] = data is in the form body
/// The resulting call will look like this:
/// https://iotstadium.azurewebsites.net/api/customer/login
///
/// Summary of Methods
/// Login     -   Login to application mechanism
/// 
/// </summary>
namespace IOTStadiumWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        /// <summary>
        /// HTTP POST
        /// This will attempt to log the user into the application
        /// usage: api/customer/login/ (data in the form body)
        /// </summary>
        /// <param name="username">The username of the customer</param>
        /// <param name="password">The password of the customer</param>
        /// <returns>Successful: Customer_Id; Unsuccessful: -1</returns>
        [HttpPost("Login")]
        public int Login([FromBody] JToken json_value)
        {
            CustomerDAL custDAL = new CustomerDAL();
            string username = json_value["username"].ToString();
            string password = json_value["password"].ToString(); 
     
            return custDAL.Login(username, password);
        }
    }
}
