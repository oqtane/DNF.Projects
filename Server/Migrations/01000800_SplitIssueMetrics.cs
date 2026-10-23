using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using DNF.Projects.Migrations.EntityBuilders;
using DNF.Projects.Repository;

namespace DNF.Projects.Migrations
{
    [DbContext(typeof(ProjectContext))]
    [Migration("DNF.Projects.01.00.08.00")]
    public class SplitIssueMetrics : MultiDatabaseMigration
    {
        public SplitIssueMetrics(IDatabase database) : base(database)
        {
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn("Issues", "DNFProjectActivity", "ClosedIssues");
            var projectActivityEntityBuilder = new ProjectActivityEntityBuilder(migrationBuilder, ActiveDatabase);
            projectActivityEntityBuilder.AddIntegerColumn("OpenIssues", true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("OpenIssues", "DNFProjectActivity");
            migrationBuilder.RenameColumn("ClosedIssues", "DNFProjectActivity", "Issues");
        }
    }
}
