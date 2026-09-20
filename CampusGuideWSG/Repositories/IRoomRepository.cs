using CampusGuideWSG.Models;

namespace CampusGuideWSG.Repositories
{
    public interface IRoomRepository
    {
        public void Add(Room room);
        public void Remove(Room room);
        public void Update(Room room);
        public Room? GetById(int id);
        public Room? GetByNumber(int number);
        public IList<Room> GetAll();
    }
}
