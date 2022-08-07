using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Api.DTOs;
using TaskTracker.Api.Interfaces;
using TaskTracker.Api.Models;

namespace TaskTracker.Api.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TaskController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITaskRepository _taskRepository;
        private readonly IProjectRepository _projectRepository;

        private readonly ILogger<TaskController> _logger;

        public TaskController(IMapper mapper, ITaskRepository taskRepository, IProjectRepository projectRepository, ILogger<TaskController> logger)
        {
            _mapper = mapper;
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<ToDoTask>> AddTaskToProjectAsync([FromForm]CreateToDoTaskDTO toDoTaskDTO)
        {   
            var existingModel = await _taskRepository.GetAsync(toDoTaskDTO.Name);
            if(existingModel is not null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            var entity = new ToDoTask();
            entity = _mapper.Map<CreateToDoTaskDTO, ToDoTask>(toDoTaskDTO, entity);
            entity.Status = Models.TodoStatus.ToDo;
            entity.ToDoTaskId = await _taskRepository.CreateAsync(entity);
            await _projectRepository.AddTaskToProject(entity.ProjectId, entity);
            return CreatedAtAction(nameof(GetTaskViewModelAsync), new { id = entity.ToDoTaskId }, entity);
        }
        [HttpGet]
        public async Task<IEnumerable<ToDoTask>> GetToDoTasksAsync()
        {
            return await _taskRepository.GetAllAsync();
        }
        [HttpGet("vm/")]
        public async Task<ActionResult<IEnumerable<ToDoTaskDTO>>> GetTaskViewModelsAsync()
        {
            var models = await _taskRepository.GetAllAsync();
            var list = new List<ToDoTaskDTO>();
            foreach (var model in models)
            {
                var dto = new ToDoTaskDTO();
                dto = _mapper.Map<ToDoTask, ToDoTaskDTO>(model, dto);
                list.Add(dto);
            }
            return list;
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
        [HttpPut("{id}")]
        public async Task<ActionResult<ToDoTaskDTO>> UpdateToDoTask(int id, UpdateToDoTaskDTO dto)
        {
            var model = await _taskRepository.GetAsync(id);
            if(model is null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            model = _mapper.Map<UpdateToDoTaskDTO, ToDoTask>(dto, model);
            await _taskRepository.UpdateAsync(id, model);
            return StatusCode(StatusCodes.Status204NoContent);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteToDoTaskAsync(int id)
        {
            var model = await _taskRepository.GetAsync(id);
            if(model is null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            await _taskRepository.DeleteAsync(id);
            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}