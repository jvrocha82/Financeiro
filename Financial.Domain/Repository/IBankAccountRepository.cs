using Financial.Domain.Entity;
using Financial.Domain.SeedWork;
using Financial.Domain.SeedWork.SearchableRepository;

namespace Financial.Domain.Repository;
public interface IBankAccountRepository 
    : IGenericRepository<BankAccount>,
    ISearchableRepository<BankAccount>
{

}
