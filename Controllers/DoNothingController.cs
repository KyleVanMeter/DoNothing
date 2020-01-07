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
            string folder = @"C:\AMD\";

            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(folder);
            IEnumerable<System.IO.FileInfo> fileList = dir.GetFiles("*.*", System.IO.SearchOption.TopDirectoryOnly);

            var queryFile = from file in fileList where file.Extension == ".txt" select file.FullName;

            return Ok(queryFile.Count());
        }
    }
}