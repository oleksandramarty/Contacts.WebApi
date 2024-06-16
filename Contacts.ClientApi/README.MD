# Migrations DataContext

Check Terminal folder for migration
Contacts.Domain

> dotnet ef migrations add YourMigrationName --context DataContext --output-dir Migrations --startup-project
> ..\Contacts.ClientApi\Contacts.ClientApi.csproj

# MacOs

> dotnet ef migrations add YourMigrationName --context DataContext --output-dir Migrations --startup-project
> ../Contacts.ClientApi/Contacts.ClientApi.csproj


#dotnet-ef not found - check version
> dotnet tool install --global dotnet-ef --version 9.0.100

## For apply migration just run the ClientApi project
> If no - check Program.cs
> app.UpdateDatabaseAsync().Wait(cancellationToken);

### Backend API (ASP.NET 9.0.100 and PostgreSQL 16.1)
- **Autofac** (Version 7.1.0)
- **Autofac.Extensions.DependencyInjection** (Version 8.0.0)
- **AutoMapper** (Version 12.0.1)
- **MediatR** (Version 12.2.0)
- **MediatR.Extensions.Microsoft.DependencyInjection** (Version 11.1.0)
- **Npgsql** (Version 8.0.0)
- **Npgsql.EntityFrameworkCore.PostgreSQL** (Version 8.0.0)
- **GraphQL** (Version 7.8.0)
- **FluentValidation** (Version 11.8.1)
- **MediatR.Contracts** (Version 2.0.1)

## API Endpoint
> Please use graphQL
> By default port is http://localhost:5016/graphql
> If not - check launch settings

## GraphQL Queries and Mutations

### Mutations

#### CreateContact
- **Description**: Create a ew contact.
- **Example**:
  ```graphql
  mutation CreateContact($input: ContactInputType!) {
    createContact(input: $input) {
      id
      firstName
      lastName
      email
      phone
      title
      middleInitial
    }
  }

#### UpdateContact
- **Description**: Update an existing contact by ID.
- **Example**:
  ```graphql
  mutation UpdateContact($id: Int!, $input: ContactInputType!) {
    updateContact(id: $id, input: $input) {
      id
      firstName
      lastName
      email
      phone
      title
      middleInitial
    }
  }

#### DeleteContact
- **Description**: Delete a contact by ID.
- **Example**:
  ```graphql
  mutation DeleteContact($id: Int!) {
    deleteContact(id: $id)
  }

#### GetContactById
- **Description**: Retrieve a contact by ID.
- **Example**:
  ```graphql
  query GetContactById($id: Int!) {
    contact(id: $id) {
      id
      firstName
      lastName
      email
      phone
      title
      middleInitial
    }
  }

#### GetFilteredContacts
- **Description**: Retrieve contacts based on filtering criteria.
- **Example**:
  ```graphql
  query GetFilteredContacts(
    $query: String,
    $isFull: Boolean,
    $pageNumber: Int,
    $pageSize: Int,
    $column: String,
    $direction: String
  ) {
    contacts(
      query: $query,
      isFull: $isFull,
      pageNumber: $pageNumber,
      pageSize: $pageSize,
      column: $column,
      direction: $direction
    ) {
      entities {
        id
        firstName
        lastName
        email
        phone
        title
        middleInitial
      }
      paginator {
        pageNumber
        pageSize
        isFull
      }
      totalCount
    }
  }

## Setting Up Testing Bench of Contacts

To populate your database with a testing bench of contacts, you can use the following SQL script in PostgreSQL. This script will generate 5000 sample contacts with simulated data:

```sql
-- SQL script to generate a testing bench of contacts
WITH contacts_data AS (
    SELECT
        'FirstName' || num AS "FirstName",
        'LastName' || num AS "LastName",
        'user' || num || '@example.com' AS "Email",
        '+234567890' || LPAD(CAST(num AS TEXT), 2, '0') AS "Phone",
        'Title' || num AS "Title",
        CASE WHEN num % 3 = 0 THEN NULL ELSE CHR(65 + num % 26) END AS "MiddleInitial",
        NOW() AS "CreatedDate"
    FROM
        GENERATE_SERIES(1, 5000) AS s(num)
)
INSERT INTO contacts ("FirstName", "LastName", "Email", "Phone", "Title", "MiddleInitial", "CreatedDate")
SELECT "FirstName", "LastName", "Email", "Phone", "Title", "MiddleInitial", "CreatedDate"
FROM contacts_data;
```

## Enhancements and Improvements

### Next Steps
While the core features have been implemented within the given time frame, here are some enhancements and improvements that could be considered for future iterations:

- **Add Logging**: Implement a logging mechanism to track important events and errors within the application.

- **Refactoring**: Conduct refactoring to improve code readability, maintainability, and adherence to best practices such as SOLID principles.

- **Add Integration Tests**: Extend test coverage by adding integration tests to validate the interaction between components of the system.

- **Update Unit Tests**: Enhance existing unit tests to cover more edge cases and critical functionalities, ensuring robustness in testing.

### Future Features
If additional time were available, these features could further enhance the application:

- **Sorting and Filtering Options**: Implement more advanced sorting and filtering options for the contact list based on various attributes.

- **Enhanced Validation**: Strengthen both client-side and server-side validation to ensure data integrity and user input correctness.

- **Deployment Scripts**: Provide scripts or documentation for easy deployment to cloud services like Azure or for running the application in different environments.

These enhancements and future features aim to improve the overall usability, performance, and maintainability of the Contact Management System.