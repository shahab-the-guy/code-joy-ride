using System.Collections.Immutable;
using Bogus;
using CodeJoyRide.Api.Customers.Services.Abstractions;
using CodeJoyRide.Fx;

namespace CodeJoyRide.Api.Customers.Services;

public sealed class CustomerRepository : ICustomerRepository
{
    private static readonly Faker<Customer> CustomerGenerator = new Faker<Customer>("en")
        .RuleFor(x => x.Id, _ => Guid.NewGuid())
        .RuleFor(x => x.FirstName, f => f.Person.FirstName)
        .RuleFor(x => x.LastName, f => f.Person.LastName)
        .RuleFor(x => x.DateOfBirth, f => f.Person.DateOfBirth.Date)
        .RuleFor(x => x.Email , f => f.Person.Email );

    private static readonly Dictionary<Guid, Customer> Customers;
    
    static CustomerRepository()
        => Customers = CustomerGenerator.Generate(30).ToDictionary(customer => customer.Id);


    public Maybe<Customer> Find(Guid customerId)
    {
        var valueExists = Customers.TryGetValue(customerId, out var customer);
        if (!valueExists)
            return CodeJoyRide.Fx.Maybe.None;

        return Maybe.Some(customer!);
    }

    public Maybe<Customer> Find(string email)
    {
        var foundedCustomer = Customers
            .Select(c => c.Value)
            .FirstOrDefault(c => c.Email.IsSome && c.Email.UnwrappedValue == email);

        if (foundedCustomer is null)
            throw new CustomerNotFoundException(email);
        
        return Maybe.Some(foundedCustomer);
    }

    public IEnumerable<Customer> GetCustomers() => Customers.Select(c => c.Value).ToImmutableList();
}
