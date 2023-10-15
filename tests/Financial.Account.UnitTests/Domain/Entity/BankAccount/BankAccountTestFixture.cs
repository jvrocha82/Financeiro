using Financial.UnitTests.Common;
using Xunit;
using DomainEntity = Financial.Domain.Entity;
namespace Financial.UnitTests.Domain.Entity.BankAccount;
public class BankAccountTestFixture : BaseFixture
{
    public BankAccountTestFixture()
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
    public decimal GetValidOpeningBalance()
    {
        var rand = new Random();
        return new decimal(rand.NextDouble());
    }

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
[CollectionDefinition(nameof(BankAccountTestFixture))]
public class BankAccountTestFixtureCollection
    : ICollectionFixture<BankAccountTestFixture>
{ }