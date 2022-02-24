using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAO;
using BusinessLogic;
using Newtonsoft.Json;
using System.Data;

namespace XfoneHomeAssignment.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        UsersFacade usersFacade = new UsersFacade();
        [HttpGet("TryLogin")]
        public async Task<ActionResult<User>> TryLogin(string userName, string password)
        {
            User user = new User
            {
                UserName = userName,
                Password = password
            };
            try
            {
                user = await Task.Run(() => usersFacade.TryLogin(user));
            }
            catch (UserNotExistsException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
            return Ok(JsonConvert.SerializeObject(user));
        }

        [HttpPost("CreateNewUser")]
        public async Task<ActionResult<Book>> CreateNewUser([FromBody] User user)
        {
            try
            {
                await Task.Run(() => usersFacade.AddNewUser(user));
            }
            catch (DuplicateNameException ex)
            {
                return StatusCode(403, $"{ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"{ex.Message}");
            }
            return Created("api/users/AddNewUser", JsonConvert.SerializeObject(User));
        }
    }
}
