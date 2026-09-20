using CampusGuideWSG.DTO;
using CampusGuideWSG.Helpers;
using CampusGuideWSG.Models;
using CampusGuideWSG.Exceptions;
using CampusGuideWSG.Repositories;

namespace CampusGuideWSG.Services.Implementations;

public class RoomService : IRoomService
{
    private readonly IRoomRepository _roomRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RoomService(IRoomRepository repository, IUnitOfWork unitOfWork)
    {
        _roomRepository = repository;
        _unitOfWork = unitOfWork;
    }

    public RoomDto Add(RoomDto dto)
    {
        Room model = RoomDto.ToModel(dto);

        _unitOfWork.Execute(() => {
            if (_roomRepository.GetById(model.Id) is not null)
                throw new UniquePropertyException("Room with this id already exists");
            if (_roomRepository.GetByNumber(model.Number) is not null)
                throw new UniquePropertyException("Room with this number already exists");
            if (dto.BuildingId == 0)
                throw new MissingDataException("Missing Building Id");
            _roomRepository.Add(model);
        });

        return RoomDto.FromModel(_roomRepository.GetById(model.Id) ??
            throw new NotFoundException("Room not found"));
    }
    
    public void Remove(int id)
    {
        Room model = _roomRepository.GetById(id) ??
            throw new NotFoundException("Room not found");

        _unitOfWork.Execute(() => _roomRepository.Remove(model));
    }

    public RoomDto Update(RoomDto dto)
    {
        if (_roomRepository.GetByNumber(dto.Number) is not null &&
            _roomRepository.GetByNumber(dto.Number)!.Id != dto.Id)
            throw new UniquePropertyException("Role with this name already exists");

        Room existing = _roomRepository.GetById(dto.Id) ??
            throw new NotFoundException("Room not found");

        existing.Number = dto.Number;
        existing.BuildingId = dto.BuildingId;

        _unitOfWork.Execute(() => _roomRepository.Update(existing));

        return RoomDto.FromModel(_roomRepository.GetById(existing.Id) ??
            throw new NotFoundException("Room not found"));
    }

    public RoomDto GetById(int id)
    {
        return RoomDto.FromModel(_roomRepository.GetById(id) ??
            throw new NotFoundException("Room not found"));
    }

    public RoomDto GetByNumber(int number)
    {
        return RoomDto.FromModel(_roomRepository.GetByNumber(number) ??
            throw new NotFoundException("Room not found"));
    }

    public IList<RoomDto> GetAll()
    {
        return [.._roomRepository.GetAll().Select(RoomDto.FromModel)];
    }
}
