using Financial.Application.UseCases.BankAccount.UpdateBankAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial.UnitTests.Application.BankAccount.UpdateBankAccount;
public class UpdateBankAccountTestDataGenerator
{
    public static IEnumerable<object[]> GetBankAccountToUpdate(int times = 10)
    {
        var fixture = new UpdateBankAccountTestFixture();
        for(int indice = 0; indice < times; indice++)
        {
            var exampleBankAccount = fixture.GetValidBankAccount();
            var exampleInput = fixture.GetValidBankAccountInput(exampleBankAccount.Id);
            yield return new object[] {
                exampleBankAccount, exampleInput
            };
        }
    }
}
