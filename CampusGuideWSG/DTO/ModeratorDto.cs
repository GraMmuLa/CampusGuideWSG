namespace CampusGuideWSG.DTO;

public class ModeratorDto
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public int RoleId { get; set; }

    public static ModeratorDto? FromModel(Models.Moderator? model)
    {
        if (model is null) return null;
        return new ModeratorDto
        {
            Id = model.Id,
            Username = model.Username,
            Name = model.Name,
            Surname = model.Surname,
            RoleId = model.RoleId
        };
    }

    public static Models.Moderator? ToModel(ModeratorDto? dto)
    {
        if (dto is null) return null;
        return new Models.Moderator
        {
            Id = dto.Id,
            Username = dto.Username,
            Name = dto.Name,
            Surname = dto.Surname,
            RoleId = dto.RoleId
        };
    }
}
