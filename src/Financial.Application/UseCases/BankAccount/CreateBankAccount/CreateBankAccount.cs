using Financial.Application.Interfaces;
using Financial.Domain.Repository;
using DomainEntity = Financial.Domain.Entity;

namespace Financial.Application.UseCases.BankAccount.CreateBankAccount;
public class CreateBankAccount : ICreateBankAccount
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBankAccountRepository _accountRepository;

    public CreateBankAccount(
        IBankAccountRepository accountRepository,
        IUnitOfWork unitOfWork

        )
    {
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateBankAccountOutput> Handle(
        CreateBankAccountInput input,
        CancellationToken cancellationToken
    )
    {
        var account = new DomainEntity.BankAccount(
            input.UserId,
            input.Name,
            input.OpeningBalance,
            input.OpeningBalanceIsNegative,
            input.IsActive
        );

        await _accountRepository.Insert(account, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

        return CreateBankAccountOutput.FromAccount(account);
    }
}
