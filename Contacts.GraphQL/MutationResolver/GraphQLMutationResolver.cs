using Contacts.Kernel.Utils;
using Contacts.Models.Responses.Base;
using GraphQL;
using GraphQL.Types;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Contacts.GraphQL.MutationResolver;

public class GraphQLMutationResolver: ObjectGraphType, IGraphQLMutationResolver
{
    public void CreateEntity<TType, TInputType, TCommand, TMapped, TId, TGraphId>(string name) 
        where TCommand: IRequest<TMapped> 
        where TInputType: InputObjectGraphType
        where TGraphId: ScalarGraphType
        where TType: ObjectGraphType<TMapped>
    {
        Field<TType>(name)
            .Arguments(new QueryArguments(
                new QueryArgument<NonNullGraphType<TInputType>> { Name = "input" }
            ))
            .ResolveAsync(async context => await this.HandleRequestAsync<TCommand, TMapped, TId>(context));
    }
    
    public void UpdateEntity<TType, TInputType, TCommand, TMapped, TId, TGraphId>(string name) 
        where TCommand: IRequest<TMapped> 
        where TInputType: InputObjectGraphType
        where TGraphId: ScalarGraphType
        where TType: ObjectGraphType<TMapped>
    {
        Field<TType>(name)
            .Arguments(new QueryArguments(
                new QueryArgument<NonNullGraphType<TGraphId>> { Name = "id" },
                new QueryArgument<NonNullGraphType<TInputType>> { Name = "input" }
            ))
            .ResolveAsync(async context => await this.HandleRequestAsync<TCommand, TMapped, TId>(context, true));
    }
    
    public void DeleteEntity<TCommand, TId, TGraphId>(string name) 
        where TCommand: IRequest
        where TGraphId: ScalarGraphType
    {
        Field<BooleanGraphType>(name)
            .Arguments(new QueryArguments(
                new QueryArgument<NonNullGraphType<TGraphId>> { Name = "id" }
            ))
            .ResolveAsync(async context =>
            {
                try
                {
                    var cancellationToken = context.CancellationToken;
                    await this.DeleteEntityAsync<TCommand, TId>(context, cancellationToken);
                    return true;
                }
                catch (Exception e)
                {
                    throw new ExecutionError(e.Message);
                }
            });
    }
    
    private async Task<TMapped> HandleRequestAsync<TCommand, TMapped, TId>(
        IResolveFieldContext<object?> context, bool isUpdate = false)
        where TCommand: IRequest<TMapped> 
    {
        try
        {
            var cancellationToken = context.CancellationToken;
            TMapped result = await this.ModifyEntityWithResultAsync<TCommand, TMapped, TId>(context, cancellationToken, isUpdate);
            return result;
        }
        catch (Exception e)
        {
            throw new ExecutionError(e.Message);
        }
    }

    private async Task<TResult> ModifyEntityWithResultAsync<TCommand, TResult, TId>(IResolveFieldContext<object?> context, CancellationToken cancellationToken, bool isUpdate = false) where TCommand: IRequest<TResult>
    {
        TCommand command = context.GetArgument<TCommand>("input");
        if (isUpdate)
        {
            ReflectionUtils.SetValue<TCommand, TId>(command, "Id", context.GetArgument<TId>("id"));
        }
        var mediator = context.RequestServices.GetRequiredService<IMediator>();
        TResult result = await mediator.Send(command, cancellationToken);
        return result;
    }
    
    private async Task DeleteEntityAsync<TCommand, TId>(IResolveFieldContext<object?> context, CancellationToken cancellationToken) where TCommand: IRequest
    {
        var commandType = typeof(TCommand);
        var constructorInfo = commandType.GetConstructor(new[] { typeof(TId) });
        if (constructorInfo == null)
        {
            throw new InvalidOperationException($"Type {commandType.Name} does not have a constructor that accepts a parameter of type {typeof(TId).Name}.");
        }

        TId id = context.GetArgument<TId>("id");
        var commandInstance = (TCommand)constructorInfo.Invoke(new object[] { id });

        var mediator = context.RequestServices.GetRequiredService<IMediator>();
        await mediator.Send(commandInstance, cancellationToken);
    }
}