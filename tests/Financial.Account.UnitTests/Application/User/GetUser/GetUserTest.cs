using Moq;
using Xunit;
using FluentAssertions;
using UseCase = Financial.Application.UseCases.User.GetUser;

namespace Financial.UnitTests.Application.User.GetUser;
[Collection(nameof(GetUserTestFixture))]
public class GetUserTest
{
    private readonly GetUserTestFixture _fixture;

    public GetUserTest(GetUserTestFixture fixture)
    => _fixture = fixture;

    [Fact(DisplayName = nameof(GetUser))]
    [Trait("Application", "GetUser - Use Cases")]
    public async Task GetUser()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var exampleUser = _fixture.GetValidUser();

        repositoryMock.Setup(x => x.Get(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(exampleUser);

        var input = new UseCase.GetUserInput(exampleUser.Id);
        var useCase = new UseCase.GetUser(repositoryMock.Object);

        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(x => x.Get(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()
        ), Times.Once);

        output.Should().NotBeNull();

        
        output.Id.Should().NotBe(default(Guid));
        output.Name.Should().Be(exampleUser.Name);
        output.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        output.CreatedAt.Should().Be(exampleUser.CreatedAt);
        output.IsActive.Should().BeTrue();
    }
  
    
}
