using System.Net;
using System.Net.Http.Json;
using Newtonsoft.Json;

namespace ToDoAppConsoleClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AddNewTask();
            GetAllTasks();
        }
        public static void GetAllTasks()
        {
            // TO work nicely with c# objects we need to do 2 things
            // 1. we define c# classes that match json being returned by web API
            // 2. make use of the Newtonsoft.JSON NuGet package for working with JSON

            // create a HttpClient:
            HttpClient client = new HttpClient();

            HttpResponseMessage resp = client.GetAsync("https://localhost:7187/tasks").Result;
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                // use the NewTonsoft method to serialize Json into list of our TaskInfo objects
                List<TaskInfo> tasks = resp.Content.ReadFromJsonAsync<List<TaskInfo>>().Result;
                foreach (var task in tasks)
                {
                    Console.WriteLine($"Task ID: {task.TaskId}; " +
                        $"description: {task.Description} ({task.Category}) " +
                        $"is due: {task.DueDate:d} and has status: {task.Status}");
                }
            }
            else
            {
                Console.WriteLine("there was a problem reading the tasks!");
            }

        }

        public static void AddNewTask()
        {
            // TODO 
        }
    }
}