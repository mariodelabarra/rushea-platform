using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rushea.Identity.API.Domain;

namespace Rushea.Identity.API.Data.Configurations;

public class SiteConfiguration : IEntityTypeConfiguration<Site>
{
    public void Configure(EntityTypeBuilder<Site> builder)
    {
        builder.ToTable("sites");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(s => s.TimeZone)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(s => s.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.HasOne<Organisation>()
            .WithMany()
            .HasForeignKey(s => s.OrganisationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
