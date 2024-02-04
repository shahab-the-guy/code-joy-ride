using MediatR;

namespace CodeJoyRide.Api.Customers.Queries;

public sealed class FindCustomer : IRequest<CustomerDto>
{
    public Guid CustomerId { get; init; }
}
