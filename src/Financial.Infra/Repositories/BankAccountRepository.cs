using Financial.Application.Exceptions;
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

    public async Task<BankAccount> Get(Guid id, CancellationToken cancellationToken)
    {
       var bankAccount =  await _banksAccount.FindAsync(
        new object[] { id },
        cancellationToken);

        NotFoundException.ThrowIfNull(bankAccount, $"BankAccount '{id}' not found.");
        
        return bankAccount!;
    }
    public Task Delete(BankAccount aggregate, CancellationToken _)
    => Task.FromResult(_banksAccount.Remove(aggregate));





    public async Task<SearchOutput<BankAccount>> Search(SearchInput input, CancellationToken cancellationToken)
    {
        var toSkip = (input.Page - 1) * input.PerPage;
        var total = await _banksAccount.CountAsync();
        var items = await _banksAccount.AsNoTracking()
            .Skip(toSkip)
            .Take(input.PerPage)
            .ToListAsync();
        return new SearchOutput<BankAccount>(input.Page, input.PerPage, total, items);

    }

    public Task Update(BankAccount aggregate, CancellationToken _)
    => Task.FromResult(_banksAccount.Update(aggregate));
    
}
