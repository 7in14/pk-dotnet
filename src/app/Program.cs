using System;
using System.Drawing;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace pkdotnet
{
	public class Program
    {
        public static void Main(string[] args)
        {
			Colorful.Console.WriteAscii("pk 7-in-14 dotnet", Color.MediumSlateBlue);

			Console.WriteLine("args: "+ string.Join(",", args));
            // OSX dotnet core doesn't like where first param is the name of the sln file :/
			BuildWebHost(new string[]{}).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
