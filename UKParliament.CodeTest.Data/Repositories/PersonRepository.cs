using Microsoft.EntityFrameworkCore;

namespace UKParliament.CodeTest.Data.Repositories;

public class PersonRepository : IPersonRepository
{
    private readonly PersonManagerContext _context;

    public PersonRepository(PersonManagerContext context)
    {
        _context = context;
    }

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
    }

    public async Task UpdateAsync(Person person)
    {
        _context.People.Update(person);
        await _context.SaveChangesAsync();
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
