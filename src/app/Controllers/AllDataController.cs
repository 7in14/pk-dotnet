using System;
using System.Linq;
using System.Threading.Tasks;
using Flurl.Http;
using Microsoft.AspNetCore.Mvc;
using pkdotnet.DataAccess;

namespace pkdotnet
{
	[Route("api/[controller]")]
	public class AllDataController : Controller
	{
		readonly MongoAccess mongoAccess;

		public AllDataController(MongoAccess mongoAccess)
		{
			this.mongoAccess = mongoAccess;
		}

		// GET: /api/allData
		public async Task<object> Get()
		{
			var sources = await mongoAccess.GetAll();

			var toExecute = sources.Select(ds =>
			{
				Console.WriteLine($"Will be reading data for {ds.Name} from {ds.Url}");
				return new
				{
					name = ds.Name,
					task = ds.Url.GetJsonAsync<object>()
				};
			}).ToArray();

			var tasks = toExecute.Select(d => d.task).ToArray();

			try
			{
				Task.WaitAll(tasks);
				Console.WriteLine("all tasks completed");
			}
			catch (AggregateException)
			{
				// not everything completed according to plan
				// ignore and read task.Exception
			}

			var data = toExecute.Select(d => new
			{
				Name = d.name,
				Data = d.task.IsCompletedSuccessfully ? d.task.Result : null,
				Error = d.task.IsFaulted ? d.task.Exception.Message : null
			}).ToList();

			return data;
		}
	}
}
