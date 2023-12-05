﻿
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

        foreach(BankAccount outputItem in output.Items)
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




}
