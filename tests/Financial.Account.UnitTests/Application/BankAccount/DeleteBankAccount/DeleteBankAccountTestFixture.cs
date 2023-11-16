using Financial.UnitTests.Application.BankAccount.Common;
using Xunit;
using DomainEntity = Financial.Domain.Entity;

namespace Financial.UnitTests.Application.BankAccount.DeleteBankAccount;
[CollectionDefinition(nameof(DeleteBankAccountTestFixture))]

public class DeleteBankAccountTestFixtureCollection
    : ICollectionFixture<DeleteBankAccountTestFixture>
{ }

public class DeleteBankAccountTestFixture : BankAccountUseCasesBaseFixture
{
    public DomainEntity.BankAccount GetValidBankAccount() => new(
    GetValidUserId(),
    GetValidName(),
    GetValidOpeningBalance()
);

}
