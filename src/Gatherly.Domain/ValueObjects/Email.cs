using Gatherly.Domain.Errors;
using Gatherly.Domain.Primitives;
using Gatherly.Domain.Shared;

namespace Gatherly.Domain.ValueObjects;

public sealed class Email : ValueObject
{
    public const int MaxLength = 255;

    public Email(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Email> Create(string email)
    {
        return Result.Create(email)
            .Ensure(
                e => !string.IsNullOrEmpty(e),
                DomainErrors.Email.Empty)
            .Ensure(
                e => e.Length <= MaxLength,
                DomainErrors.Email.TooLong)
            .Ensure(
                e => e.Split('@').Length == 2,
                DomainErrors.Email.InvalidFormat)
            .Map(e => new Email(e));
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
