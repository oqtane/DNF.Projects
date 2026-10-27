using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DNF.Projects.Models
{
    [Table("DNFProjectActivity")]
    public class ProjectActivity 
    {
        public int ProjectActivityId { get; set; }
        public int ProjectId { get; set; }
        public DateTime Date { get; set; }
        public int Watchers { get; set; }
        public int Stars { get; set; }
        public int Forks { get; set; }
        public int Contributors { get; set; }
        public int Commits { get; set; }
        public int ClosedIssues { get; set; }
        public int? OpenIssues { get; set; }
        // Accept legacy snapshots without emitting the ambiguous Issues name.
        [NotMapped]
        [JsonPropertyName("Issues")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? LegacyIssues
        {
            get => null;
            set
            {
                if (value.HasValue)
                {
                    ClosedIssues = value.Value;
                }
            }
        }
        public int PullRequests { get; set; }
        public int Downloads { get; set; }

        public Project Project { get; set; }
    }
}
