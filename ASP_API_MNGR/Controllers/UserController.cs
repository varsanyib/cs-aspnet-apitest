using ASP_API_MNGR.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ASP_API_MNGR.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private Random rnd = new Random((int)DateTime.Now.Ticks);
        static List<User> users = new List<User>();

        #region GET
        [HttpGet]
        [Route("", Name = "GetUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            if (users.Count == 0)
            {
                return StatusCode(204, "Error"); //return NoContent();
            }
            else
            {
                return Ok(users);
            }
        }
        [HttpGet]
        [Route("{id:int:min(1)}", Name = "GetUserById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<User> GetUserById(int id)
        {
            User ?selectedUser = users.Where(x => x.Id.Equals(id)).FirstOrDefault();
            if (selectedUser == null) return NotFound("User not found!");

            return Ok(selectedUser);
        }
        [HttpGet]
        [Route("{name}", Name = "GetUserByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<User> GetUserByName(string name)
        {
            if (string.IsNullOrEmpty(name)) return BadRequest("Invalid name!");
            User ?selectedUser = users.Where(x => x.Name.ToLower().Equals(name.ToLower())).FirstOrDefault();
            if (selectedUser == null) return NotFound("User not found!");

            return Ok(selectedUser);
        }
        #endregion
        #region POST
        [HttpPost(Name = "CreateUser")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<UserDTO> CreateUser([FromBody] UserDTO newUser)
        {
            try
            {
                if (newUser.BirthDate > DateTime.Now) return BadRequest("The given date of birth is incorrect!");

                int newId;
                do
                {
                    newId = rnd.Next(0, 1000);
                } while (users.Select(x => x.Id).Where(x => x.Equals(newId)).Count() != 0);

                User user = new User(
                        id: newId,
                        name: newUser.Name,
                        birthDate: newUser.BirthDate);
                
                users.Add(user);

                return StatusCode(201, user); // or => return Success();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        #endregion
        #region PUT
        [HttpPut]
        [Route("{id:int:min(1)}", Name = "ModifyUserById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<UserDTO> ModifyUserById(int id, [FromBody] UserDTO modifiedUser)
        {
            try
            {
                User? selectedUser = users.Where(x => x.Id.Equals(id)).FirstOrDefault();
                if (selectedUser == null) return NotFound($"{id} user not exist!");
                if (modifiedUser == null) return BadRequest("New user data is missing!");

                selectedUser.Name = modifiedUser.Name;
                selectedUser.BirthDate = modifiedUser.BirthDate;

                users[users.FindIndex(x => x.Equals(selectedUser))] = selectedUser;

                return Ok($"{id} user data modified.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        #endregion
        #region DELETE
        [HttpDelete]
        [Route("{id:int:min(1)}", Name = "DeleteUserById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<UserDTO> DeleteUserById(int id)
        {
            try
            {
                User ?selectedUser = users.Where(x => x.Id.Equals(id)).FirstOrDefault();
                if (selectedUser == null) return NotFound($"{id} user not exist!");

                users.Remove(selectedUser);

                return Ok($"{id} user deleted.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        #endregion
    }
}
