using System.ComponentModel.DataAnnotations;

namespace CampusGuideWSG.DTO;

public class RoomDto
{
    public int Id { get; set; }

    [Range(1, 99)]
    public int Number { get; set; }

    [Required]
    public int BuildingId { get; set; }

    public static RoomDto FromModel(Models.Room model)
    {
        return new RoomDto { Id = model.Id, Number = model.Number, BuildingId = model.BuildingId };
    }

    public static Models.Room ToModel(RoomDto dto)
    {
        return new Models.Room { Id = dto.Id, Number = dto.Number, BuildingId = dto.BuildingId };
    }
}
