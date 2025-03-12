using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Services;
using UKParliament.CodeTest.Web.Validators;
using UKParliament.CodeTest.Web.ViewModels;

namespace UKParliament.CodeTest.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PersonController : ControllerBase
{
    private readonly IValidator<PersonViewModel> _validator;
    private readonly IPersonService _personService;

    public PersonController(IValidator<PersonViewModel> validator, IPersonService personService)
    {
        _validator = validator;
        _personService = personService;
    }

    // GET api/person
    [HttpGet("count")]
    public async Task<ActionResult<int>> CountPeople()
    {
        var count = await _personService.CountPersonsAsync();

        return Ok(count);
    }

    // GET api/person
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Person>>> GetPeople()
    {
        var people = await _personService.GetPersonsAsync();

        return Ok(people);
    }

    // GET api/person?PageNumber=1&PageSize=10
    [HttpGet("paged")]
    public async Task<ActionResult<PagedResult<Person>>> GetPeoplePaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var pagedPeople = await _personService.GetPagedPersonsAsync(pageNumber, pageSize);

        return Ok(pagedPeople);
    }

    // GET api/person/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Person>> GetPerson(int id)
    {
        var person = await _personService.GetPersonAsync(id);
        if (person == null)
        {
            return NotFound();
        }

        return Ok(person);
    }

    // POST api/person
    [HttpPost]
    public async Task<ActionResult> CreatePerson([FromBody] PersonViewModel request)
    {
        ValidationResult result = await _validator.ValidateAsync(request);
        if (!result.IsValid)
        {
            result.AddToModelState(ModelState);
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var person = new Person
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            DateOfBirth = request.DateOfBirth,
            DepartmentId = request.DepartmentId
        };

        await _personService.AddPersonAsync(person);

        return CreatedAtAction(nameof(GetPerson), new { id = person.Id }, person);
    }

    // PUT api/person/5
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdatePerson(int id, [FromBody] PersonViewModel request)
    {
        ValidationResult result = await _validator.ValidateAsync(request);
        if (!result.IsValid)
        {
            result.AddToModelState(ModelState);
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingPerson = await _personService.GetPersonAsync(id);
        if (existingPerson == null)
        {
            return NotFound();
        }

        existingPerson.FirstName = request.FirstName;
        existingPerson.LastName = request.LastName;
        existingPerson.DateOfBirth = request.DateOfBirth;
        existingPerson.DepartmentId = request.DepartmentId;

        await _personService.UpdatePersonAsync(existingPerson);

        return NoContent();
    }

    // DELETE api/person/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePerson(int id)
    {
        var existingPerson = await _personService.GetPersonAsync(id);
        if (existingPerson == null)
        {
            return NotFound();
        }

        await _personService.DeletePersonAsync(id);
        
        return NoContent();
    }
}

