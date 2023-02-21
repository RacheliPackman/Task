using Task = TaskManagement.Models.Task;
using TaskManagement.Interfaces;

namespace TaskManagement.Services
{

    public class TaskService : TaskInterface
    {

        private List<Task> tasks = new List<Task>(){
        new Task () {Id=1,Name="home work", IsDone=true },
        new Task () {Id=2,Name="Dishwashing", IsDone=false}
    };
        public IEnumerable<Task> GetAll() => tasks;

        public Task? Get(int id)
        {
            return tasks.FirstOrDefault(t => t.Id == id);
        }

        public void Add(Task task)
        {
            task.Id = tasks.Max(t => t.Id) + 1;
            tasks.Add(task);
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
            return 1;
        }

        public bool Delete(int id)
        {
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
                return false;
            tasks.Remove(task);
            return true;
        }
    }
}
