namespace Gatherly.Application.Members.Queries.GetMemberById;

public record MemberResponse(Guid Id, string Email, string FirstName, string LastName);
