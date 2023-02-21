using Microsoft.AspNetCore.Mvc;
using TaskManagement.Interfaces;
using Task = TaskManagement.Models.Task;

namespace TaskManagement.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        private TaskInterface tasks;
        public TaskController(TaskInterface t)
        {
            this.tasks = t;
        }

        [HttpGet]
        public IEnumerable<Task> Get()
        {
            return tasks.GetAll();
        }

        [HttpGet("{id}")]
        public ActionResult<Task> Get(int id)
        {
            var task = tasks.Get(id);
            if (task == null)
                return NotFound();
            return task;
        }

        [HttpPost]
        public ActionResult Post(Models.Task task)
        {
            tasks.Add(task);
            return CreatedAtAction(nameof(Post), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, Models.Task task)
        {
            int result = tasks.Update(id, task);
            switch (result)
            {
                case -1: return BadRequest();
                case 0: return NotFound();
                default: return NoContent();
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if (!tasks.Delete(id))
                return NotFound();
            return NoContent();
        }
    }
}
