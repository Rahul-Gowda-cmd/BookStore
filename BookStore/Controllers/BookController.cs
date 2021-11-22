using BookStoreManager.Interface;
using BookStoreModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    //[Authorize]
    public class BookController : ControllerBase
    {
        private readonly IBookManager manager;

        // Constructor Dependency Injection
        public BookController(IBookManager manager)
        {
            this.manager = manager;
        }

        [HttpPost]
        [Route("add-book")]
        public IActionResult AddBook([FromBody] BookModel book)
        {
            try
            {
                BookModel bookData = this.manager.AddBook(book);

                if (bookData != null)
                {
                    return this.Ok(new { success = true, message = "Add Book Successful", result = bookData });
                }

                return this.Ok(new { success = false, message = "Add Book Failed, Book with the same name already exists" });
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPut]
        [Route("update-book")]
        public IActionResult UpdateBook([FromBody] BookModel book)
        {
            try
            {
                BookModel bookData = this.manager.UpdateBook(book);
                if (bookData != null)
                {
                    return this.Ok(new { success = true, message = "Update Book Successful", result = bookData });
                }

                return this.Ok(new { success = false, message = "Update Book Failed, Book doesnot exists" });
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpDelete]
        [Route("delete-book")]
        public IActionResult DeleteBook(int bookId)
        {
            try
            {
                bool result = this.manager.DeleteBook(bookId);

                if (result)
                {
                    return this.Ok(new { success = true, message = "Delete Book Successful" });
                }

                return this.Ok(new { success = false, message = "Delete Book Failed, Book doesnot exists" });
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
