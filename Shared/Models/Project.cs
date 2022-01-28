using System;
using System.ComponentModel.DataAnnotations.Schema;
using Oqtane.Models;

namespace DNF.Projects.Models
{
    [Table("DNFProject")]
    public class Project : IAuditable
    {
        public int ProjectId { get; set; }
        public int ModuleId { get; set; }
        public int SiteId { get; set; }
        public string Url { get; set; }
        public string Category { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

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
        public int Issues { get; set; }
        [NotMapped]
        public int PullRequests { get; set; }

    }
}
