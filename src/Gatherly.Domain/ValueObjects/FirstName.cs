using Gatherly.Domain.Errors;
using Gatherly.Domain.Primitives;
using Gatherly.Domain.Shared;

namespace Gatherly.Domain.ValueObjects;

public sealed class FirstName : ValueObject
{
    public const int MaxLength = 50;

    private FirstName(string value)
    {
        Value = value;
    }

    private FirstName()
    {
        
    }

    public string Value { get; private set; }

    public static Result<FirstName> Create(string firstName)
    {
        Ensure.NotNullOrWithSpace(firstName, DomainErrors.LastName.Empty);
        Ensure.NotGreaterTan(firstName.Length, MaxLength, DomainErrors.LastName.TooLong);

        return new FirstName(firstName);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
