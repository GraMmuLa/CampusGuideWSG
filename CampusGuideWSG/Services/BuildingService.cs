using CampusGuideWSG.DTO;
using CampusGuideWSG.Helpers;
using CampusGuideWSG.Helpers.Implementations;
using CampusGuideWSG.Models;
using CampusGuideWSG.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace CampusGuideWSG.Services;

public class BuildingService
{
    private readonly IBuildingRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public BuildingService(IBuildingRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public BuildingDto Add(BuildingDto dto)
    {
        var model = BuildingDto.ToModel(dto)!;
        _unitOfWork.Execute(() => _repository.Add(model));
        var saved = _repository.GetById(model.Id);
        return BuildingDto.FromModel(saved)!;
    }

    public void Remove(int id)
    {
        var model = _repository.GetById(id);
        if (model is null) return;
        _unitOfWork.Execute(() => _repository.Remove(model));
    }

    public BuildingDto? Update(BuildingDto dto)
    {
        var existing = _repository.GetById(dto.Id);
        if (existing is null) return null;
        existing.Name = dto.Name;
        _unitOfWork.Execute(() => _repository.Update(existing));
        var updated = _repository.GetById(existing.Id);
        return BuildingDto.FromModel(updated);
    }

    public BuildingDto? GetById(int id)
    {
        var model = _repository.GetById(id);
        return BuildingDto.FromModel(model);
    }

    public IList<BuildingDto> GetAll()
    {
        return _repository.GetAll().Select(b => BuildingDto.FromModel(b)!).ToList();
    }

}
