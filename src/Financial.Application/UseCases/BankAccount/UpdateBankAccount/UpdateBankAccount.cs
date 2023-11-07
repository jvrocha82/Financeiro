using Financial.Application.Interfaces;
using Financial.Application.UseCases.BankAccount.Common;
using Financial.Domain.Repository;

namespace Financial.Application.UseCases.BankAccount.UpdateBankAccount;
public class UpdateBankAccount : IUpdateBankAccount
{
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateBankAccount(IBankAccountRepository bankAccountRepository, IUnitOfWork unitOfWork)
    => (_bankAccountRepository, _unitOfWork) 
        = (bankAccountRepository, unitOfWork);



    public async Task<BankAccountModelOutput> Handle(UpdateBankAccountInput request, CancellationToken cancellationToken)
    {
        var bankAccount = await _bankAccountRepository.Get(request.Id, cancellationToken);
        bankAccount.Update(request.Name, request.OpeningBalance);

        if (request.IsActive != null &&
            request.IsActive != bankAccount.IsActive)
            if ((bool)request.IsActive!) bankAccount.Activate();
            else bankAccount.Deactivate();

        await _bankAccountRepository.Update(bankAccount, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);
        return BankAccountModelOutput.FromBankAccount(bankAccount);
    }  
}
