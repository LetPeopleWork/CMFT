﻿using Lighthouse.Backend.Models;

namespace Lighthouse.Backend.API.DTO
{
    public class WorkTrackingSystemConnectionOptionDto
    {
        public WorkTrackingSystemConnectionOptionDto()
        {            
        }

        public WorkTrackingSystemConnectionOptionDto(WorkTrackingSystemConnectionOption option)
        {
            Key = option.Key;
            IsSecret = option.IsSecret;
            Value = IsSecret ? string.Empty : option.Value;
        }

        public string Key { get; set; }

        public string Value { get; set; }

        public bool IsSecret { get; set; }
    }
}
