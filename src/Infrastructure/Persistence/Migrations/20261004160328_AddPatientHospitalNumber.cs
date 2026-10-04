using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BigLion.CPA.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPatientHospitalNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HospitalNumber",
                table: "Patients",
                type: "character varying(8)",
                maxLength: 8,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_HospitalNumber",
                table: "Patients",
                column: "HospitalNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Patients_HospitalNumber",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "HospitalNumber",
                table: "Patients");
        }
    }
}
