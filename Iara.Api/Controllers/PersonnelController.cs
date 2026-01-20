using Microsoft.AspNetCore.Mvc;
using Iara.Domain.Entities;
using Iara.Domain.Repositories;

namespace Iara.Api.Controllers;

/// <summary>
/// Controller for managing personnel
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PersonnelController : ControllerBase
{
    private readonly IPersonnelRepository _personnelRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<PersonnelController> _logger;

    public PersonnelController(
        IPersonnelRepository personnelRepository,
        IUnitOfWork unitOfWork,
        ILogger<PersonnelController> logger)
    {
        _personnelRepository = personnelRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <summary>
    /// Get all personnel
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var personnel = await _personnelRepository.GetAllAsync(cancellationToken);
        return Ok(personnel.Select(p => new PersonnelDto
        {
            PersonId = p.PersonId,
            Name = p.Name,
            Role = p.Role,
            ContactEmail = p.ContactEmail
        }));
    }

    /// <summary>
    /// Get personnel by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var person = await _personnelRepository.GetByIdAsync(id, cancellationToken);
        if (person == null)
            return NotFound(new { message = "Personnel not found" });

        return Ok(new PersonnelDto
        {
            PersonId = person.PersonId,
            Name = person.Name,
            Role = person.Role,
            ContactEmail = person.ContactEmail
        });
    }

    /// <summary>
    /// Get personnel by role
    /// </summary>
    [HttpGet("role/{role}")]
    public async Task<IActionResult> GetByRole(string role, CancellationToken cancellationToken)
    {
        var personnel = await _personnelRepository.GetByRoleAsync(role, cancellationToken);
        return Ok(personnel.Select(p => new PersonnelDto
        {
            PersonId = p.PersonId,
            Name = p.Name,
            Role = p.Role,
            ContactEmail = p.ContactEmail
        }));
    }

    /// <summary>
    /// Create a new personnel record
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePersonnelRequest request, CancellationToken cancellationToken)
    {
        if (!IsValidRole(request.Role))
            return BadRequest(new { message = "Invalid role. Must be one of: Captain, Inspector, Admin, Owner, User" });

        if (!string.IsNullOrEmpty(request.ContactEmail))
        {
            var existingEmail = await _personnelRepository.EmailExistsAsync(request.ContactEmail, cancellationToken);
            if (existingEmail)
                return BadRequest(new { message = "Email already exists" });
        }

        var personnel = new Personnel
        {
            Name = request.Name,
            Role = request.Role,
            ContactEmail = request.ContactEmail
        };

        await _personnelRepository.AddAsync(personnel, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = personnel.PersonId }, new PersonnelDto
        {
            PersonId = personnel.PersonId,
            Name = personnel.Name,
            Role = personnel.Role,
            ContactEmail = personnel.ContactEmail
        });
    }

    /// <summary>
    /// Update a personnel record
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePersonnelRequest request, CancellationToken cancellationToken)
    {
        var person = await _personnelRepository.GetByIdAsync(id, cancellationToken);
        if (person == null)
            return NotFound(new { message = "Personnel not found" });

        if (!IsValidRole(request.Role))
            return BadRequest(new { message = "Invalid role. Must be one of: Captain, Inspector, Admin, Owner, User" });

        person.Name = request.Name;
        person.Role = request.Role;
        person.ContactEmail = request.ContactEmail;

        _personnelRepository.Update(person);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Ok(new PersonnelDto
        {
            PersonId = person.PersonId,
            Name = person.Name,
            Role = person.Role,
            ContactEmail = person.ContactEmail
        });
    }

    /// <summary>
    /// Delete a personnel record
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var person = await _personnelRepository.GetByIdAsync(id, cancellationToken);
        if (person == null)
            return NotFound(new { message = "Personnel not found" });

        _personnelRepository.Remove(person);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return NoContent();
    }

    private static bool IsValidRole(string role)
    {
        return role == PersonnelRoles.Captain ||
               role == PersonnelRoles.Inspector ||
               role == PersonnelRoles.Admin ||
               role == PersonnelRoles.Owner ||
               role == PersonnelRoles.User;
    }
}

public record PersonnelDto
{
    public int PersonId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
    public string? ContactEmail { get; init; }
}

public record CreatePersonnelRequest
{
    public string Name { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
    public string? ContactEmail { get; init; }
}

public record UpdatePersonnelRequest
{
    public string Name { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
    public string? ContactEmail { get; init; }
}
