using Financial.IntegrationTests.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Financial.IntegrationTests.Infra.Data.EF.Repositories.BankAccountRepository;


[CollectionDefinition(nameof(BankAccountRepositoryTestFixture))]
public class BankAccountRepositoryTestFixtureCollection
    : ICollectionFixture<BankAccountRepositoryTestFixture>
{ }
public class BankAccountRepositoryTestFixture
    : BaseFixture
{
}
