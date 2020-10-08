using CinemaFest.Application.Features.Festival.Commands;
using CinemaFest.Application.Features.Festival.Queries;
using CinemaFest.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CinemaFest.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    public class FestivalController : BaseApiController
    {
        private readonly IMediator mediator;
        public FestivalController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        // GET: api/<controller>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await mediator.Send(new GetAllFestivalsQuery()));
        }

        //POST: api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateFestivalCommand command)
        {
            return Ok(await mediator.Send(command));
        }
        
    }
}
