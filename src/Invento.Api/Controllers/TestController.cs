using Microsoft.AspNetCore.Mvc;

namespace Invento.Api.Controllers {
	[ApiController]
	[Route("api/[controller]")]
	public class TestController : ControllerBase {
		[HttpGet]
		public ActionResult<string> Get() {
			return "API is working!";
		}
	}
}

public class TestController { }