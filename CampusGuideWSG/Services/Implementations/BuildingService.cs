using CampusGuideWSG.DTO;
using CampusGuideWSG.Exceptions;
using CampusGuideWSG.Helpers;
using CampusGuideWSG.Models;
using CampusGuideWSG.Repositories;
using CampusGuideWSG.Repositories.Implementations;

namespace CampusGuideWSG.Services.Implementations;

public class BuildingService : IBuildingService
{
    private readonly IBuildingRepository _buildingRepository;
    private readonly IModeratorBuildingRepository _moderatorBuildingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public BuildingService(IBuildingRepository repository, IModeratorBuildingRepository mbRepository, IUnitOfWork unitOfWork)
    {
        _buildingRepository = repository;
        _moderatorBuildingRepository = mbRepository;
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

    public void AddModerator(int buildingId, int moderatorId)
    {
        ModeratorBuilding link = new()
        {
            BuildingId = buildingId,
            ModeratorId = moderatorId
        };
        _unitOfWork.Execute(() => _moderatorBuildingRepository.Add(link));
    }

    public void RemoveModerator(int buildingId, int moderatorId)
    {
        ModeratorBuilding? existing = _moderatorBuildingRepository.GetAll()
            .FirstOrDefault(x => x.BuildingId == buildingId && x.ModeratorId == moderatorId);

        _unitOfWork.Execute(() => _moderatorBuildingRepository.Remove(existing ??
            throw new NotFoundException("Moderator-Building link not found")));
    }
}
