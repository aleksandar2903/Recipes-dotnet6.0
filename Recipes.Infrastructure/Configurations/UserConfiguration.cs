using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recipes.Domain.Entities;
using Recipes.Domain.ValueObjects;

namespace Recipes.Infrastructure.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(user => user.Id);
        builder.HasIndex(user => user.Email).IsUnique();
        builder.Property<string>(user => user.Email).HasMaxLength(Email.MaxLenght).IsRequired();
        builder.Property<string>(user => user.FirstName).HasMaxLength(FirstName.MaxLenght).IsRequired();
        builder.Property<string>(user => user.LastName).HasMaxLength(LastName.MaxLenght).IsRequired();

        builder.HasMany(author => author.Recipes)
            .WithOne(recipe => recipe.Author)
            .HasForeignKey(recipe => recipe.AuthorId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
