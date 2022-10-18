using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recipes.Domain.Entities;

namespace Recipes.Infrastructure.Configurations;

internal sealed class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
{
    public void Configure(EntityTypeBuilder<Recipe> builder)
    {
        builder.HasKey(recipe => recipe.Id);
        builder.Property<string>(recipe => recipe.Title).HasMaxLength(150).IsRequired();
        builder.Property<Guid>(recipe => recipe.AuthorId).IsRequired();
        builder.Property<string>(recipe => recipe.Description).HasMaxLength(500).IsRequired();
        builder.Property<int?>(recipe => recipe.NumServings);
        builder.Property<int?>(recipe => recipe.Calories);
        builder.Property<int>(recipe => recipe.TotalTimeMinutes).IsRequired();
        builder.Property<Uri?>(recipe => recipe.VideoUrl).HasMaxLength(500);
        builder.Property<Uri>(recipe => recipe.ThumbnailUrl).HasMaxLength(500).IsRequired();

        builder.HasMany(recipe => recipe.Sections)
            .WithOne()
            .HasForeignKey(section => section.RecipeId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(recipe => recipe.Instructions)
            .WithOne()
            .HasForeignKey(instruction => instruction.RecipeId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
