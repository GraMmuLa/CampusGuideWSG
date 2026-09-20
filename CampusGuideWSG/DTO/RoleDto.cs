using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CampusGuideWSG.DTO;

[Index(nameof(Name), IsUnique = true)]
public class RoleDto
{
    public int Id { get; set; }

    [MinLength(2)]
    public string Name { get; set; } = null!;

    public static RoleDto FromModel(Models.Role model)
    {
        return new RoleDto { Id = model.Id, Name = model.Name };
    }

    public static Models.Role ToModel(RoleDto dto)
    {
        return new Models.Role { Id = dto.Id, Name = dto.Name };
    }
}
