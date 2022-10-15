using MediatR;
using Recipes.Domain.Shared;

namespace Recipes.Application.Abstractions.Messaging;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
