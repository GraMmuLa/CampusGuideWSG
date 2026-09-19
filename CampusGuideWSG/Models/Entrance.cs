using System;
using System.Collections.Generic;

namespace CampusGuideWSG.Models;

public partial class Entrance
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool IsOpen { get; set; }

    public int BuildingId { get; set; }

    public virtual Building Building { get; set; } = null!;
}
