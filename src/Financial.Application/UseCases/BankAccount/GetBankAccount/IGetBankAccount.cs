using Financial.Application.UseCases.BankAccount.Common;
using MediatR;

namespace Financial.Application.UseCases.BankAccount.GetBankAccount;
public interface IGetBankAccount : IRequestHandler<GetBankAccountInput, BankAccountModelOutput>
{
}
