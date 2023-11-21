using Financial.Application.UseCases.BankAccount.Common;
using Financial.Domain.Repository;

namespace Financial.Application.UseCases.BankAccount.GetBankAccount;
public class GetBankAccount : IGetBankAccount
{
    private readonly IBankAccountRepository _bankAccountRepository;

    public GetBankAccount(IBankAccountRepository bankAccountRepository)
    => _bankAccountRepository = bankAccountRepository;

    public async Task<BankAccountModelOutput> Handle(
        GetBankAccountInput request,
        CancellationToken cancellationToken)
    {
        var bankAccount = await _bankAccountRepository.Get(request.Id, cancellationToken);

        return BankAccountModelOutput.FromBankAccount(bankAccount);
    }
}
