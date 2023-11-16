using Financial.Application.Interfaces;
using Financial.Application.UseCases.BankAccount.CreateBankAccount;
using Financial.Domain.Repository;
using Financial.UnitTests.Application.BankAccount.Common;
using Financial.UnitTests.Application.BankAccount.CreateBankAccount;
using Financial.UnitTests.Common;
using Moq;
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
