using System;
using System.Collections.Generic;

namespace CampusGuideWSG.Models;

public partial class Building
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Entrance> Entrances { get; set; } = [];

    public virtual ICollection<Moderator> Moderators { get; set; } = [];

    public virtual ICollection<Room> Rooms { get; set; } = [];
}
