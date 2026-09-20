using CampusGuideWSG.DTO;
using CampusGuideWSG.Exceptions;
using CampusGuideWSG.Services;
using Microsoft.AspNetCore.Mvc;

namespace CampusGuideWSG.Controllers;

/// <summary>
/// API controller to manage moderators.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ModeratorsController : ControllerBase
{
    private readonly IModeratorService _service;

    public ModeratorsController(IModeratorService service)
    {
        _service = service;
    }

    /// <summary>
    /// Get all moderators.
    /// </summary>
    /// <returns>List of moderators</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public ActionResult<IEnumerable<ModeratorDto>> GetAll()
    {
        IList<ModeratorDto> moderators = _service.GetAll();
        return moderators.Count == 0 ? NoContent() : Ok(moderators);
    }

    /// <summary>
    /// Get a moderator by ID.
    /// </summary>
    /// <param name="id">Moderator Id</param>
    /// <returns>Moderator if found</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<ModeratorDto> GetById(int id)
    {
        try
        {
            ModeratorDto dto = _service.GetById(id);
            return Ok(dto);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Get a moderator by username.
    /// </summary>
    /// <param name="username">Moderator Username</param>
    /// <returns>Moderator if found</returns>
    [HttpGet("username/{username}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<ModeratorDto> GetByUsername(string username)
    {
        try
        {
            ModeratorDto dto = _service.GetByUsername(username);
            return Ok(dto);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Create a new moderator.
    /// </summary>
    /// <param name="dto">Moderator DTO to create object</param>
    /// <returns>Created moderator</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult<ModeratorDto> Create(ModeratorDto dto)
    {
        try
        {
            ModeratorDto created = _service.Add(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch(UniquePropertyException ex)
        {
            return Conflict(new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = ex.Message
            });
        }
    }

    /// <summary>
    /// Update an existing moderator.
    /// </summary>
    /// <param name="id">Moderator Id</param>
    /// <param name="dto">Moderator DTO to update object by id</param>
    /// <returns>Updated moderator</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult<ModeratorDto> Update(int id, ModeratorDto dto)
    {
        if (id != dto.Id) return BadRequest();
        try
        {
            ModeratorDto updated = _service.Update(dto);
            return Ok(updated);
        }
        catch (CampusGuideWSG.Exceptions.NotFoundException)
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
    /// Delete a moderator by ID.
    /// </summary>
    /// <param name="id">Moderator Id to delete</param>
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
        catch (CampusGuideWSG.Exceptions.NotFoundException)
        {
            return NotFound();
        }
    }
}
