using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using RazorLight;

namespace Func.RazorTest
{
	public static class DaFunction
	{
		[FunctionName("Function1")]
		public static async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Function, "get", Route = "dafunction")] HttpRequest req)
		{
			string name = req.Query["name"];

			if (string.IsNullOrWhiteSpace(name))
			{
				return new BadRequestObjectResult("Please pass a name on the query string or in the request body");
			}

			var engine = new RazorLightEngineBuilder()
				.UseMemoryCachingProvider()
				.Build();

			const string Template = "Hello, @Model.Name. Welcome to RazorLight repository";
			var model = new ViewModel { Name = name };

			var result = await engine.CompileRenderAsync("templateKey", Template, model);

			return new OkObjectResult(result);
		}

		public class ViewModel
		{
			public string Name { get; set; }
		}
	}
}
