using CampusGuideWSG.DTO;
using CampusGuideWSG.Exceptions;
using CampusGuideWSG.Helpers;
using CampusGuideWSG.Models;
using CampusGuideWSG.Repositories;
using CampusGuideWSG.Repositories.Implementations;
using Microsoft.AspNetCore.Authorization;

namespace CampusGuideWSG.Services.Implementations;

public class BuildingService : IBuildingService
{
    private readonly IBuildingRepository _buildingRepository;
    private readonly IModeratorRepository _moderatorRepository;
    private readonly IUnitOfWork _unitOfWork;

    public BuildingService(IBuildingRepository repository,
        IModeratorRepository moderatorRepository,
        IUnitOfWork unitOfWork)
    {
        _buildingRepository = repository;
        _moderatorRepository = moderatorRepository;
        _unitOfWork = unitOfWork;
    }

    public BuildingDto Add(BuildingDto dto)
    {
        Building model = BuildingDto.ToModel(dto);

        _unitOfWork.Execute(() => {
            if (_buildingRepository.GetById(model.Id) is not null)
                throw new UniquePropertyException("Building with this id already exists");
            if (_buildingRepository.GetByName(model.Name) is not null)
                throw new UniquePropertyException("Building with this name already exists");
            _buildingRepository.Add(model);
        });

        return BuildingDto.FromModel(_buildingRepository.GetById(model.Id) ??
            throw new NotFoundException("Building not found"));
    }

    public void Remove(int id)
    {
        Building model = _buildingRepository.GetById(id) ??
            throw new NotFoundException("Building not found");

        model.Moderators.Clear();

        _unitOfWork.Execute(() => _buildingRepository.Remove(model));
    }

    public BuildingDto Update(BuildingDto dto)
    {
        if (_buildingRepository.GetByName(dto.Name) is not null &&
            _buildingRepository.GetByName(dto.Name)!.Id != dto.Id)
            throw new UniquePropertyException("Building with this name already exists");
        Building existing = _buildingRepository.GetById(dto.Id) ??
            throw new NotFoundException("Building not found");

        existing.Name = dto.Name;
        existing.Moderators = [.. _moderatorRepository.GetAll().Where(x => x.Id == dto.Id)];

        _unitOfWork.Execute(() => _buildingRepository.Update(existing));

        return BuildingDto.FromModel(_buildingRepository.GetById(existing.Id) ??
            throw new NotFoundException("Building not found"));
    }

    public BuildingDto GetById(int id)
    {
        return BuildingDto.FromModel(_buildingRepository.GetById(id) ??
            throw new NotFoundException("Building not found"));
    }

    public BuildingDto GetByName(string name)
    {
        return BuildingDto.FromModel(_buildingRepository.GetByName(name) ??
            throw new NotFoundException("Building not found"));
    }

    public IList<BuildingDto> GetAll()
    {
        return [.._buildingRepository.GetAll().Select(BuildingDto.FromModel)];
    }

    public BuildingDto AddModerator(int buildingId, int moderatorId)
    {
        Building building = _buildingRepository.GetById(buildingId) ??
            throw new NotFoundException("Building not found");

        Moderator moderator = _moderatorRepository.GetById(moderatorId) ??
            throw new NotFoundException("Moderator not found");

        if (building.Moderators.Any(x => x.Id == moderatorId))
            throw new UniquePropertyException("Moderator is already assigned to this building");

        building.Moderators.Add(moderator);

        _unitOfWork.Execute(() => _buildingRepository.Update(building));

        return BuildingDto.FromModel(_buildingRepository.GetById(buildingId)!);
    }

    public BuildingDto RemoveModerator(int buildingId, int moderatorId)
    {
        Building building = _buildingRepository.GetById(buildingId) ??
            throw new NotFoundException("Building not found");

        Moderator moderator = _moderatorRepository.GetById(moderatorId) ??
            throw new NotFoundException("Moderator not found");

        if (!building.Moderators.Any(x => x.Id == moderatorId))
            throw new NotFoundException("Moderator not found");

        building.Moderators.Remove(moderator);

        _unitOfWork.Execute(() => _buildingRepository.Update(building));

        return BuildingDto.FromModel(_buildingRepository.GetById(buildingId)!);
    }
}
