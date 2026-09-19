using System.Collections.Generic;
using System.Linq;
using CampusGuideWSG.Models;

namespace CampusGuideWSG.DTO;

public class BuildingDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public IList<EntranceDto> Entrances { get; set; } = new List<EntranceDto>();

    public IList<ModeratorBuildingDto> ModeratorsBuildings { get; set; } = new List<ModeratorBuildingDto>();

    public IList<RoomDto> Rooms { get; set; } = new List<RoomDto>();

    public static BuildingDto? FromModel(Building? model)
    {
        if (model is null) return null;
        return new BuildingDto
        {
            Id = model.Id,
            Name = model.Name,
        };
    }

    public static Building? ToModel(BuildingDto? dto)
    {
        if (dto is null) return null;
        return new Building
        {
            Id = dto.Id,
            Name = dto.Name,
        };
    }
}
