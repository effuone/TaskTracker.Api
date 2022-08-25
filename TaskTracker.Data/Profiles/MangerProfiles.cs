using AutoMapper;
using TaskTracker.Data.DTOs;
using TaskTracker.Data.Models;

namespace TaskTracker.Data.AutoMapperProfiles
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