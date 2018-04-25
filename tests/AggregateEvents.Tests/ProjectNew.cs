using AggregateEvents.Model;
using System;
using System.Linq;
using Xunit;

namespace AggregateEvents.Tests
{
    public class ProjectNew
    {
        [Fact]
        public void HasStatusNew()
        {
            var project = new Project();

            Assert.Equal("New", project.Status);
        }

        [Fact]
        public void HasZeroTasks()
        {
            var project = new Project();

            Assert.Equal(0, project.Tasks.Count());
        }

        [Fact]
        public void HandleOwnTaskCompletedEventOnly()
        {
            AggregateEvents.ClearCallbacks();
            var project = new Project();
            string taskName = Guid.NewGuid().ToString();
            project.AddTask(taskName, 1);

            // project 2 has no tasks assigned to it.
            var project2 = new Project();

            AggregateEvents.Raise(new TaskCompletedEvent(project.Tasks.First()));

            Assert.True(project.ToString().Contains(taskName));
            Assert.False(project2.ToString().Contains(taskName));
        }

        [Fact]
        public void HandleOwnTaskHoursUpdatedEventOnly()
        {
            AggregateEvents.ClearCallbacks();
            var project = new Project();
            string taskName = Guid.NewGuid().ToString();
            project.AddTask(taskName, 1);

            // project 2 has no tasks assigned to it.
            var project2 = new Project();

            project.Tasks.First().UpdateHoursRemaining(2);

            Assert.True(project.ToString().Contains(taskName));
            Assert.False(project2.ToString().Contains(taskName));
        }
    }
}
