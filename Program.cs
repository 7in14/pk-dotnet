using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace pk_dotnet
{
	public class Program
    {
        public static void Main(string[] args)
        {
			Console.WriteLine("args: "+ string.Join(",", args));
			BuildWebHost(new string[]{}).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
