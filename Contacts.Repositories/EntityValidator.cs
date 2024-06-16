using System.Linq.Expressions;
using Contacts.Constants.Errors;
using Contacts.Domain;
using Contacts.Interfaces;
using Contacts.Kernel.Extensions;
using FluentValidation;
using MediatR;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Repositories;

public class EntityValidator: IEntityValidator
{
    public async Task ValidateExistParamAsync<T>(Expression<Func<T, bool>> predicate, string customErrorMessage, CancellationToken cancellationToken) where T: class
    {
        T? entity = await _dataContext.Set<T>().FirstOrDefaultAsync(predicate, cancellationToken);

        if (entity != null)
        {
            throw new Exception(customErrorMessage.NotNullOrEmpty() ? customErrorMessage : ErrorMessages.EntityAlreadyExists);
        }
    }
    
    private readonly DataContext _dataContext;

    public EntityValidator(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    
    public void ValidateExist<T, TId>(T entity, TId? entityId) where T : class
    {
        if (entity == null)
        {
            throw new Exception(String.Format(ErrorMessages.EntityWithIdNotFound, typeof(T).Name, entityId));
        }
    }

    public void ValidateRequest<TCommand, TResult>(TCommand command, Func<IValidator<TCommand>> validatorFactory) where TCommand : IRequest<TResult>
    {
        this.FluentValidation<TCommand>(validatorFactory.Invoke(), command);
    }
    
    private void FluentValidation<TCommand>(IValidator<TCommand> validator, TCommand command)
    {
        ValidationResult validationResult = validator.Validate(command);
        
        if (validationResult.IsValid)
        {
            return;
        }
        
        throw new Exception(ErrorMessages.ValidationError);
    }
}