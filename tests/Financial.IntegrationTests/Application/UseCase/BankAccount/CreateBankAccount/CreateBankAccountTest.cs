using Financial.Infra.Data.EF;
using Financial.Infra.Data.EF.Repositories;
using FluentAssertions;
using Xunit;
using ApplicationUseCases = Financial.Application.UseCases.BankAccount.CreateBankAccount;
namespace Financial.IntegrationTests.Application.UseCase.BankAccount.CreateBankAccount;
[Collection(nameof(CreateBankAccountTestFixture))]
public class CreateBankAccountTest
{
    private readonly CreateBankAccountTestFixture _fixture;

    public CreateBankAccountTest(CreateBankAccountTestFixture fixture)
    => _fixture = fixture;

    [Fact(DisplayName = nameof(CreateBankAccount))]
    [Trait("Integration/Application", "CreateBankAccount - Use Cases")]
    public async void CreateBankAccount()
    {
        FinancialDbContext dbContext = _fixture.CreateDbContext();
        var repository = new BankAccountRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var useCase = new ApplicationUseCases.CreateBankAccount(
            repository,
            unitOfWork
            );
        var input = _fixture.GetInput();

        var output = await useCase.Handle(input, CancellationToken.None);

        var dbBankAccount = await (_fixture.CreateDbContext(true))
          .BankAccount.FindAsync(output.Id);
        dbBankAccount.Should().NotBeNull();
        dbBankAccount!.Name.Should().Be(output.Name);
        dbBankAccount.OpeningBalance.Should().Be(output.OpeningBalance);
        dbBankAccount.CreatedAt.Should().Be(output.CreatedAt);

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.OpeningBalance.Should().Be(input.OpeningBalance);
        output.OpeningBalanceIsNegative.Should().Be(input.OpeningBalanceIsNegative);
        output.IsActive.Should().Be(input.IsActive);
        (output.Id != Guid.Empty).Should().BeTrue();
        (output.CreatedAt != default).Should().BeTrue();

    }
}
