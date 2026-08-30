using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations;

/// <inheritdoc />
public partial class finalize_audit_migration : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateIndex(
            name: "ix_audits_correlation_id",
            schema: "public",
            table: "audits",
            column: "correlation_id");

        migrationBuilder.CreateIndex(
            name: "ix_audits_entity_name_entity_id",
            schema: "public",
            table: "audits",
            columns: ["entity_name", "entity_id"]);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "ix_audits_correlation_id",
            schema: "public",
            table: "audits");

        migrationBuilder.DropIndex(
            name: "ix_audits_entity_name_entity_id",
            schema: "public",
            table: "audits");
    }
}
