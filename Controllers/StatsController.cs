using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SignalRChat;

namespace SampleAspNetCore_Server.Controllers
{
	[FormatFilter]
	[Route("api/[controller]")]
    public class StatsController : Controller
    {
		// GET api/values
		[HttpGet]
		[Route("{format?}")]
		public IActionResult Get()
		{
			int connectionNum = ChatHub.ConnectedIds.Count();
			Console.WriteLine(String.Format("Connection Num: {0}", connectionNum));

			return Ok(connectionNum);
		}
    }
}
