using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Services;
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
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Person>>> GetPeople()
    {
        var people = await _personService.GetPersonsAsync();
        return Ok(people);
    }

    // GET api/person?PageNumber=1&PageSize=10
    [HttpGet()]
    public async Task<ActionResult<IEnumerable<Person>>> GetPeople([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var people = await _personService.GetPagedPersonsAsync(pageNumber, pageSize);
        return Ok(people);
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
}

public static class Extensions
{
    public static void AddToModelState(this ValidationResult result, ModelStateDictionary modelState)
    {
        foreach (var error in result.Errors)
        {
            modelState.AddModelError(error.PropertyName, error.ErrorMessage);
        }
    }
}