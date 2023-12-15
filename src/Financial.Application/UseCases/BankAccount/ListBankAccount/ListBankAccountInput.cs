using Financial.Application.Common;
using Financial.Domain.SeedWork.SearchableRepository;
using MediatR;

namespace Financial.Application.UseCases.BankAccount.ListBankAccount;
public class ListBankAccountInput
    : PaginatedListInput, IRequest<ListBankAccountOutput>
{
    public ListBankAccountInput(
        int page = 1,
        int perPage = 15,
        string search = "",
        string sort = "",
        SearchOrder dir = SearchOrder.Asc)
        : base(
            page,
            perPage,
            search,
            sort,
            dir)
    { }
}
