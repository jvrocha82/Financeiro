using Financial.Domain.Entity;
using Financial.IntegrationTests.Base;
using Xunit;

namespace Financial.IntegrationTests.Infra.Data.EF.UnitOfWork;


[CollectionDefinition(nameof(UnitOfWorkTestFixture))]
public class unitOfWorkTestFixtureCollection
    : ICollectionFixture<UnitOfWorkTestFixture>
{ }
public class UnitOfWorkTestFixture
    : BaseFixture
{

    public string GetValidName()
    {
        var randomName = "";

        while (randomName.Length < 3)
            randomName = Faker.Commerce.Categories(1)[0];

        if (randomName.Length > 255)
            randomName = randomName[..255];

        return randomName;
    }
    public static int GetValidOpeningBalance() => new Random().Next();

    public static bool GetRandomBoolean() => new Random().NextDouble() < 0.5;
    public static Guid GetValidUserId() => Guid.NewGuid();

    public BankAccount GetExampleBankAccount()
    => new(
        GetValidUserId(),
        GetValidName(),
        GetValidOpeningBalance());


    public List<BankAccount> GetExampleBankAccountList(int length = 10)
  => Enumerable.Range(1, length)
        .Select(_ => GetExampleBankAccount())
        .ToList();
}
