using System;
using System.Collections.Generic;

namespace CampusGuideWSG.Models;

public partial class Role
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Moderator> Moderators { get; set; } = new List<Moderator>();
}
