using MediatR;
using Recipes.Domain.Shared;

namespace Recipes.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
