using Financial.Domain.Repository;
using MediatR;

namespace Financial.Application.UseCases.BankAccount.GetBankAccount;
public class GetBankAccount : IRequestHandler<GetBankAccountInput, GetBankAccountOutput>
{
    private readonly IBankAccountRepository _accountRepository;

    public GetBankAccount(IBankAccountRepository accountRepository)
    => _accountRepository = accountRepository;


    public async Task<GetBankAccountOutput> Handle(
        GetBankAccountInput request,
        CancellationToken cancellationToken
    )
    {
        var account = await _accountRepository.Get(request.Id, cancellationToken);

        return GetBankAccountOutput.FromAccount(account);
    }
}
