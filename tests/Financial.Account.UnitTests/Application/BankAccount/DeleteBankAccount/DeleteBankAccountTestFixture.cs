using Financial.Application.Interfaces;
using Financial.Application.UseCases.BankAccount.CreateBankAccount;
using Financial.Domain.Repository;
using Financial.UnitTests.Application.BankAccount.CreateBankAccount;
using Financial.UnitTests.Common;
using Moq;
using Xunit;
using DomainEntity = Financial.Domain.Entity;

namespace Financial.UnitTests.Application.BankAccount.DeleteBankAccount;
public class DeleteBankAccountTestFixtureCollection
    : ICollectionFixture<DeleteBankAccountTestFixture>
{ }
[CollectionDefinition(nameof(DeleteBankAccountTestFixture))]

public class DeleteBankAccountTestFixture : BaseFixture
{
    public static Mock<IBankAccountRepository> GetRepositoryMock()
        => new();

    public static Mock<IUnitOfWork> GetUnitOfWorkMock()
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
    public static int GetValidOpeningBalance() => new Random().Next();

    public static bool GetRandomBoolean() => new Random().NextDouble() < 0.5;
    public static Guid GetValidUserId() => Guid.NewGuid();


    public DomainEntity.BankAccount GetValidBankAccount() => new(
    GetValidUserId(),
    GetValidName(),
    GetValidOpeningBalance()
);

}
