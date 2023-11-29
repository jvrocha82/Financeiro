
using Xunit;
using FluentAssertions;
using Financial.Infra.Data.EF;

using Repository = Financial.Infra.Data.EF.Repositories;


namespace Financial.IntegrationTests.Infra.Data.EF.Repositories.BankAccountRepository;

[Collection(nameof(BankAccountRepositoryTestFixture))]
public class BankAccounRepositoryTest
{
    private readonly BankAccountRepositoryTestFixture _fixture;

    public BankAccounRepositoryTest(BankAccountRepositoryTestFixture fixture)
    =>    _fixture = fixture;
    

    [Fact(DisplayName = nameof(Insert))]
    [Trait("Integration/infra.Data", "BankAccountRepository - Repositories")]

    public async Task Insert()
    {
        FinancialDbContext dbContext = _fixture.CreateDbContext();
        var exampleBankAccount = _fixture.GetExampleBankAccount();
        var bankAccountRepository = new Repository.BankAccountRepository(dbContext);
        
        await bankAccountRepository.Insert(exampleBankAccount, CancellationToken.None);
        await dbContext.SaveChangesAsync();


        var dbBankAccount = await dbContext.BankAccount.FindAsync(exampleBankAccount.Id);
        dbBankAccount.Should().NotBeNull();
        dbBankAccount.Name.Should().Be(exampleBankAccount.Name);
        dbBankAccount.OpeningBalance.Should().Be(exampleBankAccount.OpeningBalance);
        dbBankAccount.CreatedAt.Should().Be(exampleBankAccount.CreatedAt);

    }


}
