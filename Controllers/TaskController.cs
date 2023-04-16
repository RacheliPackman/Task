using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Interfaces;
using Task = TaskManagement.Models.Task;

namespace TaskManagement.Controllers
{

    [ApiController]
    [Route("[controller]")]
    [Authorize]
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
            string? jwtEncoded = Request.Headers.Authorization;
            return tasks.GetAll(jwtEncoded);
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

        public ActionResult Post(Task task)
        {
            string? jwtEncoded = Request.Headers.Authorization;
            tasks.Add(task, jwtEncoded);
            return CreatedAtAction(nameof(Post), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, Task task)
        {
            string? jwtEncoded = Request.Headers.Authorization;
            int result = tasks.Update(id, task, jwtEncoded);
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
            string? jwtEncoded = Request.Headers.Authorization;
            if (!tasks.Delete(id, jwtEncoded))
                return NotFound();
            return NoContent();
        }

    }
}
