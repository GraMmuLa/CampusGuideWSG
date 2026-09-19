using CampusGuideWSG.Models;

namespace CampusGuideWSG.Repositories
{
    public interface IEntranceRepository
    {
        public void Add(Entrance entrance);
        public void Remove(Entrance entrance);
        public void Update(Entrance entrance);
        public Entrance? GetById(int id);
        public IList<Entrance> GetAll();
    }
}
