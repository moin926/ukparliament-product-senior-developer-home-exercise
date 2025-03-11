using Microsoft.EntityFrameworkCore;

namespace UKParliament.CodeTest.Data.Repositories;

public class DepartmentRepository : IDepartmentRepository
{

    private readonly PersonManagerContext _context;

    public DepartmentRepository(PersonManagerContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Department>> GetAsync()
    {
        return await _context.Departments.ToListAsync();
    }
}
