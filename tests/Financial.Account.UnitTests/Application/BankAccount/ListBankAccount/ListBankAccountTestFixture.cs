using Financial.UnitTests.Application.BankAccount.Common;
using Xunit;
using DomainEntity = Financial.Domain.Entity;

namespace Financial.UnitTests.Application.BankAccount.ListBankAccount;
[CollectionDefinition(nameof(ListBankAccountTestFixture))]

public class ListBankAccountTestFixtureCollection
    : ICollectionFixture<ListBankAccountTestFixture>
{ }
public class ListBankAccountTestFixture
    : BankAccountUseCasesBaseFixture
{


    public DomainEntity.BankAccount GetValidBankAccount()
        => new(
            GetValidUserId(),
            GetValidName(),
            GetValidOpeningBalance());
    public List<DomainEntity.BankAccount> GetExampleBankAccountList(int length = 10)
    {
        var list = new List<DomainEntity.BankAccount>();
        for (int i = 0; i < length; i++)
            list.Add(GetValidBankAccount());
        return list;
    }
}
