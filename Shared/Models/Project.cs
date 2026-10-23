using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Oqtane.Models;

namespace DNF.Projects.Models
{
    [Table("DNFProject")]
    public class Project : ModelBase
    {
        public int ProjectId { get; set; }
        public int ModuleId { get; set; }
        public int SiteId { get; set; }
		public string Url { get; set; }
        public string Package { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public bool IsActive { get; set; }

        [NotMapped]
        public DateTime? Date { get; set; }
        [NotMapped]
        public int Watchers { get; set; }
        [NotMapped]
        public int Stars { get; set; }
        [NotMapped]
        public int Forks { get; set; }
        [NotMapped]
        public int Contributors { get; set; }
        [NotMapped]
        public int Commits { get; set; }
        [NotMapped]
        public int ClosedIssues { get; set; }
        [NotMapped]
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
        [NotMapped]
        public int PullRequests { get; set; }
        [NotMapped]
        public int Downloads { get; set; }

    }
}
