using CampusGuideWSG.DTO;
using CampusGuideWSG.Exceptions;
using CampusGuideWSG.Helpers;
using CampusGuideWSG.Models;
using CampusGuideWSG.Repositories;
using CampusGuideWSG.Repositories.Implementations;

namespace CampusGuideWSG.Services.Implementations;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RoleService(IRoleRepository repository, IUnitOfWork unitOfWork)
    {
        _roleRepository = repository;
        _unitOfWork = unitOfWork;
    }

    public RoleDto Add(RoleDto dto)
    {
        Role model = RoleDto.ToModel(dto);

        _unitOfWork.Execute(() => {
            if (_roleRepository.GetById(model.Id) is not null)
                throw new UniquePropertyException("Role with this id already exists");
            if (_roleRepository.GetByName(model.Name) is not null)
                throw new UniquePropertyException("Role with this name already exists");
            _roleRepository.Add(model);
        });

        return RoleDto.FromModel(_roleRepository.GetById(model.Id) ??
            throw new NotFoundException("Role not found"));
    }

    public void Remove(int id)
    {
        Role model = _roleRepository.GetById(id) ??
            throw new NotFoundException("Role not found");

        _unitOfWork.Execute(() => _roleRepository.Remove(model));
    }

    public RoleDto Update(RoleDto dto)
    {
        if (_roleRepository.GetByName(dto.Name) is not null &&
            _roleRepository.GetByName(dto.Name)!.Id != dto.Id)
            throw new UniquePropertyException("Role with this name already exists");

        Role? existing = _roleRepository.GetById(dto.Id) ??
            throw new NotFoundException("Role not found");

        _unitOfWork.Execute(() => _roleRepository.Update(existing));

        return RoleDto.FromModel(_roleRepository.GetById(existing.Id) ??
            throw new NotFoundException("Role not found"));
    }

    public RoleDto GetById(int id)
    {
        return RoleDto.FromModel(_roleRepository.GetById(id) ??
            throw new NotFoundException("Role not found"));
    }

    public RoleDto GetByName(string name)
    {
        return RoleDto.FromModel(_roleRepository.GetByName(name) ??
            throw new NotFoundException("Role not found"));
    }

    public IList<RoleDto> GetAll()
    {
        return [.._roleRepository.GetAll().Select(RoleDto.FromModel)];
    }
}
