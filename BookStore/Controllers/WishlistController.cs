using BookStoreManager.Interface;
using BookStoreModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistManager manager;

        // Constructor Dependency Injection
        public WishlistController(IWishlistManager manager)
        {
            this.manager = manager;
        }

        [HttpPost]
        [Route("api/wishlist-add-book")]
        public IActionResult AddBookToWishlist(int bookId, int userId)
        {
            try
            {
                string resultMessage = this.manager.AddBookToWishlist(bookId, userId);

                if (resultMessage.Equals("Add Book To Wishlist Successful"))
                {
                    return this.Ok(new { success = true, message = resultMessage });
                }
                else if (resultMessage.Equals("Add Book To Wishlist Failed, Book already Exists in Cart"))
                {
                    return this.Ok(new { success = false, message = resultMessage });
                }

                return this.Ok(new { success = false, message = resultMessage });
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        [Route("api/wishlist-get-books")]
        public IActionResult GetWishlistItems(int userId)
        {
            List<WishlistModel> wishlist = this.manager.GetWishlistItems(userId);
            try
            {
                if (wishlist.Count > 0)
                {
                    return this.Ok(new { success = true, message = "Get Wishlist Items Successful", data = wishlist });

                }
                return this.Ok(new { success = false, message = "Get Wishlist Item Failed, Wishlist list is Empty" });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { success = false, message = e.Message });
            }
        }

        [HttpDelete]
        [Route("api/wishlist-delete-book")]
        public IActionResult DeleteBookFromWishlist(int wishlistId)
        {
            try
            {
                bool result = this.manager.DeleteBookFromWishlist(wishlistId);

                if (result)
                {
                    return this.Ok(new { success = true, message = "Delete Book From Wishlist Successful" });
                }

                return this.Ok(new { success = false, message = "Delete Book From Wishlist Failed, Book doesnot exists" });
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
