#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
using System.Collections.Immutable;
using CodeJoyRide.Api.Customers.Services.Abstractions;
using MediatR;

namespace CodeJoyRide.Api.Customers.Queries.Handlers;

public sealed class GetAllHandler(ICustomerRepository customerRepository)
    : IRequestHandler<GetAll, IReadOnlyList<CustomerDto>>
{
    public async Task<IReadOnlyList<CustomerDto>> Handle(GetAll request, CancellationToken cancellationToken)
    {
        var customers = customerRepository.GetCustomers().Select(CustomerDto.MapFrom);
        
        return customers.ToImmutableList();
    }
}
