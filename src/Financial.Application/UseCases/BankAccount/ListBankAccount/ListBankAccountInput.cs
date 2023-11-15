using Financial.Application.Common;
using Financial.Domain.SeedWork.SearchableRepository;
using MediatR;

namespace Financial.Application.UseCases.BankAccount.ListBankAccount;
public class ListBankAccountInput
    : PaginatedListInput, IRequest<ListBankAccountOutput>
{
    public ListBankAccountInput(
        int page,
        int perPage,
        string search,
        string sort,
        SearchOrder dir)
        : base(
            page,
            perPage,
            search,
            sort,
            dir)
    { }
}
