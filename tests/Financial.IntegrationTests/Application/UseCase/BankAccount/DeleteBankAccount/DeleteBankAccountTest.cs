using Financial.Infra.Data.EF;
using Financial.Infra.Data.EF.Repositories;
using Xunit;
using Microsoft.EntityFrameworkCore;
using ApplicationUseCase = Financial.Application.UseCases.BankAccount.DeleteBankAccount;
using Financial.Application.UseCases.BankAccount.DeleteBankAccount;
using FluentAssertions;
using Financial.Application.Exceptions;

namespace Financial.IntegrationTests.Application.UseCase.BankAccount.DeleteBankAccount;

[Collection(nameof(DeleteBankAccountTestFixture))]
public class DeleteBankAccountTest
{
    private readonly DeleteBankAccountTestFixture _fixture;

    public DeleteBankAccountTest(DeleteBankAccountTestFixture fixture)
    => _fixture = fixture;
    

    [Fact(DisplayName = nameof(DeleteBankAccount))]
    [Trait("Integration/Application", "DeleteBankAccunt - Use Cases")]

    public async Task DeleteBankAccunt()
    {
        var dbContext = _fixture.CreateDbContext();
        var bankAccuntExample = _fixture.GetExampleBankAccount();
        var exampleList = _fixture.GetExampleBankAccountList(10);
        await dbContext.AddRangeAsync(exampleList);
        var tracking = await dbContext.AddAsync(bankAccuntExample);
        await dbContext.SaveChangesAsync();
        tracking.State = EntityState.Detached;
        var repository = new BankAccountRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);
        var useCase = new ApplicationUseCase.DeleteBankAccount(
            repository,
            unitOfWork
        );
        var input = new DeleteBankAccountInput(bankAccuntExample.Id);

        await useCase.Handle(input, CancellationToken.None);

        var assertDbContext = _fixture.CreateDbContext(true);
        var dbBankAccountDeleted = await assertDbContext.BankAccount.FindAsync(input.Id);
        
        dbBankAccountDeleted.Should().BeNull();
        
        var dbBankAccounts = await assertDbContext.BankAccount.ToListAsync();
        dbBankAccounts.Should().HaveCount(exampleList.Count);   
    }


    [Fact(DisplayName = nameof(DeleteBankAccuntThrowsWhenNotFound))]
    [Trait("Integration/Application", "DeleteBankAccunt - Use Cases")]

    public async Task DeleteBankAccuntThrowsWhenNotFound()
    {
        var dbContext = _fixture.CreateDbContext();
        var bankAccuntExample = _fixture.GetExampleBankAccount();
        var exampleList = _fixture.GetExampleBankAccountList(10);
        await dbContext.AddRangeAsync(exampleList);
        var tracking = await dbContext.AddAsync(bankAccuntExample);
        await dbContext.SaveChangesAsync();
        tracking.State = EntityState.Detached;

        var repository = new BankAccountRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);
        var useCase = new ApplicationUseCase.DeleteBankAccount(
            repository,
            unitOfWork
        );
        var input = new DeleteBankAccountInput(Guid.NewGuid()); ;

        var task = async () 
            => await useCase.Handle(input, CancellationToken.None);
        await task.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"BankAccount '{input.Id}' not found.");
    }
}
