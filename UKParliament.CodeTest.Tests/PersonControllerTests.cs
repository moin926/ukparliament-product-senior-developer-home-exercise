using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Services;
using UKParliament.CodeTest.Web.Controllers;
using UKParliament.CodeTest.Web.Validators;
using UKParliament.CodeTest.Web.ViewModels;
using Xunit;

namespace UKParliament.CodeTest.Tests;

public class PersonControllerTests
{
    private readonly IValidator<PersonViewModel> _realValidator;
    private readonly Mock<IPersonService> _mockService;
    private readonly PersonController _controller;

    public PersonControllerTests()
    {
        _realValidator = new PersonRequestValidator();
        _mockService = new Mock<IPersonService>();

        _controller = new PersonController(_realValidator, _mockService.Object);
    }

    [Fact]
    public async Task GetPeople_ReturnsOkResult_WithListOfPeople()
    {
        // Arrange
        var persons = new List<Person>
            {
                new Person { Id = 1, FirstName = "John", LastName = "Smith", DateOfBirth = System.DateTime.Now, DepartmentId = 1 },
                new Person { Id = 2, FirstName = "Tommy", LastName = "Townsend", DateOfBirth = System.DateTime.Now, DepartmentId = 2 }
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
    public async Task GetPerson_ReturnsNotFound_WhenPersonDoesNotExist()
    {
        // Arrange
        _mockService.Setup(service => service.GetPersonAsync(It.IsAny<int>())).ReturnsAsync((Person?)null);

        // Act
        var result = await _controller.GetPerson(1);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

}