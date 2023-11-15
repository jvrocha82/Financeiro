using Financial.Application.UseCases.BankAccount.Common;
using Financial.Domain.SeedWork.SearchableRepository;
using FluentAssertions;
using Moq;
using Xunit;
using DomainEntity = Financial.Domain.Entity;
using UseCase = Financial.Application.UseCases.BankAccount.ListBankAccount;
namespace Financial.UnitTests.Application.BankAccount.ListBankAccount;
[Collection(nameof(ListBankAccountTestFixture))]

public class ListBankAccountTest
{
    private readonly ListBankAccountTestFixture _fixture;

    public ListBankAccountTest(ListBankAccountTestFixture fixture)
        => _fixture = fixture;

    [Fact(DisplayName = nameof(List))]
    [Trait("Application", "ListBankAccount - Use Cases")]
    public async Task List()
    {
        var bankAccountExampleList = _fixture.GetExampleBankAccountList();
        var repositoryMock = _fixture.GetRepositoryMock();
        var input = new UseCase.ListBankAccountInput(
            page: 2,
            perPage: 15,
            search: "search-example",
            sort: "name",
            dir: SearchOrder.Asc

            );
        var outputRepositorySearch = new SearchOutput<DomainEntity.BankAccount>(
                currentPage: input.Page,
                perPage: input.PerPage,
                items: (IReadOnlyList<DomainEntity.BankAccount>)bankAccountExampleList,
                total: (new Random()).Next(50, 200)

            );
        repositoryMock.Setup(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page
                && searchInput.PerPage == input.PerPage
                && searchInput.OrderBy == input.Sort
                && searchInput.Order == input.Dir
                ),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(outputRepositorySearch);

        var useCase = new UseCase.ListBankAccount(repositoryMock.Object);
        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Page.Should().Be(outputRepositorySearch.CurrentPage);
        output.PerPage.Should().Be(outputRepositorySearch.PerPage);
        output.Total.Should().Be(outputRepositorySearch.Total);
        output.Items.Should().HaveCount(outputRepositorySearch.Items.Count);
        ((List<BankAccountModelOutput>)output.Items).ForEach(outputItem =>
        {
            var repositoryBankAccount = outputRepositorySearch.Items
                .FirstOrDefault(x => x.Id == outputItem.Id);
            outputItem.Should().NotBeNull();
            outputItem.Name.Should().Be(repositoryBankAccount!.Name);
            outputItem.OpeningBalance.Should().Be(repositoryBankAccount.OpeningBalance);
            outputItem.OpeningBalanceIsNegative.Should().Be(repositoryBankAccount.OpeningBalanceIsNegative);
            outputItem.IsActive.Should().Be(repositoryBankAccount.IsActive);
            (outputItem.Id != Guid.Empty).Should().BeTrue();
            (outputItem.CreatedAt != default).Should().BeTrue();
        });
        repositoryMock.Verify(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page
                && searchInput.PerPage == input.PerPage
                && searchInput.OrderBy == input.Sort
                && searchInput.Order == input.Dir
                ),
            It.IsAny<CancellationToken>()
            ), Times.Once);
    }
    [Fact(DisplayName = nameof(ListOkWhenEmpty))]
    [Trait("Application", "ListBankAccount - Use Cases")]
    public async Task ListOkWhenEmpty()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var input = new UseCase.ListBankAccountInput(
            page: 2,
            perPage: 15,
            search: "search-example",
            sort: "name",
            dir: SearchOrder.Asc

            );
        var outputRepositorySearch = new SearchOutput<DomainEntity.BankAccount>(
                currentPage: input.Page,
                perPage: input.PerPage,
                items: (new List<DomainEntity.BankAccount>()).AsReadOnly(),
                total: 0

            );
        repositoryMock.Setup(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page
                && searchInput.PerPage == input.PerPage
                && searchInput.OrderBy == input.Sort
                && searchInput.Order == input.Dir
                ),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(outputRepositorySearch);

        var useCase = new UseCase.ListBankAccount(repositoryMock.Object);
        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Page.Should().Be(outputRepositorySearch.CurrentPage);
        output.PerPage.Should().Be(outputRepositorySearch.PerPage);
        output.Total.Should().Be(0);
        output.Items.Should().HaveCount(0);
  
        repositoryMock.Verify(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page
                && searchInput.PerPage == input.PerPage
                && searchInput.OrderBy == input.Sort
                && searchInput.Order == input.Dir
                ),
            It.IsAny<CancellationToken>()
            ), Times.Once);
    }
}
