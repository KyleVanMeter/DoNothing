using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using TagLib;

namespace test02.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DoNothingController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<DoNothing> Get()
        {
            string folder = @"E:\Music\Main\Bleep\";

            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(folder);
            IEnumerable<System.IO.FileInfo> fileList = dir.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

            var queryFile = from file in fileList select file;
            var queryMP3 = from file in queryFile 
                           where file.Extension == ".mp3" 
                           orderby file.Name select file;
            var queryFLAC = from file in queryFile 
                            where file.Extension == ".flac" 
                            orderby file.Name select file;

            List<DoNothing> container = new List<DoNothing>
            {
                new DoNothing { }
            };

            foreach(System.IO.FileInfo fi in queryMP3)
            {
                var tFile = TagLib.File.Create(fi.FullName);
                Func<TagLib.File, string> convertTime = (x) => { 
                    if (x.Properties.Duration.Hours == 0)
                    {
                        return x.Properties.Duration.Minutes 
                        + ":" + x.Properties.Duration.Seconds;
                    } else
                    {
                        return x.Properties.Duration.Hours
                        + ":" + x.Properties.Duration.Minutes
                        + ":" + x.Properties.Duration.Seconds;
                    }
                };

                container.Add(new DoNothing
                {
                    Track  = tFile.Tag.Disc + "." + tFile.Tag.Track,
                    Album  = tFile.Tag.Album,
                    Artist = tFile.Tag.AlbumArtists,
                    Year   = "(" + tFile.Tag.Year + ")",
                    Time   = convertTime(tFile)
                });
            }

            container = container.OrderBy(x => x.Album).ToList();
            container.RemoveAll(x => x.Artist == null);

            return container.ToArray();
        }
    }
}