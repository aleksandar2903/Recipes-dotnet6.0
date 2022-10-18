using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Recipes.Domain.Entities;

namespace Recipes.Infrastructure.Configurations;

internal sealed class IngredientConfiguration : IEntityTypeConfiguration<Ingredient>
{
    public void Configure(EntityTypeBuilder<Ingredient> builder)
    {
        builder.HasKey(ingredient => ingredient.Id);
        builder.Property<int>(ingredient => ingredient.Position).IsRequired();
        builder.Property<string>(ingredient => ingredient.Text).HasMaxLength(150).IsRequired();
        builder.Property<Guid>(ingredient => ingredient.SectionId).IsRequired();
    }
}
