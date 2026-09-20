using CampusGuideWSG.Context;
using CampusGuideWSG.Models;
using Microsoft.EntityFrameworkCore;

namespace CampusGuideWSG.Repositories.Implementations;

public class ModeratorRepository : IModeratorRepository
{
    private readonly AppDbContext _dbContext;

    public ModeratorRepository(AppDbContext context)
    {
        _dbContext = context;
    }

    public void Add(Moderator moderator)
    {
        _dbContext.Moderators.Add(moderator);
    }

    public void Remove(Moderator moderator)
    {
        _dbContext.Moderators.Remove(moderator);
    }

    public void Update(Moderator moderator)
    {
        _dbContext.Moderators.Update(moderator);
    }

    public Moderator? GetById(int id)
    {
        return _dbContext.Moderators
            .Include(m => m.ModeratorsBuildings)
            .Include(m => m.Role)
            .FirstOrDefault(m => m.Id == id);
    }

    public Moderator? GetByUsername(string username)
    {
        return _dbContext.Moderators
            .Include(m => m.ModeratorsBuildings)
            .Include(m => m.Role)
            .FirstOrDefault(m => m.Username == username);
    }

    public IList<Moderator> GetAll()
    {
        return _dbContext.Moderators
            .Include(m => m.ModeratorsBuildings)
            .Include(m => m.Role)
            .ToList();
    }
}
