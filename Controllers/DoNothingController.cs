using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace test02.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DoNothingController : ControllerBase
    {
        [HttpGet]
        public ActionResult Get()
        {
            return Ok(42);
        }
    }
}