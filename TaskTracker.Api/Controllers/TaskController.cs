
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Data.DTOs;
using TaskTracker.Data.Models;
using TaskTracker.Domain.Interfaces;

namespace TaskTracker.Api.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TaskController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITaskRepository _taskRepository;
        private readonly IProjectRepository _projectRepository;
        public TaskController(IMapper mapper, ITaskRepository taskRepository, IProjectRepository projectRepository)
        {
            _mapper = mapper;
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
        }

        [HttpPost]
        public async Task<ActionResult<ToDoTask>> AddTaskToProjectAsync([FromForm]CreateToDoTaskDTO toDoTaskDTO)
        {   
            var existingModel = await _taskRepository.GetAsync(toDoTaskDTO.Name);
            if(existingModel is not null)
            {
                return BadRequest();
            }
            var entity = _mapper.Map<CreateToDoTaskDTO, ToDoTask>(toDoTaskDTO, new ToDoTask());
            entity.Status = TodoStatus.ToDo;
            entity.ToDoTaskId = await _taskRepository.CreateAsync(entity);
            await _projectRepository.AddTaskToProject(entity.ProjectId, entity);
            return CreatedAtAction(nameof(GetTaskViewModelAsync), new { id = entity.ToDoTaskId }, entity);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoTaskDTO>>> GetTaskViewModelsAsync()
        {
            var toDoTaskDTOList = new List<ToDoTaskDTO>();
            var models = (await _taskRepository.GetAllAsync()).Select(model=>{
                var dto = new ToDoTaskDTO();
                return _mapper.Map<ToDoTask, ToDoTaskDTO>(model, dto);
            }).ToList();
            return models;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoTask>> GetTaskAsync(int id)
        {
            var todo = await _taskRepository.GetAsync(id);
            if(todo is null)
            {
                return NotFound();
            }
            return Ok(todo);
        }
        [HttpGet("vm/{id}")]
        public async Task<ActionResult<ToDoTaskDTO>> GetTaskViewModelAsync(int id)
        {
            var todo = await _taskRepository.GetAsync(id);
            if(todo is null)
            {
                return NotFound();
            }
            var toDoTaskDto = _mapper.Map<ToDoTaskDTO>(todo);
            return Ok(toDoTaskDto);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteToDoTaskAsync(int id)
        {
            var model = await _taskRepository.GetAsync(id);
            if(model is null)
            {
                return NotFound();
            }
            await _taskRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}