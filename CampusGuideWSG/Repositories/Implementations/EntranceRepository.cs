using CampusGuideWSG.Context;
using CampusGuideWSG.Models;
using Microsoft.EntityFrameworkCore;

namespace CampusGuideWSG.Repositories.Implementations;

public class EntranceRepository : IEntranceRepository
{
    private readonly AppDbContext _dbContext;

    public EntranceRepository(AppDbContext context)
    {
        _dbContext = context;
    }

    public void Add(Entrance entrance)
    {
        _dbContext.Entrances.Add(entrance);
    }

    public void Remove(Entrance entrance)
    {
        _dbContext.Entrances.Remove(entrance);
    }

    public void Update(Entrance entrance)
    {
        _dbContext.Entrances.Update(entrance);
    }

    public Entrance? GetById(int id)
    {
        return _dbContext.Entrances
            .Include(e => e.Building)
            .FirstOrDefault(e => e.Id == id);
    }

    public Entrance? GetByName(string name)
    {
        return _dbContext.Entrances
            .Include(e => e.Building)
            .FirstOrDefault(e => e.Name == name);
    }

    public IList<Entrance> GetAll()
    {
        return _dbContext.Entrances
            .Include(e => e.Building)
            .ToList();
    }
}
