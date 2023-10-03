using Xunit;
using FluentAssertions;
using Financial.Domain.Entity;
using System.Security.Principal;
using Financial.UnitTests.Domain.Entity.Account;
using DomainEntity = Financial.Domain.Entity;


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

}
