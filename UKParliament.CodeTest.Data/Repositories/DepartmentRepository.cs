using Microsoft.EntityFrameworkCore;

namespace UKParliament.CodeTest.Data.Repositories;

public class DepartmentRepository : IDepartmentRepository
{

    private readonly PersonManagerContext _context;

    public DepartmentRepository(PersonManagerContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Department>> GetAsync() =>
        await _context.Departments.ToListAsync();    

    public async Task<Department?> GetByIdAsync(int id) => 
        await _context.Departments.FirstOrDefaultAsync(x => x.Id == id);
    
}
