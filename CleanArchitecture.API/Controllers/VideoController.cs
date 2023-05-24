using CleanArchitecture.Application.Features.Shared.Queries;
using CleanArchitecture.Application.Features.Videos.Queries.GetVideosList;
using CleanArchitecture.Application.Features.Videos.Queries.PaginationVideo;
using CleanArchitecture.Application.Features.Videos.Queries.Vms;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CleanArchitecture.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class VideoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VideoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{username}", Name = "GetVideo")]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<VideosVm>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<VideosVm>>> GetVideosByUsername(string username)
        {
            var query = new GetVideosListQuery(username);
            var videos = await _mediator.Send(query);
            return Ok(videos);
        }

        [Authorize]
        [HttpGet("pagination", Name = "PaginationVideo")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PaginationVm<VideosWithIncludesVm>))]
        public async Task<ActionResult<PaginationVm<VideosWithIncludesVm>>> GetPaginationVideo([FromQuery] PaginationVideoQuery paginationVideoParams)
        {
            var paginationVideo = await _mediator.Send(paginationVideoParams);
            return Ok(paginationVideo);
        }
    }

}
