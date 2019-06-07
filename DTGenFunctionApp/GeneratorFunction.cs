using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using DTGenFunctionApp.Models;
using Core;
using System.Net;

namespace DTGenFunctionApp
{
    public static class GeneratorFunction
    {
        [FunctionName("GeneratorFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var genRequest = JsonConvert.DeserializeObject<GenRequest>(requestBody);

                if (genRequest == null || string.IsNullOrWhiteSpace(genRequest.Source))
                    return new BadRequestResult();

                var generator = new Generator(new GenOptions()
                {
                    IsCamelCaseEnabled = genRequest.IsCamelCaseEnabled,
                    IsMapToInterfaceEnabled = genRequest.IsMapToInterfaceEnabled,
                    Language = genRequest.Language
                });

                var result = await generator.GenerateAsync(genRequest.Source);

                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                log.LogError(ex, ex.Message);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
