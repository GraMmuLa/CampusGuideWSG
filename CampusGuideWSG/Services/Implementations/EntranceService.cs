using CampusGuideWSG.DTO;
using CampusGuideWSG.Exceptions;
using CampusGuideWSG.Helpers;
using CampusGuideWSG.Models;
using CampusGuideWSG.Repositories;
using CampusGuideWSG.Repositories.Implementations;
using System.Collections.Generic;
using System.Linq;

namespace CampusGuideWSG.Services.Implementations;

public class EntranceService : IEntranceService
{
    private readonly IEntranceRepository _entranceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EntranceService(IEntranceRepository repository, IUnitOfWork unitOfWork)
    {
        _entranceRepository = repository;
        _unitOfWork = unitOfWork;
    }

    public EntranceDto Add(EntranceDto dto)
    {
        Entrance model = EntranceDto.ToModel(dto);

        _unitOfWork.Execute(() => {
            if (_entranceRepository.GetById(model.Id) is not null)
                throw new UniquePropertyException("Entrance with this id already exists");
            if (_entranceRepository.GetByName(model.Name) is not null)
                throw new UniquePropertyException("Entrance with this name already exists");
            if (dto.BuildingId == 0)
                throw new MissingDataException("Missing Building Id");
            _entranceRepository.Add(model);
        });

        return EntranceDto.FromModel(_entranceRepository.GetById(model.Id) ??
            throw new NotFoundException("Entrance not found"));
    }

    public void Remove(int id)
    {
        Entrance model = _entranceRepository.GetById(id) ??
            throw new NotFoundException("Entrance not found");

        _unitOfWork.Execute(() => _entranceRepository.Remove(model));
    }

    public EntranceDto Update(EntranceDto dto)
    {
        if (_entranceRepository.GetByName(dto.Name) is not null &&
            _entranceRepository.GetByName(dto.Name)!.Id != dto.Id)
            throw new UniquePropertyException("Entrance with this name already exists");

        Entrance existing = _entranceRepository.GetById(dto.Id) ??
            throw new NotFoundException("Entrance not found");

        existing.Name = dto.Name;
        existing.IsOpen = dto.IsOpen;
        existing.BuildingId = dto.BuildingId;

        _unitOfWork.Execute(() => _entranceRepository.Update(existing));

        return EntranceDto.FromModel(_entranceRepository.GetById(existing.Id) ??
            throw new NotFoundException("Entrance not found"));
    }

    public EntranceDto GetById(int id)
    {
        return EntranceDto.FromModel(_entranceRepository.GetById(id) ??
            throw new NotFoundException("Entrance not found"));
    }

    public EntranceDto GetByName(string name)
    {
        return EntranceDto.FromModel(_entranceRepository.GetByName(name) ??
            throw new NotFoundException("Entrance not found"));
    }

    public IList<EntranceDto> GetAll()
    {
        return [.._entranceRepository.GetAll().Select(EntranceDto.FromModel)];
    }
}
