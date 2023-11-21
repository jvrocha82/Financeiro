using Financial.Application.Interfaces;
using Financial.Domain.Repository;

namespace Financial.Application.UseCases.User.UpdateUser;
public class UpdateUser
{
    public readonly IUserRepository _userRepository;
    public readonly IUnitOfWork _unitOfWork;

    public UpdateUser(IUserRepository userRepository, IUnitOfWork unitOfWork)
        => (_userRepository, _unitOfWork) = (userRepository, unitOfWork);

    public async Task<GetUserOutput> Handle(UpdateUserInput request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.Get(request.Id, cancellationToken);
        user.Update(request.Name);

        await _userRepository.Update(user, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

        return GetUserOutput.FromUser(user);

    }
    
}
