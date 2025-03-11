namespace UKParliament.CodeTest.Web.ViewModels;

public class PersonViewModel
{
    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public DateTime DateOfBirth { get; set; }

    public int DepartmentId { get; set; }
}