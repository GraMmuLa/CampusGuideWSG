using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CampusGuideWSG.Models;

public partial class Moderator
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int RoleId { get; set; }

    public virtual ICollection<Building> Buildings { get; set; } = new List<Building>();

    public virtual Role Role { get; set; } = null!;
}
