using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using pkdotnet.DataAccess;
using pkdotnet.Models;

namespace pkdotnet.Controllers
{
    [Route("api/[controller]")]
    public class DataSourceController : Controller
    {
		readonly MongoAccess mongoAccess;

		public DataSourceController(MongoAccess mongoAccess)
        {
            this.mongoAccess = mongoAccess;
        }

		// GET api/dataSource/{id}
		[HttpGet("{id:regex(^[[a-f\\d]]{{24}}$)}")]
		public async Task<DataSource> Get(string id)
        {
			var ds = await mongoAccess.GetOne(id);

			if(ds == null) {
				throw new NotFoundException($"Data source with id '{id}' not found!");
			}

			return ds;
        }

		// DELETE api/dataSource/{id}
		[HttpDelete("{id:regex(^[[a-f\\d]]{{24}}$)}")]
		public async Task<IActionResult> Delete(string id)
		{
			var isOk = await mongoAccess.DeleteOne(id);

			if (!isOk)
			{
				throw new NotFoundException($"Could not delete! Data source with id <{id}> not found!");
			}

			return StatusCode(202, new {status = "Deleted"});
		}

		// PUT api/dataSource
		[HttpPut]
		public async Task<IActionResult> Add([FromBody]DataSource ds)
		{
			var result = await mongoAccess.AddOne(ds);
			return Created($"/api/dataSource/{result.Id}", result);
		}
    }
}
