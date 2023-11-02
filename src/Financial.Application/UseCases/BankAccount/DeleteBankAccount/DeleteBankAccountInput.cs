using MediatR;

namespace Financial.Application.UseCases.BankAccount.DeleteBankAccount;
public class DeleteBankAccountInput : IRequest
{
    public Guid Id { get; set; }

    public DeleteBankAccountInput(Guid id)
    => Id = id;
}
