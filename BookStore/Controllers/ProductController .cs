using BookStoreManager.Interface;
using BookStoreModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    public class ProductController : ControllerBase
    {
        private readonly IProductManager manager;

        // Constructor Dependency Injection
        public ProductController(IProductManager manager)
        {
            this.manager = manager;
        }

        [HttpGet]
        [Route("get-books")]
        public IActionResult GetAllBooks()
        {
            List<BookModel> books = this.manager.GetAllBooks();
            try
            {
                if (books.Count > 0)
                {
                    return this.Ok(new { success = true, message = "Get Books Successful", data = books });

                }
                return this.Ok(new { success = false, message = "Book list is Empty" });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { success = false, message = e.Message });
            }
        }
    }
}
