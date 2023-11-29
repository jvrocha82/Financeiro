using Financial.Domain.Entity;
using Financial.Domain.Repository;
using Financial.Domain.SeedWork.SearchableRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial.Infra.Data.EF.Repositories;
public class BankAccountRepository
    : IBankAccountRepository
{
    private readonly FinancialDbContext _context;
    private DbSet<BankAccount> _banksAccount => 
        _context.Set<BankAccount>();

    public BankAccountRepository(FinancialDbContext context)
    {
        _context = context;
        
    }

    public async Task Insert(
        BankAccount aggregate,
        CancellationToken cancellationToken
        )
    {
        await _banksAccount.AddAsync(aggregate, cancellationToken);
    }


    public Task Delete(BankAccount aggregate, CancellationToken cancellationToken)
    => throw new NotImplementedException();
    

    public Task<BankAccount> Get(Guid id, CancellationToken cancellationToken)
    => throw new NotImplementedException();
    
    
    public Task<SearchOutput<BankAccount>> Search(SearchInput input, CancellationToken cancellationToken)
    => throw new NotImplementedException();
    

    public Task Update(BankAccount aggregate, CancellationToken cancellationToken)
    => throw new NotImplementedException();
    
}
