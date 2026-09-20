using System.Collections.Generic;
using CampusGuideWSG.DTO;

namespace CampusGuideWSG.Services;

public interface IEntranceService
{
    EntranceDto Add(EntranceDto dto);
    void Remove(int id);
    EntranceDto Update(EntranceDto dto);
    EntranceDto GetById(int id);
    EntranceDto GetByName(string name);
    IList<EntranceDto> GetAll();
}
