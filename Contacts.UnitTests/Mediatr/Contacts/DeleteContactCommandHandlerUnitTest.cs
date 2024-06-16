using Contacts.Domain.Models.Contacts;
using Contacts.Interfaces;
using Contacts.Mediatr.Mediatr.Contacts.Command;
using Contacts.Mediatr.Mediatr.Contacts.Handlers;
using Moq;
using Xunit;

namespace Contacts.UnitTests.Mediatr.Contacts;

public class DeleteContactCommandHandlerUnitTest
{
    private readonly Mock<IGenericRepository<Contact>> _contactRepositoryMock;
    private readonly Mock<IEntityValidator> _entityValidatorMock;
    private readonly DeleteContactCommandHandler _handler;

    public DeleteContactCommandHandlerUnitTest()
    {
        _contactRepositoryMock = new Mock<IGenericRepository<Contact>>();
        _entityValidatorMock = new Mock<IEntityValidator>();
        _handler = new DeleteContactCommandHandler(
            _contactRepositoryMock.Object,
            _entityValidatorMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldDeleteContact_WhenContactExists()
    {
        // Arrange
        var command = new DeleteContactCommand(1);
        var contact = new Contact();

        _contactRepositoryMock.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(contact);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _entityValidatorMock.Verify(v => v.ValidateExist<Contact, int>(contact, command.Id), Times.Once);
        _contactRepositoryMock.Verify(r => r.DeleteAsync<int>(command.Id, It.IsAny<CancellationToken>()), Times.Once);
    }
}