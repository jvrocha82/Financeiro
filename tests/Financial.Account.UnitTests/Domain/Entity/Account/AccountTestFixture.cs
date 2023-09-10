using Xunit;
using DomainEntity = Financial.Domain.Entity;
namespace Financial.UnitTests.Domain.Entity.Account;
public class AccountTestFixture
{

    public DomainEntity.Account GetValidAccount() => new("Account Name", 100);
}
[CollectionDefinition(nameof(AccountTestFixture))]
public class AccountTestFixtureCollection 
    : ICollectionFixture<AccountTestFixture> 
{ }