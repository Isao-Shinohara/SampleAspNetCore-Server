using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SampleAspNetCore_Server.Controllers
{
	[FormatFilter]
	[Route("api/[controller]")]
    public class ValuesController : Controller
    {
		// GET api/values
		[HttpGet]
		[Route("{format?}")]
		public IActionResult Get()
		{
            var addClass = new Address
			{
                Country = "Japan",
			};

			var mc = new MyClass
			{
				Age = 99,
				FirstName = "hoge",
				LastName = "huga",
				Address = addClass,
			};

			string json = Newtonsoft.Json.JsonConvert.SerializeObject(mc);
			Console.WriteLine(json);

			return Ok(mc);
		}

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
