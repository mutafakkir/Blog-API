using System.Net.Mime;
using System.IO;
using System.Threading.Tasks;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;
using api.Entity;
using System.Linq;
using System;
using api.Mappers;

namespace api.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class MediaController : ControllerBase
    {
        private readonly IMediaService _mds;

        public MediaController(IMediaService mediaservice)
        {
            _mds = mediaservice;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromForm] NewMedia media)
        {
            var images = media.Data.Select(f => 
            {
                using var stream = new MemoryStream();
                f.CopyTo(stream);
                return new Media(contentType: f.ContentType, data: stream.ToArray());
            }).ToList();

            var result = await _mds.InsertAsync(images);
            if(result.IsSuccess)
            {
                var ImagePostId = media.Data.Select(m => m.GetMediaEntity()).ToList();

                return Ok(ImagePostId.Select(m => new { 
                    Id = m.Id,
                    ContentType = m.ContentType
                    }).ToList());
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
            => Ok(await _mds.GetAllAsync());

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var file = await _mds.GetAsync(id);
            var stream = new MemoryStream(file.Data);
            return File(stream.ToArray(), file.ContentType);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var result = await _mds.DeleteAsync(id);
            if(result.IsSuccess)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
