using Financial.Application.Interfaces;

namespace Financial.Infra.Data.EF;
public class UnitOfWork
    : IUnitOfWork
{
    private readonly FinancialDbContext _context;

    public UnitOfWork(FinancialDbContext context)
    => _context = context;

    public Task Commit(CancellationToken cancellationToken)
        => _context.SaveChangesAsync(cancellationToken);

    public Task Rollback(CancellationToken cancellationToken)
        => Task.CompletedTask;
}
