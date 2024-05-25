﻿using AutoMapper;
using Entities.Models;
using Shared.DataTransferObjects;

namespace CompanyEmployees;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Company, CompanyDto>().ForMember(e => e.FullAddress, opt => opt.MapFrom(x => x.Address + ", " + x.Country));
        CreateMap<CompanyForCreationDto, Company>();
        CreateMap<CompanyForUpdateDto, Company>();

        CreateMap<Employee, EmployeeDto>();
        CreateMap<EmployeeForCreationDto, Employee>();
        CreateMap<EmployeeForUpdateDto, Employee>().ReverseMap();
    }
}
