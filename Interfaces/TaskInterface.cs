using Task = TaskManagement.Models.Task;
namespace TaskManagement.Interfaces
{

    public interface TaskInterface
    {
        public IEnumerable<Task> GetAll(string? token);
        public Task? Get(int id);
        public void Add(Task task, string? token);
        public int Update(int id, Task newTask, string? token);
        public bool Delete(int id, string? token);
    }
}
