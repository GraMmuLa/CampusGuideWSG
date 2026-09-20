using CampusGuideWSG.DTO;
using CampusGuideWSG.Exceptions;
using CampusGuideWSG.Helpers;
using CampusGuideWSG.Models;
using CampusGuideWSG.Repositories;
using CampusGuideWSG.Repositories.Implementations;
using System.Collections.Generic;
using System.Linq;

namespace CampusGuideWSG.Services.Implementations;

public class ModeratorService : IModeratorService
{
    private readonly IModeratorRepository _moderatorRepository;
    private readonly IModeratorBuildingRepository _moderatorBuildingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ModeratorService(IModeratorRepository repository,
        IModeratorBuildingRepository moderatorBuildingRepository, IUnitOfWork unitOfWork)
    {
        _moderatorRepository = repository;
        _moderatorBuildingRepository = moderatorBuildingRepository;
        _unitOfWork = unitOfWork;
    }

    public ModeratorDto Add(ModeratorDto dto)
    {
        Moderator model = ModeratorDto.ToModel(dto)!;

        _unitOfWork.Execute(() =>
        {
            if (_moderatorRepository.GetById(model.Id) is not null)
                throw new UniquePropertyException("Moderator with this id already exists");
            if (_moderatorRepository.GetByUsername(model.Username) is not null)
                throw new UniquePropertyException("Moderator with this username already exists");

            _moderatorRepository.Add(model);

            if (dto.BuildingIds != null)
            {
                foreach (int buildingId in dto.BuildingIds)
                {
                    ModeratorBuilding link = new()
                    {
                        Moderator = model,
                        BuildingId = buildingId
                    };

                    _moderatorBuildingRepository.Add(link);
                }
            }
        });

        Moderator? saved = _moderatorRepository.GetById(model.Id) ??
            throw new NotFoundException("Moderator not found");

        return ModeratorDto.FromModel(saved)!;
    }

    public void Remove(int id)
    {
        Moderator? model = _moderatorRepository.GetById(id);
        if (model is null) return;

        _unitOfWork.Execute(() =>
        {
            IList<ModeratorBuilding> links = [.._moderatorBuildingRepository
            .GetAll().Where(x => x.ModeratorId == model.Id)];

            foreach (ModeratorBuilding link in links) _moderatorBuildingRepository.Remove(link);

            _moderatorRepository.Remove(model);
        });
    }

    public ModeratorDto Update(ModeratorDto dto)
    {
        if (_moderatorRepository.GetByUsername(dto.Username) is not null &&
            _moderatorRepository.GetByUsername(dto.Username)!.Id != dto.Id)
            throw new UniquePropertyException("Moderator with this name already exists");

        Moderator existing = _moderatorRepository.GetById(dto.Id) ??
            throw new NotFoundException("Moderator not found");

        existing.Username = dto.Username;
        existing.Name = dto.Name;
        existing.Surname = dto.Surname;
        existing.RoleId = dto.RoleId;

        _unitOfWork.Execute(() =>
        {
            _moderatorRepository.Update(existing);

            if (dto.BuildingIds != null)
            {
                IList<ModeratorBuilding> old = [.. _moderatorBuildingRepository.GetAll().Where(x => x.ModeratorId == existing.Id)];
                foreach (ModeratorBuilding o in old) _moderatorBuildingRepository.Remove(o);

                foreach (int buildingId in dto.BuildingIds)
                {
                    ModeratorBuilding link = new()
                    {
                        Moderator = existing,
                        BuildingId = buildingId
                    };
                    _moderatorBuildingRepository.Add(link);
                }
            }
        });

        return ModeratorDto.FromModel(_moderatorRepository.GetById(existing.Id) ??
            throw new NotFoundException("Moderator not found"));
    }

    public ModeratorDto GetById(int id)
    {
        return ModeratorDto.FromModel(_moderatorRepository.GetById(id) ??
            throw new NotFoundException("Moderator not found"));
    }

    public ModeratorDto GetByUsername(string username)
    {
        return ModeratorDto.FromModel(_moderatorRepository.GetByUsername(username) ??
            throw new NotFoundException("Moderator not found"));
    }

    public IList<ModeratorDto> GetAll()
    {
        return [.._moderatorRepository.GetAll().Select(ModeratorDto.FromModel)];
    }

    public void AddBuilding(int moderatorId, int buildingId)
    {
        ModeratorBuilding link = new()
        {
            ModeratorId = moderatorId,
            BuildingId = buildingId
        };
        _unitOfWork.Execute(() => _moderatorBuildingRepository.Add(link));
    }

    public void RemoveBuilding(int moderatorId, int buildingId)
    {
        ModeratorBuilding existing = _moderatorBuildingRepository
            .GetAll()
            .FirstOrDefault(x => x.ModeratorId == moderatorId && x.BuildingId == buildingId) ??
            throw new NotFoundException("Moderator-Building link not found");

        _unitOfWork.Execute(() => _moderatorBuildingRepository.Remove(existing));
    }
}
