using CampusGuideWSG.DTO;
using CampusGuideWSG.Exceptions;
using CampusGuideWSG.Services;
using Microsoft.AspNetCore.Mvc;

namespace CampusGuideWSG.Controllers;

/// <summary>
/// API controller to manage building entrances.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class EntrancesController : ControllerBase
{
    private readonly IEntranceService _service;

    public EntrancesController(IEntranceService service)
    {
        _service = service;
    }

    /// <summary>
    /// Get all entrances.
    /// </summary>
    /// <returns>List of entrances</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public ActionResult<IEnumerable<EntranceDto>> GetAll()
    {
        IList<EntranceDto> entrances = _service.GetAll();
        return entrances.Count == 0 ? NoContent() : Ok(entrances);
    }

    /// <summary>
    /// Get an entrance by its ID.
    /// </summary>
    /// <param name="id">Entrance Id</param>
    /// <returns>Entrance if found</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<EntranceDto> GetById(int id)
    {
        try
        {
            EntranceDto dto = _service.GetById(id);
            return Ok(dto);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Get an entrance by its name.
    /// </summary>
    /// <param name="name">Entrance Name</param>
    /// <returns>Entrance if found</returns>
    [HttpGet("name/{name}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<EntranceDto> GetByName(string name)
    {
        try
        {
            EntranceDto dto = _service.GetByName(name);
            return Ok(dto);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Create a new entrance.
    /// </summary>
    /// <param name="dto">Entrance DTO to create object</param>
    /// <returns>Created entrance</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult<EntranceDto> Create(EntranceDto dto)
    {
        try
        {
            EntranceDto created = _service.Add(dto);
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
    /// Update an existing entrance.
    /// </summary>
    /// <param name="id">Entrance Id</param>
    /// <param name="dto">Entrance DTO to update object by id</param>
    /// <returns>Updated entrance</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult<EntranceDto> Update(int id, EntranceDto dto)
    {
        if (id != dto.Id) return BadRequest();
        try
        {
            EntranceDto updated = _service.Update(dto);
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
    /// Delete an entrance by its ID.
    /// </summary>
    /// <param name="id">Entrance Id to delete</param>
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
        catch (Exceptions.NotFoundException)
        {
            return NotFound();
        }
    }
}
