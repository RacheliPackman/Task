using Task = TaskManagement.Models.Task;
using TaskManagement.Interfaces;
using System.Text.Json;

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
                });
            }
        }
        private void saveToFile()
        {
            File.WriteAllText(filePath, JsonSerializer.Serialize(tasks));
        }
        public IEnumerable<Task> GetAll() => tasks;

        public Task? Get(int id)
        {
            return tasks.FirstOrDefault(t => t.Id == id);
        }

        public void Add(Task task)
        {
            task.Id = tasks.Max(t => t.Id) + 1;
            tasks.Add(task);
            saveToFile();
        }

        public int Update(int id, Task newTask)
        {
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

        public bool Delete(int id)
        {
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
                return false;
            tasks.Remove(task);
            saveToFile();
            return true;
        }
        public int Count => tasks.Count();
    }
}
