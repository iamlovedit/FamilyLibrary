﻿namespace GalaFamilyLibrary.EventBus.IntegrationEventLog
{
    public enum EventStateEnum
    {
        NotPublished = 0,
        InProgress = 1,
        Published = 2,
        PublishedFailed = 3
    }
}