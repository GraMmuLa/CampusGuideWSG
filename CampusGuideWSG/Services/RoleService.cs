using CampusGuideWSG.Helpers;
using CampusGuideWSG.Context;
using CampusGuideWSG.DTO;
using CampusGuideWSG.Models;
using CampusGuideWSG.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace CampusGuideWSG.Services;

public class RoleService
{
    private readonly IRoleRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public RoleService(IRoleRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public RoleDto Add(RoleDto dto)
    {
        var model = RoleDto.ToModel(dto)!;
        _unitOfWork.Execute(() => _repository.Add(model));
        var saved = _repository.GetById(model.Id);
        return RoleDto.FromModel(saved)!;
    }

    public void Remove(int id)
    {
        var model = _repository.GetById(id);
        if (model is null) return;
        _unitOfWork.Execute(() => _repository.Remove(model));
    }

    public RoleDto? Update(RoleDto dto)
    {
        var existing = _repository.GetById(dto.Id);
        if (existing is null) return null;
        existing.Name = dto.Name;
        _unitOfWork.Execute(() => _repository.Update(existing));
        var updated = _repository.GetById(existing.Id);
        return RoleDto.FromModel(updated);
    }

    public RoleDto? GetById(int id)
    {
        var model = _repository.GetById(id);
        return RoleDto.FromModel(model);
    }

    public IList<RoleDto> GetAll()
    {
        return _repository.GetAll().Select(r => RoleDto.FromModel(r)!).ToList();
    }

}
