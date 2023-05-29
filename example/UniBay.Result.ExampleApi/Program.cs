using UniBay.Result;
using UniBay.Result.AspNetCore;
using UniBay.Result.AspNetCore.ResponseMapper;
using UniBay.Result.ExampleApi.Domain;
using UniBay.Result.ExampleApi.Infrastructure;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
builder.Services.AddSingleton<IBooksRepository, BooksRepository>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Log when certain error type occurs.
app.UseResultLogger();

// When ServiceError is received return custom error model
ResultResponseMapper.RegisterErrorMapper(ResultCode.ServiceError, result => new
{
    Details = result.Exception.Message,
});
// For AuthorizationFailed return the exception message
ResultResponseMapper.RegisterErrorMapper(ResultCode.AuthorizationFailed, result => new
{
    Details = result.Exception.Message,
});

app.Run();