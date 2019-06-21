using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using IOTStadiumWeb.Models;

/// <summary>
/// This class contains the api and methods associated with the Customer Orders
/// How to call APIs
/// {website}/{controller}/{action}/(optional){parameter}
/// {website} =  https://iotstadium.azurewebsites.net/api
/// {controller} = products
/// {action} = getmenu
/// [Formbody] = data is in the form body
/// The resulting call will look like this:
/// https://iotstadium.azurewebsites.net/api/products/getmenu
///
/// Summary of Methods
/// GetMenu     -   Get the list of products
/// GetProduct  -   Get the details of a product
/// 
/// </summary>
namespace IOTStadiumWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        /// <summary>
        /// HTTP GET
        /// This will retrieve the list of products and their details
        /// usage: api/products/getmenu 
        /// </summary>
        /// <returns>List of products in JSON format</returns>
        [HttpGet("GetMenu")]
        public List<Product> GetMenu()
        {
            ProductDAL api = new ProductDAL();
            return api.GetProductList();
        }

        /// <summary>
        /// HTTP GET
        /// usage: api/products/getproduct/9 (this will retrieve the product with a product_id = 5)
        /// Sample JSON product:
        /// {
        ///     "product_id":6,
        ///     "product_name":"Hot Dog",
        ///     "product_desc":"Hot Dog with sauce and mustard",
        ///     "product_category_id":2,
        ///     "product_category":"Hot Food",
        ///     "product_image":"hot_dog.jpg",
        ///     "cost_price":1.50,
        ///     "sale_price":5.00,
        ///     "max_qty_in_basket":10
        /// } 
        /// </summary>
        /// <param name="id">Product Id</param>
        /// <returns>Object that contains the product information in JSON format</returns>
        [HttpGet("GetProduct/{id}")]
        public Product GetProduct(int id)
        {
            ProductDAL api = new ProductDAL();
            return api.GetProduct(id);
        }       
    }
}
