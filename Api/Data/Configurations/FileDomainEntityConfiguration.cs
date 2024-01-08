using Filio.Api.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Filio.Api.Data.Configurations;

/// <summary>
/// File Domain entity configuration
/// </summary>
public class FileDomainEntityConfiguration : IEntityTypeConfiguration<FileDomain>
{
    public void Configure(EntityTypeBuilder<FileDomain> builder)
    {
        builder.HasIndex(x => x.Id).IsUnique();
        builder.HasIndex(x => x.IsDeleted);
    }
}