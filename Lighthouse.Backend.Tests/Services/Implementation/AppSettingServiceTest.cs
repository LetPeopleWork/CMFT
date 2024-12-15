﻿using Lighthouse.Backend.Models;
using Lighthouse.Backend.Models.AppSettings;
using Lighthouse.Backend.Services.Interfaces;
using Lighthouse.Backend.Services.Implementation;
using Moq;
using Lighthouse.Backend.API.DTO;

namespace Lighthouse.Backend.Tests.Services.Implementation
{
    public class AppSettingServiceTests
    {
        private Mock<IRepository<AppSetting>> repositoryMock;

        [SetUp]
        public void Setup()
        {
            repositoryMock = new Mock<IRepository<AppSetting>>();
        }

        [Test]
        public void GetFeatureRefreshSettings_ReturnsCorrectSettings()
        {
            SetupRepositoryForKeys(AppSettingKeys.FeaturesRefreshInterval, "60", AppSettingKeys.FeaturesRefreshAfter, "360", AppSettingKeys.FeaturesRefreshStartDelay, "1");

            var service = CreateService();

            var settings = service.GetFeaturRefreshSettings();

            Assert.Multiple(() =>
            {
                Assert.That(settings.Interval, Is.EqualTo(60));
                Assert.That(settings.RefreshAfter, Is.EqualTo(360));
                Assert.That(settings.StartDelay, Is.EqualTo(1));
            });
        }

        [Test]
        public void GetForecastRefreshSettings_ReturnsCorrectSettings()
        {
            SetupRepositoryForKeys(AppSettingKeys.ForecastRefreshInterval, "20", AppSettingKeys.ForecastRefreshAfter, "120", AppSettingKeys.ForecastRefreshStartDelay, "3");

            var service = CreateService();

            var settings = service.GetForecastRefreshSettings();

            Assert.Multiple(() =>
            {
                Assert.That(settings.Interval, Is.EqualTo(20));
                Assert.That(settings.RefreshAfter, Is.EqualTo(120));
                Assert.That(settings.StartDelay, Is.EqualTo(3));
            });
        }

        [Test]
        public void GetThroughputRefreshSettings_ReturnsCorrectSettings()
        {
            SetupRepositoryForKeys(AppSettingKeys.ThroughputRefreshInterval, "30", AppSettingKeys.ThroughputRefreshAfter, "180", AppSettingKeys.ThroughputRefreshStartDelay, "2");

            var service = CreateService();

            var settings = service.GetThroughputRefreshSettings();

            Assert.Multiple(() =>
            {
                Assert.That(settings.Interval, Is.EqualTo(30));
                Assert.That(settings.RefreshAfter, Is.EqualTo(180));
                Assert.That(settings.StartDelay, Is.EqualTo(2));
            });
        }

        [Test]
        public async Task UpdateFeatureRefreshSettings_UpdatesCorrectlyAsync()
        {
            SetupRepositoryForKeys(AppSettingKeys.FeaturesRefreshInterval, "60", AppSettingKeys.FeaturesRefreshAfter, "360", AppSettingKeys.FeaturesRefreshStartDelay, "1");

            var service = CreateService();

            var newSettings = new RefreshSettings { Interval = 70, RefreshAfter = 370, StartDelay = 10 };
            await service.UpdateFeatureRefreshSettingsAsync(newSettings);

            VerifyUpdateCalled(AppSettingKeys.FeaturesRefreshInterval, "70");
            VerifyUpdateCalled(AppSettingKeys.FeaturesRefreshAfter, "370");
            VerifyUpdateCalled(AppSettingKeys.FeaturesRefreshStartDelay, "10");
        }

        [Test]
        public async Task UpdateForecastRefreshSettings_UpdatesCorrectlyAsync()
        {
            SetupRepositoryForKeys(AppSettingKeys.ForecastRefreshInterval, "20", AppSettingKeys.ForecastRefreshAfter, "120", AppSettingKeys.ForecastRefreshStartDelay, "3");

            var service = CreateService();

            var newSettings = new RefreshSettings { Interval = 25, RefreshAfter = 130, StartDelay = 5 };
            await service.UpdateForecastRefreshSettingsAsync(newSettings);

            VerifyUpdateCalled(AppSettingKeys.ForecastRefreshInterval, "25");
            VerifyUpdateCalled(AppSettingKeys.ForecastRefreshAfter, "130");
            VerifyUpdateCalled(AppSettingKeys.ForecastRefreshStartDelay, "5");
        }

        [Test]
        public async Task UpdateThroughputRefreshSettings_UpdatesCorrectlyAsync()
        {
            SetupRepositoryForKeys(AppSettingKeys.ThroughputRefreshInterval, "30", AppSettingKeys.ThroughputRefreshAfter, "180", AppSettingKeys.ThroughputRefreshStartDelay, "2");

            var service = CreateService();

            var newSettings = new RefreshSettings { Interval = 35, RefreshAfter = 190, StartDelay = 3 };
            await service.UpdateThroughputRefreshSettingsAsync(newSettings);

            VerifyUpdateCalled(AppSettingKeys.ThroughputRefreshInterval, "35");
            VerifyUpdateCalled(AppSettingKeys.ThroughputRefreshAfter, "190");
            VerifyUpdateCalled(AppSettingKeys.ThroughputRefreshStartDelay, "3");
        }

        [Test]
        public void GetDefaultTeamSettings_ReturnsCorrectSettings()
        {
            SetupRepositoryForKeys(
                AppSettingKeys.TeamSettingName, "MyTeam",
                AppSettingKeys.TeamSettingHistory, "90",
                AppSettingKeys.TeamSettingFeatureWIP, "2",
                AppSettingKeys.TeamSettingWorkItemQuery, "[System.TeamProject] = \"MyProject\"",
                AppSettingKeys.TeamSettingWorkItemTypes, "Product Backlog Item, Bug",
                AppSettingKeys.TeamSettingToDoStates, "New,Planned",
                AppSettingKeys.TeamSettingDoingStates, "In Progress,Committed",
                AppSettingKeys.TeamSettingDoneStates, "Closed,Done",
                AppSettingKeys.TeamSettingRelationCustomField, "Custom.RemoteParentID");

            var service = CreateService();

            var settings = service.GetDefaultTeamSettings();

            Assert.Multiple(() =>
            {
                Assert.That(settings.Name, Is.EqualTo("MyTeam"));
                Assert.That(settings.ThroughputHistory, Is.EqualTo(90));
                Assert.That(settings.FeatureWIP, Is.EqualTo(2));
                Assert.That(settings.WorkItemQuery, Is.EqualTo("[System.TeamProject] = \"MyProject\""));
                Assert.That(settings.RelationCustomField, Is.EqualTo("Custom.RemoteParentID"));

                Assert.That(settings.WorkItemTypes, Has.Count.EqualTo(2));
                Assert.That(settings.WorkItemTypes, Does.Contain("Product Backlog Item"));
                Assert.That(settings.WorkItemTypes, Does.Contain("Bug"));

                Assert.That(settings.ToDoStates, Has.Count.EqualTo(2));
                Assert.That(settings.ToDoStates, Does.Contain("New"));
                Assert.That(settings.ToDoStates, Does.Contain("Planned"));

                Assert.That(settings.DoingStates, Has.Count.EqualTo(2));
                Assert.That(settings.DoingStates, Does.Contain("In Progress"));
                Assert.That(settings.DoingStates, Does.Contain("Committed"));

                Assert.That(settings.DoneStates, Has.Count.EqualTo(2));
                Assert.That(settings.DoneStates, Does.Contain("Done"));
                Assert.That(settings.DoneStates, Does.Contain("Closed"));
            });
        }

        [Test]
        public async Task UpdateDefaultTeamSettingsAsync_UpdatesCorrectlyAsync()
        {
            SetupRepositoryForKeys(
                AppSettingKeys.TeamSettingName, "MyTeam",
                AppSettingKeys.TeamSettingHistory, "90",
                AppSettingKeys.TeamSettingFeatureWIP, "2",
                AppSettingKeys.TeamSettingToDoStates, "New,Planned",
                AppSettingKeys.TeamSettingDoingStates, "In Progress,Committed",
                AppSettingKeys.TeamSettingDoneStates, "Closed,Done",
                AppSettingKeys.TeamSettingWorkItemQuery, "[System.TeamProject] = \"MyProject\"",
                AppSettingKeys.TeamSettingWorkItemTypes, "Product Backlog Item, Bug",
                AppSettingKeys.TeamSettingRelationCustomField, "Custom.RemoteParentID");

            var service = CreateService();

            var newSettings = new TeamSettingDto { 
                Name = "Other Team",
                ThroughputHistory = 190,
                FeatureWIP = 3,
                ToDoStates = ["Backlog"],
                DoingStates = ["Ongoing"],
                DoneStates = ["Over"],
                WorkItemQuery = "project = MyJiraProject",
                WorkItemTypes = ["Task", "Spike"],
                RelationCustomField = "CUSTOM_12039213"
            };

            await service.UpdateDefaultTeamSettingsAsync(newSettings);

            VerifyUpdateCalled(AppSettingKeys.TeamSettingName, "Other Team");
            VerifyUpdateCalled(AppSettingKeys.TeamSettingHistory, "190");
            VerifyUpdateCalled(AppSettingKeys.TeamSettingFeatureWIP, "3");
            VerifyUpdateCalled(AppSettingKeys.TeamSettingToDoStates, "Backlog");
            VerifyUpdateCalled(AppSettingKeys.TeamSettingDoingStates, "Ongoing");
            VerifyUpdateCalled(AppSettingKeys.TeamSettingDoneStates, "Over");
            VerifyUpdateCalled(AppSettingKeys.TeamSettingWorkItemQuery, "project = MyJiraProject");
            VerifyUpdateCalled(AppSettingKeys.TeamSettingWorkItemTypes, "Task,Spike");
            VerifyUpdateCalled(AppSettingKeys.TeamSettingRelationCustomField, "CUSTOM_12039213");
        }

        [Test]
        public void GetDefaultProjectSettings_ReturnsCorrectSettings()
        {
            SetupRepositoryForKeys(
                AppSettingKeys.ProjectSettingName, "My Project",
                AppSettingKeys.ProjectSettingWorkItemQuery, "[System.TeamProject] = \"MyProject\"",
                AppSettingKeys.ProjectSettingWorkItemTypes, "Epic",
                AppSettingKeys.ProjectSettingToDoStates, "New,Planned",
                AppSettingKeys.ProjectSettingDoingStates, "In Progress,Committed",
                AppSettingKeys.ProjectSettingDoneStates, "Closed,Done",
                AppSettingKeys.ProjectSettingOverrideRealChildCountStates, "New,Proposed",
                AppSettingKeys.ProjectSettingUnparentedWorkItemQuery, "[System.TeamProject] = \"MyProject\"",
                AppSettingKeys.ProjectSettingUsePercentileToCalculateDefaultAmountOfWorkItems, "True",
                AppSettingKeys.ProjectSettingDefaultAmountOfWorkItemsPerFeature, "15",
                AppSettingKeys.ProjectSettingDefaultWorkItemPercentile, "85",
                AppSettingKeys.ProjectSettingHistoricalFeaturesWorkItemQuery, "[System.TeamProject] = \"MyProject\"",
                AppSettingKeys.ProjectSettingSizeEstimateField, "Microsoft.VSTS.Scheduling.Size");

            var service = CreateService();

            var settings = service.GetDefaultProjectSettings();

            Assert.Multiple(() =>
            {
                Assert.That(settings.Name, Is.EqualTo("My Project"));
                Assert.That(settings.WorkItemQuery, Is.EqualTo("[System.TeamProject] = \"MyProject\""));
                Assert.That(settings.UnparentedItemsQuery, Is.EqualTo("[System.TeamProject] = \"MyProject\""));
                Assert.That(settings.UsePercentileToCalculateDefaultAmountOfWorkItems, Is.True);
                Assert.That(settings.DefaultAmountOfWorkItemsPerFeature, Is.EqualTo(15));
                Assert.That(settings.DefaultWorkItemPercentile, Is.EqualTo(85));
                Assert.That(settings.HistoricalFeaturesWorkItemQuery, Is.EqualTo("[System.TeamProject] = \"MyProject\""));
                Assert.That(settings.SizeEstimateField, Is.EqualTo("Microsoft.VSTS.Scheduling.Size"));

                Assert.That(settings.WorkItemTypes, Has.Count.EqualTo(1));
                Assert.That(settings.WorkItemTypes, Does.Contain("Epic"));

                Assert.That(settings.ToDoStates, Has.Count.EqualTo(2));
                Assert.That(settings.ToDoStates, Does.Contain("New"));
                Assert.That(settings.ToDoStates, Does.Contain("Planned"));

                Assert.That(settings.DoingStates, Has.Count.EqualTo(2));
                Assert.That(settings.DoingStates, Does.Contain("In Progress"));
                Assert.That(settings.DoingStates, Does.Contain("Committed"));

                Assert.That(settings.DoneStates, Has.Count.EqualTo(2));
                Assert.That(settings.DoneStates, Does.Contain("Done"));
                Assert.That(settings.DoneStates, Does.Contain("Closed"));

                Assert.That(settings.OverrideRealChildCountStates, Has.Count.EqualTo(2));
                Assert.That(settings.OverrideRealChildCountStates, Does.Contain("New"));
                Assert.That(settings.OverrideRealChildCountStates, Does.Contain("Proposed"));
            });
        }

        [Test]
        public async Task UpdateDefaultProjectSettingsAsync_UpdatesCorrectlyAsync()
        {
            SetupRepositoryForKeys(
                AppSettingKeys.ProjectSettingName, "My Project",
                AppSettingKeys.ProjectSettingWorkItemQuery, "[System.TeamProject] = \"MyProject\"",
                AppSettingKeys.ProjectSettingWorkItemTypes, "Epic",
                AppSettingKeys.ProjectSettingToDoStates, "New,Planned",
                AppSettingKeys.ProjectSettingDoingStates, "In Progress,Committed",
                AppSettingKeys.ProjectSettingDoneStates, "Closed,Done",
                AppSettingKeys.ProjectSettingOverrideRealChildCountStates, "New,Proposed",
                AppSettingKeys.ProjectSettingUnparentedWorkItemQuery, "[System.TeamProject] = \"MyProject\"",
                AppSettingKeys.ProjectSettingUsePercentileToCalculateDefaultAmountOfWorkItems, "false",
                AppSettingKeys.ProjectSettingDefaultAmountOfWorkItemsPerFeature, "10",
                AppSettingKeys.ProjectSettingDefaultWorkItemPercentile, "85",
                AppSettingKeys.ProjectSettingHistoricalFeaturesWorkItemQuery, "[System.TeamProject] = \"MyProject\"",
                AppSettingKeys.ProjectSettingSizeEstimateField, "Microsoft.VSTS.Scheduling.Size");

            var service = CreateService();

            var newSettings = new ProjectSettingDto { 
                Name = "Other Project",
                WorkItemQuery = "project = MyJiraProject",
                WorkItemTypes = ["Feature"],
                ToDoStates = ["Backlog"],
                DoingStates = ["Ongoing"],
                DoneStates = ["Over"],
                OverrideRealChildCountStates = ["Backlog,Proposed"],
                UnparentedItemsQuery = "project = MyJiraProject",
                UsePercentileToCalculateDefaultAmountOfWorkItems = true,
                DefaultAmountOfWorkItemsPerFeature = 22,
                DefaultWorkItemPercentile = 75,
                HistoricalFeaturesWorkItemQuery = "project = MyJiraProject",
                SizeEstimateField = "customfield_10037"
            };

            await service.UpdateDefaultProjectSettingsAsync(newSettings);

            VerifyUpdateCalled(AppSettingKeys.ProjectSettingName, "Other Project");
            VerifyUpdateCalled(AppSettingKeys.ProjectSettingWorkItemQuery, "project = MyJiraProject");
            VerifyUpdateCalled(AppSettingKeys.ProjectSettingWorkItemTypes, "Feature");
            VerifyUpdateCalled(AppSettingKeys.ProjectSettingToDoStates, "Backlog");
            VerifyUpdateCalled(AppSettingKeys.ProjectSettingDoingStates, "Ongoing");
            VerifyUpdateCalled(AppSettingKeys.ProjectSettingDoneStates, "Over");
            VerifyUpdateCalled(AppSettingKeys.ProjectSettingOverrideRealChildCountStates, "Backlog,Proposed");
            VerifyUpdateCalled(AppSettingKeys.ProjectSettingUnparentedWorkItemQuery, "project = MyJiraProject");
            VerifyUpdateCalled(AppSettingKeys.ProjectSettingUsePercentileToCalculateDefaultAmountOfWorkItems, "True");
            VerifyUpdateCalled(AppSettingKeys.ProjectSettingDefaultAmountOfWorkItemsPerFeature, "22");
            VerifyUpdateCalled(AppSettingKeys.ProjectSettingDefaultWorkItemPercentile, "75");
            VerifyUpdateCalled(AppSettingKeys.ProjectSettingHistoricalFeaturesWorkItemQuery, "project = MyJiraProject");
            VerifyUpdateCalled(AppSettingKeys.ProjectSettingSizeEstimateField, "customfield_10037");
        }

        [Test]
        public void GetSettingByKey_KeyDoesNotExist_ThrowsException()
        {
            repositoryMock.Setup(x => x.GetByPredicate(It.IsAny<Func<AppSetting, bool>>())).Returns((AppSetting)null);

            var service = CreateService();

            Assert.Throws<ArgumentNullException>(() => service.GetFeaturRefreshSettings());
        }

        [Test]
        public void GetCleanUpDataHistorySettings_ReturnsCorrectSettings()
        {
            SetupRepositoryForKeys(AppSettingKeys.CleanUpDataHistorySettingsMaxStorageTimeInDays, "42");

            var service = CreateService();

            var settings = service.GetCleanUpDataHistorySettings();

            Assert.That(settings.MaxStorageTimeInDays, Is.EqualTo(42));
        }

        [Test]
        public async Task UpdateCleanUpDataHistorySettings_UpdatesCorrectlyAsync()
        {
            SetupRepositoryForKeys(AppSettingKeys.CleanUpDataHistorySettingsMaxStorageTimeInDays, "60");

            var service = CreateService();

            var cleanUpDataHistorySettings = new CleanUpDataHistorySettings { MaxStorageTimeInDays = 42 };
            await service.UpdateCleanUpDataHistorySetting(cleanUpDataHistorySettings);

            VerifyUpdateCalled(AppSettingKeys.CleanUpDataHistorySettingsMaxStorageTimeInDays, "42");
        }

        private AppSettingService CreateService()
        {
            return new AppSettingService(repositoryMock.Object);
        }

        private void SetupRepositoryForKeys(params string[] keyValuePairs)
        {
            for (int i = 0; i < keyValuePairs.Length; i += 2)
            {
                var key = keyValuePairs[i];
                var value = keyValuePairs[i + 1];
                repositoryMock.Setup(x => x.GetByPredicate(It.Is<Func<AppSetting, bool>>(predicate => predicate(new AppSetting { Key = key })))).Returns(new AppSetting { Key = key, Value = value });
            }
        }

        private void VerifyUpdateCalled(string key, string value)
        {
            repositoryMock.Verify(x => x.Update(It.Is<AppSetting>(s => s.Key == key && s.Value == value)), Times.Once);
        }
    }
}