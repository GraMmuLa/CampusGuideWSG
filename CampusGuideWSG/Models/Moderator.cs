using System;
using System.Collections.Generic;

namespace CampusGuideWSG.Models;

public partial class Moderator
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public int RoleId { get; set; }

    public virtual ICollection<ModeratorBuilding> ModeratorsBuildings { get; set; } = new List<ModeratorBuilding>();

    public virtual Role Role { get; set; } = null!;
}
