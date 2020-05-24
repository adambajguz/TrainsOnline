using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainsOnline.Persistence.Migrations
{
    public partial class EntityAuditLogIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_EntityAuditLogs_Key",
                table: "EntityAuditLogs",
                column: "Key");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EntityAuditLogs_Key",
                table: "EntityAuditLogs");
        }
    }
}
