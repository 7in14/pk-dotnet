using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using pkdotnet.DataAccess;
using pkdotnet.Models;

namespace pkdotnet.Controllers
{
    [Route("api/[controller]")]
    public class DataSourcesController : Controller
    {
		readonly MongoAccess mongoAccess;

		public DataSourcesController(MongoAccess mongoAccess)
        {
            this.mongoAccess = mongoAccess;
        }

		// GET api/dataSources
        [HttpGet]
        public async Task<List<DataSource>> Get()
        {
            return await mongoAccess.GetAll();
        }
    }
}
