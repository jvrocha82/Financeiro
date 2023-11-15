using MediatR;

namespace Financial.Application.UseCases.BankAccount.ListBankAccount;
public interface IListBankAccount
    : IRequestHandler<ListBankAccountInput, ListBankAccountOutput>
{ }
