using Xunit;
using FluentAssertions;
using Financial.Domain.Entity;
using System.Security.Principal;
using Financial.UnitTests.Domain.Entity.Account;
using DomainEntity = Financial.Domain.Entity;
using Financial.Domain.Exceptions;
using FluentAssertions.Equivalency;

namespace Financial.UnitTests.Domain.Entity.User;
public class UserTest
{
    
    [Fact(DisplayName =nameof(Instantiate))]
    [Trait("Domain", "User - Aggregate")]
    public void Instantiate()
    {
   
        var datetimeBefore = DateTime.Now.AddSeconds(-1);
        var user = new DomainEntity.User("Account Name", true);
        var datetimeAfter = DateTime.Now.AddSeconds(1);

        Guid GuidExampleValue = new Guid();
        var typeGuidName = GuidExampleValue.GetType().Name;



        user.Should().NotBeNull();

        user.Id.GetType().Name.Should().Be(typeGuidName);
        user.Id.Should().NotBe(default(Guid));
        user.Name.Should().Be("Account Name");
        user.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        user.CreatedAt.Should().BeAfter(datetimeBefore);
        user.CreatedAt.Should().BeBefore(datetimeAfter);
        user.IsActive.Should().BeTrue();
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
    [Trait("Domain", "User - Aggregate")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("    ")]

    public void InstantiateErrorWhenNameIsEmpty(string? name)
    {
  
        Action action = () 
            => new DomainEntity.User(name!, true);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should not be empty or null");
    }
    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsLessThan3Characters))]
    [Trait("Domain", "User - Aggregate")]
    [MemberData(nameof(GetNamesWhenNameIsLessThan3Characters), parameters: 10)]
    public void InstantiateErrorWhenNameIsLessThan3Characters(string name)
    {
        Action action = ()
            => new DomainEntity.User(name!, true);
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should be at leats 3 characters long");
    }
    public static IEnumerable<object[]> GetNamesWhenNameIsLessThan3Characters(int numberOfTests = 6)
    {
        yield return new object[] {"21"};
        yield return new object[] {"t"};
        yield return new object[] { "te" };
        yield return new object[] { "le" };
        yield return new object[] { "s" };

    }
    [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "User - Aggregate")]

    public void InstantiateErrorWhenNameIsGreaterThan255Characters()
    {
        var invalidName = String.Join(null, Enumerable.Range(0, 256).Select(_ => "a").ToArray());
       
        Action action = ()
            => new DomainEntity.User(invalidName, true);
        
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should be less or equal 255 characters long");
    }

}
