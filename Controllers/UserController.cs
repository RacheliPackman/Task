using Microsoft.AspNetCore.Mvc;
using TaskManagement.Interfaces;
using TaskManagement.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;

namespace TaskManagement.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private UserInterface users;
        public UserController(UserInterface u)
        {
            this.users = u;
        }

        [HttpPost]
        [Route("[action]")]
        public ActionResult<String> Login([FromBody] User user)
        {
            var result = users.Login(user);
            if (result == null)
                return Unauthorized();
            return result;
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public IEnumerable<User> Get()
        {
            return users.GetAll();
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "User")]
        public ActionResult<User> Get(int id)
        {
            string? jwtEncoded = Request.Headers.Authorization;
            var user = users.Get(id, jwtEncoded);
            if (user == null)
                return NotFound();
            return user;
        }

        [HttpPost]
        public ActionResult Post(User user)
        {
            users.Add(user);
            return CreatedAtAction(nameof(Post), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, User user)
        {
            int result = users.Update(id, user);
            switch (result)
            {
                case -1: return BadRequest();
                case 0: return NotFound();
                default: return NoContent();
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if (!users.Delete(id))
                return NotFound();
            return NoContent();
        }
    }
}
