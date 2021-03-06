﻿using System;

namespace AggregateEvents.Model
{
    public class Task : Entity
    {
        public Task(string name, 
            int hoursRemaining,
            Guid projectId)
        {
            Name = name;
            HoursRemaining = hoursRemaining;
            ProjectId = projectId;
        }
        private Task()
        {
        }
        public Guid ProjectId { get; }
        public string Name { get; private set; }
        public bool IsComplete { get; private set; }
        public int HoursRemaining { get; private set; }
        
        public void MarkComplete()
        {
            if (IsComplete) return;
            IsComplete = true;
            HoursRemaining = 0;
            AggregateEvents.Raise(new TaskCompletedEvent(this));
        }

        public void UpdateHoursRemaining(int hours)
        {
            if (hours < 0) return;
            int currentHoursRemaining = HoursRemaining;
            try
            {
                HoursRemaining = hours;
                if (HoursRemaining == 0)
                {
                    MarkComplete();
                    return;
                }
                IsComplete = false;
                AggregateEvents.Raise(new TaskHoursUpdatedEvent(this));
            }
            catch (Exception)
            {
                HoursRemaining = currentHoursRemaining;
            }
        }
    }
}