using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Data.Repositories;
using UKParliament.CodeTest.Services;
using UKParliament.CodeTest.Web.Controllers;
using UKParliament.CodeTest.Web.Validators;
using UKParliament.CodeTest.Web.ViewModels;
using Xunit;

namespace UKParliament.CodeTest.Tests;

public class PersonControllerTests
{
    private readonly IValidator<PersonViewModel> _realValidator;
    private readonly Mock<IPersonRepository> _mockRepository;
    private readonly Mock<IPersonService> _mockService;
    private readonly IPersonService _realService;
    private readonly PersonController _controller;

    public PersonControllerTests()
    {
        _realValidator = new PersonRequestValidator();
        _mockRepository = new Mock<IPersonRepository>();
        _mockService = new Mock<IPersonService>(MockBehavior.Loose);
        _realService = new PersonService(_mockRepository.Object);
        _controller = new PersonController(_realValidator, _mockService.Object);
    }

    [Fact]
    public async Task GetPeople_ReturnsOkResult_WithListOfPeople()
    {
        // Arrange
        var persons = new List<Person>
            {
                new Person { Id = 1, FirstName = "John", LastName = "Smith", DateOfBirth = DateTime.Parse("1976-03-26"), DepartmentId = 1 },
                new Person { Id = 2, FirstName = "Tommy", LastName = "Townsend", DateOfBirth = DateTime.Parse("2005-11-15"), DepartmentId = 2 }
            };
        _mockService.Setup(service => service.GetPersonsAsync()).ReturnsAsync(persons);

        // Act
        var result = await _controller.GetPeople();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnPersons = Assert.IsAssignableFrom<IEnumerable<Person>>(okResult.Value);

        Assert.Equal(2, returnPersons.Count());
    }

    [Fact]
    public async Task GetPeoplePaged_ReturnsPagedResults()
    {
        // Arrange: create a list representing all of the data.
        var pageSize = 5;
        var persons = new List<Person>
            {
                new Person { Id = 1, FirstName = "John", LastName = "Smith", DateOfBirth = DateTime.Parse("1976-03-26"), DepartmentId = 1 },
                new Person { Id = 2, FirstName = "Tommy", LastName = "Townsend", DateOfBirth = DateTime.Parse("2005-11-15"), DepartmentId = 2 },                
                new Person { Id = 3, FirstName = "John", LastName = "Smith", DateOfBirth = System.DateTime.Now, DepartmentId = 1 },
                new Person { Id = 4, FirstName = "Sioban", LastName = "Knight", DateOfBirth = DateTime.Parse("2005-11-15"), DepartmentId = 1 },
                new Person { Id = 5, FirstName = "Edward", LastName = "Hover", DateOfBirth = DateTime.Parse("1983-03-19"), DepartmentId = 3 },
                new Person { Id = 6, FirstName = "Anwar", LastName = "Suleman", DateOfBirth = DateTime.Parse("1989-05-21"), DepartmentId = 2 },
                new Person { Id = 7, FirstName = "Yasmin", LastName = "Al Talib", DateOfBirth = DateTime.Parse("2001-09-30"), DepartmentId = 4 },
                new Person { Id = 8, FirstName = "Misha", LastName = "Nakisj", DateOfBirth = DateTime.Parse("1979-08-11"), DepartmentId = 4 },
                new Person { Id = 9, FirstName = "Vladimir", LastName = "Crakatan", DateOfBirth = DateTime.Parse("2001-01-02"), DepartmentId = 2 },
                new Person { Id = 10, FirstName = "Peter", LastName = "File", DateOfBirth = DateTime.Parse("2000-11-19"), DepartmentId = 4 }
            };

        _mockRepository.Setup(repository => repository.CountAsync()).ReturnsAsync(persons.Count);
        _mockRepository.Setup(repository => repository.GetPagedAsync(1, pageSize)).ReturnsAsync(persons.Take(pageSize));

        // Act
        var result = await _realService.GetPagedPersonsAsync(1, pageSize);

        // Assert
        var okResult = Assert.IsType<PagedResult<Person>>(result);
        Assert.Equal(2, result.Pages);
        Assert.Equal(pageSize, result.Values.Count());
    }

    [Fact]
    public async Task GetPerson_ReturnsNotFound_WhenPersonDoesNotExist()
    {
        // Arrange
        _mockService.Setup(service => service.GetPersonAsync(It.IsAny<int>())).ReturnsAsync((Person?)null);

        // Act
        var result = await _controller.GetPerson(1);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreatePerson_ReturnsCreatedAtActionResult_WhenPersonIsCreated()
    {
        // Arrange: Create a DTO for a new person.
        var request = new PersonViewModel
        {
            FirstName = "John",
            LastName = "Smith",
            DateOfBirth = DateTime.Parse("1976-03-26"),
            DepartmentId = 2
        };

        // Simulate the AddPersonAsync call by setting the Id on the person.
        _mockService
            .Setup(service => service.AddPersonAsync(It.IsAny<Person>()))
            .Callback<Person>(p => p.Id = 1)
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.CreatePerson(request);

        // Assert: Verify that a CreatedAtActionResult is returned with the mapped Person.
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var createdPerson = Assert.IsType<Person>(createdResult.Value);

        Assert.Equal("John", createdPerson.FirstName);
        Assert.Equal(1, createdPerson.Id);
    }

    [Fact]
    public async Task UpdatePerson_ReturnsNoContent_WhenSuccessful()
    {
        // Arrange: Create a DTO for updating a person.
        var request = new PersonViewModel
        {
            FirstName = "John",
            LastName = "Smith",
            DateOfBirth = DateTime.Parse("1976-03-26"),
            DepartmentId = 2
        };

        // Simulate an existing person.
        var existingPerson = new Person
        {
            Id = 1,
            FirstName = "Old",
            LastName = "Name",
            DateOfBirth = DateTime.Now.AddYears(-30),
            DepartmentId = 1
        };

        _mockService.Setup(service => service.GetPersonAsync(1)).ReturnsAsync(existingPerson);
        _mockService.Setup(service => service.UpdatePersonAsync(existingPerson)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdatePerson(1, request);

        // Assert: Verify that the result is a NoContent result and that the domain model was updated.
        Assert.IsType<NoContentResult>(result);
        Assert.Equal("John", existingPerson.FirstName);
        Assert.Equal("Smith", existingPerson.LastName);
        Assert.Equal(2, existingPerson.DepartmentId);
    }

    [Fact]
    public async Task UpdatePerson_ReturnsNotFound_WhenPersonDoesNotExist()
    {
        // Arrange: Create a DTO for updating a person.
        var request = new PersonViewModel
        {
            FirstName = "John",
            LastName = "Smith",
            DateOfBirth = DateTime.Parse("1976-03-26"),
            DepartmentId = 2
        };

        // Simulate no existing person found.
        _mockService.Setup(service => service.GetPersonAsync(1)).ReturnsAsync((Person?)null);

        // Act
        var result = await _controller.UpdatePerson(1, request);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeletePerson_ReturnsNotFound_WhenPersonDoesNotExist()
    {
        // Arrange: Simulate that no person is found.
        _mockService.Setup(service => service.GetPersonAsync(It.IsAny<int>())).ReturnsAsync((Person?)null);

        // Act
        var result = await _controller.DeletePerson(1);

        // Assert: Should return a NotFound result.
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeletePerson_ReturnsNoContent_WhenSuccessful()
    {
        // Arrange: Simulate an existing person.
        var existingPerson = new Person
        {
            Id = 1,
            FirstName = "John",
            LastName = "Smith",
            DateOfBirth = DateTime.Parse("1976-03-26"),
            DepartmentId = 2
        };

        _mockService.Setup(service => service.GetPersonAsync(1)).ReturnsAsync(existingPerson);
        _mockService.Setup(service => service.DeletePersonAsync(1)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeletePerson(1);

        // Assert: Should return a NoContent result.
        Assert.IsType<NoContentResult>(result);
    }

}