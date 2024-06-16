using Contacts.Mediatr.Mediatr.Base;
using MediatR;

namespace Contacts.Mediatr.Mediatr.Contacts.Command;

public class DeleteContactCommand: BaseIdCommand<int>, IRequest
{
    public DeleteContactCommand(int id)
    {
        Id = id;
    }
}