using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GsCore.Api.V1.Contracts.Responses;
using GsCore.Api.V1.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace GsCore.Api.V1.Controllers
{
    [Route(ApiRoutes.RootRoute.Root)]
    [ApiController]
    public class RootController: ControllerBase
    {
        [HttpGet(Name = "GetRoot")]
        public IActionResult GetRoot()
        {
            var links = new List<LinkModel>();
            
            links.Add(new LinkModel(Url.Link("GetRoot",new{version = HttpContext.GetRequestedApiVersion().ToString()}),"self","GET"));
            links.Add(new LinkModel(Url.Link("GetArtists", new { version = HttpContext.GetRequestedApiVersion().ToString()}), "artists", "GET"));
            links.Add(new LinkModel(Url.Link("GetGenres", new { version = HttpContext.GetRequestedApiVersion().ToString() }), "genres", "GET"));
            links.Add(new LinkModel(Url.Link("GetAlbums", new { version = HttpContext.GetRequestedApiVersion().ToString() }), "albums", "GET"));


            return Ok(links);

        }
    }
}
