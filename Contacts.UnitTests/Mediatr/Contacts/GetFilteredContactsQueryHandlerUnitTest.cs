using System.Linq.Expressions;
using Contacts.Domain.Models.Contacts;
using Contacts.Interfaces;
using Contacts.Mediatr.Mediatr.Contacts.Command;
using Contacts.Mediatr.Mediatr.Contacts.Handlers;
using Contacts.Models.Requests.Base;
using Contacts.Models.Responses.Base;
using Contacts.Models.Responses.Contacts;
using Moq;
using Xunit;

namespace Contacts.UnitTests.Mediatr.Contacts;

public class GetFilteredContactsQueryHandlerUnitTest
{
    private readonly Mock<IReadGenericRepository<Contact>> _contactRepositoryMock;
    private readonly GetFilteredContactsQueryHandler _handler;

    public GetFilteredContactsQueryHandlerUnitTest()
    {
        _contactRepositoryMock = new Mock<IReadGenericRepository<Contact>>();
        _handler = new GetFilteredContactsQueryHandler(_contactRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnFilteredContacts()
    {
        // Arrange
        var query = new GetFilteredContactsQuery
        {
            Paginator = new PaginatorRequest(1, 10, false),
            Sort = new BaseSortableRequest("FirstName", "asc")
        };

        var contacts = new List<Contact>
        {
            new Contact { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" },
            new Contact { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com" }
        };

        var mappedContacts = contacts.Select(c => new ContactResponse
        {
            Id = c.Id,
            FirstName = c.FirstName,
            LastName = c.LastName,
            Email = c.Email
        }).ToList();

        _contactRepositoryMock.Setup(r => r.GetListWithIncludeAsync<ContactResponse>(
            It.IsAny<Expression<Func<Contact, bool>>>(),
            query,
            It.IsAny<CancellationToken>(),
            null
        )).ReturnsAsync(new ListWithIncludeResponse<ContactResponse>
        {
            Entities = mappedContacts,
            TotalCount = contacts.Count
        });

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(contacts.Count, result.Entities.Count);
        Assert.Equal(contacts.Count, result.TotalCount);

        // You may add more assertions based on your specific requirements
    }
}