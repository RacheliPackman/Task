using Task = TaskManagement.Models.Task;
namespace TaskManagement.Interfaces
{

    public interface TaskInterface
    {
        public IEnumerable<Task> GetAll();
        public Task? Get(int id);
        public void Add(Task task);
        public int Update(int id, Task newTask);
        public bool Delete(int id);
    }
}
