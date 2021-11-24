using BookStoreManager.Interface;
using BookStoreModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    //[ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderManager manager;

        public OrderController(IOrderManager manager)
        {
            this.manager = manager;

        }

        [HttpPost]
        [Route("PlaceOrders")]
        public IActionResult PlaceOrders([FromBody] List<CartModel> orderDetails)
        {
            try
            {
                var result = this.manager.PlaceTheOrder(orderDetails);
                if (result)
                {

                    return this.Ok(new  { Status = true, Message = "Order placed successfully" });
                }
                else
                {

                    return this.BadRequest(new{ Status = false, Message = "Failed to place order, Try again" });
                }
            }
            catch (Exception ex)
            {

                return this.NotFound(new { Status = false, Message = ex.Message });

            }
        }

        [HttpGet]
        [Route("getorderlist")]
        public IActionResult GetOrderList(int userId)
        {
            var result = this.manager.GetOrderList(userId);
            try
            {
                if (result.Count > 0)
                {
                    return this.Ok(new { Status = true, Message = "Wish List successfully retrived", Data = result });

                }
                else
                {
                    return this.BadRequest(new  { Status = false, Message = "No WishList available" });
                }
            }
            catch (Exception e)
            {
                return this.NotFound(new { Status = false, Message = e.Message });
            }
        }
    }
}
