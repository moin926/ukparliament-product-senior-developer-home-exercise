using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Data.Repositories;

namespace UKParliament.CodeTest.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;

    public DepartmentService(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    public async Task<IEnumerable<Department>> GetDepartmentsAsync() => 
        await _departmentRepository.GetAsync();

    public async Task<Department?> GetDepartmentAsync(int id) =>
        await _departmentRepository.GetByIdAsync(id);
}