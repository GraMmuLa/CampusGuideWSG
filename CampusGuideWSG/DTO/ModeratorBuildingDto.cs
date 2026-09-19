namespace CampusGuideWSG.DTO;

public class ModeratorBuildingDto
{
    public int Id { get; set; }

    public int ModeratorId { get; set; }

    public int BuildingId { get; set; }

    public static ModeratorBuildingDto? FromModel(Models.ModeratorBuilding? model)
    {
        if (model is null)
            return null;
        return new ModeratorBuildingDto
        {
            Id = model.Id,
            ModeratorId = model.ModeratorId,
            BuildingId = model.BuildingId
        };
    }

    public static Models.ModeratorBuilding? ToModel(ModeratorBuildingDto? dto)
    {
        if (dto is null)
            return null;
        return new Models.ModeratorBuilding
        {
            Id = dto.Id,
            ModeratorId = dto.ModeratorId,
            BuildingId = dto.BuildingId
        };
    }
}
