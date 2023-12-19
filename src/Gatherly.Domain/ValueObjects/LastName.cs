using Gatherly.Domain.Errors;
using Gatherly.Domain.Primitives;
using Gatherly.Domain.Shared;

namespace Gatherly.Domain.ValueObjects;

public sealed class LastName : ValueObject
{
    public const int MaxLength = 50;

    public LastName(string value)
    {
        Value = value;
    }

    private LastName()
    {
        
    }

    public string Value { get; private set; }

    public static Result<LastName> Create(string lastName)
    {
        Ensure.NotNullOrWithSpace(lastName, DomainErrors.LastName.Empty);
        Ensure.NotGreaterTan(lastName.Length, MaxLength, DomainErrors.LastName.TooLong);

        return new LastName(lastName);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
