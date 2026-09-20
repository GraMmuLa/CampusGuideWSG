using CampusGuideWSG.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace CampusGuideWSG.DTO;

[Index(nameof(Name), IsUnique = true)]
public class EntranceDto
{
    public int Id { get; set; }

    [MinLength(3), MaxLength(4)]
    public string Name { get; set; } = null!;

    public bool IsOpen { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Missing or invalid id")]
    public int BuildingId { get; set; }

    public static EntranceDto FromModel(Models.Entrance model)
    {
        return new EntranceDto { Id = model.Id, Name = model.Name, IsOpen = model.IsOpen, BuildingId = model.BuildingId };
    }

    public static Models.Entrance ToModel(EntranceDto dto)
    {
        return new Models.Entrance { Id = dto.Id, Name = dto.Name, IsOpen = dto.IsOpen, BuildingId = dto.BuildingId };
    }
}
