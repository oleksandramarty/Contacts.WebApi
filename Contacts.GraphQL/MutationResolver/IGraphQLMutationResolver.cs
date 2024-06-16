using Contacts.Models.Responses.Base;
using GraphQL.Types;
using MediatR;

namespace Contacts.GraphQL.MutationResolver;

public interface IGraphQLMutationResolver
{
    void CreateEntity<TType, TInputType, TCommand, TMapped, TId, TGraphId>(string name) 
        where TCommand: IRequest<TMapped> 
        where TInputType: InputObjectGraphType
        where TGraphId: ScalarGraphType
        where TType: ObjectGraphType<TMapped>;

    void UpdateEntity<TType, TInputType, TCommand, TMapped, TId, TGraphId>(string name) 
        where TCommand: IRequest<TMapped> 
        where TInputType: InputObjectGraphType
        where TGraphId: ScalarGraphType
        where TType: ObjectGraphType<TMapped>;

    void DeleteEntity<TCommand, TId, TGraphId>(string name)
        where TCommand : IRequest
        where TGraphId : ScalarGraphType;
}