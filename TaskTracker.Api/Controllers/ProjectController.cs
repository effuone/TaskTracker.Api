using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Api.DTOs;
using TaskTracker.Api.Interfaces;
using TaskTracker.Api.Models;

namespace TaskTracker.Api.Controllers
{
    [ApiController]
    [Route("api/projects")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly ILogger<ProjectController> _logger;
        private readonly IMapper _mapper;

        public ProjectController(IProjectRepository projectRepository, ITaskRepository taskRepository, ILogger<ProjectController> logger, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _taskRepository = taskRepository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<ProjectDTO>> PostProjectAsync([FromForm]CreateProjectDTO projectDTO)
        {
            var existingModel = await _projectRepository.GetAsync(projectDTO.ProjectName);
            if(existingModel is not null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            var entity = new Project();
            entity = _mapper.Map<CreateProjectDTO, Project>(projectDTO, entity);
            entity.Status = ProjectStatus.Active;
            entity.ProjectId = await _projectRepository.CreateAsync(entity);
            return CreatedAtAction(nameof(GetProjectAsync), new { id = entity.ProjectId }, entity);
        }
        [HttpGet("vm")]
        public async Task<IEnumerable<ProjectDTO>> GetProjectViewModelsAsync()
        {
            var models = await _projectRepository.GetAllAsync();
            var viewModels = new List<ProjectDTO>();
            foreach (var model in models)
            {
                var projectDTO = new ProjectDTO();
                model.Tasks = await _taskRepository.GetAllAsync(model.ProjectId);
                projectDTO = _mapper.Map<Project, ProjectDTO>(model, projectDTO);
                var taskDTOs = new List<ToDoTaskDTO>();
                foreach(var task in model.Tasks)
                {
                    var taskDto = new ToDoTaskDTO();
                    taskDTOs.Add(_mapper.Map<ToDoTask, ToDoTaskDTO>(task, taskDto));
                }
                projectDTO.ToDoTasks = taskDTOs;
                viewModels.Add(projectDTO);
            }
            return viewModels;
        }
        [HttpGet]
        public async Task<IEnumerable<Project>> GetProjectsAsync()
        {
            var models = await _projectRepository.GetAllAsync();
            foreach(var model in models)
            {
                model.Tasks = await _taskRepository.GetAllAsync(model.ProjectId);
            }
            return models;
        }
        [HttpGet("vm/{id}")]
        public async Task<ActionResult<ProjectDTO>> GetProjectViewModelAsync(int id)
        {
            var model = await _projectRepository.GetAsync(id);
            if(model is null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            model.Tasks = await _taskRepository.GetAllAsync(id);
            var taskDTOs = new List<ToDoTaskDTO>();
            foreach(var task in model.Tasks)
            {
                var taskDto = new ToDoTaskDTO();
                taskDTOs.Add(_mapper.Map<ToDoTask, ToDoTaskDTO>(task, taskDto));
            }
            var projectDTO = new ProjectDTO();
            projectDTO = _mapper.Map<Project, ProjectDTO>(model, projectDTO);
            projectDTO.ToDoTasks = taskDTOs;
            return projectDTO;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProjectAsync(int id)
        {
            var model = await _projectRepository.GetAsync(id);
            if(model is null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            model.Tasks = await _taskRepository.GetAllAsync(id);
            return model;
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProjectAsync(int id, UpdateProjectDTO projectDTO)
        {
            var model = await _projectRepository.GetAsync(id);
            if(model is null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            model = _mapper.Map<UpdateProjectDTO, Project>(projectDTO, model);
            await _projectRepository.UpdateAsync(id, model);
            return StatusCode(StatusCodes.Status204NoContent);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProjectAsync(int id)
        {
            var model = await _projectRepository.GetAsync(id);
            if(model is null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            await _projectRepository.DeleteAsync(id);
            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}