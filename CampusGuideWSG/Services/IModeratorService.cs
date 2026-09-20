using System.Collections.Generic;
using CampusGuideWSG.DTO;

namespace CampusGuideWSG.Services;

public interface IModeratorService
{
    (string token, DateTime expiresAt) Register(ModeratorDto dto);
    (string token, DateTime expiresAt) Login(LoginDto dto);
    void Remove(int id);
    ModeratorDto Update(ModeratorDto dto);
    ModeratorDto GetById(int id);
    ModeratorDto GetByUsername(string username);
    IList<ModeratorDto> GetAll();
    void AddBuilding(int moderatorId, int buildingId);
    void RemoveBuilding(int moderatorId, int buildingId);
}
