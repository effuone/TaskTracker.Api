using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Data.DTOs;
using TaskTracker.Data.Models;
using TaskTracker.Domain.Interfaces;

namespace TaskTracker.Api.Controllers
{
    [ApiController]
    [Route("api/projects")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;

        public ProjectController(IProjectRepository projectRepository, ITaskRepository taskRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _taskRepository = taskRepository;
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<ActionResult<ProjectDTO>> PostProjectAsync([FromForm]CreateProjectDTO projectDTO)
        {
            var existingModel = await _projectRepository.GetAsync(projectDTO.ProjectName);
            if(existingModel is not null)
            {
                return BadRequest();
            }
            var entity = new Project();
            entity = _mapper.Map<CreateProjectDTO, Project>(projectDTO, entity);
            entity.Status = ProjectStatus.Active;
            entity.ProjectId = await _projectRepository.CreateAsync(entity);
            return CreatedAtAction(nameof(GetProjectAsync), new { id = entity.ProjectId }, entity);
        }
        [HttpGet]
        public async Task<IEnumerable<ProjectDTO>> GetProjectsAsync()
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
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDTO>> GetProjectAsync(int id)
        {
            var model = await _projectRepository.GetAsync(id);
            if(model is null)
            {
                return NotFound();
            }
            model.Tasks = await _taskRepository.GetAllAsync(id);

            var projectDTO = _mapper.Map<Project, ProjectDTO>(model, new ProjectDTO());
            
            projectDTO.ToDoTasks = model.Tasks.Select(x=>{
                var taskDto = new ToDoTaskDTO();
                return _mapper.Map<ToDoTask, ToDoTaskDTO>(x, taskDto);
            });
            return Ok(projectDTO);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProjectAsync(int id, UpdateProjectDTO projectDTO)
        {
            var model = await _projectRepository.GetAsync(id);
            if(model is null)
            {
                return NotFound();
            }
            model = _mapper.Map<UpdateProjectDTO, Project>(projectDTO, model);
            await _projectRepository.UpdateAsync(id, model);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProjectAsync(int id)
        {
            var model = await _projectRepository.GetAsync(id);
            if(model is null)
            {
                return NotFound();
            }
            await _projectRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}