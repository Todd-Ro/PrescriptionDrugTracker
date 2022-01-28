using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrescriptionTracker.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PrescriptionSet",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DrugName = table.Column<string>(nullable: true),
                    Tier = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrescriptionSet", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrescriptionSet");
        }
    }
}
