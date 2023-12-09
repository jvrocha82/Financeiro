using Financial.Domain.Entity;
using Financial.Domain.SeedWork.SearchableRepository;
using Financial.Infra.Data.EF;
using Financial.IntegrationTests.Base;
using Microsoft.EntityFrameworkCore;
using Xunit;
namespace Financial.IntegrationTests.Infra.Data.EF.Repositories.BankAccountRepository;


[CollectionDefinition(nameof(BankAccountRepositoryTestFixture))]
public class BankAccountRepositoryTestFixtureCollection
    : ICollectionFixture<BankAccountRepositoryTestFixture>
{ }
public class BankAccountRepositoryTestFixture
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
    public List<BankAccount> GetExampleBankAccountListWithNames(List<string> names)
    => names.Select(name =>
    {
        var bankAccount = GetExampleBankAccount();
        bankAccount.Update(name);
        return bankAccount;
    }).ToList();

    public List<BankAccount> CloneBankAccountListOrderer(
        List<BankAccount> bankAccountList,
        string orderBy,
        SearchOrder order)
    {
        var listClone = new List<BankAccount>(bankAccountList);
        var orderedEnumerable = (orderBy.ToLower(), order)switch
        {
            ("name", SearchOrder.Asc) => listClone.OrderBy(x => x.Name),
            ("name", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Name),
            ("id", SearchOrder.Asc) => listClone.OrderBy(x => x.Id),
            ("id", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Id),
            ("createdat", SearchOrder.Asc) => listClone.OrderBy(x => x.CreatedAt),
            ("createdat", SearchOrder.Desc) => listClone.OrderByDescending(x => x.CreatedAt),
            _ => listClone.OrderBy(x => x.Name),
        };
        return orderedEnumerable.ToList();
    }




}
