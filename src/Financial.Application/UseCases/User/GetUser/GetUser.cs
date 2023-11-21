using Financial.Domain.Repository;


namespace Financial.Application.UseCases.User.GetUser;
public class GetUser : IGetUser 
{
    private readonly IUserRepository _userRepository;

    public GetUser(IUserRepository userRepository)
    => _userRepository = userRepository;

    public async Task<GetUserOutput> Handle(
        GetUserInput request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.Get(request.Id, cancellationToken);

        return GetUserOutput.FromUser(user);
    }
    
}
