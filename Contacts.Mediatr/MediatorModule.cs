using System.Reflection;
using Autofac;
using Contacts.Mediatr.Mediatr.Contacts.Command;
using Contacts.Mediatr.Mediatr.Contacts.Handlers;
using MediatR;

namespace Contacts.Mediatr;

public class MediatorModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
            .AsImplementedInterfaces();

        #region Contacts
        builder.RegisterAssemblyTypes(typeof(CreateOrUpdateContactCommand).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IRequestHandler<,>));
        builder.RegisterAssemblyTypes(typeof(DeleteContactCommand).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IRequestHandler<>));
        builder.RegisterAssemblyTypes(typeof(GetFilteredContactsQueryHandler).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IRequestHandler<,>));
        builder.RegisterAssemblyTypes(typeof(GetContactByIdQueryHandler).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IRequestHandler<,>));
        #endregion
    }
}