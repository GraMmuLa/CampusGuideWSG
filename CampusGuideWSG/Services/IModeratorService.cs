using System.Collections.Generic;
using CampusGuideWSG.DTO;

namespace CampusGuideWSG.Services;

public interface IModeratorService
{
    AuthenticationResponse Register(RegisterDto dto);
    AuthenticationResponse Login(LoginDto dto);
    void Remove(int id);
    ModeratorDto Update(ModeratorDto dto);
    ModeratorDto GetById(int id);
    ModeratorDto GetByUsername(string username);
    IList<ModeratorDto> GetAll();
    ModeratorDto AddBuilding(int moderatorId, int buildingId);
    ModeratorDto RemoveBuilding(int moderatorId, int buildingId);
}
