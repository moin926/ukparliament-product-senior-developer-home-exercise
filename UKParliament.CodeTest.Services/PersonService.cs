using UKParliament.CodeTest.Data;

namespace UKParliament.CodeTest.Services;

public class PersonService : IPersonService
{
    public Task AddPersonAsync(Person person)
    {
        throw new NotImplementedException();
    }

    public Task DeletePersonAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Person>> GetPagedPersonsAsync(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public Task<Person?> GetPersonAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Person>> GetPersonsAsync()
    {
        throw new NotImplementedException();
    }

    public Task UpdatePersonAsync(Person person)
    {
        throw new NotImplementedException();
    }
}