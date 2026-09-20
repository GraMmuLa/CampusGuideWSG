using System.Collections.Generic;
using CampusGuideWSG.DTO;

namespace CampusGuideWSG.Services;

public interface IRoomService
{
    RoomDto Add(RoomDto dto);
    void Remove(int id);
    RoomDto Update(RoomDto dto);
    RoomDto GetById(int id);
    RoomDto GetByNumber(int number);
    IList<RoomDto> GetAll();
}
