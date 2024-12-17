import axios from 'axios';
import { describe, it, expect, beforeEach, afterEach, vi } from 'vitest';
import { SettingsService } from './SettingsService';
import { IRefreshSettings, RefreshSettings } from '../../models/AppSettings/RefreshSettings';
import { ITeamSettings } from '../../models/Team/TeamSettings';
import { IProjectSettings } from '../../models/Project/ProjectSettings';
import { IDataRetentionSettings } from '../../models/AppSettings/DataRetentionSettings';

vi.mock('axios');
const mockedAxios = vi.mocked(axios, true);

describe('SettingsService', () => {
    let settingsService: SettingsService;

    beforeEach(() => {
        mockedAxios.create.mockReturnThis();
        settingsService = new SettingsService();
    });

    afterEach(() => {
        vi.resetAllMocks();
    });

    it('should get refresh settings', async () => {
        const mockResponse: IRefreshSettings = new RefreshSettings(20, 20, 20);
        mockedAxios.get.mockResolvedValueOnce({ data: mockResponse });

        const settingName = 'exampleSetting';
        const refreshSettings = await settingsService.getRefreshSettings(settingName);

        expect(refreshSettings).toEqual(mockResponse);
        expect(mockedAxios.get).toHaveBeenCalledWith(`/appsettings/${settingName}Refresh`);
    });

    it('should update refresh settings', async () => {
        const mockRefreshSettings: IRefreshSettings = new RefreshSettings(20, 20, 20);
        mockedAxios.put.mockResolvedValueOnce({});

        const settingName = 'exampleSetting';
        await settingsService.updateRefreshSettings(settingName, mockRefreshSettings);

        expect(mockedAxios.put).toHaveBeenCalledWith(`/appsettings/${settingName}Refresh`, mockRefreshSettings);
    });

    it('should get default team settings', async () => {
        const mockResponse: ITeamSettings = {
            id: 1,
            name: "Team 1",
            throughputHistory: 30,
            featureWIP: 1,
            workItemQuery: "Query",
            workItemTypes: ["Epic"],
            workTrackingSystemConnectionId: 12,
            relationCustomField: "",
            toDoStates: ["New"],
            doingStates: ["Active"],
            doneStates: ["Done"]
        };        
        mockedAxios.get.mockResolvedValueOnce({ data: mockResponse });

        const teamSettings = await settingsService.getDefaultTeamSettings();

        expect(teamSettings).toEqual(mockResponse);
        expect(mockedAxios.get).toHaveBeenCalledWith(`/appsettings/defaultteamsettings`);
    });

    it('should update default team settings', async () => {
        const mockTeamSettings: ITeamSettings = {
            id: 1,
            name: "Team 1",
            throughputHistory: 30,
            featureWIP: 1,
            workItemQuery: "Query",
            workItemTypes: ["User Story", "Bug"],
            workTrackingSystemConnectionId: 12,
            relationCustomField: "",
            toDoStates: ["New"],
            doingStates: ["Active"],
            doneStates: ["Done"]
        };        
        mockedAxios.put.mockResolvedValueOnce({});

        await settingsService.updateDefaultTeamSettings(mockTeamSettings);

        expect(mockedAxios.put).toHaveBeenCalledWith(`/appsettings/defaultteamsettings`, mockTeamSettings);
    });

    it('should get default project settings', async () => {
        const mockResponse: IProjectSettings = {
            id: 1,
            name: "Project A",
            workItemTypes: ["Epic"],
            milestones: [],
            workItemQuery: "Query",
            unparentedItemsQuery: "Unparented Query",
            usePercentileToCalculateDefaultAmountOfWorkItems: false,
            defaultAmountOfWorkItemsPerFeature: 15,
            defaultWorkItemPercentile: 85,
            historicalFeaturesWorkItemQuery: "",
            workTrackingSystemConnectionId: 2,
            sizeEstimateField: "EstimatedSize",
            toDoStates: ["New"],
            doingStates: ["Active"],
            doneStates: ["Done"],
            overrideRealChildCountStates: [""],
            involvedTeams: [],
        };

        mockedAxios.get.mockResolvedValueOnce({ data: mockResponse });

        const projectSettings = await settingsService.getDefaultProjectSettings();

        expect(projectSettings).toEqual(mockResponse);
        expect(mockedAxios.get).toHaveBeenCalledWith(`/appsettings/defaultprojectsettings`);
    });

    it('should update default project settings', async () => {
        const mockProjectSettings: IProjectSettings = {
            id: 1,
            name: "Project A",
            workItemTypes: ["Epic"],
            milestones: [],
            workItemQuery: "Query",
            unparentedItemsQuery: "Unparented Query",
            usePercentileToCalculateDefaultAmountOfWorkItems: false,
            defaultAmountOfWorkItemsPerFeature: 15,
            defaultWorkItemPercentile: 85,
            historicalFeaturesWorkItemQuery: "",
            workTrackingSystemConnectionId: 2,
            sizeEstimateField: "EstimatedSize",
            toDoStates: ["New"],
            doingStates: ["Active"],
            doneStates: ["Done"],
            overrideRealChildCountStates: [""],
            involvedTeams: [],
        };

        mockedAxios.put.mockResolvedValueOnce({});

        await settingsService.updateDefaultProjectSettings(mockProjectSettings);

        expect(mockedAxios.put).toHaveBeenCalledWith(`/appsettings/defaultprojectsettings`, mockProjectSettings);
    });

    it('should get data retention settings', async () => {
        const mockResponse: IDataRetentionSettings = {
            maxStorageTimeInDays: 30
        };

        mockedAxios.get.mockResolvedValueOnce({ data: mockResponse });

        const dataRetentionSettings = await settingsService.getDataRetentionSettings();

        expect(dataRetentionSettings).toEqual(mockResponse);
        expect(mockedAxios.get).toHaveBeenCalledWith('/appsettings/dataRetentionSettings');
    });

    it('should update data retention settings', async () => {
        const mockDataRetentionSettings: IDataRetentionSettings = {
            maxStorageTimeInDays: 45
        };

        mockedAxios.put.mockResolvedValueOnce({});

        await settingsService.updateDataRetentionSettings(mockDataRetentionSettings);

        expect(mockedAxios.put).toHaveBeenCalledWith(
            '/appsettings/dataRetentionSettings',
            mockDataRetentionSettings
        );
    });
});