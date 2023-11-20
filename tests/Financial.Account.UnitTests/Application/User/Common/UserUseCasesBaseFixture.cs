using Financial.Application.Interfaces;
using Financial.Application.UseCases.User.CreateUser;
using Financial.Application.UseCases.User.UpdateUser;
using Financial.Domain.Repository;
using Financial.UnitTests.Common;
using Moq;
using DomainEntity = Financial.Domain.Entity;
namespace Financial.UnitTests.Application.User.Common;
public abstract class UserUseCasesBaseFixture : BaseFixture
{
    public Mock<IUserRepository> GetRepositoryMock()
    => new();

    public Mock<IUnitOfWork> GetUnitOfWorkMock()
        => new();
    public DomainEntity.User GetValidUser() => new(
         GetValidName(),
         true
     );
    public string GetValidName()
    {
        var randomName = "";

        while (randomName.Length < 3)
            randomName = Faker.Commerce.Categories(1)[0];

        if (randomName.Length > 255)
            randomName = randomName[..255];

        return randomName;
    }

}
