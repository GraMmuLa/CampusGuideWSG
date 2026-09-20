using System.Collections.Generic;
using CampusGuideWSG.DTO;
using CampusGuideWSG.Exceptions;

namespace CampusGuideWSG.Services;

public interface IBuildingService
{
    BuildingDto Add(BuildingDto dto);
    void Remove(int id);
    BuildingDto Update(BuildingDto dto);
    BuildingDto GetById(int id);
    BuildingDto GetByName(string name);
    IList<BuildingDto> GetAll();
    BuildingDto AddModerator(int buildingId, int moderatorId);
    BuildingDto RemoveModerator(int buildingId, int moderatorId);
}
