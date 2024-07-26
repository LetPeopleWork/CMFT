﻿using Lighthouse.Backend.Models.AppSettings;

namespace Lighthouse.Backend.Services.Interfaces
{
    public interface IAppSettingService
    {
        RefreshSettings GetThroughputRefreshSettings();

        Task UpdateThroughputRefreshSettingsAsync(RefreshSettings refreshSettings);
        
        RefreshSettings GetFeaturRefreshSettings();

        Task UpdateFeatureRefreshSettingsAsync(RefreshSettings refreshSettings);

        RefreshSettings GetForecastRefreshSettings();

        Task UpdateForecastRefreshSettingsAsync(RefreshSettings refreshSettings);

    }
}
