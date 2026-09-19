using CampusGuideWSG.Models;

namespace CampusGuideWSG.Repositories
{
    public interface IBuildingRepository
    {
        public void Add(Building building);
        public void Remove(Building building);
        public void Update(Building building);
        public Building? GetById(int id);
        public IList<Building> GetAll();
    }
}
