using Financial.IntegrationTests.Application.UseCase.BankAccount.Common;
using Xunit;

namespace Financial.IntegrationTests.Application.UseCase.BankAccount.DeleteBankAccount;
[CollectionDefinition(nameof(DeleteBankAccountTestFixture))]
public class DeleteBankAccountTestFixtureCollection
    : ICollectionFixture<DeleteBankAccountTestFixture>
{ }
public class DeleteBankAccountTestFixture
    : BankAccountUseCaseBaseFixture
{

}
