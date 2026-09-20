using CampusGuideWSG.DTO;
using CampusGuideWSG.Exceptions;
using CampusGuideWSG.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CampusGuideWSG.Controllers;

/// <summary>
/// API controller to manage campus buildings.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Moderator,Admin")]
public class BuildingsController : ControllerBase
{
    private readonly IBuildingService _service;

    public BuildingsController(IBuildingService service)
    {
        _service = service;
    }

    /// <summary>
    /// Get all buildings.
    /// </summary>
    /// <returns>List of all buildings</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [AllowAnonymous]
    public ActionResult<IEnumerable<BuildingDto>> GetAll()
    {
        IList<BuildingDto> buildings = _service.GetAll();
        return buildings.Count == 0 ? NoContent() : Ok(buildings);
    }

    /// <summary>
    /// Get building by id.
    /// </summary>
    /// <param name="id">Building Id</param>
    /// <returns>Building if found</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AllowAnonymous]
    public ActionResult<BuildingDto> GetById(int id)
    {
        try
        {
            BuildingDto dto = _service.GetById(id);
            return Ok(dto);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Get building by name.
    /// </summary>
    /// <param name="name">Building Name</param>
    /// <returns>Building if found</returns>
    [HttpGet("name/{name}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AllowAnonymous]
    public ActionResult<BuildingDto> GetByName(string name)
    {
        try
        {
            BuildingDto dto = _service.GetByName(name);
            return Ok(dto);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Create a new building.
    /// </summary>
    /// <param name="dto">Building DTO to create object</param>
    /// <returns>Created building</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult<BuildingDto> Create(BuildingDto dto)
    {
        try
        {
            BuildingDto created = _service.Add(dto);
            return CreatedAtAction(nameof(GetById),
                new { id = created.Id }, created);
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
    /// Update an existing building.
    /// </summary>
    /// <param name="id">Building Id</param>
    /// <param name="dto">Building DTO to update object by id</param>
    /// <returns>Updated building</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult<BuildingDto> Update(int id, BuildingDto dto)
    {
        if (id != dto.Id) return BadRequest();

        try
        {
            BuildingDto updated = _service.Update(dto);
            return Ok(updated);
        }
        catch (NotFoundException)
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
    /// Delete a building by id.
    /// </summary>
    /// <param name="id">Building Id to delete</param>
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
        catch (NotFoundException)
        {
            return NotFound();
        }
    }
}
