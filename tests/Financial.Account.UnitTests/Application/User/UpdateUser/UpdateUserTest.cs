using Financial.Application.UseCases.User.UpdateUser;
using FluentAssertions;
using Moq;
using Xunit;
using DomainEntity = Financial.Domain.Entity;
using UseCases = Financial.Application.UseCases.User.UpdateUser;
namespace Financial.UnitTests.Application.User.UpdateUser;
[Collection(nameof(UpdateUserTestFixture))]
public class UpdateUserTest
{
    private readonly UpdateUserTestFixture _fixture;

    public UpdateUserTest(UpdateUserTestFixture fixture)
    => _fixture = fixture;

    [Theory(DisplayName = nameof(UpdateUserOk))]
    [Trait("Application", "Update User - Use Cases")]
    [MemberData(
        nameof(UpdateUserTestDataGenerator.GetUsersToUpdate),
        parameters: 10,
        MemberType = typeof(UpdateUserTestDataGenerator)
        )]
    public async Task UpdateUserOk(
        DomainEntity.User exampleUser,
        UpdateUserInput input)
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

        repositoryMock.Setup(x => x.Get(
            exampleUser.Id,
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(exampleUser);

        var useCase = new UseCases.UpdateUser(
            repositoryMock.Object,
            unitOfWorkMock.Object
        );

        UpdateUserOutput output = await useCase.Handle(input, CancellationToken.None);
        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);

        repositoryMock.Verify(x => x.Get(
            exampleUser.Id,
            It.IsAny<CancellationToken>()),
        Times.Once);
        
        repositoryMock.Verify(x => x.Update(
            exampleUser,
            It.IsAny<CancellationToken>()),
        Times.Once);

        unitOfWorkMock.Verify(x => x.Commit(
            It.IsAny<CancellationToken>()),
        Times.Once );

    }
}
