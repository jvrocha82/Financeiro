using Financial.Application.Interfaces;
using Financial.Application.UseCases.BankAccount.Common;
using Financial.Domain.Repository;
using DomainEntity = Financial.Domain.Entity;

namespace Financial.Application.UseCases.BankAccount.CreateBankAccount;
public class CreateBankAccount : ICreateBankAccount
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBankAccountRepository _bankAccountRepository;

    public CreateBankAccount(
        IBankAccountRepository BankAccountRepository,
        IUnitOfWork unitOfWork

        )
    {
        _bankAccountRepository = BankAccountRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<BankAccountModelOutput> Handle(
        CreateBankAccountInput input,
        CancellationToken cancellationToken
    )
    {
        var BankAccount = new DomainEntity.BankAccount(
            input.UserId,
            input.Name,
            input.OpeningBalance,
            input.OpeningBalanceIsNegative,
            input.IsActive
        );

        await _bankAccountRepository.Insert(BankAccount, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

        return BankAccountModelOutput.FromBankAccount(BankAccount);
    }
}
