using Financial.Application.Interfaces;
using Financial.Domain.Repository;
using DomainEntity = Financial.Domain.Entity;

namespace Financial.Application.UseCases.Account.CreateAccount;
public class CreateAccount : ICreateAccount
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAccountRepository _accountRepository;

    public CreateAccount(
        IAccountRepository accountRepository,
        IUnitOfWork unitOfWork

        )
    {
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateAccountOutput> Handle(
        CreateAccountInput input,
        CancellationToken cancellationToken
    )
    {
        var account = new DomainEntity.Account(
            input.UserId,
            input.Name,
            input.OpeningBalance,
            input.OpeningBalanceIsNegative,
            input.IsActive
        );

        await _accountRepository.Insert(account, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

        return new CreateAccountOutput(
            account.Id,
            account.CreatedAt,
            account.Name,
            account.OpeningBalance,
            account.OpeningBalanceIsNegative,
            account.IsActive
        );
    }
}
