using Financial.Application.Exceptions;
using Financial.Application.UseCases.BankAccount.UpdateBankAccount;
using Financial.Domain.Exceptions;
using Financial.Infra.Data.EF;
using Financial.Infra.Data.EF.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using ApplicationUseCase = Financial.Application.UseCases.BankAccount.UpdateBankAccount;
using DomainEntity = Financial.Domain.Entity;
namespace Financial.IntegrationTests.Application.UseCase.BankAccount.UpdateBankAccount;
[Collection(nameof(UpdateBankAccountTestFixture))]
public class UpdateBankAccountTest
{
    private readonly UpdateBankAccountTestFixture _fixture;

    public UpdateBankAccountTest(UpdateBankAccountTestFixture fixture)
    => _fixture = fixture;
    

    [Theory(DisplayName = nameof(UpdateBankAccount))]
    [Trait("Integration/Application", "UpdateBankAccount - Use Cases")]
    [MemberData(
       nameof(UpdateBankAccountTestDataGenerator.GetBankAccountToUpdate),
       parameters: 5,
       MemberType = typeof(UpdateBankAccountTestDataGenerator)
   )]
    public async Task UpdateBankAccount(
       DomainEntity.BankAccount exampleBankAccount,
       UpdateBankAccountInput input)
    {

        var dbContext = _fixture.CreateDbContext();
        await dbContext.AddRangeAsync(_fixture.GetExampleBankAccountList());
        var trackingInfo = await dbContext.AddAsync(exampleBankAccount);
        dbContext.SaveChanges();
        trackingInfo.State = EntityState.Detached;
        var repository = new BankAccountRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);
        var useCase = new ApplicationUseCase.UpdateBankAccount(repository, unitOfWork);

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
        output.IsActive.Should().Be((bool)input.IsActive!);
    }
   
    
    [Theory(DisplayName = nameof(UpdateBankAccountWithoutIsActive))]
    [Trait("Integration/Application", "UpdateBankAccount - Use Cases")]
    [MemberData(
        nameof(UpdateBankAccountTestDataGenerator.GetBankAccountToUpdate),
        parameters: 5,
        MemberType = typeof(UpdateBankAccountTestDataGenerator)
    )]
    public async Task UpdateBankAccountWithoutIsActive(
        DomainEntity.BankAccount exampleBankAccount,
        UpdateBankAccountInput exampleInput)
    {
        var input = new UpdateBankAccountInput(exampleInput.Id, exampleInput.Name, exampleInput.OpeningBalance);
        var dbContext = _fixture.CreateDbContext();
        await dbContext.AddRangeAsync(_fixture.GetExampleBankAccountList());
        var trackingInfo = await dbContext.AddAsync(exampleBankAccount);
        dbContext.SaveChanges();
        trackingInfo.State = EntityState.Detached;
        var repository = new BankAccountRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);
        var useCase = new ApplicationUseCase.UpdateBankAccount(repository, unitOfWork);

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
        output.IsActive.Should().Be((bool)exampleBankAccount.IsActive!);
    }


    [Theory(DisplayName = nameof(UpdateBankAccountOnlyName))]
    [Trait("Integration/Application", "UpdateBankAccount - Use Cases")]
    [MemberData(
       nameof(UpdateBankAccountTestDataGenerator.GetBankAccountToUpdate),
       parameters: 5,
       MemberType = typeof(UpdateBankAccountTestDataGenerator)
   )]
    public async Task UpdateBankAccountOnlyName(
       DomainEntity.BankAccount exampleBankAccount,
       UpdateBankAccountInput exampleInput)
    {
        var input = new UpdateBankAccountInput(
            exampleInput.Id, 
            exampleInput.Name);
        var dbContext = _fixture.CreateDbContext();
        await dbContext.AddRangeAsync(_fixture.GetExampleBankAccountList());
        var trackingInfo = await dbContext.AddAsync(exampleBankAccount);
        dbContext.SaveChanges();
        trackingInfo.State = EntityState.Detached;
        var repository = new BankAccountRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);
        var useCase = new ApplicationUseCase.UpdateBankAccount(repository, unitOfWork);

        var output = await useCase.Handle(input, CancellationToken.None);

        var dbBankAccount = await (_fixture.CreateDbContext(true))
             .BankAccount.FindAsync(output.Id);
        dbBankAccount.Should().NotBeNull();
        dbBankAccount!.Name.Should().Be(output.Name);
        dbBankAccount.OpeningBalance.Should().Be(exampleBankAccount.OpeningBalance);
        dbBankAccount.CreatedAt.Should().Be(output.CreatedAt);

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.OpeningBalance.Should().Be(exampleBankAccount.OpeningBalance);
        output.IsActive.Should().Be((bool)exampleBankAccount.IsActive!);
    }

    [Fact(DisplayName = nameof(UpdateThrowsWhenNotFoundBankAccount))]
    [Trait("Integration/Application", "UpdateBankAccount - Use Cases")]

    public async Task UpdateThrowsWhenNotFoundBankAccount(){
        var input = _fixture.GetValidBankAccountInput();
        var dbContext = _fixture.CreateDbContext();
        await dbContext.AddRangeAsync(_fixture.GetExampleBankAccountList());
    
        dbContext.SaveChanges();

        var repository = new BankAccountRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);
        var useCase = new ApplicationUseCase.UpdateBankAccount(repository, unitOfWork);

        var task = async ()
            => await useCase.Handle(input, CancellationToken.None);
        await task.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"BankAccount '{input.Id}' not found.");

    }

    [Theory(DisplayName = nameof(UpdateThrowsWhenCantInstantiateBankAccount))]
    [Trait("Integration/Application", "UpdateBankAccount - Use Cases")]
    [MemberData(
     nameof(UpdateBankAccountTestDataGenerator.GetInvalidInputs),
     parameters: 5,
     MemberType = typeof(UpdateBankAccountTestDataGenerator)
 )]
    public async Task UpdateThrowsWhenCantInstantiateBankAccount(
     UpdateBankAccountInput input,
    string expectedExceptionMessage)
    {


        var dbContext = _fixture.CreateDbContext();
        var exampleBankAccont = _fixture.GetExampleBankAccountList();
        await dbContext.AddRangeAsync(exampleBankAccont);
  
        dbContext.SaveChanges();

        var repository = new BankAccountRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);
        var useCase = new ApplicationUseCase.UpdateBankAccount(repository, unitOfWork);
        input.Id = exampleBankAccont[0].Id;
        var task = async () =>
            await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<EntityValidationException>()
            .WithMessage(expectedExceptionMessage);
      
    }
}
