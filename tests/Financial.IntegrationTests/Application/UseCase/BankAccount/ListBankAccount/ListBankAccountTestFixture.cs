using Financial.IntegrationTests.Application.UseCase.BankAccount.Common;
using Xunit;

namespace Financial.IntegrationTests.Application.UseCase.BankAccount.ListBankAccount;

[CollectionDefinition(nameof(ListBankAccountTestFixture))]

public class ListBankAccountTestFixtureCollection
    : ICollectionFixture<ListBankAccountTestFixture>
{ }
public class ListBankAccountTestFixture
    : BankAccountUseCaseBaseFixture
{
}
