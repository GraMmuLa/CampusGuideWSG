namespace CampusGuideWSG.DTO;

public class RoomDto
{
    public int Id { get; set; }

    public int Number { get; set; }

    public int BuildingId { get; set; }

    public static RoomDto? FromModel(Models.Room? model)
    {
        if (model is null) return null;
        return new RoomDto { Id = model.Id, Number = model.Number, BuildingId = model.BuildingId };
    }

    public static Models.Room? ToModel(RoomDto? dto)
    {
        if (dto is null) return null;
        return new Models.Room { Id = dto.Id, Number = dto.Number, BuildingId = dto.BuildingId };
    }
}
