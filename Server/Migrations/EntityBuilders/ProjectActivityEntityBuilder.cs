using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using Oqtane.Migrations.EntityBuilders;

namespace DNF.Projects.Migrations.EntityBuilders
{
    public class ProjectActivityEntityBuilder : BaseEntityBuilder<ProjectActivityEntityBuilder>
    {
        private const string _entityTableName = "DNFProjectActivity";
        private readonly PrimaryKey<ProjectActivityEntityBuilder> _primaryKey = new("PK_DNFProjectActivity", x => x.ProjectActivityId);
        private readonly ForeignKey<ProjectActivityEntityBuilder> _foreignKey = new("FK_DNFProjectActivity_DNFProject", x => x.ProjectId, "DNFProject", "ProjectId", ReferentialAction.Cascade);

        public ProjectActivityEntityBuilder(MigrationBuilder migrationBuilder, IDatabase database) : base(migrationBuilder, database)
        {
            EntityTableName = _entityTableName;
            PrimaryKey = _primaryKey;
            ForeignKeys.Add(_foreignKey);
        }

        protected override ProjectActivityEntityBuilder BuildTable(ColumnsBuilder table)
        {
            ProjectActivityId = AddAutoIncrementColumn(table, "ProjectActivityId");
            ProjectId = AddIntegerColumn(table, "ProjectId");
            Date = AddDateTimeColumn(table, "Date");
            Watchers = AddIntegerColumn(table, "Watchers");
            Stars = AddIntegerColumn(table, "Stars");
            Forks = AddIntegerColumn(table, "Forks");
            Contributors = AddIntegerColumn(table, "Contributors");
            Commits = AddIntegerColumn(table, "Commits");
            Issues = AddIntegerColumn(table, "Issues");
            PullRequests = AddIntegerColumn(table, "PullRequests");
            return this;
        }

        public OperationBuilder<AddColumnOperation> ProjectActivityId { get; set; }
        public OperationBuilder<AddColumnOperation> ProjectId { get; set; }
        public OperationBuilder<AddColumnOperation> Date { get; set; }
        public OperationBuilder<AddColumnOperation> Watchers { get; set; }
        public OperationBuilder<AddColumnOperation> Stars { get; set; }
        public OperationBuilder<AddColumnOperation> Forks { get; set; }
        public OperationBuilder<AddColumnOperation> Contributors { get; set; }
        public OperationBuilder<AddColumnOperation> Commits { get; set; }
        public OperationBuilder<AddColumnOperation> Issues { get; set; }
        public OperationBuilder<AddColumnOperation> PullRequests { get; set; }
    }
}