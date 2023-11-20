using Financial.Application.Interfaces;
using Financial.Application.UseCases.User.CreateUser;
using Financial.Application.UseCases.User.UpdateUser;
using Financial.Domain.Repository;
using Financial.UnitTests.Application.User.Common;
using Financial.UnitTests.Common;
using Moq;
using Xunit;

namespace Financial.UnitTests.Application.User.CreateUser;
[CollectionDefinition(nameof(CreateUserTestFixture))]

public class CreateUserTestFixtureCollection
    : ICollectionFixture<CreateUserTestFixture>
{ }
public class CreateUserTestFixture : UserUseCasesBaseFixture
{

    public CreateUserInput GetUserInput() => new(
    
    GetValidName(),
    true);

}
