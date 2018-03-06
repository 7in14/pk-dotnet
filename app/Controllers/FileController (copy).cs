using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace pkdotnet
{
	[Route("api/[controller]")]
	public class FileController : Controller
	{
		readonly string _contentRootPath;

		public FileController(IHostingEnvironment hostingEnvironment)
        {
			_contentRootPath = hostingEnvironment.ContentRootPath;
        }

		// GET: /api/file
		[HttpGet]
		public async Task<string> Get()
		{
			var path = Path.Combine(_contentRootPath, "../README.md");
			return await System.IO.File.ReadAllTextAsync(path);
		}
	}
}
