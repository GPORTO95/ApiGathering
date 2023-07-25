using FluentAssertions;
using Gatherly.Application.Members.Commands.CreateMember;
using Gatherly.Domain.Entities;
using Gatherly.Domain.Errors;
using Gatherly.Domain.Repositories;
using Gatherly.Domain.Shared;
using Gatherly.Domain.ValueObjects;
using Moq;

namespace Gatherly.Application.UnitTests.Members.Commands;

public class CreateMemberCommandHandlerTests
{
    private readonly Mock<IMemberRepository> _memberRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public CreateMemberCommandHandlerTests()
    {
        _memberRepositoryMock = new();
        _unitOfWorkMock = new();
    }

    [Fact]
    public async Task Handle_Should_ReturnFailureResult_WhenEmailIsNotUnique()
    {
        // Arrange
        //Act
        Result<Guid> result = await Execute(false);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Member.EmailAlreadyInUse);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccessResult_WhenEmailIsUnique()
    {
        // Arrange
        // Act
        Result<Guid> result = await Execute(true);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Handle_Should_CallAddOnRepository_WhenEmailIsUnique()
    {
        // Arrange
        // Act
        Result<Guid> result = await Execute(true);

        // Assert
        _memberRepositoryMock.Verify(
            x => x.Add(It.Is<Member>(m => m.Id == result.Value)),
            Times.Once);
    }

    [Fact]
    public async Task Handle_Should_NotCallUnitOfWord_WhenEmailIsNotUnique()
    {
        // Arrange
        // Act
        await Execute(false);

        // Assert
        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }

    private async Task<Result<Guid>> Execute(bool emailIsUnique)
    {
        // Arrange
        var command = new CreateMemberCommand("email@test.com", "first", "last");

        _memberRepositoryMock.Setup(
            x => x.IsEmailUniqueAsync(
                It.IsAny<Email>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(emailIsUnique);

        var handler = new CreateMemberCommandHandler(
            _memberRepositoryMock.Object,
            _unitOfWorkMock.Object);

        // Act
        return await handler.Handle(command, default);
    }
}
