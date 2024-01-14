using System;

namespace RunPlanner.Models
{
    public class GpxTrackViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string GpxData { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow; // Set default value to current UTC time
    }
}
