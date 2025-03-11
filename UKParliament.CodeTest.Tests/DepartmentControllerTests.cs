using Microsoft.AspNetCore.Mvc;
using Moq;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Services;
using UKParliament.CodeTest.Web.Controllers;

namespace UKParliament.CodeTest.Tests;

public class DepartmentControllerTests
{
    private readonly Mock<IDepartmentService> _mockService;
    private readonly DepartmentController _controller;

    public DepartmentControllerTests()
    {
        _mockService = new Mock<IDepartmentService>();
        _controller = new DepartmentController(_mockService.Object);
    }

    [Fact]
    public async Task GetDepartments_ReturnsOkResult_WithListOfDepartments()
    {
        // Arrange: simulate a list of departments.
        var departments = new List<Department>
            {
                new Department { Id = 1, Name = "Finance" },
                new Department { Id = 2, Name = "HR" },
                new Department { Id = 3, Name = "IT" }
            };
        _mockService.Setup(s => s.GetDepartmentsAsync()).ReturnsAsync(departments);

        // Act
        var result = await _controller.GetDepartments();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedDepartments = Assert.IsAssignableFrom<IEnumerable<Department>>(okResult.Value);

        Assert.Equal(3, ((List<Department>)returnedDepartments).Count);
    }
}
