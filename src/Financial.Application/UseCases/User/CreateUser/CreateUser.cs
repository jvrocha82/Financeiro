using Financial.Application.Interfaces;

using Financial.Domain.Repository;


using DomainEntity = Financial.Domain.Entity;

namespace Financial.Application.UseCases.User.CreateUser;
public  class CreateUser : ICreateUser
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;

    public CreateUser(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork

        )
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateUserOutput> Handle(
        CreateUserInput input,
        CancellationToken cancellationToken
    )
    {
        var user = new DomainEntity.User(
            input.Name,
            input.IsActive
        );

        await _userRepository.Insert(user, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

        return new CreateUserOutput(
            user.Id,
            user.Name,
            user.IsActive,
            user.CreatedAt
  
        );
    }
}
