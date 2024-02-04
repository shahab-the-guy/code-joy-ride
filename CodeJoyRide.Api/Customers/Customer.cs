using CodeJoyRide.Fx;

namespace CodeJoyRide.Api.Customers;

public sealed class Customer
{
    public required Guid Id { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required DateTime DateOfBirth { get; init; }
    
    public Maybe<string> Email { get; private set; }

    public bool RegisterEmailAddress(string email)
    {
        if (!email.Contains('@'))
            return false;

        Email = Maybe.Some(email);

        return true;
    }
}
