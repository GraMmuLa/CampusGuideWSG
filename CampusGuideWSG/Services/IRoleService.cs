using System.Collections.Generic;
using CampusGuideWSG.DTO;

namespace CampusGuideWSG.Services;

public interface IRoleService
{
    RoleDto Add(RoleDto dto);
    void Remove(int id);
    RoleDto Update(RoleDto dto);
    RoleDto GetById(int id);
    RoleDto GetByName(string name);
    IList<RoleDto> GetAll();
}
