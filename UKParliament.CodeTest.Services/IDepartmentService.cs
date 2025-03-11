using UKParliament.CodeTest.Data;

namespace UKParliament.CodeTest.Services;

public interface IDepartmentService
{
    Task<IEnumerable<Department>> GetDepartmentsAsync();
    Task<Department?> GetDepartmentAsync(int id);
}
