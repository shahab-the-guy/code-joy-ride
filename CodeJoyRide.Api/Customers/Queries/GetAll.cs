using MediatR;

namespace CodeJoyRide.Api.Customers.Queries;

public sealed class GetAll : IRequest<IReadOnlyList<CustomerDto>>
{
}
