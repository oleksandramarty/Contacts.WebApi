using AutoMapper;
using Contacts.Domain.Models.Contacts;
using Contacts.Interfaces;
using Contacts.Mediatr.Mediatr.Contacts.Command;
using Contacts.Mediatr.Mediatr.Contacts.Handlers;
using Contacts.Models.Responses.Contacts;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace Contacts.UnitTests.Mediatr.Contacts;

public class CreateOrUpdateContactCommandHandlerUnitTest
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IEntityValidator> _entityValidatorMock;
    private readonly Mock<IGenericRepository<Contact>> _contactRepositoryMock;
    private readonly CreateOrUpdateContactCommandHandler _handler;

    public CreateOrUpdateContactCommandHandlerUnitTest()
    {
        _mapperMock = new Mock<IMapper>();
        _entityValidatorMock = new Mock<IEntityValidator>();
        _contactRepositoryMock = new Mock<IGenericRepository<Contact>>();
        _handler = new CreateOrUpdateContactCommandHandler(
            _mapperMock.Object,
            _entityValidatorMock.Object,
            _contactRepositoryMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldCreateNewContact_WhenIdIsNull()
    {
        // Arrange
        var command = new CreateOrUpdateContactCommand(null, "John", "Doe", "john.doe@example.com", "1234567890", "Manager", null);

        _mapperMock.Setup(m => m.Map<CreateOrUpdateContactCommand, Contact>(It.IsAny<CreateOrUpdateContactCommand>(), It.IsAny<Action<IMappingOperationOptions>>()))
            .Returns(new Contact());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _contactRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Contact>(), It.IsAny<CancellationToken>()), Times.Once);
        _contactRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Contact>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldUpdateContact_WhenIdIsNotNull()
    {
        // Arrange
        var contact = new Contact();
        var command = new CreateOrUpdateContactCommand(1, "John", "Doe", "john.doe@example.com", "1234567890", "Manager", null);

        _contactRepositoryMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(contact);
        _mapperMock.Setup(m => m.Map<CreateOrUpdateContactCommand, Contact>(It.IsAny<CreateOrUpdateContactCommand>(), It.IsAny<Action<IMappingOperationOptions>>()))
            .Returns(new Contact());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _contactRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Contact>(), It.IsAny<CancellationToken>()), Times.Once);
        _contactRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Contact>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldValidateRequest()
    {
        // Arrange
        var command = new CreateOrUpdateContactCommand(null, "John", "Doe", "john.doe@example.com", "1234567890", "Manager", null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _entityValidatorMock.Verify(v => v.ValidateRequest<CreateOrUpdateContactCommand, ContactResponse>(
            It.IsAny<CreateOrUpdateContactCommand>(), 
            It.IsAny<Func<FluentValidation.IValidator<CreateOrUpdateContactCommand>>>()
        ), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldValidateEmailAndPhoneExistence()
    {
        // Arrange
        var command = new CreateOrUpdateContactCommand(null, "John", "Doe", "john.doe@example.com", "1234567890", "Manager", null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _entityValidatorMock.Verify(v => v.ValidateExistParamAsync<Contact>(
            It.IsAny<Expression<Func<Contact, bool>>>(), 
            It.IsAny<string>(), 
            It.IsAny<CancellationToken>()
        ), Times.Exactly(2)); // Once for Email and once for Phone
    }
}