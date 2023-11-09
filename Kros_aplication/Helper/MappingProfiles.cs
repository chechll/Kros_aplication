using AutoMapper;
using Kros_aplication.Dto;
using Kros_aplication.Models;

namespace Kros_aplication.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            CreateMap<Worker, WorkerDto>();
            CreateMap<WorkerDto, Worker>();
            CreateMap<Project, ProjectDto>();
            CreateMap<ProjectDto, Project>();
            CreateMap<Firm, FirmDto>();
            CreateMap<FirmDto, Firm>();
            CreateMap<Department, DepartmentDto>();
            CreateMap<DepartmentDto, Department>();
            CreateMap<Division, DivisionDto>();
            CreateMap<DivisionDto, Division>();
        }
        
    }
}
