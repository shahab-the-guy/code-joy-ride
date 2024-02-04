#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
using CodeJoyRide.Api.Customers.Services.Abstractions;
using MediatR;

namespace CodeJoyRide.Api.Customers.Queries.Handlers;

public sealed class FindCustomerHandler : IRequestHandler<FindCustomer, CustomerDto>
{
    private readonly ICustomerRepository _customerRepository;

    // ReSharper disable once ConvertToPrimaryConstructor
    public FindCustomerHandler(ICustomerRepository customerRepository) => _customerRepository = customerRepository;

    public async Task<CustomerDto> Handle(FindCustomer request, CancellationToken cancellationToken)
    {
        var customer =  _customerRepository.Find(request.CustomerId).UnwrappedValue;
        return CustomerDto.MapFrom(customer);
    }
}
