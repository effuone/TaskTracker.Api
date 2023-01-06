using TaskTracker.Domain.Interfaces;
using TaskTracker.Data.Models;
using TaskTracker.Api.Controllers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using TaskTracker.Data.AutoMapperProfiles;
using TaskTracker.Data.DTOs;

namespace TaskTracker.UnitTests;

public class ProjectControllerTests
{
    private readonly Mock<IProjectRepository> _projectRepositoryStub = new();
    private readonly Mock<IMapper> _mapperStub = new();
    private readonly Random random = new();
    [Fact]
    public async void GetProjectAsync_WithUnexistingProject_ReturnsNotFound()
    {
        //Arrange
        _projectRepositoryStub.Setup(repo=>repo.GetAsync(It.IsAny<int>())).ReturnsAsync((Project?)null);

        //controller configuration setup
        var controller = new ProjectController(_projectRepositoryStub.Object, _mapperStub.Object);

        //Act
        var result = await controller.GetProjectAsync(random.Next(0,100));

        //Assert
        result.Result.Should().BeOfType<NotFoundResult>();
    }
    [Fact]
    public async Task GetProjectAsync_WithExistingProject_ReturnsExpectedProjectDTO()
    {
        // Arrange
        var expectedItem = CreateRandomProject();
        _projectRepositoryStub.Setup(repo=>repo.GetAsync(It.IsAny<int>())).ReturnsAsync(expectedItem);

        var controller = new ProjectController(_projectRepositoryStub.Object, _mapperStub.Object);
        //Act
        var result = await controller.GetProjectAsync(random.Next(1000));
        var dto = _mapperStub.Object.Map<Project, ProjectDTO>(expectedItem, new ProjectDTO());
        //Assert
        result.Value.Should().BeEquivalentTo(
            dto
            ,options=>options.ComparingByMembers<ProjectDTO>());
    }
    [Fact]
    public async Task GetProjectsAsync_WithExistingProjects_ReturnsExpectedProjects()
    {
        //Arrange
        var expectedItems = new[]{CreateRandomProject(), CreateRandomProject(), CreateRandomProject()};
        _projectRepositoryStub.Setup(repo=>repo.GetAllAsync()).ReturnsAsync(expectedItems);

        var controller = new ProjectController(_projectRepositoryStub.Object, _mapperStub.Object);
        //Act
        var actualProjects = await controller.GetProjectsAsync();
        var dtos = expectedItems.Select(project=>{
            return _mapperStub.Object.Map<Project, ProjectDTO>(project, new ProjectDTO());
        });
        //Assert
        actualProjects.Should().BeEquivalentTo(
            dtos,
            options=>options.ComparingByMembers<ProjectDTO>()
        );
    }
    [Fact]
    public async Task CreateProjectAsync_WithExistingProjectProjectToCreate_ReturnsProjectId()
    {
        //Arrange

        var newProjectDTO = new CreateProjectDTO() {ProjectName = "Name", StartDate = DateTime.Now, Priority = 3};
        var newProject = _mapperStub.Object.Map<CreateProjectDTO, Project>(newProjectDTO, new Project());
        _projectRepositoryStub.Setup(repo=>repo.CreateAsync(newProject)).ReturnsAsync(newProject.ProjectId);

        var controller = new ProjectController(_projectRepositoryStub.Object, _mapperStub.Object);

        //Act
        var result = await controller.PostProjectAsync(newProjectDTO);
        //Assert
        var createdProject = (result.Result as CreatedAtActionResult).Value as ProjectDTO;
        newProjectDTO.Should().BeEquivalentTo(
            newProjectDTO,
            options => options.ComparingByMembers<ProjectDTO>().ExcludingMissingMembers()
        );
    }
    private Project CreateRandomProject()
    {
        return new()
        {
            ProjectId = random.Next(0,100000),
            ProjectName = "Random project",
            StartDate = new DateTime(year: 2022, month: 9, day: 1),
            Priority = random.Next(1,4)
        };
    }
}