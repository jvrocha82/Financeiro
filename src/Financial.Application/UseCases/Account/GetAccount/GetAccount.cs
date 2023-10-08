using Financial.Domain.Repository;
using MediatR;

namespace Financial.Application.UseCases.Account.GetAccount;
public class GetAccount : IRequestHandler<GetAccountInput, GetAccountOutput>
{
    private readonly IAccountRepository _accountRepository;

    public GetAccount(IAccountRepository accountRepository)
    => _accountRepository = accountRepository;
    

    public async Task<GetAccountOutput> Handle(
        GetAccountInput request,
        CancellationToken cancellationToken
    )
    {
        var account = await _accountRepository.Get(request.Id, cancellationToken);

        return GetAccountOutput.FromAccount(account);
    }
}
