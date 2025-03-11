using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UKParliament.CodeTest.Data;

public class Person
{
    public int Id { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public DateTime DateOfBirth { get; set; }

    public int DepartmentId { get; set; }

    public Department? Department { get; set; }
}