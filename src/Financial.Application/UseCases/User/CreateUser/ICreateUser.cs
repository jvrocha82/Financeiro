namespace Financial.Application.UseCases.User.CreateUser;
public interface ICreateUser
{
    public Task<CreateUserOutput> Handle(
        CreateUserInput input,
        CancellationToken cancellationToken);
}
