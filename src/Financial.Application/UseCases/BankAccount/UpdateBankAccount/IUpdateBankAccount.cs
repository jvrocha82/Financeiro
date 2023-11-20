using Financial.Application.UseCases.BankAccount.Common;
using MediatR;

namespace Financial.Application.UseCases.BankAccount.UpdateBankAccount;
public interface IUpdateBankAccount
    : IRequestHandler<UpdateBankAccountInput, BankAccountModelOutput>
{
}
