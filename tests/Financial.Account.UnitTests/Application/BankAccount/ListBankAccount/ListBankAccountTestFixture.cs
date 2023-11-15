using Financial.Domain.Repository;
using Financial.UnitTests.Common;
using Moq;
using Xunit;
using DomainEntity = Financial.Domain.Entity;

namespace Financial.UnitTests.Application.BankAccount.ListBankAccount;
[CollectionDefinition(nameof(ListBankAccountTestFixture))]

public class ListBankAccountTestFixtureCollection
    : ICollectionFixture<ListBankAccountTestFixture>
{ }
public class ListBankAccountTestFixture
    : BaseFixture
{
    public Mock<IBankAccountRepository> GetRepositoryMock()
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
    public int GetValidOpeningBalance() => new Random().Next();

    public bool GetRandomBoolean() => new Random().NextDouble() < 0.5;
    public Guid GetValidUserId() => Guid.NewGuid();


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
