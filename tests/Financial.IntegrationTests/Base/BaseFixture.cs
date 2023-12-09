using Bogus;
using Financial.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace Financial.IntegrationTests.Base;
public class BaseFixture
{
    public BaseFixture()
    => Faker = new Faker("pt_BR");
    

    protected Faker Faker {  get; set; }

    public FinancialDbContext CreateDbContext(bool preserveData = false)
    {
        var context = new FinancialDbContext(
            new DbContextOptionsBuilder<FinancialDbContext>()
            .UseInMemoryDatabase("integration-tests-db")
            .Options
           );
        if (preserveData == false)
            context.Database.EnsureDeleted();
        return context;

    }

}
