using Microsoft.AspNetCore.Mvc;


namespace TaskManagement.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {

        [HttpGet]
        public IEnumerable<Models.Task> Get()
        {
            return Services.TaskService.GetAll();
        }

        [HttpGet("{id}")]
        public ActionResult<Models.Task> Get(int id)
        {
            var task = Services.TaskService.Get(id);
            if (task == null)
                return NotFound();
            return task;
        }

        [HttpPost]
        public ActionResult Post(Models.Task task)
        {
            Services.TaskService.Add(task);
            return CreatedAtAction(nameof(Post), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, Models.Task task)
        {
            int result = Services.TaskService.Update(id, task);
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
            if (!Services.TaskService.Delete(id))
                return NotFound();
            return NoContent();
        }
    }
}
