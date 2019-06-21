using IOTStadiumWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

/// <summary>
/// This class contains the api and methods associated with the Customer Orders
/// How to call APIs
/// {website}/{controller}/{action}/(optional){parameter}
/// {website} =  https://iotstadium.azurewebsites.net/api
/// {controller} = products
/// {action} = getmenu
/// [Formbody] = data is in the form body
/// The resulting call will look like this:
/// https://iotstadium.azurewebsites.net/api/order/getorder/102
///
/// Summary of Methods
/// SetBasketLocation   -   updates the current location of the basket
/// RestockBasket   -   resets the contents of the basket to full for each item
/// 
/// </summary>
namespace IOTStadiumWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {

        /// <summary>
        /// HTTP POST
        /// usage: api/basket/setbasketlocation/    (data in the formbody)
        /// This will update the location_cell of the basket
        /// Sample JSON BasketLocation:
        /// {
        ///    "basket_id": 4,
        ///    "beacon_id": "c7c1a1bf-bb00-4cad-8704-9f2d2917ded2"
        /// }
        /// </summary>
        /// <param name="BasketLocation">Basket Location object {basket_id, beacon_id}</param>
        [HttpPost("SetBasketLocation")]
        public void SetBasketLocation([FromBody] BasketLocation basket_location)
        {
            BasketDAL basket = new BasketDAL();

            basket.UpdateBasketLocation(basket_location.basket_id, basket_location.beacon_id);
        }

        /// <summary>
        /// HTTP POST
        /// usage: api/basket/restockbasket/ 
        /// </summary>
        /// <param name="basket_id">The basket id to be restocked</param>
        [HttpPost("RestockBasket")]
        public string RestockBasket([FromBody] JToken json_value)
        {
            BasketDAL basketDAL = new BasketDAL();
            int basket_id = int.Parse(json_value["order_id"].ToString());

            return basketDAL.RestockBasket(basket_id);
        }

    }
}
