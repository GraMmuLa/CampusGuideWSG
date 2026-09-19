using CampusGuideWSG.Context;
using CampusGuideWSG.Models;
using Microsoft.EntityFrameworkCore;

namespace CampusGuideWSG.Repositories.Implementations;

public class ModeratorBuildingRepository : IModeratorBuildingRepository
{
    private readonly AppDbContext _dbContext;

    public ModeratorBuildingRepository(AppDbContext context)
    {
        _dbContext = context;
    }

    public void Add(ModeratorBuilding moderatorsBuilding)
    {
        _dbContext.ModeratorsBuildings.Add(moderatorsBuilding);
    }

    public void Remove(ModeratorBuilding moderatorsBuilding)
    {
        _dbContext.ModeratorsBuildings.Remove(moderatorsBuilding);
    }

    public void Update(ModeratorBuilding moderatorsBuilding)
    {
        _dbContext.ModeratorsBuildings.Update(moderatorsBuilding);
    }

    public ModeratorBuilding? GetById(int id)
    {
        return _dbContext.ModeratorsBuildings
            .Include(mb => mb.Building)
            .Include(mb => mb.Moderator)
            .FirstOrDefault(mb => mb.Id == id);
    }

    public IList<ModeratorBuilding> GetAll()
    {
        return _dbContext.ModeratorsBuildings
            .Include(mb => mb.Building)
            .Include(mb => mb.Moderator)
            .ToList();
    }
}
