using MediatR;
using Microsoft.AspNetCore.Mvc;
using UniBay.Result.AspNetCore;
using UniBay.Result.ExampleApi.Application.Commands;

namespace UniBay.Result.ExampleApi.Controllers;

[ApiController]
[Route("[controller]")]
public class BooksController : ControllerBase
{
    private readonly IMediator mediator;

    public BooksController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> AddBook([FromBody] AddBookCommand command) => await HandleAsync(command);
    
    [HttpGet("{id}/unknown-error")]
    public async Task<IActionResult> UnknownError([FromRoute] int id, [FromQuery] string role)
        => await HandleAsync(new GetBookQuery(id, role, false, true));
    
    [HttpGet("{id}/service-error")]
    public async Task<IActionResult> GetBookWithError([FromRoute] int id, [FromQuery] string role)
        => await HandleAsync(new GetBookQuery(id, role, true));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBook([FromRoute] int id, [FromQuery] string role)
        => await HandleAsync(new GetBookQuery(id, role));

    private async Task<IActionResult> HandleAsync<TResult>(IRequest<Result<TResult>> request)
    {
        var result = await this.mediator.Send(request);
        return result.ToActionResult();
    }
}