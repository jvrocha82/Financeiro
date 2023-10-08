using MediatR;

namespace Financial.Application.UseCases.Account.CreateAccount;
public interface ICreateAccount : 
    IRequestHandler<CreateAccountInput,CreateAccountOutput>
{
    public Task<CreateAccountOutput> Handle(
        CreateAccountInput input,
        CancellationToken cancellationToken);
}
