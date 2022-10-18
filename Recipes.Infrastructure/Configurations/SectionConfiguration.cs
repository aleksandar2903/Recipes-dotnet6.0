using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Recipes.Domain.Entities;

namespace Recipes.Infrastructure.Configurations;

internal class SectionConfiguration : IEntityTypeConfiguration<Section>
{
    public void Configure(EntityTypeBuilder<Section> builder)
    {
        builder.HasKey(section => section.Id);
        builder.Property<int>(section => section.Position).IsRequired();
        builder.Property<string>(section => section.Text).HasMaxLength(150).IsRequired();
        builder.Property<Guid>(section => section.RecipeId).IsRequired();

        builder.HasMany(section => section.Ingredients)
            .WithOne()
            .HasForeignKey(ingredient => ingredient.SectionId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
