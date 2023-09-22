using Financial.Application.Interfaces;
using Financial.Application.UseCases.Account.CreateAccount;
using Financial.Domain.Repository;
using Financial.UnitTests.Common;
using Moq;
using Xunit;

namespace Financial.UnitTests.Application.CreateAccount;

[CollectionDefinition(nameof(CreateAccountTestFixture))]
public class CreateAccountTestFixtureCollection 
    : ICollectionFixture<CreateAccountTestFixture>
{}
public class CreateAccountTestFixture : BaseFixture
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
    public int GetValidOpeningBalance() => new Random().Next();

    public bool GetRandomBoolean() => (new Random()).NextDouble() < 0.5;

    public CreateAccountInput GetInput() 
        => new(
            GetValidName(),
            GetValidOpeningBalance(),
            GetRandomBoolean(),
            GetRandomBoolean()
        );

    public Mock<IAccountRepository> GetRepositoryMock() 
        => new();
    
    public Mock<IUnitOfWork> GetUnitOfWorkMock() 
        => new();


}
