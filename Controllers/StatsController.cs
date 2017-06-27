using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace SampleAspNetCore_Server.Controllers
{
	[FormatFilter]
	[Route("api/[controller]")]
    public class StatsController : Controller
    {
		private readonly SignalRContext signalRContext;

		public StatsController(SignalRContext context)
		{
			this.signalRContext = context;
		}

		// GET api/values
		[HttpGet]
		[Route("{format?}")]
		public IActionResult Get()
		{
			Console.WriteLine(String.Format("Connection Num: {0}", this.signalRContext.SignalRItemList.Count()));

			return Ok(this.signalRContext.SignalRItemList.Count());
		}
    }
}
