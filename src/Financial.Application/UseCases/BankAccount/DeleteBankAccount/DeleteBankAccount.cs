using Financial.Application.Interfaces;
using Financial.Domain.Repository;
using MediatR;

namespace Financial.Application.UseCases.BankAccount.DeleteBankAccount;
public class DeleteBankAccount : IDeleteBankAccount
{
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBankAccount(IBankAccountRepository bankAccountRepository, IUnitOfWork unitOfWork)
    => (_bankAccountRepository, _unitOfWork) = (bankAccountRepository, unitOfWork);

    public async Task Handle(DeleteBankAccountInput request, CancellationToken cancellationToken)
    {
        var bankAccount = await _bankAccountRepository.Get(request.Id, cancellationToken);
        await _bankAccountRepository.Delete(bankAccount, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

    }

 
}
