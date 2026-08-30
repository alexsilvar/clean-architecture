using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable IDE0161

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Add_Auditing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "audits",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    correlation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    action = table.Column<int>(type: "integer", nullable: false),
                    entity_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    user_id = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    date_time_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    entity_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_audits", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "affected_column",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    column = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    audit_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_affected_column", x => x.id);
                    table.ForeignKey(
                        name: "fk_affected_column_audits_audit_id",
                        column: x => x.audit_id,
                        principalSchema: "public",
                        principalTable: "audits",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "audit_entry",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    key = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    value = table.Column<string>(type: "text", nullable: false),
                    audit_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_audit_entry", x => x.id);
                    table.ForeignKey(
                        name: "fk_audit_entry_audits_audit_id",
                        column: x => x.audit_id,
                        principalSchema: "public",
                        principalTable: "audits",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_affected_column_audit_id",
                schema: "public",
                table: "affected_column",
                column: "audit_id");

            migrationBuilder.CreateIndex(
                name: "ix_audit_entry_audit_id",
                schema: "public",
                table: "audit_entry",
                column: "audit_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "affected_column",
                schema: "public");

            migrationBuilder.DropTable(
                name: "audit_entry",
                schema: "public");

            migrationBuilder.DropTable(
                name: "audits",
                schema: "public");
        }
    }
}
