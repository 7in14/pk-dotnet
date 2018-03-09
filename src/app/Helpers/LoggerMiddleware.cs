using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Colorful;
using Microsoft.AspNetCore.Http;

namespace pkdotnet
{
	public class LoggerMiddleware
	{
		const string MessageTemplate =
			"HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";

		readonly RequestDelegate _next;

		public LoggerMiddleware(RequestDelegate next)
		{
			_next = next ?? throw new ArgumentNullException(nameof(next));
		}

		public async Task Invoke(HttpContext httpContext)
		{
			if (httpContext == null) throw new ArgumentNullException(nameof(httpContext));

			var sw = Stopwatch.StartNew();
			try
			{
				await _next(httpContext);
				sw.Stop();
				LogRequestInColor(httpContext, sw);
			}
			// Never caught, because `LogException()` returns false.
			catch (Exception ex) when (LogRequestInColor(httpContext, sw, ex)) { }
		}

		static bool LogRequestInColor(HttpContext httpContext, Stopwatch sw, Exception ex = null)
		{
			var request = httpContext.Request;
			var statusCode = httpContext.Response?.StatusCode;
			var isOk = statusCode >= 200 && statusCode < 400;

			const string pattern = "HTTP {0} {1} responded {2} in {3} ms";
			Formatter[] requestProps = {
				new Formatter(request.Method, Color.Green),
				new Formatter(request.Path, Color.Blue),
				new Formatter(statusCode, Color.Orange),
				new Formatter(string.Format("{0:0.0000}",sw.Elapsed.TotalMilliseconds), Color.Yellow)
			};

			Colorful.Console.WriteLineFormatted(pattern, Color.Gray, requestProps);

			if (!isOk)
			{
				const string failedPattern = "\tError! Host: {0} Protocol: {1} Headers: {2}";
				Formatter[] extraProps = {
					new Formatter(request.Host, Color.Yellow),
					new Formatter(request.Protocol, Color.Yellow),
					new Formatter(request.Headers.Select(x=> $"{x.Key}: {x.Value}").Aggregate((cur, next) => cur+ ", "+ next), Color.Yellow)
				};

				Colorful.Console.WriteLineFormatted(failedPattern, Color.Pink, extraProps);
			}

			if (ex != null)
			{
				const string exPattern = "\tException: {0}";
				Formatter[] exProps = {
					new Formatter(ex, Color.Red)
				};

				Colorful.Console.WriteLineFormatted(exPattern, Color.White, exProps);
			}

			return false;
		}
	}

}
