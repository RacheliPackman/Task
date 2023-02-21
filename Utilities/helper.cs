using TaskManagement.Interfaces;
using TaskManagement.Services;
namespace TaskManagement.Utilities
{
    public static class Helper
    {
        public static void AddTasks(this IServiceCollection services)
        {
            services.AddSingleton<TaskInterface, TaskService>();

        }
    }
}