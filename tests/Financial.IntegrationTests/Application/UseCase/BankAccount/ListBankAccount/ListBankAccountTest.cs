using Financial.Application.UseCases.BankAccount.Common;
using Financial.Application.UseCases.BankAccount.ListBankAccount;
using Financial.Domain.SeedWork.SearchableRepository;
using Financial.Infra.Data.EF;
using Financial.Infra.Data.EF.Repositories;
using FluentAssertions;
using Xunit;
using ApplicationUseCases = Financial.Application.UseCases.BankAccount.ListBankAccount;
namespace Financial.IntegrationTests.Application.UseCase.BankAccount.ListBankAccount;

[Collection(nameof(ListBankAccountTestFixture))]
public class ListBankAccountTest

{
    private readonly ListBankAccountTestFixture _fixture;

    public ListBankAccountTest(ListBankAccountTestFixture fixture)
    => _fixture = fixture;


    [Fact(DisplayName = nameof(List))]
    [Trait("Integration/Application", "ListBankAccount - Use Cases")]
    public async Task List()
    {
        FinancialDbContext dbContext = _fixture.CreateDbContext(); 
        var bankAccountExampleList = _fixture.GetExampleBankAccountList(15);
        await dbContext.AddRangeAsync(bankAccountExampleList);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        var bankAccountRepository = new BankAccountRepository(dbContext);
        var input = new ListBankAccountInput(1, 20);

        var useCase = new ApplicationUseCases.ListBankAccount(bankAccountRepository);

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(bankAccountExampleList.Count);
        output.Items.Should().HaveCount(bankAccountExampleList.Count);
        ((List<BankAccountModelOutput>)output.Items).ForEach(outputItem =>
        {
            var repositoryBankAccount = output.Items
                .FirstOrDefault(x => x.Id == outputItem.Id);
            outputItem.Should().NotBeNull();
            outputItem.Name.Should().Be(repositoryBankAccount!.Name);
            outputItem.OpeningBalance.Should().Be(repositoryBankAccount.OpeningBalance);
            outputItem.OpeningBalanceIsNegative.Should().Be(repositoryBankAccount.OpeningBalanceIsNegative);
            outputItem.IsActive.Should().Be(repositoryBankAccount.IsActive);
            (outputItem.Id != Guid.Empty).Should().BeTrue();
            (outputItem.CreatedAt != default).Should().BeTrue();
        });

    }

    [Fact(DisplayName = nameof(ListReturnsEmptyWhenEmpty))]
    [Trait("Integration/Application", "ListBankAccount - Use Cases")]
    public async Task ListReturnsEmptyWhenEmpty()
    {
        FinancialDbContext dbContext = _fixture.CreateDbContext();

        var bankAccountRepository = new BankAccountRepository(dbContext);
        var input = new ListBankAccountInput(1, 20);

        var useCase = new ApplicationUseCases.ListBankAccount(bankAccountRepository);

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(0);
        output.Items.Should().HaveCount(0);
       

    }

}
