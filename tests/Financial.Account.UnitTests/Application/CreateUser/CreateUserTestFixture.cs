using Financial.Application.Interfaces;
using Financial.Application.UseCases.User.CreateUser;
using Financial.Domain.Repository;
using Financial.UnitTests.Common;
using Moq;
using Xunit;

namespace Financial.UnitTests.Application.CreateUser;
[CollectionDefinition(nameof(CreateUserTestFixture))]

public class CreateUserTestFixtureCollection
    : ICollectionFixture<CreateUserTestFixture>
{}
public class CreateUserTestFixture : BaseFixture
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
    public CreateUserInput GetInput() => new(
        GetValidName(),
        true
        );

    public Mock<IUserRepository> GetRepositoryMock()
    => new();

    public Mock<IUnitOfWork> GetUnitOfWorkMock()
        => new();
}
