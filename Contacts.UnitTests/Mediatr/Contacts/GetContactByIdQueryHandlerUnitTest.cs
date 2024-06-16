using AutoMapper;
using Contacts.Domain.Models.Contacts;
using Contacts.Interfaces;
using Contacts.Mediatr.Mediatr.Contacts.Command;
using Contacts.Mediatr.Mediatr.Contacts.Handlers;
using Contacts.Models.Responses.Contacts;
using Moq;
using Xunit;

namespace Contacts.UnitTests.Mediatr.Contacts;

public class GetContactByIdQueryHandlerUnitTest
{
    private readonly Mock<IReadGenericRepository<Contact>> _contactRepositoryMock;
    private readonly Mock<IEntityValidator> _entityValidatorMock;
    private readonly IMapper _mapper;
    private readonly GetContactByIdQueryHandler _handler;

    public GetContactByIdQueryHandlerUnitTest()
    {
        _contactRepositoryMock = new Mock<IReadGenericRepository<Contact>>();
        _entityValidatorMock = new Mock<IEntityValidator>();

        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Contact, ContactResponse>();
        });
        _mapper = configuration.CreateMapper();

        _handler = new GetContactByIdQueryHandler(
            _mapper,
            _contactRepositoryMock.Object,
            _entityValidatorMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnContact_WhenContactExists()
    {
        // Arrange
        var query = new GetContactByIdQuery(1);
        var contact = new Contact { Id = 1, FirstName = "John", LastName = "Doe" };

        _contactRepositoryMock.Setup(r => r.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(contact);

        _entityValidatorMock.Setup(v => v.ValidateExist(It.IsAny<Contact>(), query.Id));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(contact.Id, result.Id);
        _contactRepositoryMock.Verify(r => r.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()), Times.Once);
        _entityValidatorMock.Verify(v => v.ValidateExist(contact, query.Id), Times.Once);
    }
}