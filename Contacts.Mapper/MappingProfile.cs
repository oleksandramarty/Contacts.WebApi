using AutoMapper;
using Contacts.Domain.Models.Contacts;
using Contacts.Mediatr.Mediatr.Contacts.Command;
using Contacts.Models.Requests.Base;
using Contacts.Models.Responses.Base;
using Contacts.Models.Responses.Contacts;

namespace Contacts.Mapper;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        CreateMap<CreateOrUpdateContactCommand, Contact>()
            .ForMember(dest => dest.Id, opt => opt.Condition(src => src.Id.HasValue))
            .AfterMap((src, dest) =>
            {
                if (src.Id == null)
                {
                    dest.CreatedDate = DateTime.UtcNow;
                }
                else
                {
                    dest.ModifiedDate = DateTime.UtcNow;
                }
            });
        this.CreateMap<Contact, ContactResponse>();
        this.CreateMap<PaginatorRequest, PaginatorResponse>();
    }
}