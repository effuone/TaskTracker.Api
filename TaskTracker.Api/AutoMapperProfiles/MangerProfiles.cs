using AutoMapper;
using TaskTracker.Api.DTOs;
using TaskTracker.Api.Models;

namespace TaskTracker.Api.AutoMapperProfiles
{
    //mapping profiles for data transfer objects
    public class ManagerProfiles : Profile
    {
        public ManagerProfiles()
        {
            //Mapping task entity models
            CreateMap<CreateToDoTaskDTO, ToDoTask>();
            CreateMap<UpdateToDoTaskDTO, ToDoTask>();
            CreateMap<ToDoTask, ToDoTaskDTO>();

            //Mapping project entity models
            CreateMap<CreateProjectDTO, Project>();
            CreateMap<UpdateProjectDTO, Project>();
            CreateMap<Project, ProjectDTO>();
        }
    }
}