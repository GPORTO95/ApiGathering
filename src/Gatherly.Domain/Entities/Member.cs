using Gatherly.Domain.DomainEvents;
using Gatherly.Domain.Primitives;
using Gatherly.Domain.ValueObjects;

namespace Gatherly.Domain.Entities;

public sealed class Member : AggregateRoot, IAuditableEntity
{
    private Member(Guid id, FirstName firstName, LastName lastName, Email email)
        : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    private Member() { }

    public FirstName FirstName { get; private set; }

    public LastName LastName { get; private set; }

    public Email Email { get; private set; }

    public DateTime CreatedOnUtc { get; set; }

    public DateTime? ModifiedOnUtc { get; set; }

    public ICollection<Role> Roles { get; set; }

    public static Member Create(Guid id, FirstName firstName, LastName lastName, Email email)
    {
        var member = new Member(id, firstName, lastName, email);

        member.RaiseDomainEvent(new MemberRegisteredDomainEvent(
            Guid.NewGuid(),
            member.Id));

        return member;
    }

    public void ChangeName(FirstName firstName, LastName lastName) 
    {
        if(!FirstName.Equals(firstName) || !LastName.Equals(lastName))
        {
            RaiseDomainEvent(new MemberNameChangedDomainEvent(
                Guid.NewGuid(), Id));
        }

        FirstName = firstName;
        LastName = lastName;
    }
}
