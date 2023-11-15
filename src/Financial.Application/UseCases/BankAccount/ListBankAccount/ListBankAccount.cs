using Financial.Application.UseCases.BankAccount.Common;
using Financial.Domain.Repository;

namespace Financial.Application.UseCases.BankAccount.ListBankAccount;
public class ListBankAccount : IListBankAccount
{
    private readonly IBankAccountRepository _bankAccountRepository;

    public ListBankAccount(IBankAccountRepository bankAccountRepository)
    => _bankAccountRepository = bankAccountRepository;
    

    public async Task<ListBankAccountOutput> Handle(
        ListBankAccountInput request,
        CancellationToken cancellationToken)
    {
        var searchOutput = await _bankAccountRepository.Search(
            new(
                request.Page,
                request.PerPage,
                request.Search,
                request.Sort,
                request.Dir
            ),
            cancellationToken
        );
        return new ListBankAccountOutput(
            searchOutput.CurrentPage,
            searchOutput.PerPage,
            searchOutput.Total,
            searchOutput.Items
                .Select(x => BankAccountModelOutput.FromBankAccount(x))
                .ToList()
            );
    }
}
