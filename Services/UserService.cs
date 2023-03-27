using TaskManagement.Models;
using System.Text.Json;
using TaskManagement.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.IdentityModel.Tokens.Jwt;

namespace TaskManagement.Services
{

    public class UserService : UserInterface
    {
        private IWebHostEnvironment webHost;
        private string filePath;
        private List<User> users { get; } = new List<User>();

        public UserService(IWebHostEnvironment webHost)
        {
            this.webHost = webHost;
            this.filePath = Path.Combine(webHost.ContentRootPath, "Data", "user.json");
            using (var jsonFile = File.OpenText(filePath))
            {
                users = JsonSerializer.Deserialize<List<User>>(jsonFile.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
        }


        private void saveToFile()
        {
            File.WriteAllText(filePath, JsonSerializer.Serialize(users));
        }
        public ActionResult<String>? Login(User user)
        {
            User? findUser = users.FirstOrDefault(u => u.Username == user.Username && u.Password == user.Password);
            if (findUser == null)
            {
                return null;
            }
            String? type = findUser.Type;
            var claims = new List<Claim>
            {
                new Claim("id", findUser.Id+""),
                new Claim("userName", findUser.Username),
                new Claim("type", type),
            };

            var token = TaskTokenService.GetToken(claims);

            return new OkObjectResult(TaskTokenService.WriteToken(token));
        }
        public IEnumerable<User> GetAll() => users;

        public User? Get(int id, string? token)
        {
            string user = decodedToken(token);
            if (user == id + "")
                return users.FirstOrDefault(t => t.Id == id);
            else return null;
        }

        public void Add(User user)
        {
            user.Id = users.Max(t => t.Id) + 1;
            users.Add(user);
            saveToFile();
        }

        public int Update(int id, User newUser)
        {
            if (id != newUser.Id)
                return -1;
            var user = users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return 0;
            user.Username = newUser.Username;
            user.Password = newUser.Password;
            saveToFile();
            return 1;
        }

        public bool Delete(int id)
        {
            var user = users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return false;
            users.Remove(user);
            saveToFile();
            return true;
        }
        public int Count => users.Count();

        private string decodedToken(string? token)
        {
            if (token != null)
                token = token.Replace("Bearer ", "");
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            string id = jwtSecurityToken.Claims.First(claim => claim.Type == "id").Value;
            return id;
        }
    }
}
