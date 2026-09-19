namespace CampusGuideWSG.DTO;

public class RoleDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public static RoleDto? FromModel(Models.Role? model)
    {
        if (model is null) return null;
        return new RoleDto { Id = model.Id, Name = model.Name };
    }

    public static Models.Role? ToModel(RoleDto? dto)
    {
        if (dto is null) return null;
        return new Models.Role { Id = dto.Id, Name = dto.Name };
    }
}
