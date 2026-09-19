using System;
using System.Collections.Generic;

namespace CampusGuideWSG.Models;

public partial class ModeratorBuilding
{
    public int Id { get; set; }

    public int ModeratorId { get; set; }

    public int BuildingId { get; set; }

    public virtual Building Building { get; set; } = null!;

    public virtual Moderator Moderator { get; set; } = null!;
}
