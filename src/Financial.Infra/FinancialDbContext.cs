using Financial.Domain.Entity;
using Financial.Infra.Data.EF.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Financial.Infra.Data.EF;
public class FinancialDbContext
    : DbContext
{
    public DbSet<BankAccount> BankAccount => Set<BankAccount>();
    public FinancialDbContext(
        DbContextOptions<FinancialDbContext> options
        ) : base( options) { }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        //builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        builder.ApplyConfiguration(new BankAccountConfiguration());
    }
}
