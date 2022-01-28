using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrescriptionTracker.Migrations
{
    public partial class MigrationWithProperRelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrescriptionSet_Drug_TheDrugPrescribedId",
                table: "PrescriptionSet");

            migrationBuilder.DropTable(
                name: "Drug");

            migrationBuilder.CreateTable(
                name: "DrugSet",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DrugName = table.Column<string>(nullable: true),
                    Tier = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugSet", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_PrescriptionSet_DrugSet_TheDrugPrescribedId",
                table: "PrescriptionSet",
                column: "TheDrugPrescribedId",
                principalTable: "DrugSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrescriptionSet_DrugSet_TheDrugPrescribedId",
                table: "PrescriptionSet");

            migrationBuilder.DropTable(
                name: "DrugSet");

            migrationBuilder.CreateTable(
                name: "Drug",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DrugName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Tier = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drug", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_PrescriptionSet_Drug_TheDrugPrescribedId",
                table: "PrescriptionSet",
                column: "TheDrugPrescribedId",
                principalTable: "Drug",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
