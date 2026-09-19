using CampusGuideWSG.Helpers;
using CampusGuideWSG.Context;
using CampusGuideWSG.DTO;
using CampusGuideWSG.Models;
using CampusGuideWSG.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace CampusGuideWSG.Services;

public class ModeratorService
{
    private readonly IModeratorRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ModeratorService(IModeratorRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public ModeratorDto Add(ModeratorDto dto)
    {
        var model = ModeratorDto.ToModel(dto)!;
        _unitOfWork.Execute(() => _repository.Add(model));
        var saved = _repository.GetById(model.Id);
        return ModeratorDto.FromModel(saved)!;
    }

    public void Remove(int id)
    {
        var model = _repository.GetById(id);
        if (model is null) return;
        _unitOfWork.Execute(() => _repository.Remove(model));
    }

    public ModeratorDto? Update(ModeratorDto dto)
    {
        var existing = _repository.GetById(dto.Id);
        if (existing is null) return null;
        existing.Username = dto.Username;
        existing.Name = dto.Name;
        existing.Surname = dto.Surname;
        existing.RoleId = dto.RoleId;
        _unitOfWork.Execute(() => _repository.Update(existing));
        var updated = _repository.GetById(existing.Id);
        return ModeratorDto.FromModel(updated);
    }

    public ModeratorDto? GetById(int id)
    {
        var model = _repository.GetById(id);
        return ModeratorDto.FromModel(model);
    }

    public IList<ModeratorDto> GetAll()
    {
        return _repository.GetAll().Select(m => ModeratorDto.FromModel(m)!).ToList();
    }

}
