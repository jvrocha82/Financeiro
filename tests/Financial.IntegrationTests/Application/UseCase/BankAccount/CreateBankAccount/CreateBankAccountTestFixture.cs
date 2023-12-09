using Financial.Application.UseCases.BankAccount.CreateBankAccount;
using Financial.IntegrationTests.Application.UseCase.BankAccount.Common;
using Xunit;

namespace Financial.IntegrationTests.Application.UseCase.BankAccount.CreateBankAccount;

[CollectionDefinition(nameof(CreateBankAccountTestFixture))]
public class CreateBankAccountTestFixtureCollection
    : ICollectionFixture<CreateBankAccountTestFixture>
{ }
public class CreateBankAccountTestFixture
    : BankAccountUseCaseBaseFixture
{
    public CreateBankAccountInput GetInput()
    {
        var bankAccount = GetExampleBankAccount();
        return new CreateBankAccountInput(bankAccount.UserId, bankAccount.Name, bankAccount.OpeningBalance);
    }
}
