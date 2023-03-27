using Microsoft.AspNetCore.Mvc;
using TaskManagement.Models;
namespace TaskManagement.Interfaces
{

    public interface UserInterface
    {
        public IEnumerable<User> GetAll();
        public User? Get(int id, string? jwtEncoded);
        public void Add(User user);
        public int Update(int id, User newUser);
        public bool Delete(int id);

        public ActionResult<String> Login(User user);
    }
}