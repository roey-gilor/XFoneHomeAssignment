using DAO;
using BusinessLogic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XfoneHomeAssignment.Controllers
{
    [Route("api/[controller]")]
    public class BooksController : Controller
    {
        BooksFacade booksFacade = new BooksFacade();

        [HttpGet("GetAllBooks")]
        public async Task<ActionResult<Book>> GetAllBooks()
        {
            List<Book> books = null;
            try
            {
                books = await Task.Run(() => booksFacade.GetAllBooks());
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
            return Ok(JsonConvert.SerializeObject(books));
        }

        [HttpGet("GetAllCategories")]
        public async Task<ActionResult<Category>> GetAllCategories()
        {
            List<Category> categories = null;
            try
            {
                categories = await Task.Run(() => booksFacade.GetAllCategories());
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
            return Ok(JsonConvert.SerializeObject(categories));
        }

        [HttpPost("AddNewBook")]
        public async Task<ActionResult<Book>> CreateNewBook([FromBody] Book book)
        {
            try
            {
                await Task.Run(() => booksFacade.AddNewBook(book));
            }
            catch (DuplicateBookNameException ex)
            {
                return StatusCode(403, $"{ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"{ex.Message}");
            }
            return Created("api/books/AddNewBook", JsonConvert.SerializeObject(book));
        }

        [HttpPut("UpdateBookPrice")]
        public async Task<ActionResult> UpdateUserDetails([FromBody] Book book)
        {
            try
            {
                await Task.Run(() => booksFacade.UpdateBookPrice(book));
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"{{ error: \"{ex.Message}\" }}");
            }
            return Ok();
        }
    }
}
