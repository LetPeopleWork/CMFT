import { IDataRetentionSettings } from '../../models/AppSettings/DataRetentionSettings';
import { IRefreshSettings } from '../../models/AppSettings/RefreshSettings';
import { IProjectSettings } from '../../models/Project/ProjectSettings';
import { ITeamSettings } from '../../models/Team/TeamSettings';
import { BaseApiService } from './BaseApiService';

export interface ISettingsService {
    getRefreshSettings(settingName: string): Promise<IRefreshSettings>;
    updateRefreshSettings(settingName: string, refreshSettings: IRefreshSettings): Promise<void>;
    getDefaultTeamSettings(): Promise<ITeamSettings>;
    updateDefaultTeamSettings(teamSettings: ITeamSettings): Promise<void>;
    getDefaultProjectSettings(): Promise<IProjectSettings>;
    updateDefaultProjectSettings(projecSettings: IProjectSettings): Promise<void>;
    getDataRetentionSettings(): Promise<IDataRetentionSettings>;
    updateDataRetentionSettings(dataRetentionSettings : IDataRetentionSettings): Promise<void>
}

export class SettingsService extends BaseApiService implements ISettingsService {

    async getRefreshSettings(settingName: string): Promise<IRefreshSettings> {
        return this.withErrorHandling(async () => {
            const response = await this.apiService.get<IRefreshSettings>(`/appsettings/${settingName}Refresh`);

            return response.data;
        });
    }

    async updateRefreshSettings(settingName: string, refreshSettings: IRefreshSettings): Promise<void> {
        this.withErrorHandling(async () => {
            await this.apiService.put<IRefreshSettings>(`/appsettings/${settingName}Refresh`, refreshSettings);
        });
    }

    async getDefaultTeamSettings(): Promise<ITeamSettings> {
        return this.withErrorHandling(async () => {
            const response = await this.apiService.get<ITeamSettings>(`/appsettings/defaultteamsettings`);

            return response.data;
        });
    }

    async updateDefaultTeamSettings(teamSettings: ITeamSettings): Promise<void> {
        this.withErrorHandling(async () => {
            await this.apiService.put<ITeamSettings>(`/appsettings/defaultteamsettings`, teamSettings);
        });
    }
    

    async getDefaultProjectSettings(): Promise<IProjectSettings> {
        return this.withErrorHandling(async () => {
            const response = await this.apiService.get<IProjectSettings>(`/appsettings/defaultprojectsettings`);

            return this.deserializeProjectSettings(response.data);
        });
    }

    async updateDefaultProjectSettings(projecSettings: IProjectSettings): Promise<void> {
        this.withErrorHandling(async () => {
            await this.apiService.put<IProjectSettings>(`/appsettings/defaultprojectsettings`, projecSettings);
        });
    }

    async getDataRetentionSettings(): Promise<IDataRetentionSettings> {
        return this.withErrorHandling(async () => {
            const response = await this.apiService.get<IDataRetentionSettings>(`/appsettings/dataRetentionSettings`);

            return response.data;
        });
    }

    async updateDataRetentionSettings(dataRetentionSettings: IDataRetentionSettings): Promise<void> {
        this.withErrorHandling(async () => {
            await this.apiService.put<IDataRetentionSettings>(`/appsettings/dataRetentionSettings`, dataRetentionSettings);
        });
    }
}