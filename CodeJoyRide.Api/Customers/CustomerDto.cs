namespace CodeJoyRide.Api.Customers;

public sealed class CustomerDto
{
    public required Guid Id { get; init; }
    public required string FullName { get; init; }
    public string? Email { get; init; }

    private CustomerDto()
    {
    }

    public static CustomerDto MapFrom(Customer customer)
        => new()
        {
            Id = customer.Id, FullName = $"{customer.FirstName} {customer.LastName}",
            Email = customer.Email.Unwrap(string.Empty)
        };
}
