using CampusGuideWSG.Helpers;
using CampusGuideWSG.Context;
using CampusGuideWSG.DTO;
using CampusGuideWSG.Models;
using CampusGuideWSG.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace CampusGuideWSG.Services;

public class EntranceService
{
    private readonly IEntranceRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public EntranceService(IEntranceRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public EntranceDto Add(EntranceDto dto)
    {
        var model = EntranceDto.ToModel(dto)!;
        _unitOfWork.Execute(() => _repository.Add(model));
        var saved = _repository.GetById(model.Id);
        return EntranceDto.FromModel(saved)!;
    }

    public void Remove(int id)
    {
        var model = _repository.GetById(id);
        if (model is null) return;
        _unitOfWork.Execute(() => _repository.Remove(model));
    }

    public EntranceDto? Update(EntranceDto dto)
    {
        var existing = _repository.GetById(dto.Id);
        if (existing is null) return null;
        existing.Name = dto.Name;
        existing.IsOpen = dto.IsOpen;
        existing.BuildingId = dto.BuildingId;
        _unitOfWork.Execute(() => _repository.Update(existing));
        var updated = _repository.GetById(existing.Id);
        return EntranceDto.FromModel(updated);
    }

    public EntranceDto? GetById(int id)
    {
        var model = _repository.GetById(id);
        return EntranceDto.FromModel(model);
    }

    public IList<EntranceDto> GetAll()
    {
        return _repository.GetAll().Select(e => EntranceDto.FromModel(e)!).ToList();
    }

}
