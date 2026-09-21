using System;
using System.Collections.Generic;

namespace CampusGuideWSG.Models;

public partial class Room
{
    public int Id { get; set; }

    public int Number { get; set; }

    public virtual List<Building> Buildings { get; set; } = [];
}
