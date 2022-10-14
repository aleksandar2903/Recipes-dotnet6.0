using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipes.Domain.Primitives
{
    public abstract class Entity
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        /// <param name="id">The entity identifier.</param>
        protected Entity(Guid id)
        {
            Id = id;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        /// <remarks>
        /// Required by EF Core.
        /// </remarks>
        protected Entity()
        {
        }

        /// <summary>
        /// Gets the entity identifier.
        /// </summary>
        public Guid Id { get; private init; }

        public static bool operator ==(Entity? first,
                                       Entity? second)
        {
            return first is not null && second is not null && first.Equals(second);
        }

        public static bool operator !=(Entity? first, Entity? second) => !(first == second);

        /// <inheritdoc />
        public bool Equals(Entity? other)
        {
            if (other is null)
            {
                return false;
            }

            if (other.GetType() != GetType())
            {
                return false;
            }

            return other.Id == Id;
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            if (obj is not Entity other)
            {
                return false;
            }

            return Id == other.Id;
        }

        /// <inheritdoc />
        public override int GetHashCode() => Id.GetHashCode();
    }
}
