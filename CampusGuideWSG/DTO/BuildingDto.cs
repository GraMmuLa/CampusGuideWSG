using CampusGuideWSG.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CampusGuideWSG.DTO;

[Index(nameof(Name), IsUnique = true)]
public class BuildingDto
{
    public int Id { get; set; }

    [MinLength(1), MaxLength(2)]
    public string Name { get; set; } = null!;

    public IList<int> ModeratorIds { get; set; } = [];

    public IList<int> EntranceIds { get; set; } = [];

    public IList<int> RoomIds { get; set; } = [];

    public static BuildingDto FromModel(Building model)
    {
        return new BuildingDto
        {
            Id = model.Id,
            Name = model.Name,
            ModeratorIds = model.Moderators.Select(x => x.Id).ToList() ?? [],
            EntranceIds = model.Entrances?.Select(x => x.Id).ToList() ?? [],
            RoomIds = model.Rooms?.Select(x => x.Id).ToList() ?? []
        };
    }

    public static Building ToModel(BuildingDto dto)
    {
        return new Building
        {
            Id = dto.Id,
            Name = dto.Name,
        };
    }
}
