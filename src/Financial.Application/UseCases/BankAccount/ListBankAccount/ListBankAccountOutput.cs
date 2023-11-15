using Financial.Application.Common;
using Financial.Application.UseCases.BankAccount.Common;

namespace Financial.Application.UseCases.BankAccount.ListBankAccount;
public class ListBankAccountOutput
    : PaginatedListOutput<BankAccountModelOutput>
{
    public ListBankAccountOutput(
        int page,
        int perPage,
        int total,
        IReadOnlyList<BankAccountModelOutput> items) 
        : base(
            page,
            perPage,
            total,
            items)
    { }
}
