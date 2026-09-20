using CampusGuideWSG.DTO;
using CampusGuideWSG.Services;
using Microsoft.AspNetCore.Mvc;

namespace CampusGuideWSG.Controllers;

/// <summary>
/// API controller to manage rooms.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    private readonly IRoomService _service;

    public RoomsController(IRoomService service)
    {
        _service = service;
    }

    /// <summary>
    /// Get all rooms.
    /// </summary>
    /// <returns>List of RoomDto objects.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public ActionResult<IEnumerable<RoomDto>> GetAll()
    {
        IList<RoomDto> rooms = _service.GetAll();
        return rooms.Count == 0 ? NoContent() : Ok(rooms);
    }

    /// <summary>
    /// Get a room by its ID.
    /// </summary>
    /// <param name="id">Room Id</param>
    /// <returns>Room if found</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<RoomDto> GetById(int id)
    {
        try
        {
            RoomDto dto = _service.GetById(id);
            return Ok(dto);
        }
        catch (Exceptions.NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Get a room by its number.
    /// </summary>
    /// <param name="roomNumber">Room Number</param>
    /// <returns>Room if found</returns>
    [HttpGet("number/{roomNumber:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<RoomDto> GetByNumber(int roomNumber)
    {
        try
        {
            RoomDto dto = _service.GetByNumber(roomNumber);
            return Ok(dto);
        }
        catch (Exceptions.NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Create a new room.
    /// </summary>
    /// <param name="dto">Room DTO to create object</param>
    /// <returns>Created room</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult<RoomDto> Create(RoomDto dto)
    {
        try
        {
            RoomDto created = _service.Add(dto);
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
    /// Update an existing room.
    /// </summary>
    /// <param name="id">Room Id</param>
    /// <param name="dto">Room DTO to update object by id</param>
    /// <returns>Updated room</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult<RoomDto> Update(int id, RoomDto dto)
    {
        if (id != dto.Id) return BadRequest();
        try
        {
            RoomDto updated = _service.Update(dto);
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
    /// Delete a room by its ID.
    /// </summary>
    /// <param name="id">Room Id to delete</param>
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
