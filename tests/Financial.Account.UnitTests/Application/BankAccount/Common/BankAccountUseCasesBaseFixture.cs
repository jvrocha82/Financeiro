using Financial.Application.Interfaces;
using Financial.Domain.Repository;
using Financial.UnitTests.Common;
using Moq;

namespace Financial.UnitTests.Application.BankAccount.Common;
public abstract class BankAccountUseCasesBaseFixture
    : BaseFixture
{
    public Mock<IBankAccountRepository> GetRepositoryMock()
    => new();

    public Mock<IUnitOfWork> GetUnitOfWorkMock()
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
}
