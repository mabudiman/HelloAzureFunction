using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace HelloAzureFunction
{
    public static class ToDoList
    {
        public static List<string> theList = new List<string>();

        [FunctionName("GetToDoList")]
        public static async Task<IActionResult> Run1(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("GetToDoList Function");

            return new OkObjectResult(theList);
        }

        [FunctionName("AddToDoList")]
        public static async Task<IActionResult> Run2(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("AddToDoList Function");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            try
            {
                theList.Add(requestBody);

                return new OkObjectResult(theList);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                return new OkObjectResult("Add failed. " + ex.Message);

            }
        }

        [FunctionName("DeleteToDoList")]
        public static async Task<IActionResult> Run3(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("DeleteToDoList Function");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            try
            {
                var itemToRemove = theList.FirstOrDefault(todo => todo == requestBody);
                if (itemToRemove == null) throw new Exception("Item not found.");
                theList.Remove(itemToRemove);

                return new OkObjectResult(theList);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                return new OkObjectResult("Delete failed. " + ex.Message);

            }
        }

        [FunctionName("UpdateToDoList")]
        public static async Task<IActionResult> Run4(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("UpdateToDoList Function");

            string title = req.Query["title"];
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            try
            {
                var indexItemToUpdate = theList.FindIndex(x => x == title);
                if (indexItemToUpdate == -1) throw new Exception("Item not found.");
                theList[indexItemToUpdate] = requestBody;

                return new OkObjectResult(theList);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                return new OkObjectResult("Update failed. " + ex.Message);

            }
        }
    }
}
