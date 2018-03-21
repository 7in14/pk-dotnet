using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using pkdotnet.DataAccess;
using Flurl.Http;
using System.Linq;
using System;
using System.Collections.Generic;

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
				System.Console.WriteLine($"Will be reading data for {ds.Name} from {ds.Url}");
				return new
				{
					name = ds.Name,
					task = (Task<Object>)ds.Url.GetJsonAsync<object>()
				};
			}).ToArray();

			var tasks = toExecute.Select(d => d.task).ToArray();

			List<DataResult> data = new List<DataResult>();

			foreach (var t in toExecute)
			{
				object o = null;
				try
				{
					o = await t.task;
				}
				catch (Exception ex)
				{

				}

				data.Add(new DataResult
				{
					Name = t.name,
					Data = o,
					Error = t.task.IsFaulted ? t.task.Exception.Message : null
				});
			}

			//try
			//{
			//	Task.WaitAll(tasks);
			//	System.Console.WriteLine("all tasks completed");
			//}
			//catch (System.AggregateException ex)
			//{
			//	// not everything completed according to plan
			//}

			//var data = toExecute.Select(d => new DataResult
			//{
			//	Name = d.name,
			//	Data = d.task.Result,
			//	Error = d.task.IsFaulted ? d.task.Exception : null
			//}).ToList();

			return data;
		}

		class DataResult
		{
			public string Name;
			public object Data;
			public string Error;
		}

		[Route("test")]
		public async Task<dynamic> Test()
		{
			await "https://jsonplaceholder.typicode.com/users".GetJsonAsync();

			return "hello";
		}
	}
}
