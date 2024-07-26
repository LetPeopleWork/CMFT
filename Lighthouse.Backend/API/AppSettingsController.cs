﻿using Lighthouse.Backend.Models.AppSettings;
using Lighthouse.Backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Lighthouse.Backend.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppSettingsController : ControllerBase
    {
        private readonly IAppSettingService appSettingService;

        public AppSettingsController(IAppSettingService appSettingService)
        {
            this.appSettingService = appSettingService;
        }

        [HttpGet("FeatureRefresh")]
        public ActionResult<RefreshSettings> GetFeatureRefreshSettings()
        {
            var settings = appSettingService.GetFeaturRefreshSettings();
            return Ok(settings);
        }

        [HttpPut("FeatureRefresh")]
        public async Task<ActionResult> UpdateFeatureRefreshSettingsAsync(RefreshSettings refreshSettings)
        {
            await appSettingService.UpdateFeatureRefreshSettingsAsync(refreshSettings);
            return Ok();
        }

        [HttpGet("ThroughputRefresh")]
        public ActionResult<RefreshSettings> GetThroughputRefreshSettings()
        {
            var settings = appSettingService.GetThroughputRefreshSettings();
            return Ok(settings);
        }

        [HttpPut("ThroughputRefresh")]
        public async Task<ActionResult> UpdateThroughputRefreshSettingsAsync(RefreshSettings refreshSettings)
        {
            await appSettingService.UpdateThroughputRefreshSettingsAsync(refreshSettings);
            return Ok();
        }

        [HttpGet("ForecastRefresh")]
        public ActionResult<RefreshSettings> GetForecastRefreshSettings()
        {
            var settings = appSettingService.GetForecastRefreshSettings();
            return Ok(settings);
        }

        [HttpPut("ForecastRefresh")]
        public async Task<ActionResult> UpdateForecastRefreshSettingsAsync(RefreshSettings refreshSettings)
        {
            await appSettingService.UpdateForecastRefreshSettingsAsync(refreshSettings);
            return Ok();
        }
    }
}
