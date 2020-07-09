using System;
using System.ComponentModel.DataAnnotations.Schema;

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
        public int Issues { get; set; }
        public int PullRequests { get; set; }

        public Project Project { get; set; }
    }
}
