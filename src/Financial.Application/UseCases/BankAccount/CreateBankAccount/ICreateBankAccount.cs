using Financial.Application.UseCases.BankAccount.Common;
using MediatR;

namespace Financial.Application.UseCases.BankAccount.CreateBankAccount;
public interface ICreateBankAccount :
    IRequestHandler<CreateBankAccountInput, BankAccountModelOutput>
{ }
