namespace Financial.UnitTests.Application.User.UpdateUser;
public class UpdateUserTestDataGenerator
{
    public static IEnumerable<object[]> GetUsersToUpdate(int times = 10)
    {
        var fixture = new UpdateUserTestFixture();
        for (int i = 0; i < times; i++) {

            var exampleUser = fixture.GetValidUser();
            var exampleInput = fixture.GetUserInput(exampleUser.Id);
            yield return  new object[] {
                    exampleUser, exampleInput
            };
        }

    }
}
