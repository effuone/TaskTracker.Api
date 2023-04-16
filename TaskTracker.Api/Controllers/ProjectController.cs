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
        private readonly IMapper _mapper;

        public ProjectController(IProjectRepository projectRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<ActionResult<ProjectDTO>> CreateProjectAsync([FromForm]CreateProjectDTO createProjectDTO)
        {
            Project existingProject = await _projectRepository.GetAsync(createProjectDTO.ProjectName);
            if(existingProject != null)
                return BadRequest("A project with the same name already exists.");

            Project newProject = _mapper.Map<CreateProjectDTO, Project>(createProjectDTO);
            newProject.Status = ProjectStatus.Active;

            int projectId = await _projectRepository.CreateAsync(newProject);
            return CreatedAtAction(nameof(GetProjectAsync), new { id = projectId }, newProject);
        }
        [HttpGet]
        public async Task<IEnumerable<ProjectDTO>> GetProjectsAsync()
        {
            return (await _projectRepository.GetAllAsync()).Select(project=>{
                return _mapper.Map<Project, ProjectDTO>(project, new ProjectDTO());
            });
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDTO>> GetProjectAsync(int id)
        {
            var model = await _projectRepository.GetAsync(id);
            if(model is null)
                return NotFound();
            return Ok(_mapper.Map<Project, ProjectDTO>(model, new ProjectDTO()));
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProjectAsync(int id, UpdateProjectDTO projectDTO)
        {
            var model = await _projectRepository.GetAsync(id);
            if(model is null)
                return NotFound();
            model = _mapper.Map<UpdateProjectDTO, Project>(projectDTO, model);
            await _projectRepository.UpdateAsync(id, model);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProjectAsync(int id)
        {
            var model = await _projectRepository.GetAsync(id);
            if(model is null)
                return NotFound();
            await _projectRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}