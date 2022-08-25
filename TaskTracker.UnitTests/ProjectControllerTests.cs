using TaskTracker.Api.Interfaces;
using TaskTracker.Api.Models;
using TaskTracker.Api.Controllers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using TaskTracker.Api.DTOs;

namespace TaskTracker.UnitTests;

public class ProjectControllerTests
{
    private readonly Mock<IProjectRepository> _projectRepositoryStub = new();
    private readonly Mock<ITaskRepository> _taskRepositoryStub = new();
    private readonly Mock<IMapper> _mapperStub = new();
    private readonly Random random = new();
    [Fact]
    public async void GetProjectAsync_WithUnexistingProject_ReturnsNotFound()
    {
        //Arrange
        _projectRepositoryStub.Setup(repo=>repo.GetAsync(It.IsAny<int>())).ReturnsAsync((Project)null);
        _taskRepositoryStub.Setup(repo=>repo.GetAsync(It.IsAny<int>())).ReturnsAsync((ToDoTask)null);
        var controller = new ProjectController(_projectRepositoryStub.Object, _taskRepositoryStub.Object, _mapperStub.Object);

        //Act
        var result = await controller.GetProjectAsync(random.Next(0,100));

        //Assert
        result.Result.Should().BeOfType<NotFoundResult>();
    }
    [Fact]
    public async Task GetProjectAsync_WithExistingProject_ReturnsExpectedProject()
    {
        // Arrange
        var expectedItem = CreateRandomProject();
        
        _mapperStub.Setup(x => x.Map<Project>(It.IsAny<ProjectDTO>()));
        _taskRepositoryStub.Setup(repo=>repo.GetAsync(It.IsAny<int>())).ReturnsAsync((ToDoTask)null);
        _projectRepositoryStub.Setup(repo=>repo.GetAsync(It.IsAny<int>())).ReturnsAsync(expectedItem);
        
        var projectDTO = _mapperStub.Object.Map<Project, ProjectDTO>(expectedItem, new ProjectDTO());
        var controller = new ProjectController(_projectRepositoryStub.Object, _taskRepositoryStub.Object, _mapperStub.Object);
        //Act
        var result = await controller.GetProjectAsync(random.Next(1000));
        //Assert
        result.Value.Should().BeEquivalentTo
        (
            projectDTO,
            options=>options.ComparingByMembers<Project>()
        );
    }
    [Fact]
    public async Task GetProjectsAsync_WithExistingProjects_ReturnsExpectedProjects()
    {
        //Arrange
        var expectedItems = new[]{CreateRandomProject(), CreateRandomProject(), CreateRandomProject()};
        _projectRepositoryStub.Setup(repo=>repo.GetAllAsync()).ReturnsAsync(expectedItems);

        var controller = new ProjectController(_projectRepositoryStub.Object, _taskRepositoryStub.Object, _mapperStub.Object);
        //Act
        var actualProjects = await controller.GetProjectsAsync();
        //Assert
        actualProjects.Should().BeEquivalentTo
        (
            expectedItems,
            options=>options.ComparingByMembers<Project>()
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