using Financial.Application.UseCases.BankAccount.Common;
using MediatR;

namespace Financial.Application.UseCases.BankAccount.GetBankAccount;
public class GetBankAccountInput : IRequest<BankAccountModelOutput>
{
    public Guid Id { get; set; }

    public GetBankAccountInput(Guid id)
    => Id = id;
}
