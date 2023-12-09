using Financial.Application.Exceptions;
using Financial.Infra.Data.EF;
using Financial.Infra.Data.EF.Repositories;
using FluentAssertions;
using Xunit;
using UseCases = Financial.Application.UseCases.BankAccount.GetBankAccount;
namespace Financial.IntegrationTests.Application.UseCase.BankAccount.GetBankAccount;
[Collection(nameof(GetBankAccountTestFixture))]
public class GetBankAccountTest
{
    private readonly GetBankAccountTestFixture _fixture;

    public GetBankAccountTest(GetBankAccountTestFixture fixture)
    => _fixture = fixture;

    [Fact(DisplayName = nameof(GetBankAccount))]
    [Trait("Integration/Application", "GetBankAccount - Use Cases")]
    public async Task GetBankAccount()
    {
        FinancialDbContext dbContext = _fixture.CreateDbContext();
        var exampleBankAccount = _fixture.GetExampleBankAccount();
        dbContext.BankAccount.Add(exampleBankAccount);
        dbContext.SaveChanges();
        var repository = new BankAccountRepository(dbContext);


        var input = new UseCases.GetBankAccountInput(exampleBankAccount.Id);
        var useCase = new UseCases.GetBankAccount(repository);

        var output = await useCase.Handle(input, CancellationToken.None);


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

    [Fact(DisplayName = nameof(NotFountExceptionWhenBankAccountDoesntExist))]
    [Trait("Integration/Application", "GetBankAccount - Use Cases")]
    public async Task NotFountExceptionWhenBankAccountDoesntExist()
    {
        FinancialDbContext dbContext = _fixture.CreateDbContext();
        var exampleBankAccount = _fixture.GetExampleBankAccount();
        dbContext.BankAccount.Add(exampleBankAccount);
        dbContext.SaveChanges();
        var repository = new BankAccountRepository(dbContext);


        var input = new UseCases.GetBankAccountInput(Guid.NewGuid());
        var useCase = new UseCases.GetBankAccount(repository);

        var task = async ()
              => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"BankAccount '{input.Id}' not found.");

    }
}
