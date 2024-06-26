﻿using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service;

internal sealed class EmployeeService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper) : IEmployeeService
{
    private readonly IRepositoryManager _repository = repository;
    private readonly ILoggerManager _logger = logger;
    private readonly IMapper _mapper = mapper;

    public async Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges)
    {
        var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges);
        if(company is null)
            throw new CompanyNotFoundException(companyId);

        var employee = await _repository.Employee.GetEmployeeAsync(companyId, id, trackChanges);
        if (employee is null)
            throw new EmployeeNotFoundException(id);

        var dto = _mapper.Map<EmployeeDto>(employee);
        return dto;
    }

    public async Task<IEnumerable<EmployeeDto>> GetEmployeesAsync(Guid companyId, bool trackChanges)
    {
        var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges);
        if (company is null)
            throw new CompanyNotFoundException(companyId);
        var employeesFromDb = await _repository.Employee.GetEmployeesAsync(companyId, trackChanges);
        var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesFromDb);
        return employeesDto;
    }

    public async Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId, EmployeeForCreationDto employeeForCreation, bool trackChanges)
    {
        var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges);
        if(company is null)
        {
            throw new CompanyNotFoundException(companyId);
        }

        var employeeEntity = _mapper.Map<Employee>(employeeForCreation);

        _repository.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
        await _repository.SaveAsync();

        var employeeToReturn = _mapper.Map <EmployeeDto>(employeeEntity);
        return employeeToReturn;
    }

    public async Task DeleteEmployeeForCompanyAsync(Guid companyId, Guid id, bool trackChanges)
    {
        var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges);
        if (company is null)
            throw new CompanyNotFoundException(companyId);

        var employeeForCompany = await _repository.Employee.GetEmployeeAsync(companyId, id, trackChanges);
        if(employeeForCompany is null)
            throw new EmployeeNotFoundException(id);

        _repository.Employee.DeleteEmployee(employeeForCompany);
        await _repository.SaveAsync();
    }

    public async Task UpdateEmployeeForCompanyAsync(Guid compnayId, Guid id, EmployeeForUpdateDto employeeForUpdate, bool compTrackChanges, bool empTrackChanges)
    {
        var company = await _repository.Company.GetCompanyAsync(compnayId, compTrackChanges);
        if (company is null)
            throw new CompanyNotFoundException(compnayId);

        var employeeEntity = await _repository.Employee.GetEmployeeAsync(compnayId, id, empTrackChanges);
        if (employeeEntity is null)
            throw new EmployeeNotFoundException(id);

        _mapper.Map(employeeForUpdate, employeeEntity);
        await _repository.SaveAsync();
    }

    public async Task<(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)> GetEmployeeForPatchAsync(Guid companyId, Guid id, bool compTrackChanges, bool empTrackChanges)
    {
        var company = await _repository.Company.GetCompanyAsync(companyId, compTrackChanges);
        if(company is null)
            throw new CompanyNotFoundException(companyId);

        var employeeEntity = await _repository.Employee.GetEmployeeAsync(companyId, id, empTrackChanges);
        if(employeeEntity is null)
            throw new EmployeeNotFoundException(id);

        var employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employeeEntity);
        return (employeeToPatch, employeeEntity);
    }

    public async Task SaveChangesForPatchAsync(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)
    {
        _mapper.Map(employeeToPatch, employeeEntity);
        await _repository.SaveAsync();
    }
}
