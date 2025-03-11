namespace UKParliament.CodeTest.Data.Repositories;

public interface IPersonRepository
{
    Task<int> CountAsync();
    Task<IEnumerable<Person>> GetAsync();
    Task<IEnumerable<Person>> GetPagedAsync(int pageNumber, int pageSize);
    Task<Person?> GetByIdAsync(int id);
    Task AddAsync(Person person);
    Task UpdateAsync(Person person);
    Task DeleteAsync(int id);
}
