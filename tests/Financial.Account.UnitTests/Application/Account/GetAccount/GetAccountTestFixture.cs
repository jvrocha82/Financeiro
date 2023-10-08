using Financial.Domain.Repository;
using Financial.UnitTests.Common;
using Moq;
using Xunit;
using DomainEntity = Financial.Domain.Entity;
namespace Financial.UnitTests.Application.Account.GetAccount;
[CollectionDefinition(nameof(GetAccountTestFixture))]
public class GetAccountTestFistureCollection :
   ICollectionFixture<GetAccountTestFixture>
{ }
public class GetAccountTestFixture : BaseFixture
{
    public Mock<IAccountRepository> GetRepositoryMock()
    => new();

    public string GetValidName()
    {
        var randomName = "";

        while (randomName.Length < 3)
            randomName = Faker.Commerce.Categories(1)[0];

        if (randomName.Length > 255)
            randomName = randomName[..255];

        return randomName;
    }
    public Decimal GetValidOpeningBalance()
    {
        var rand = new Random();
        return new decimal(rand.NextDouble());
    }

    public DomainEntity.User GetValidUser() => new(
        GetValidName(),
        true
    );

    public DomainEntity.Account GetValidAccount() => new(
        GetValidUser().Id,
        GetValidName(),
        GetValidOpeningBalance()
    );
}
