using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using DNF.Projects.Migrations.EntityBuilders;
using DNF.Projects.Repository;

namespace DNF.Projects.Migrations
{
    [DbContext(typeof(ProjectContext))]
    [Migration("DNF.Projects.01.00.07.00")]
    public class AddPackageDownloads : MultiDatabaseMigration
    {
        public AddPackageDownloads(IDatabase database) : base(database)
        {
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var projectEntityBuilder = new ProjectEntityBuilder(migrationBuilder, ActiveDatabase);
            projectEntityBuilder.AddStringColumn("Package", 50, true);
            projectEntityBuilder.UpdateColumn("Package", "''");

            var projectActivityEntityBuilder = new ProjectActivityEntityBuilder(migrationBuilder, ActiveDatabase);
            projectActivityEntityBuilder.AddIntegerColumn("Downloads", true);
            projectActivityEntityBuilder.UpdateColumn("Downloads", "0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}