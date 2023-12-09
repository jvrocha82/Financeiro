using Financial.IntegrationTests.Base;
using DomainEntity = Financial.Domain.Entity;
namespace Financial.IntegrationTests.Application.UseCase.BankAccount.Common;
public class BankAccountUseCaseBaseFixture
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

    public DomainEntity.BankAccount GetExampleBankAccount()
    => new(
        GetValidUserId(),
        GetValidName(),
        GetValidOpeningBalance());


    public List<DomainEntity.BankAccount> GetExampleBankAccountList(int length = 10)
  => Enumerable.Range(1, length)
        .Select(_ => GetExampleBankAccount())
        .ToList();
}
