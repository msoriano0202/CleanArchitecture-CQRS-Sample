using CleanArchitecture.Application.Features.Actors.Queries.PaginationActor;
using CleanArchitecture.Application.Features.Actors.Queries.Vms;
using CleanArchitecture.Application.Features.Shared.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CleanArchitecture.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ActorController : ControllerBase
    {
        private IMediator _mediator;

        public ActorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("pagination", Name = "PaginationActor")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PaginationVm<ActorVm>))]
        public async Task<ActionResult<PaginationVm<ActorVm>>> GetPaginationActor([FromQuery] PaginationActorQuery PaginationActorQuery) 
        {
            var paginationActor = await _mediator.Send(PaginationActorQuery);
            return Ok(paginationActor);
        }
    }
}
