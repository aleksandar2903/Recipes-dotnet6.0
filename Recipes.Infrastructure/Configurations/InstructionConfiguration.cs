using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recipes.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipes.Infrastructure.Configurations
{
    internal class InstructionConfiguration : IEntityTypeConfiguration<Instruction>
    {
        public void Configure(EntityTypeBuilder<Instruction> builder)
        {
            builder.HasKey(instruction => instruction.Id);
            builder.Property<string>(instruction => instruction.Text).HasMaxLength(500).IsRequired();
            builder.Property<int>(instruction => instruction.Position).IsRequired();
            builder.Property<Guid>(instruction => instruction.RecipeId).IsRequired();
        }
    }
}
