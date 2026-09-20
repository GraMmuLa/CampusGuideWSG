using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CampusGuideWSG.DTO;

[Index(nameof(Username), IsUnique = true)]
public class ModeratorDto
{
    public int Id { get; set; }

    [MinLength(4)]
    public string Username { get; set; } = null!;

    [MinLength(1)]
    public string Name { get; set; } = null!;

    [MinLength(1)]
    public string Surname { get; set; } = null!;

    public int RoleId { get; set; }

    public IList<int> BuildingIds { get; set; } = [];

    public static ModeratorDto FromModel(Models.Moderator model)
    {
        return new ModeratorDto
        {
            Id = model.Id,
            Username = model.Username,
            Name = model.Name,
            Surname = model.Surname,
            RoleId = model.RoleId,
            BuildingIds = model.ModeratorsBuildings?.Where(x => x.ModeratorId == model.Id).Select(x => x.BuildingId).ToList() ?? []
        };
    }

    public static Models.Moderator ToModel(ModeratorDto dto)
    {
        return new Models.Moderator
        {
            Id = dto.Id,
            Username = dto.Username,
            Name = dto.Name,
            Surname = dto.Surname,
            RoleId = dto.RoleId
        };
    }
}
