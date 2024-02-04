namespace CodeJoyRide.Api.Customers;

public class CustomerNotFoundException : Exception
{
    public CustomerNotFoundException(string email) : base($"Customer with email: {email} was not found")
    {
    }

    public CustomerNotFoundException(Guid customerId) : base($"Customer with id: {customerId} was not found")
    {
    }
}