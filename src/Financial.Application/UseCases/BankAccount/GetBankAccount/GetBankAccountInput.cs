using MediatR;

namespace Financial.Application.UseCases.BankAccount.GetBankAccount;
public class GetBankAccountInput : IRequest<GetBankAccountOutput>
{
    public GetBankAccountInput(Guid id)
    => Id = id;


    public Guid Id { get; set; }
}
