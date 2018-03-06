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
		private readonly IHostingEnvironment _hostingEnvironment;

		public FileController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

		// GET: /api/file
		[HttpGet]
		public async Task<string> Get()
		{
			string webRootPath = _hostingEnvironment.WebRootPath;
			string contentRootPath = _hostingEnvironment.ContentRootPath;
			System.Console.WriteLine($"webRoot: {webRootPath} contentRoot: {contentRootPath}");
			var path = Path.Combine(contentRootPath, "../README.md");

			return await System.IO.File.ReadAllTextAsync(path);
		}
	}
}
