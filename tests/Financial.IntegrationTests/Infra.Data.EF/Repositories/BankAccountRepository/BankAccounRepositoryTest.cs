
using Xunit;
using FluentAssertions;
using Financial.Infra.Data.EF;

using Repository = Financial.Infra.Data.EF.Repositories;
using Financial.Application.Exceptions;

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
    [Fact(DisplayName = nameof(Get))]
    [Trait("Integration/infra.Data", "BankAccountRepository - Repositories")]

    public async Task Get()
    {
        FinancialDbContext dbContext = _fixture.CreateDbContext();
        var exampleBankAccount = _fixture.GetExampleBankAccount();

        var exampleBankAccountList = _fixture.GetExampleBankAccountList(15);
        exampleBankAccountList.Add(exampleBankAccount);

        await dbContext.AddRangeAsync(exampleBankAccountList);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        var bankAccountRepository = new Repository.BankAccountRepository(dbContext);




        var dbBankAccount = await bankAccountRepository.Get(exampleBankAccount.Id, CancellationToken.None);


        dbBankAccount.Should().NotBeNull();
        dbBankAccount.Id.Should().Be(exampleBankAccount.Id);
        dbBankAccount.Name.Should().Be(exampleBankAccount.Name);
        dbBankAccount.OpeningBalance.Should().Be(exampleBankAccount.OpeningBalance);
        dbBankAccount.CreatedAt.Should().Be(exampleBankAccount.CreatedAt);

    }
    [Fact(DisplayName = nameof(GetThrowIfNotFound))]
    [Trait("Integration/infra.Data", "BankAccountRepository - Repositories")]

    public async Task GetThrowIfNotFound()
    {
        FinancialDbContext dbContext = _fixture.CreateDbContext();
        var exampleId = Guid.NewGuid();


        await dbContext.AddRangeAsync(_fixture.GetExampleBankAccountList(15));
        await dbContext.SaveChangesAsync(CancellationToken.None);
        var bankAccountRepository = new Repository.BankAccountRepository(dbContext);




        var task = async () => await bankAccountRepository.Get(
            exampleId,
            CancellationToken.None);

        await task.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"BankAccount '{exampleId}' not found.");


    }




}
