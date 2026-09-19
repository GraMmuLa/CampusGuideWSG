using CampusGuideWSG.Models;

namespace CampusGuideWSG.Repositories
{
    public interface IModeratorBuildingRepository
    {
        public void Add(ModeratorBuilding moderatorsBuilding);
        public void Remove(ModeratorBuilding moderatorsBuilding);
        public void Update(ModeratorBuilding moderatorsBuilding);
        public ModeratorBuilding? GetById(int id);
        public IList<ModeratorBuilding> GetAll();
    }
}
