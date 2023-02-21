using Task = TaskManagement.Models.Task;

namespace TaskManagement.Services
{

    public static class TaskService
    {

        private static List<Task> tasks = new List<Task>(){
        new Task () {Id=1,Name="home work", IsDone=true },
        new Task () {Id=2,Name="Dishwashing", IsDone=false}
    };
        public static IEnumerable<Task> GetAll() => tasks;

        public static Task? Get(int id)
        {
            return tasks.FirstOrDefault(t => t.Id == id);
        }

        public static void Add(Task task)
        {
            task.Id = tasks.Max(t => t.Id) + 1;
            tasks.Add(task);
        }

        public static int Update(int id, Task newTask)
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

        public static bool Delete(int id)
        {
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
                return false;
            tasks.Remove(task);
            return true;
        }
    }
}
