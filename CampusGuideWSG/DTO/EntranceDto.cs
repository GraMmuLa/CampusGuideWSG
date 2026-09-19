using CampusGuideWSG.Models;
namespace CampusGuideWSG.DTO;

public class EntranceDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool IsOpen { get; set; }

    public int BuildingId { get; set; }

    public static EntranceDto? FromModel(Models.Entrance? model)
    {
        if (model is null) return null;
        return new EntranceDto { Id = model.Id, Name = model.Name, IsOpen = model.IsOpen, BuildingId = model.BuildingId };
    }

    public static Models.Entrance? ToModel(EntranceDto? dto)
    {
        if (dto is null) return null;
        return new Models.Entrance { Id = dto.Id, Name = dto.Name, IsOpen = dto.IsOpen, BuildingId = dto.BuildingId };
    }
}
