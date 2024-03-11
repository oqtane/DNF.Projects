using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using DNF.Projects.Migrations.EntityBuilders;
using DNF.Projects.Repository;

namespace DNF.Projects.Migrations
{
    [DbContext(typeof(ProjectContext))]
    [Migration("DNF.Projects.01.00.00.00")]
    public class InitializeModule : MultiDatabaseMigration
    {
        public InitializeModule(IDatabase database) : base(database)
        {
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var projectEntityBuilder = new ProjectEntityBuilder(migrationBuilder, ActiveDatabase);
            projectEntityBuilder.Create();

            var projectActivityEntityBuilder = new ProjectActivityEntityBuilder(migrationBuilder, ActiveDatabase);
            projectActivityEntityBuilder.Create();
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var projectActivityEntityBuilder = new ProjectActivityEntityBuilder(migrationBuilder, ActiveDatabase);
            projectActivityEntityBuilder.Drop();

            var projectEntityBuilder = new ProjectEntityBuilder(migrationBuilder, ActiveDatabase);
            projectEntityBuilder.Drop();
        }
    }
}