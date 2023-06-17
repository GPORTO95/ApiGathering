using Gatherly.Application.Members.Commands.CreateMember;
using Gatherly.Domain.Shared;
using Gatherly.Presentation.Abstractions;
using Gatherly.Presentation.Contracts.Members;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gatherly.Presentation.Controllers;

[Route("api/members")]
public sealed class MembersController : ApiController
{
    public MembersController(ISender sender) 
        : base(sender)
    { }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetMemberId(Guid id, CancellationToken cancellationToken)
    {
        //var query = new 

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> RegisterMember(
        [FromBody] RegisterMemberRequest request,
        CancellationToken cancellationToken)
    {
        //var command = new CreateMemberCommand(
        //    request.Email,
        //    request.FirstName,
        //    request.LastName);

        //Result<Guid> result = await Sender.Send(command, cancellationToken);

        return Ok();
    }

}
