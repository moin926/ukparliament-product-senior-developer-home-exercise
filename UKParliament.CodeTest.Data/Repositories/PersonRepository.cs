namespace UKParliament.CodeTest.Data.Repositories;

public class PersonRepository : IPersonRepository
{
    public Task<Person> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
    public Task<IEnumerable<Person>> GetAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Person>> GetPagedAsync(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(Person person)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Person person)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}
