using Bogus.DataSets;
using Financial.UnitTests.Common;
using Xunit;
using DomainEntity = Financial.Domain.Entity;
namespace Financial.UnitTests.Domain.Entity.Account;
public class AccountTestFixture : BaseFixture
{
    public AccountTestFixture()
    : base() { }
    public string GetValidName()
    {
        var randomName = "";

        while (randomName.Length < 3)
            randomName = Faker.Commerce.Categories(1)[0];

        if (randomName.Length > 255)
            randomName = randomName[..255];

        return randomName;
    }
    public Decimal GetValidOpeningBalance(){
        var rand = new Random();
        return new decimal(rand.NextDouble());
    }



public DomainEntity.Account GetValidAccount() => new(
        GetValidUser().Id,
        GetValidName(),
        GetValidOpeningBalance()
    );
public DomainEntity.User GetValidUser() => new(
        GetValidName(),
        true
    );
}
[CollectionDefinition(nameof(AccountTestFixture))]
public class AccountTestFixtureCollection 
    : ICollectionFixture<AccountTestFixture> 
{ }