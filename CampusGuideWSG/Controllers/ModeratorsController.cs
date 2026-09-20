using CampusGuideWSG.DTO;
using CampusGuideWSG.Exceptions;
using CampusGuideWSG.Services;
using Microsoft.AspNetCore.Authentication;
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

    private void SetJwtCookie(string token, DateTime expiresAt)
    {
        CookieOptions cookieOptions = new()
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = expiresAt
        };

        Response.Cookies.Append("jwt-token", token, cookieOptions);
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
    /// Registers a new moderator.
    /// </summary>
    /// <param name="dto">Moderator DTO to register object</param>
    /// <returns>JWT Token and expiration time</returns>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult<(string token, DateTime expiresAt)> Register(RegisterDto dto)
    {
        try
        {
            AuthenticationResponse response = _service.Register(dto);

            SetJwtCookie(response.JwtToken, response.ExpiresAt);

            return StatusCode(StatusCodes.Status201Created, response.Value);
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
    /// Logins a moderator.
    /// </summary>
    /// <param name="dto">Login DTO to login object</param>
    /// <returns>JWT Token and expiration time</returns>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<(string token, DateTime expiresAt)> Login(LoginDto dto)
    {
        try
        {
            AuthenticationResponse response = _service.Login(dto);

            SetJwtCookie(response.JwtToken, response.ExpiresAt);

            return Ok(response.Value);
        }
        catch(AuthenticationFailureException ex)
        {
            return Unauthorized(new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
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
    /// Add a building to Moderator
    /// </summary>
    /// <param name="moderatorId"></param>
    /// <param name="buildingId"></param>
    /// <returns>Moderator Dto</returns>
    [HttpPost("/add-building")]
    public ActionResult<ModeratorDto> AddBuilding([FromQuery] int moderatorId, [FromQuery] int buildingId)
    {
        try
        {
            ModeratorDto moderatorDto = _service.AddBuilding(moderatorId, buildingId);
            return Ok(moderatorDto);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = ex.Message
            });
        }
    }

    /// <summary>
    /// Remove a building from Moderator
    /// </summary>
    /// <param name="moderatorId"></param>
    /// <param name="buildingId"></param>
    /// <returns>Moderator Dto</returns>
    [HttpPost("/remove-building")]
    public ActionResult<BuildingDto> RemoveBuilding([FromQuery] int moderatorId, [FromQuery] int buildingId)
    {
        try
        {
            ModeratorDto moderatorDto = _service.RemoveBuilding(moderatorId, buildingId);
            return Ok(moderatorDto);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
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
