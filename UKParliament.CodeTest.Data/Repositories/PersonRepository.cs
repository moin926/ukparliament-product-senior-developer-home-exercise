using Microsoft.EntityFrameworkCore;

namespace UKParliament.CodeTest.Data.Repositories;

public class PersonRepository : IPersonRepository
{
    private readonly PersonManagerContext _context;
    private readonly IDepartmentRepository _departmentRepository;

    public PersonRepository(PersonManagerContext context, IDepartmentRepository departmentRepository)
    {
        _context = context;
        _departmentRepository = departmentRepository;
    }

    public async Task<int> CountAsync() =>
        await _context.People.CountAsync();

    public async Task<IEnumerable<Person>> GetAsync() => 
        await _context.People.Include(p => p.Department).ToListAsync();

    public async Task<IEnumerable<Person>> GetPagedAsync(int pageNumber, int pageSize) => 
        await _context.People
            .Include(p => p.Department)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

    public async Task<Person?> GetByIdAsync(int id) => 
        await _context.People
            .Include(p => p.Department)
            .FirstOrDefaultAsync(p => p.Id == id);

    public async Task AddAsync(Person person)
    {
        _context.People.Add(person);
        await _context.SaveChangesAsync();

        person.Department = await _departmentRepository.GetByIdAsync(person.DepartmentId);
    }

    public async Task UpdateAsync(Person person)
    {
        _context.People.Update(person);
        await _context.SaveChangesAsync();

        person.Department = await _departmentRepository.GetByIdAsync(person.DepartmentId);
    }

    public async Task DeleteAsync(int id)
    {
        var person = await _context.People.FindAsync(id);
        if (person is not null)
        {
            _context.People.Remove(person);
            await _context.SaveChangesAsync();
        }
    }
}
