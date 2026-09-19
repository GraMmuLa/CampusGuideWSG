using CampusGuideWSG.Models;

namespace CampusGuideWSG.Repositories
{
    public interface IModeratorRepository
    {
        public void Add(Moderator moderator);
        public void Remove(Moderator moderator);
        public void Update(Moderator moderator);
        public Moderator? GetById(int id);
        public IList<Moderator> GetAll();
    }
}
