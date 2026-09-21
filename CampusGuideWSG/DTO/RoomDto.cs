using System.ComponentModel.DataAnnotations;

namespace CampusGuideWSG.DTO;

public class RoomDto
{
    public int Id { get; set; }

    [Range(1, 999)]
    public int Number { get; set; }

    public List<int> BuildingIds { get; set; } = [];

    public static RoomDto FromModel(Models.Room model)
    {
        return new RoomDto { Id = model.Id, Number = model.Number,
            BuildingIds = [..model.Buildings.Select(x=>x.Id)] };
    }

    public static Models.Room ToModel(RoomDto dto)
    {
        return new Models.Room { Id = dto.Id, Number = dto.Number };
    }
}
