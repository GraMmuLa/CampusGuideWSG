using CampusGuideWSG.Context;
using CampusGuideWSG.Models;
using Microsoft.EntityFrameworkCore;

namespace CampusGuideWSG.Repositories.Implementations;

public class RoleRepository : IRoleRepository
{
    private readonly AppDbContext _dbContext;

    public RoleRepository(AppDbContext context)
    {
        _dbContext = context;
    }

    public void Add(Role role)
    {
        _dbContext.Roles.Add(role);
    }

    public void Remove(Role role)
    {
        _dbContext.Roles.Remove(role);
    }

    public void Update(Role role)
    {
        _dbContext.Roles.Update(role);
    }

    public Role? GetById(int id)
    {
        return _dbContext.Roles
            .Include(r => r.Moderators)
            .FirstOrDefault(r => r.Id == id);
    }

    public IList<Role> GetAll()
    {
        return _dbContext.Roles
            .Include(r => r.Moderators)
            .ToList();
    }
}
