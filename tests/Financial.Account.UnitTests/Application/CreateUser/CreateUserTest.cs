using Financial.Domain.Entity;
using FluentAssertions;
using Moq;
using Xunit;
using UseCases = Financial.Application.UseCases.User.CreateUser;

namespace Financial.UnitTests.Application.CreateUser;
[Collection(nameof(CreateUserTestFixture))]
public class CreateUserTest
{
    private readonly CreateUserTestFixture _fixture;

    public CreateUserTest(CreateUserTestFixture fixture)
    => _fixture = fixture;

    [Fact(DisplayName = nameof(CreateUser))]
    [Trait("Application", "CreateUser - Use Cases")]
    public async void createUser()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

               var useCase = new UseCases.CreateUser(
                repositoryMock.Object,
                unitOfWorkMock.Object
            );
        var input = _fixture.GetInput();

        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(
            repository => repository.Insert(
                It.IsAny<User>(),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
        unitOfWorkMock.Verify(
            uow => uow.Commit(It.IsAny<CancellationToken>()),
            Times.Once
        );

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.IsActive.Should().Be(input.IsActive);
        (output.Id != Guid.Empty).Should().BeTrue();
        (output.CreatedAt != default(DateTime)).Should().BeTrue();

    }
}
