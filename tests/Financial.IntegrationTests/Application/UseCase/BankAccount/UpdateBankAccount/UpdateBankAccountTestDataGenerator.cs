namespace Financial.IntegrationTests.Application.UseCase.BankAccount.UpdateBankAccount;
public class UpdateBankAccountTestDataGenerator
{
    public static IEnumerable<object[]> GetBankAccountToUpdate(int times = 10)
    {
        var fixture = new UpdateBankAccountTestFixture();
        for (int indice = 0; indice < times; indice++)
        {
            var exampleBankAccount = fixture.GetValidBankAccount();
            var exampleInput = fixture.GetValidBankAccountInput(exampleBankAccount.Id);
            yield return new object[] {
                exampleBankAccount, exampleInput
            };
        }
    }
    public static IEnumerable<object[]> GetInvalidInputs(int times = 12)
    {
        var fixture = new UpdateBankAccountTestFixture();
        var invalidInputsList = new List<object[]>();
        var totalInvalidCases = 3;
        for (int i = 0; i < times; i++)
        {
            switch (i % totalInvalidCases)
            {
                case 0:
                    invalidInputsList.Add(new object[]
                    {
                        fixture.GetInvalidInputShortName(),
                            $"Name should be at leats 3 characters long"
                            });
                    break;
                case 1:
                    invalidInputsList.Add(new object[]
                    {
                            fixture.GetInvalidInputLongName(),
                            $"Name should be less or equal 255 characters long"
                    });
                    break;
                case 2:
                    invalidInputsList.Add(new object[]
                    {
                            fixture.GetInvalidInputWithNegativeOpeningBalance(),
                            $"OpeningBalance cannot be negative value"
                    });
                    break;
                default:
                    break;
            }
        }
        return invalidInputsList;
    }
}
