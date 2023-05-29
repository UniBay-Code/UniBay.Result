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
    public async Task<IActionResult> AddBook([FromBody] AddBookCommand command, [FromQuery]bool throwOnError = false) => await HandleAsync(command, throwOnError);
    
    [HttpGet("{id}/unknown-error")]
    public async Task<IActionResult> UnknownError([FromRoute] int id, [FromQuery] string role, [FromQuery]bool throwOnError = false)
        => await HandleAsync(new GetBookQuery(id, role, false, true), throwOnError);
    
    [HttpGet("{id}/service-error")]
    public async Task<IActionResult> GetBookWithError([FromRoute] int id, [FromQuery] string role, [FromQuery]bool throwOnError = false)
        => await HandleAsync(new GetBookQuery(id, role, true), throwOnError);

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBook([FromRoute] int id, [FromQuery] string role, [FromQuery]bool throwOnError = false)
        => await HandleAsync(new GetBookQuery(id, role), throwOnError);

    private async Task<IActionResult> HandleAsync<TResult>(IRequest<Result<TResult>> request, bool throwOnError = false)
    {
        var result = await this.mediator.Send(request);
        if (throwOnError && result.IsFailed) throw result.Exception;
        return result.ToActionResult();
    }
}