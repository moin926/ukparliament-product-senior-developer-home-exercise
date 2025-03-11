using UKParliament.CodeTest.Data;

namespace UKParliament.CodeTest.Services;

public interface IPersonService
{
    Task<IEnumerable<Person>> GetPersonsAsync();
    Task<IEnumerable<Person>> GetPagedPersonsAsync(int pageNumber, int pageSize);
    Task<Person?> GetPersonAsync(int id);
    Task AddPersonAsync(Person person);
    Task UpdatePersonAsync(Person person);
    Task DeletePersonAsync(int id);
}