using System.Reflection;
using CodeJoyRide.Api.Customers;
using CodeJoyRide.Api.Customers.Queries;
using CodeJoyRide.Api.Customers.Services;
using CodeJoyRide.Api.Customers.Services.Abstractions;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddSingleton<ICustomerRepository, CustomerRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/customers", async (IMediator mediator, CancellationToken cancellationToken) =>
    {
        var result = await mediator.Send(new GetAll(), cancellationToken);

        return result;
    })
    .WithName("GetAllCustomers")
    .WithOpenApi();

app.MapGet("/customers/{id:guid}", async (IMediator mediator, Guid id, CancellationToken cancellationToken) =>
    {
        var result = await mediator.Send(new FindCustomer { CustomerId = id }, cancellationToken);

        return result;
    })
    .WithName("GetCustomerById")
    .WithOpenApi();

app.Run();
    
