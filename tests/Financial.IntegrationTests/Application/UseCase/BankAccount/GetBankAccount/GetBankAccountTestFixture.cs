using Financial.IntegrationTests.Application.UseCase.BankAccount.Common;
using Xunit;

namespace Financial.IntegrationTests.Application.UseCase.BankAccount.GetBankAccount;

[CollectionDefinition(nameof(GetBankAccountTestFixture))]
public class GetbankAccountTestFixtureCollection
    : ICollectionFixture<GetBankAccountTestFixture>
{ }
public class GetBankAccountTestFixture
    : BankAccountUseCaseBaseFixture
{
   
}
