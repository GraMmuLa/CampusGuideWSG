using CampusGuideWSG.Helpers;
using CampusGuideWSG.Context;
using CampusGuideWSG.DTO;
using CampusGuideWSG.Models;
using CampusGuideWSG.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace CampusGuideWSG.Services;

public class RoomService
{
    private readonly IRoomRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public RoomService(IRoomRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public RoomDto Add(RoomDto dto)
    {
        var model = RoomDto.ToModel(dto)!;
        _unitOfWork.Execute(() => _repository.Add(model));
        var saved = _repository.GetById(model.Id);
        return RoomDto.FromModel(saved)!;
    }

    public void Remove(int id)
    {
        var model = _repository.GetById(id);
        if (model is null) return;
        _unitOfWork.Execute(() => _repository.Remove(model));
    }

    public RoomDto? Update(RoomDto dto)
    {
        var existing = _repository.GetById(dto.Id);
        if (existing is null) return null;
        existing.Number = dto.Number;
        existing.BuildingId = dto.BuildingId;
        _unitOfWork.Execute(() => _repository.Update(existing));
        var updated = _repository.GetById(existing.Id);
        return RoomDto.FromModel(updated);
    }

    public RoomDto? GetById(int id)
    {
        var model = _repository.GetById(id);
        return RoomDto.FromModel(model);
    }

    public IList<RoomDto> GetAll()
    {
        return _repository.GetAll().Select(r => RoomDto.FromModel(r)!).ToList();
    }

}
