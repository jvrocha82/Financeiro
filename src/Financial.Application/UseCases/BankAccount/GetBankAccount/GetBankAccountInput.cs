using Financial.Application.UseCases.BankAccount.Common;
using MediatR;

namespace Financial.Application.UseCases.BankAccount.GetBankAccount;
public class GetBankAccountInput : IRequest<BankAccountModelOutput>
{
    public GetBankAccountInput(Guid id)
    => Id = id;


    public Guid Id { get; set; }
}
