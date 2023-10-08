using FluentAssertions;
using Moq;
using Xunit;
using UseCases = Financial.Application.UseCases.Account.GetAccount;

namespace Financial.UnitTests.Application.Account.GetAccount;
[Collection(nameof(GetAccountTestFixture))]
public class GetAccountTest
{
    private readonly GetAccountTestFixture _fixture;

    public GetAccountTest(GetAccountTestFixture fixture)
    => _fixture = fixture;


    [Fact(DisplayName = nameof(GetAccount))]
    [Trait("Application", "GetAccount - Use Cases")]
    public async Task GetAccount()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var exampleAccount = _fixture.GetValidAccount();
        repositoryMock.Setup(x => x.Get(
            It.IsAny<Guid>(), 
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(exampleAccount);

        var input = new UseCases.GetAccountInput(exampleAccount.Id);
        var useCase = new UseCases.GetAccount(repositoryMock.Object);

        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(x => x.Get(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()
            ), Times.Once);

        output.Should().NotBeNull();
        output.Name.Should().Be(exampleAccount.Name);
        output.OpeningBalance.Should().Be(exampleAccount.OpeningBalance);
        output.OpeningBalanceIsNegative.Should().Be(exampleAccount.OpeningBalanceIsNegative);
        output.IsActive.Should().Be(exampleAccount.IsActive);
        (output.Id != Guid.Empty).Should().BeTrue();
        output.Id.Should().Be(exampleAccount.Id);
        output.CreatedAt.Should().Be(exampleAccount.CreatedAt);
        (output.CreatedAt != default).Should().BeTrue();
    }
    
}
