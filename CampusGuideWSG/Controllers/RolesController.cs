using CampusGuideWSG.DTO;
using CampusGuideWSG.Services;
using Microsoft.AspNetCore.Mvc;

namespace CampusGuideWSG.Controllers;

/// <summary>
/// API controller to manage roles.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly IRoleService _service;

    public RolesController(IRoleService service)
    {
        _service = service;
    }

    /// <summary>
    /// Get all roles.
    /// </summary>
    /// <returns>List of roles</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public ActionResult<IEnumerable<RoleDto>> GetAll()
    {
        IList<RoleDto> roles = _service.GetAll();
        return roles.Count == 0 ? NoContent() : Ok(roles);
    }

    /// <summary>
    /// Get a role by its ID.
    /// </summary>
    /// <param name="id">Role Id</param>
    /// <returns>Role if found</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<RoleDto> GetById(int id)
    {
        try
        {
            RoleDto dto = _service.GetById(id);
            return Ok(dto);
        }
        catch (Exceptions.NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Get a role by its name.
    /// </summary>
    /// <param name="name">Role name</param>
    /// <returns>Role if found</returns>
    [HttpGet("name/{name}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<RoleDto> GetByName(string name)
    {
        try
        {
            RoleDto dto = _service.GetByName(name);
            return Ok(dto);
        }
        catch (Exceptions.NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Create a new role.
    /// </summary>
    /// <param name="dto">Role DTO to create object</param>
    /// <returns>Created role</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult<RoleDto> Create(RoleDto dto)
    {
        try
        {
            RoleDto created = _service.Add(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (Exceptions.UniquePropertyException ex)
        {
            return Conflict(new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = ex.Message
            });
        }
    }

    /// <summary>
    /// Update an existing role.
    /// </summary>
    /// <param name="id">Role Id</param>
    /// <param name="dto">Role DTO to update object by id</param>
    /// <returns>Updated role</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult<RoleDto> Update(int id, RoleDto dto)
    {
        if (id != dto.Id) return BadRequest();
        try
        {
            RoleDto updated = _service.Update(dto);
            return Ok(updated);
        }
        catch (Exceptions.NotFoundException)
        {
            return NotFound();
        }
        catch (Exceptions.UniquePropertyException ex)
        {
            return Conflict(new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = ex.Message
            });
        }
    }

    /// <summary>
    /// Delete a role by its ID.
    /// </summary>
    /// <param name="id">Role Id to delete</param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        try
        {
            _service.Remove(id);
            return NoContent();
        }
        catch
        {
            return NotFound();
        }
    }
}
