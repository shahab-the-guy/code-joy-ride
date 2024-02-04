using CodeJoyRide.Fx;

namespace CodeJoyRide.Api.Customers.Services.Abstractions;

public interface ICustomerRepository
{
    Maybe<Customer> Find(Guid customerId);
    Maybe<Customer> Find(string email);

    IEnumerable<Customer> GetCustomers();
}
