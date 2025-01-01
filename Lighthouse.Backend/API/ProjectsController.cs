﻿using Lighthouse.Backend.API.DTO;
using Lighthouse.Backend.Models;
using Lighthouse.Backend.Services.Factories;
using Lighthouse.Backend.Services.Interfaces;
using Lighthouse.Backend.Services.Interfaces.Update;
using Microsoft.AspNetCore.Mvc;

namespace Lighthouse.Backend.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IRepository<Project> projectRepository;
        private readonly IRepository<Team> teamRepository;
        private readonly IWorkItemUpdateService workItemUpdateService;
        private readonly IWorkItemServiceFactory workItemServiceFactory;

        private readonly IRepository<WorkTrackingSystemConnection> workTrackingSystemConnectionRepository;

        public ProjectsController(
            IRepository<Project> projectRepository, 
            IRepository<Team> teamRepository, 
            IWorkItemUpdateService workItemUpdateService, 
            IWorkItemServiceFactory workItemServiceFactory,
            IRepository<WorkTrackingSystemConnection> workTrackingSystemConnectionRepository)
        {
            this.projectRepository = projectRepository;
            this.teamRepository = teamRepository;
            this.workItemUpdateService = workItemUpdateService;
            this.workItemServiceFactory = workItemServiceFactory;
            this.workTrackingSystemConnectionRepository = workTrackingSystemConnectionRepository;
        }

        [HttpGet]
        public IEnumerable<ProjectDto> GetProjects()
        {
            var projectDtos = new List<ProjectDto>();

            var allProjects = projectRepository.GetAll();

            foreach (var project in allProjects)
            {
                var projectDto = new ProjectDto(project);
                projectDtos.Add(projectDto);
            }

            return projectDtos;
        }

        [HttpGet("{id}")]
        public ActionResult<ProjectDto> Get(int id)
        {
            var project = projectRepository.GetById(id);
            if (project == null)
            {
                return NotFound();
            }

            return Ok(new ProjectDto(project));
        }

        [HttpPost("refresh/{id}")]
        public ActionResult UpdateFeaturesForProject(int id)
        {
            workItemUpdateService.TriggerUpdate(id);

            return Ok();
        }

        [HttpDelete("{id}")]
        public void DeleteProject(int id)
        {
            projectRepository.Remove(id);
            projectRepository.Save();
        }

        [HttpGet("{id}/settings")]
        public ActionResult<ProjectSettingDto> GetProjectSettings(int id)
        {
            var project = projectRepository.GetById(id);

            if (project == null)
            {
                return NotFound();
            }

            var projectSettingDto = new ProjectSettingDto(project);
            return Ok(projectSettingDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProjectSettingDto>> UpdateProject(int id, ProjectSettingDto projectSetting)
        {
            var project = projectRepository.GetById(id);

            if (project == null)
            {
                return NotFound();
            }

            SyncProjectWithProjectSettings(project, projectSetting);

            projectRepository.Update(project);
            await projectRepository.Save();

            var updatedProjectSettingDto = new ProjectSettingDto(project);
            return Ok(updatedProjectSettingDto);
        }

        [HttpPost]
        public async Task<ActionResult<ProjectSettingDto>> CreateProject(ProjectSettingDto projectSetting)
        {
            var newProject = new Project();
            SyncProjectWithProjectSettings(newProject, projectSetting);

            projectRepository.Add(newProject);
            await projectRepository.Save();

            var projectSettingDto = new ProjectSettingDto(newProject);
            return Ok(projectSettingDto);
        }

        [HttpPost("validate")]
        public async Task<ActionResult<bool>> ValidateProjectSettings(ProjectSettingDto projectSettingDto)
        {
            var workTrackingSystem = workTrackingSystemConnectionRepository.GetById(projectSettingDto.WorkTrackingSystemConnectionId);

            if (workTrackingSystem == null)
            {
                return NotFound(false);
            }

            var project = new Project { WorkTrackingSystemConnection = workTrackingSystem };
            SyncProjectWithProjectSettings(project, projectSettingDto);

            var workItemService = workItemServiceFactory.GetWorkItemServiceForWorkTrackingSystem(project.WorkTrackingSystemConnection.WorkTrackingSystem);

            var result = await workItemService.ValidateProjectSettings(project);

            return Ok(result);
        }

        private void SyncProjectWithProjectSettings(Project project, ProjectSettingDto projectSetting)
        {
            project.Name = projectSetting.Name;
            project.WorkItemTypes = projectSetting.WorkItemTypes;
            project.WorkItemQuery = projectSetting.WorkItemQuery;
            project.UnparentedItemsQuery = projectSetting.UnparentedItemsQuery;

            project.UsePercentileToCalculateDefaultAmountOfWorkItems = projectSetting.UsePercentileToCalculateDefaultAmountOfWorkItems;
            project.DefaultAmountOfWorkItemsPerFeature = projectSetting.DefaultAmountOfWorkItemsPerFeature;
            project.DefaultWorkItemPercentile = projectSetting.DefaultWorkItemPercentile;
            project.HistoricalFeaturesWorkItemQuery = projectSetting.HistoricalFeaturesWorkItemQuery;
            project.SizeEstimateField = projectSetting.SizeEstimateField;
            project.OverrideRealChildCountStates = projectSetting.OverrideRealChildCountStates;

            project.WorkTrackingSystemConnectionId = projectSetting.WorkTrackingSystemConnectionId;

            project.FeatureOwnerField = projectSetting.FeatureOwnerField;

            SyncStates(project, projectSetting);
            SyncMilestones(project, projectSetting);
            SyncTeams(project, projectSetting);
        }

        private static void SyncStates(Project project, ProjectSettingDto projectSetting)
        {
            project.ToDoStates = projectSetting.ToDoStates;
            project.DoingStates = projectSetting.DoingStates;
            project.DoneStates = projectSetting.DoneStates;
        }

        private void SyncTeams(Project project, ProjectSettingDto projectSetting)
        {
            var teams = new List<Team>();
            foreach (var teamDto in projectSetting.InvolvedTeams)
            {
                var team = teamRepository.GetById(teamDto.Id);
                if (team != null)
                {
                    teams.Add(team);
                }
            }

            project.UpdateTeams(teams);

            project.OwningTeam = null;
            if (projectSetting.OwningTeam != null)
            {
                project.OwningTeam = teamRepository.GetById(projectSetting.OwningTeam.Id);
            }
        }

        private static void SyncMilestones(Project project, ProjectSettingDto projectSetting)
        {
            project.Milestones.Clear();
            foreach (var milestone in projectSetting.Milestones)
            {
                project.Milestones.Add(new Milestone
                {
                    Id = milestone.Id,
                    Name = milestone.Name,
                    Date = milestone.Date,
                    Project = project,
                    ProjectId = project.Id,
                });
            }
        }
    }
}
