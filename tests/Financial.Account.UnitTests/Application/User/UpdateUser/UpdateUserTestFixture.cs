using Financial.Application.UseCases.User.UpdateUser;
using Financial.UnitTests.Application.User.Common;
using Xunit;

namespace Financial.UnitTests.Application.User.UpdateUser;
[CollectionDefinition(nameof(UpdateUserTestFixture))]
public class UpdateUserTestFixtureCollection
    : ICollectionFixture<UpdateUserTestFixture>
{ }

public  class UpdateUserTestFixture
    : UserUseCasesBaseFixture
{
    public UpdateUserInput GetUserInput(Guid? id = null) => new(
    id ?? Guid.NewGuid(),
    GetValidName(),
    true);
}
