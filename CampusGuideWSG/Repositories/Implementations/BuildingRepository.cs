using CampusGuideWSG.Context;
using CampusGuideWSG.Models;
using Microsoft.EntityFrameworkCore;

namespace CampusGuideWSG.Repositories.Implementations;

public class BuildingRepository : IBuildingRepository
{
    private readonly AppDbContext _dbContext;

    public BuildingRepository(AppDbContext context)
    {
        _dbContext = context;
    }

    public void Add(Building building)
    {
        _dbContext.Buildings.Add(building);
    }

    public void Remove(Building building)
    {
        _dbContext.Buildings.Remove(building);
    }

    public void Update(Building building)
    {
        _dbContext.Buildings.Update(building);
    }

    public Building? GetById(int id)
    {
        return _dbContext.Buildings
            .Include(b => b.Entrances)
            .Include(b => b.ModeratorsBuildings)
            .Include(b => b.Rooms)
            .FirstOrDefault(b => b.Id == id);
    }

    public IList<Building> GetAll()
    {
        return _dbContext.Buildings
            .Include(b => b.Entrances)
            .Include(b => b.ModeratorsBuildings)
            .Include(b => b.Rooms)
            .ToList();
    }
}
