using CampusGuideWSG.Models;

namespace CampusGuideWSG.Repositories
{
    public interface IRoleRepository
    {
        public void Add(Role role);
        public void Remove(Role role);
        public void Update(Role role);
        public Role? GetById(int id);
        public IList<Role> GetAll();
    }
}
