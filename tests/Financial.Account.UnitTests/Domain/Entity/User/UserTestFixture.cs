using Financial.UnitTests.Common;
using Xunit;
using DomainEntity = Financial.Domain.Entity;
public class UserTestFixture : BaseFixture
{
    public UserTestFixture ()
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
    public DomainEntity.User GetValidUser(bool isActive = true) => new(
        GetValidName(),
        isActive
    );
}
[CollectionDefinition(nameof(UserTestFixture))]
public class UserTestFixtureCollection
    : ICollectionFixture<UserTestFixture>
{ }
