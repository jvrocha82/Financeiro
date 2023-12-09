using FluentAssertions;
using System.Data.Entity;
using Xunit;
using UnitOfWorkInfra = Financial.Infra.Data.EF;

namespace Financial.IntegrationTests.Infra.Data.EF.UnitOfWork;
[Collection(nameof(UnitOfWorkTestFixture))]
public class UnitOfWorkTest
{
private readonly UnitOfWorkTestFixture _fixture;

    public UnitOfWorkTest(UnitOfWorkTestFixture fixture)
        =>_fixture = fixture;
    [Fact(DisplayName = nameof(Commit))]
    [Trait("Integration/infra.Data", "UnitOfWork - Persistence")]

    public async Task Commit()
    {
        var dbContext = _fixture.CreateDbContext();
        var exampleBankAccountList = _fixture.GetExampleBankAccountList();
        await dbContext.AddRangeAsync(exampleBankAccountList);
        var unitOfWork = new UnitOfWorkInfra.UnitOfWork(dbContext);

        await unitOfWork.Commit(CancellationToken.None);

        var assertDbContext = _fixture.CreateDbContext(true);
        var savedBankAccounts = assertDbContext.BankAccount
            .AsNoTracking().ToList();
        savedBankAccounts.Should()
            .HaveCount(exampleBankAccountList.Count);

    }
    [Fact(DisplayName = nameof(Rollback))]
    [Trait("Integration/infra.Data", "UnitOfWork - Persistence")]
    public async Task Rollback()
    {
        var dbContext = _fixture.CreateDbContext();
        var unitOfWork = new UnitOfWorkInfra.UnitOfWork(dbContext);

        var task = async ()
            => await unitOfWork.Rollback(CancellationToken.None);

        await task.Should().NotThrowAsync();
    }
}
