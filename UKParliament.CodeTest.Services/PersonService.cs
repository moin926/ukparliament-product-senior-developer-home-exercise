using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Data.Repositories;

namespace UKParliament.CodeTest.Services;

public class PersonService : IPersonService
{
    private readonly IPersonRepository _personRepository;

    public PersonService(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task<IEnumerable<Person>> GetPersonsAsync()
    {
        return await _personRepository.GetAsync();
    }

    public async Task<IEnumerable<Person>> GetPagedPersonsAsync(int pageNumber, int pageSize)
    {
        return await  _personRepository.GetPagedAsync(pageNumber, pageSize);
    }

    public async Task<Person?> GetPersonAsync(int id)
    {
        return await _personRepository.GetByIdAsync(id);
    }

    public async Task AddPersonAsync(Person person)
    {
        person.Id = 0;

        await _personRepository.AddAsync(person);
    }

    public async Task UpdatePersonAsync(Person person)
    {
        await _personRepository.UpdateAsync(person);
    }

    public async Task DeletePersonAsync(int id)
    {
        await _personRepository.DeleteAsync(id);
    }
}