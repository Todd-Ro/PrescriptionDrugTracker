using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrescriptionTracker.Migrations
{
    public partial class MigrationWithPrescriptionRelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DrugId",
                table: "PrescriptionSet");

            migrationBuilder.AddColumn<string>(
                name: "PatientId",
                table: "PrescriptionSet",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TheDrugPrescribedId",
                table: "PrescriptionSet",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Drug",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DrugName = table.Column<string>(nullable: true),
                    Tier = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drug", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionSet_PatientId",
                table: "PrescriptionSet",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionSet_TheDrugPrescribedId",
                table: "PrescriptionSet",
                column: "TheDrugPrescribedId");

            migrationBuilder.AddForeignKey(
                name: "FK_PrescriptionSet_AspNetUsers_PatientId",
                table: "PrescriptionSet",
                column: "PatientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PrescriptionSet_Drug_TheDrugPrescribedId",
                table: "PrescriptionSet",
                column: "TheDrugPrescribedId",
                principalTable: "Drug",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrescriptionSet_AspNetUsers_PatientId",
                table: "PrescriptionSet");

            migrationBuilder.DropForeignKey(
                name: "FK_PrescriptionSet_Drug_TheDrugPrescribedId",
                table: "PrescriptionSet");

            migrationBuilder.DropTable(
                name: "Drug");

            migrationBuilder.DropIndex(
                name: "IX_PrescriptionSet_PatientId",
                table: "PrescriptionSet");

            migrationBuilder.DropIndex(
                name: "IX_PrescriptionSet_TheDrugPrescribedId",
                table: "PrescriptionSet");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "PrescriptionSet");

            migrationBuilder.DropColumn(
                name: "TheDrugPrescribedId",
                table: "PrescriptionSet");

            migrationBuilder.AddColumn<int>(
                name: "DrugId",
                table: "PrescriptionSet",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
