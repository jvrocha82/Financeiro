using Financial.Application.Interfaces;
using Financial.Application.UseCases.Account;
using Financial.Domain.Entity;
using Financial.Domain.Repository;
using FluentAssertions;
using Moq;
using System.Threading;
using Xunit;
using UseCases = Financial.Application.UseCases.Account.CreateAccount;

namespace Financial.UnitTests.Application.CreateAccount;
public class CreateAccountTest
{
    [Fact(DisplayName = nameof(CreateAccount))]
    [Trait("Application", "CreateAccount - Use Cases")]
    public async void CreateAccount()
    {
        var repositoryMock = new Mock<IAccountRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var useCase = new UseCases.CreateAccount(
                repositoryMock.Object,
                unitOfWorkMock.Object
            );
        var input = new UseCases.CreateAccountInput(
                "Account Name",
                0,
                false,
                true
            );

        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(
            repository => repository.Insert(
                It.IsAny<Account>(),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
        unitOfWorkMock.Verify(
            uow => uow.Commit(It.IsAny<CancellationToken>()),
            Times.Once
        );


        output.Should().NotBeNull();
        output.Name.Should().Be("Account Name");
        output.OpeningBalance.Should().Be(0);
        output.OpeningBalanceIsNegative.Should().Be(false);
        output.IsActive.Should().Be(true);
        (output.Id != Guid.Empty).Should().BeTrue();
        ( output.CreatedAt != default(DateTime)).Should().BeTrue();

    }
}
