using System;
using System.Collections.Generic;

namespace CampusGuideWSG.Models;

public partial class Building
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Entrance> Entrances { get; set; } = [];

    public virtual ICollection<ModeratorBuilding> ModeratorsBuildings { get; set; } = new List<ModeratorBuilding>();

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
