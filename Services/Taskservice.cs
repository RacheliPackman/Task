using Task = TaskManagement.Models.Task;
using TaskManagement.Interfaces;
using System.Text.Json;
using System.IdentityModel.Tokens.Jwt;

namespace TaskManagement.Services
{

    public class TaskService : TaskInterface
    {

        private IWebHostEnvironment? webHost;
        private string filePath;
        private List<Task> tasks { get; } = new List<Task>();

        public TaskService(IWebHostEnvironment webHost)
        {
            this.webHost = webHost;
            this.filePath = Path.Combine(webHost.ContentRootPath, "Data", "task.json");
            using (var jsonFile = File.OpenText(filePath))
            {
                tasks = JsonSerializer.Deserialize<List<Task>>(jsonFile.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!;
            }
        }
        private void saveToFile()
        {
            File.WriteAllText(filePath, JsonSerializer.Serialize(tasks));
        }
        public IEnumerable<Task> GetAll(string? token) => tasks.Where((task) => task.UserId == int.Parse(decodedToken(token)));

        public Task? Get(int id)
        {
            return tasks.FirstOrDefault(t => t.Id == id);
        }

        public void Add(Task task, string? token)
        {
            task.Id = tasks.Max(t => t.Id) + 1;
            task.UserId = int.Parse(decodedToken(token));
            tasks.Add(task);
            saveToFile();
        }

        public int Update(int id, Task newTask, string? token)
        {
            int userId = int.Parse(decodedToken(token));
            if (newTask.UserId != userId)
                return -1;
            if (id != newTask.Id)
                return -1;
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
                return 0;
            task.Name = newTask.Name;
            task.IsDone = newTask.IsDone;
            saveToFile();
            return 1;
        }

        public bool Delete(int id, string? token)
        {
            int userId = int.Parse(decodedToken(token));
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task == null || task.UserId != userId)
                return false;
            tasks.Remove(task);
            saveToFile();
            return true;
        }
        public int Count => tasks.Count();
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
