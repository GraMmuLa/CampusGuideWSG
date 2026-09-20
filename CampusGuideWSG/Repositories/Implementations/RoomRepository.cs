using CampusGuideWSG.Context;
using CampusGuideWSG.Models;
using Microsoft.EntityFrameworkCore;

namespace CampusGuideWSG.Repositories.Implementations;

public class RoomRepository : IRoomRepository
{
    private readonly AppDbContext _dbContext;

    public RoomRepository(AppDbContext context)
    {
        _dbContext = context;
    }

    public void Add(Room room)
    {
        _dbContext.Rooms.Add(room);
    }

    public void Remove(Room room)
    {
        _dbContext.Rooms.Remove(room);
    }

    public void Update(Room room)
    {
        _dbContext.Rooms.Update(room);
    }

    public Room? GetById(int id)
    {
        return _dbContext.Rooms
            .Include(r => r.Building)
            .FirstOrDefault(r => r.Id == id);
    }

    public Room? GetByNumber(int number)
    {
        return _dbContext.Rooms
            .Include(r => r.Building)
            .FirstOrDefault(r => r.Number == number);
    }

    public IList<Room> GetAll()
    {
        return _dbContext.Rooms
            .Include(r => r.Building)
            .ToList();
    }
}
