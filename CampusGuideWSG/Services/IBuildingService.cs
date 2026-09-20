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
    void AddModerator(int buildingId, int moderatorId);
    void RemoveModerator(int buildingId, int moderatorId);
}
