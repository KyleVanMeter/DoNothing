﻿using System;
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
            string folder = @"E:\Music\Main\Rap\";

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
                container.Add(new DoNothing
                {
                    Album  = tFile.Tag.Album,
                    Artist = tFile.Tag.AlbumArtists,
                    Year   = "(" + tFile.Tag.Year + ")"
                });
            }


            container.RemoveAll(x => x.Artist == null);
            return container.ToArray();

            //return Ok(queryFile.Count());
        }
    }
}