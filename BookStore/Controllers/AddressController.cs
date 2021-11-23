using BookStoreModel;
using BookStoreRepositary.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressRepository manager;

        public AddressController(IAddressRepository manager)
        {
            this.manager = manager;

        }
        [HttpPost]
        [Route("AddUserAddress")]
        public IActionResult AddUserAddress([FromBody] AddressModel userDetails)
        {
            try
            {
                var result = this.manager.AddUserAddress(userDetails);
                if (result)
                {

                    return this.Ok(new { Status = true, Message = "Added New UserDetails Successfully !" });
                }
                else
                {

                    return this.BadRequest(new{ Status = false, Message = "Failed to add user Details, Try again" });
                }
            }
            catch (Exception ex)
            {

                return this.NotFound(new { Status = false, Message = ex.Message });

            }
        }
 
        [HttpGet]
        [Route("getUserAddress")]
        public IActionResult getUserAddress(int userId)
        {
            var result = this.manager.GetUserDetails(userId);
            try
            {
                if (result.Count > 0)
                {
                    return this.Ok(new { Status = true, Message = "Address successfully retrived", Data = result });

                }
                else
                {
                    return this.BadRequest(new { Status = false, Message = "No address available" });
                }
            }
            catch (Exception e)
            {
                return this.NotFound(new { Status = false, Message = e.Message });
            }
        }
        [HttpPost]
        [Route("EditAddress")]
        public IActionResult EditAddress([FromBody] AddressModel details)
        {
            var result = this.manager.EditAddress(details);
            try
            {
                if (result)
                {
                    return this.Ok(new { Status = true, Message = "Address updated successfully" });

                }
                else
                {
                    return this.BadRequest(new  { Status = false, Message = "Address updation fails" });
                }
            }
            catch (Exception e)
            {
                return this.NotFound(new  { Status = false, Message = e.Message });
            }
        }

    }
}
