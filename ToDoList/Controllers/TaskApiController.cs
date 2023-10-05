using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    [ApiController()]
    public class TaskApiController : Controller
    {
        public TaskApiController(ToDoContext toDoContext) 
        {
            _toDoContext = toDoContext;
        }
        [HttpGet("/tasks")]
        public IActionResult GetAllTasks()
        {

            List<TaskInfo> tasks = _toDoContext.ToDos
                .Include(t => t.Category)
                .Include(t => t.Status)
                .OrderByDescending(t => t.DueDate)
                .Select(t => new TaskInfo()
                {
                    Description = t.Description,
                    DueDate = t.DueDate,
                    Status = t.Status.Name,
                    Category = t.Category.Name,
                    TaskId = t.Id
                })
                .ToList();

            return Json(tasks);
        }

        [HttpGet("/tasks/{id}")]
        public IActionResult GetTaskById(int id)
        {
            // using the where method to filter to specific task by ID...
            TaskInfo task = _toDoContext.ToDos
                .Include(t => t.Category)
                .Include(t => t.Status)
                .OrderByDescending(t => t.DueDate)
                .Select(t => new TaskInfo()
                {
                    Description = t.Description,
                    DueDate = t.DueDate,
                    Status = t.Status.Name,
                    Category = t.Category.Name,
                    TaskId = t.Id
                })
                .Where(t => t.TaskId == id)
                .FirstOrDefault();

            return Json(task);
        }

        /*
         - Use attr routing again
        - Specify that request comes from body as JSON
        - Get cat'y by name, give duedate Now if null, use cat'y ID & task request to create new ToDo & save
        - Flesh out a full task info object to return
        - Return CreatedAtAction using nameof, new ID, and full task info
         */
        [HttpPost("/tasks")]
        public IActionResult AddNewTask([FromBody]NewTaskRequest newTaskRequest) 
        {
            // query for the full category object based on the name in req:
            Category category = _toDoContext.Categories.Where(c => c.Name == newTaskRequest.Category).FirstOrDefault();

            // if due date in req is null make be now:
            DateTime? dueDate = newTaskRequest.DueDate == null ? DateTime.Now : newTaskRequest.DueDate;

            ToDo newToDo = new ToDo()
            {
                Description = newTaskRequest.Description,
                DueDate = dueDate,
                CategoryId = category.CategoryId,
                StatusId = "open"
            };

            _toDoContext.ToDos.Add(newToDo);
            _toDoContext.SaveChanges();

            TaskInfo task = new TaskInfo()
            {
                TaskId = newToDo.Id,
                DueDate = newToDo.DueDate,
                Description = newToDo.Description,
                Category = category.Name,
                Status = "Open"
            };


            // we use the createdAction method to return 201 created response + at the url of newly created task
            // by ID in the laction header
            return CreatedAtAction(nameof(GetTaskById), new {id = task.TaskId}, task);

        }


        private ToDoContext _toDoContext;
    }
}
