using Financial.Domain.Repository;
using Financial.UnitTests.Application.BankAccount.Common;
using Financial.UnitTests.Common;
using Moq;
using Xunit;
using DomainEntity = Financial.Domain.Entity;
namespace Financial.UnitTests.Application.BankAccount.GetBankAccount;
[CollectionDefinition(nameof(GetBankAccountTestFixture))]
public class GetAccountTestFistureCollection :
   ICollectionFixture<GetBankAccountTestFixture>
{ }
public class GetBankAccountTestFixture : BankAccountUseCasesBaseFixture
{
    public DomainEntity.User GetValidUser() => new(
        GetValidName(),
        true
    );

    public DomainEntity.BankAccount GetValidBankAccount() => new(
        GetValidUser().Id,
        GetValidName(),
        GetValidOpeningBalance()
    );
}
