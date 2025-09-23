using AutoMapper;
using DemoG03.BusinessLogic.DTOs.Employees;
using DemoG03.DataAccess.Models.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoG03.BusinessLogic.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Employee, EmployeeDto>()
                .ForMember(dist => dist.EmpGender, options => options.MapFrom(src => src.Gender))
                .ForMember(dist => dist.EmpType, options => options.MapFrom(src => src.EmployeeType))
                .ForMember(dist => dist.DepartmentName, options => options.MapFrom(src => src.Department == null ? "No Department" : src.Department.Name));
            //.ReverseMap();
            CreateMap<Employee, EmployeeDetailsDto>()
                .ForMember(dist => dist.Gender, options => options.MapFrom(src => src.Gender))
                .ForMember(dist => dist.EmployeeType, options => options.MapFrom(src => src.EmployeeType))
                .ForMember(dist => dist.HiringDate, options => options.MapFrom(src => DateOnly.FromDateTime(src.HiringDate)))
                .ForMember(dist => dist.DepartmentName, options => options.MapFrom(src => src.Department == null ? "No Department" : src.Department.Name));

            CreateMap<CreatedEmployeeDto, Employee>()
                .ForMember(dist => dist.HiringDate, options => options.MapFrom(src => src.HiringDate.ToDateTime(new TimeOnly())));


            CreateMap<UpdatedEmployeeDto, Employee>()
                .ForMember(dist => dist.HiringDate, options => options.MapFrom(src => src.HiringDate.ToDateTime(new TimeOnly())));
        }
    }
}
