using Financial.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Financial.Infra.Data.EF.Configurations;
internal class BankAccountConfiguration
    : IEntityTypeConfiguration<BankAccount>
{
    public void Configure(EntityTypeBuilder<BankAccount> builder)
    {
        builder.HasKey(BankAccount => BankAccount.Id);
        builder.Property(BankAccount => BankAccount.Name)
            .HasMaxLength(255);
        
    }

}
