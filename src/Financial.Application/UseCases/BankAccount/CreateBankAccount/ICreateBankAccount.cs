using MediatR;

namespace Financial.Application.UseCases.BankAccount.CreateBankAccount;
public interface ICreateBankAccount :
    IRequestHandler<CreateBankAccountInput, CreateBankAccountOutput>
{
    public new Task<CreateBankAccountOutput> Handle(
        CreateBankAccountInput input,
        CancellationToken cancellationToken);
}
