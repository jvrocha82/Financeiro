
using Xunit;
using FluentAssertions;
using Financial.Infra.Data.EF;

using Repository = Financial.Infra.Data.EF.Repositories;
using Financial.Application.Exceptions;
using Financial.Domain.SeedWork.SearchableRepository;
using Financial.Domain.Entity;

namespace Financial.IntegrationTests.Infra.Data.EF.Repositories.BankAccountRepository;

[Collection(nameof(BankAccountRepositoryTestFixture))]
public class BankAccounRepositoryTest
{
    private readonly BankAccountRepositoryTestFixture _fixture;

    public BankAccounRepositoryTest(BankAccountRepositoryTestFixture fixture)
    => _fixture = fixture;


    [Fact(DisplayName = nameof(Insert))]
    [Trait("Integration/infra.Data", "BankAccountRepository - Repositories")]

    public async Task Insert()
    {
        FinancialDbContext dbContext = _fixture.CreateDbContext();
        var exampleBankAccount = _fixture.GetExampleBankAccount();
        var bankAccountRepository = new Repository.BankAccountRepository(dbContext);

        await bankAccountRepository.Insert(exampleBankAccount, CancellationToken.None);
        await dbContext.SaveChangesAsync();


        var dbBankAccount = await (_fixture.CreateDbContext(true))
            .BankAccount.FindAsync(exampleBankAccount.Id);
        dbBankAccount.Should().NotBeNull();
        dbBankAccount!.Name.Should().Be(exampleBankAccount.Name);
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
        var bankAccountRepository = new Repository.BankAccountRepository(
            _fixture.CreateDbContext(true)
            );




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
        var bankAccountRepository = new Repository.BankAccountRepository(_fixture.CreateDbContext(true));




        var task = async () => await bankAccountRepository.Get(
            exampleId,
            CancellationToken.None);

        await task.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"BankAccount '{exampleId}' not found.");


    }


    [Fact(DisplayName = nameof(Update))]
    [Trait("Integration/infra.Data", "BankAccountRepository - Repositories")]

    public async Task Update()
    {
        FinancialDbContext dbContext = _fixture.CreateDbContext();
        var exampleBankAccount = _fixture.GetExampleBankAccount();
        var newBankAccountValues = _fixture.GetExampleBankAccount();
        var exampleBankAccountList = _fixture.GetExampleBankAccountList(15);
        exampleBankAccountList.Add(exampleBankAccount);

        await dbContext.AddRangeAsync(exampleBankAccountList);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        var bankAccountRepository = new Repository.BankAccountRepository(dbContext);

        exampleBankAccount.Update(newBankAccountValues.Name, newBankAccountValues.OpeningBalance);
        await bankAccountRepository.Update(exampleBankAccount, CancellationToken.None);
        await dbContext.SaveChangesAsync();

        var dbBankAccount = await (_fixture.CreateDbContext(true))
            .BankAccount.FindAsync(exampleBankAccount.Id);

        dbBankAccount.Should().NotBeNull();
        dbBankAccount!.Id.Should().Be(exampleBankAccount.Id);
        dbBankAccount.Name.Should().Be(exampleBankAccount.Name);
        dbBankAccount.OpeningBalance.Should().Be(exampleBankAccount.OpeningBalance);
        dbBankAccount.CreatedAt.Should().Be(exampleBankAccount.CreatedAt);

    }
    [Fact(DisplayName = nameof(Delete))]
    [Trait("Integration/infra.Data", "BankAccountRepository - Repositories")]

    public async Task Delete()
    {
        FinancialDbContext dbContext = _fixture.CreateDbContext();
        var exampleBankAccount = _fixture.GetExampleBankAccount();
        var exampleBankAccountList = _fixture.GetExampleBankAccountList(15);
        exampleBankAccountList.Add(exampleBankAccount);

        await dbContext.AddRangeAsync(exampleBankAccountList);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        var bankAccountRepository = new Repository.BankAccountRepository(dbContext);

        await bankAccountRepository.Delete(exampleBankAccount, CancellationToken.None);
        await dbContext.SaveChangesAsync();

        var dbBankAccount = await (_fixture.CreateDbContext(true))
            .BankAccount.FindAsync(exampleBankAccount.Id);

        dbBankAccount.Should().BeNull();


    }

    [Fact(DisplayName = nameof(SearchReturnsListAndTotal))]
    [Trait("Integration/infra.Data", "BankAccountRepository - Repositories")]

    public async Task SearchReturnsListAndTotal()
    {
        FinancialDbContext dbContext = _fixture.CreateDbContext();
        var exampleBankAccountList = _fixture.GetExampleBankAccountList(15);

        await dbContext.AddRangeAsync(exampleBankAccountList);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        var bankAccountRepository = new Repository.BankAccountRepository(dbContext);
        var searchInput = new SearchInput(1, 20, "", "", SearchOrder.Asc);
        var output = await bankAccountRepository.Search(searchInput, CancellationToken.None);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(searchInput.Page);
        output.PerPage.Should().Be(searchInput.PerPage);
        output.Total.Should().Be(exampleBankAccountList.Count);
        output.Items.Should().HaveCount(exampleBankAccountList.Count);

        foreach (BankAccount outputItem in output.Items)
        {
            var exampleItem = exampleBankAccountList.Find(
                bankAccount => bankAccount.Id == outputItem.Id);

            outputItem.Should().NotBeNull();
            outputItem.Name.Should().Be(exampleItem!.Name);
            outputItem.OpeningBalance.Should().Be(exampleItem.OpeningBalance);
            outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
        }



    }

    [Fact(DisplayName = nameof(SearchReturnsEmptyWhenPersistenceIsEmpty))]
    [Trait("Integration/infra.Data", "BankAccountRepository - Repositories")]

    public async Task SearchReturnsEmptyWhenPersistenceIsEmpty()
    {
        FinancialDbContext dbContext = _fixture.CreateDbContext();
        var bankAccountRepository = new Repository.BankAccountRepository(dbContext);
        var searchInput = new SearchInput(1, 20, "", "", SearchOrder.Asc);
        var output = await bankAccountRepository.Search(searchInput, CancellationToken.None);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(searchInput.Page);
        output.PerPage.Should().Be(searchInput.PerPage);
        output.Total.Should().Be(0);
        output.Items.Should().HaveCount(0);

    }

    [Theory(DisplayName = nameof(SearchReturnsPaginated))]
    [Trait("Integration/infra.Data", "BankAccountRepository - Repositories")]
    [InlineData(10, 1, 5, 5)]
    [InlineData(10, 2, 5, 5)]
    [InlineData(7, 2, 5, 2)]
    [InlineData(7, 3, 5, 0)]


    public async Task SearchReturnsPaginated(
        int quantityBankAccountToGenerate,
        int page,
        int perPage,
        int expectedQuantityItems
        )
    {
        FinancialDbContext dbContext = _fixture.CreateDbContext();
        var exampleBankAccountList = _fixture.GetExampleBankAccountList(quantityBankAccountToGenerate);

        await dbContext.AddRangeAsync(exampleBankAccountList);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        var bankAccountRepository = new Repository.BankAccountRepository(dbContext);
        var searchInput = new SearchInput(page, perPage, "", "", SearchOrder.Asc);
        var output = await bankAccountRepository.Search(searchInput, CancellationToken.None);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(searchInput.Page);
        output.PerPage.Should().Be(searchInput.PerPage);
        output.Total.Should().Be(quantityBankAccountToGenerate);
        output.Items.Should().HaveCount(expectedQuantityItems);

        foreach (BankAccount outputItem in output.Items)
        {
            var exampleItem = exampleBankAccountList.Find(
                bankAccount => bankAccount.Id == outputItem.Id);

            outputItem.Should().NotBeNull();
            outputItem.Name.Should().Be(exampleItem!.Name);
            outputItem.OpeningBalance.Should().Be(exampleItem.OpeningBalance);
            outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
        }
    }
    [Theory(DisplayName = nameof(SearchReturnsPaginated))]
    [Trait("Integration/infra.Data", "BankAccountRepository - Repositories")]
    [InlineData("Itau", 1, 5, 1, 1)]
    [InlineData("Bradesco", 1, 5, 1, 1)]
    [InlineData("Santander", 1, 5, 1, 1)]
    [InlineData("NuBank Brasil", 1, 5, 1, 1)]
    [InlineData("Neon", 1, 2, 1, 1)]
    [InlineData("Inter", 2, 2, 1, 3)]
    [InlineData("Banco do Brasil Other", 1, 3, 0, 0)]
    [InlineData("Brasil", 2, 3, 2, 5)]
    

    public async Task SearchByText(
        string search,
        int page,
        int perPage,
        int expectedQuantityItemsReturned,
        int expectedQuantityTotalItems
        )
    {
        FinancialDbContext dbContext = _fixture.CreateDbContext();
        var exampleBankAccountList = 
            _fixture.GetExampleBankAccountListWithNames(new List<string>()
            {
                "Banco do Brasil",
                "Itau",
                "Bradesco",
                "Santander",
                "Inter",
                "NuBank Brasil",
                "Brasil Neon",
                "Inter Brasil",
                "NuBank Other",
                "Pic Pay",
                "Inter Brasil"
            });

        await dbContext.AddRangeAsync(exampleBankAccountList);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        var bankAccountRepository = new Repository.BankAccountRepository(dbContext);
        var searchInput = new SearchInput(page, perPage, search, "", SearchOrder.Asc);
        var output = await bankAccountRepository.Search(searchInput, CancellationToken.None);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(searchInput.Page);
        output.PerPage.Should().Be(searchInput.PerPage);
        output.Total.Should().Be(expectedQuantityTotalItems);
        output.Items.Should().HaveCount(expectedQuantityItemsReturned);

        foreach (BankAccount outputItem in output.Items)
        {
            var exampleItem = exampleBankAccountList.Find(
                bankAccount => bankAccount.Id == outputItem.Id);

            outputItem.Should().NotBeNull();
            outputItem.Name.Should().Be(exampleItem!.Name);
            outputItem.OpeningBalance.Should().Be(exampleItem.OpeningBalance);
            outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
        }
    }
}
