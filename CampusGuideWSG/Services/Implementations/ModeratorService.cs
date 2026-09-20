using BCrypt.Net;
using CampusGuideWSG.DTO;
using CampusGuideWSG.Exceptions;
using CampusGuideWSG.Helpers;
using CampusGuideWSG.Models;
using CampusGuideWSG.Repositories;
using CampusGuideWSG.Repositories.Implementations;
using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;
using System.Linq;

namespace CampusGuideWSG.Services.Implementations;

public class ModeratorService : IModeratorService
{
    private readonly IModeratorRepository _moderatorRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IModeratorBuildingRepository _moderatorBuildingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public ModeratorService(IModeratorRepository repository,
        IRoleRepository roleRepository,
        IModeratorBuildingRepository moderatorBuildingRepository,
        IUnitOfWork unitOfWork,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _moderatorRepository = repository;
        _roleRepository = roleRepository;
        _moderatorBuildingRepository = moderatorBuildingRepository;
        _unitOfWork = unitOfWork;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public AuthenticationResponse Register(RegisterDto registerDto)
    {

        if (_moderatorRepository.GetByUsername(registerDto.Username) is not null)
            throw new UniquePropertyException("Moderator with this username already exists");

        registerDto.Password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

        _unitOfWork.Execute(() =>
        {
            _moderatorRepository.Add(new Moderator
            {
                Username = registerDto.Username,
                Name = registerDto.Name,
                Surname = registerDto.Surname,
                Password = registerDto.Password,
                RoleId = (_roleRepository.GetByName("Moderator") ??
                    throw new NotFoundException("Role not found")).Id
            });
        });

        ModeratorDto moderatorDto = ModeratorDto.FromModel(_moderatorRepository.GetByUsername(registerDto.Username) ??
                throw new NotFoundException("Moderator not found"));

        (string token, DateTime expiresAt) = _jwtTokenGenerator.CreateToken(moderatorDto,
            [(_roleRepository.GetByName("Moderator") ??
                throw new NotFoundException("Role not found")).Name]);

        return new AuthenticationResponse
        {
            JwtToken = token,
            ExpiresAt = expiresAt,
            Value = moderatorDto
        };
    }

    public AuthenticationResponse Login(LoginDto loginDto)
    {
        Moderator model = _moderatorRepository.GetByUsername(loginDto.Username) ??
            throw new AuthenticationFailureException("Failed to authenticate user");

        if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, model.Password))
            throw new AuthenticationFailureException("Failed to authenticate user");

        ModeratorDto moderatorDto = ModeratorDto.FromModel(model);

        (string token, DateTime expiresAt) = _jwtTokenGenerator.CreateToken(moderatorDto, [model.Role.Name]);

        return new AuthenticationResponse
        {
            JwtToken = token,
            ExpiresAt = expiresAt,
            Value = moderatorDto
        };
    }

    public void Remove(int id)
    {
        Moderator model = _moderatorRepository.GetById(id) ??
            throw new NotFoundException("Moderator not found");

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
