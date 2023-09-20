namespace Financial.Application.UseCases.Account.CreateAccount;
public interface ICreateAccount
{
    public Task<CreateAccountOutput> Handle(
        CreateAccountInput input,
        CancellationToken cancellationToken);
}
