using Financial.UnitTests.Application.User.Common;
using Xunit;

namespace Financial.UnitTests.Application.User.GetUser;
[CollectionDefinition(nameof(GetUserTestFixture))]

public class GetUserTestFixtureCollection :
    ICollectionFixture<GetUserTestFixture>
{ }

public class GetUserTestFixture : UserUseCasesBaseFixture
{


}
