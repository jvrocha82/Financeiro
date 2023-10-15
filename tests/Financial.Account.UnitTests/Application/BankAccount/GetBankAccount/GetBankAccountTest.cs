using FluentAssertions;
using Moq;
using Xunit;
using UseCases = Financial.Application.UseCases.BankAccount.GetBankAccount;

namespace Financial.UnitTests.Application.BankAccount.GetBankAccount;
[Collection(nameof(GetBankAccountTestFixture))]
public class GetBankAccountTest
{
    private readonly GetBankAccountTestFixture _fixture;

    public GetBankAccountTest(GetBankAccountTestFixture fixture)
    => _fixture = fixture;


    [Fact(DisplayName = nameof(GetBankAccount))]
    [Trait("Application", "GetBankAccount - Use Cases")]
    public async Task GetBankAccount()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var exampleBankAccount = _fixture.GetValidAccount();
        repositoryMock.Setup(x => x.Get(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(exampleBankAccount);

        var input = new UseCases.GetBankAccountInput(exampleBankAccount.Id);
        var useCase = new UseCases.GetBankAccount(repositoryMock.Object);

        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(x => x.Get(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()
            ), Times.Once);

        output.Should().NotBeNull();
        output.Name.Should().Be(exampleBankAccount.Name);
        output.OpeningBalance.Should().Be(exampleBankAccount.OpeningBalance);
        output.OpeningBalanceIsNegative.Should().Be(exampleBankAccount.OpeningBalanceIsNegative);
        output.IsActive.Should().Be(exampleBankAccount.IsActive);
        (output.Id != Guid.Empty).Should().BeTrue();
        output.Id.Should().Be(exampleBankAccount.Id);
        output.CreatedAt.Should().Be(exampleBankAccount.CreatedAt);
        (output.CreatedAt != default).Should().BeTrue();
    }

}
