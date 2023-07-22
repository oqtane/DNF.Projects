using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using Oqtane.Migrations.EntityBuilders;

namespace DNF.Projects.Migrations.EntityBuilders
{
    public class ProjectEntityBuilder : AuditableBaseEntityBuilder<ProjectEntityBuilder>
    {
        private const string _entityTableName = "DNFProject";
        private readonly PrimaryKey<ProjectEntityBuilder> _primaryKey = new("PK_DNFProject", x => x.ProjectId);
        private readonly ForeignKey<ProjectEntityBuilder> _foreignKey = new("FK_DNFProject_Module", x => x.ModuleId, "Module", "ModuleId", ReferentialAction.Cascade);

        public ProjectEntityBuilder(MigrationBuilder migrationBuilder, IDatabase database) : base(migrationBuilder, database)
        {
            EntityTableName = _entityTableName;
            PrimaryKey = _primaryKey;
            ForeignKeys.Add(_foreignKey);
        }

        protected override ProjectEntityBuilder BuildTable(ColumnsBuilder table)
        {
            ProjectId = AddAutoIncrementColumn(table, "ProjectId");
            ModuleId = AddIntegerColumn(table, "ModuleId");
            SiteId = AddIntegerColumn(table, "SiteId");
            Url = AddStringColumn(table, "Url", 256);
            Title = AddStringColumn(table, "Title", 50);
            Description = AddStringColumn(table, "Description", 500);
            Category = AddStringColumn(table, "Category", 50);
            IsActive = AddBooleanColumn(table, "IsActive");
            AddAuditableColumns(table);
            return this;
        }

        public OperationBuilder<AddColumnOperation> ProjectId { get; set; }
        public OperationBuilder<AddColumnOperation> ModuleId { get; set; }
        public OperationBuilder<AddColumnOperation> SiteId { get; set; }
        public OperationBuilder<AddColumnOperation> Url { get; set; }
        public OperationBuilder<AddColumnOperation> Title { get; set; }
        public OperationBuilder<AddColumnOperation> Description { get; set; }
        public OperationBuilder<AddColumnOperation> Category { get; set; }
        public OperationBuilder<AddColumnOperation> IsActive { get; set; }
    }
}